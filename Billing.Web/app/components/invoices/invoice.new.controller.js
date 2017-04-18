(function() {
    angular
        .module("Billing")
        .controller('InvoicesNewController', ['$scope', '$uibModal', '$route', '$routeParams', 'DataFactory', 'ToasterService', '$location', 'InvoicesFactory', function($scope, $uibModal, $route, $routeParams, DataFactory, ToasterService, $location, InvoicesFactory) {
            $scope.states = BillingConfig.states;

            $scope.active = 0;
            $scope.step2 = true;
            $scope.step3 = true;
            document.getElementById('step2').style.pointerEvents = 'none';
            document.getElementById('step3').style.pointerEvents = 'none';

            $scope.invoice = InvoicesFactory.empty();

            var invoiceId = $routeParams.id;

            if (invoiceId) {
                DataFactory.read("invoices", invoiceId, function(data) {
                    $scope.invoice = data;
                    $scope.invoice.date = new Date($scope.invoice.date);
                });
            }

            $scope.add_new_item = function(item) {
                /* check if item exists */
                var exists = false;
                for (var i = $scope.invoice.items.length - 1; i >= 0; i--) {
                    if ($scope.invoice.items[i].product.name === item.name && $scope.invoice.items[i].price === item.price) {
                        exists = true;
                        $scope.invoice.items[i].quantity += item.quantity;
                    }
                }
                if (!exists) {
                    $scope.invoice.items.unshift(InvoicesFactory.item(item));
                }
                ToasterService.pop('info', "Info", "Item added");
            };

            $scope.deleteItem = function(item) {
                for (var i = $scope.invoice.items.length - 1; i >= 0; i--) {
                    if ($scope.invoice.items[i].product.name === item.product.name && $scope.invoice.items[i].price === item.price) {
                        $scope.invoice.items.splice(i, 1);
                    }
                }
                ToasterService.pop('success', "Success", "Item deleted");
            };

            $scope.open = function() {
                $scope.popup.opened = true;
            };

            $scope.popup = {
                opened: false
            };

            $scope.setActieveTab = function(index) {
                if (index === 2) $scope.invoice = InvoicesFactory.invoice($scope.invoice);
                $scope.active = index;
            };

            $scope.getProducts = function(value) {
                var url = 'products/' + value;
                return DataFactory.promise(url)
                    .then(function(response) {
                        return response.data.list;
                    });
            };

            $scope.getCustomers = function(value) {
                var url = 'customers/' + value;
                return DataFactory.promise(url)
                    .then(function(response) {
                        return response.data.list;
                    });
            };

            $scope.getShippers = function(value) {
                var url = 'shippers/' + value;
                return DataFactory.promise(url)
                    .then(function(response) {
                        return response.data.list;
                    });
            }

            $scope.save = function() {
                if (invoiceId == null) {
                    DataFactory.insert("invoices", $scope.invoice, function(data) {
                        if (data) {
                            $scope.invoice = InvoicesFactory.invoice(data);
                            ToasterService.pop('success', "Success", "Invoice added");
                            $location.path('/invoice/' + data.id);
                        }
                    });
                } else {
                    DataFactory.update("invoices", $scope.invoice.id, $scope.invoice, function(data) {
                        if (data) {
                            $scope.invoice = InvoicesFactory.invoice(data);
                            ToasterService.pop('success', "Success", "Invoice saved");
                            $location.path('/invoice/' + data.id);
                        }
                    });
                }

            };


            var navListItems = $('div.setup-panel div a'),
                allWells = $('.setup-content'),
                allNextBtn = $('.nextBtn');

            allWells.hide();

            navListItems.click(function (e) {
                e.preventDefault();
                var $target = $($(this).attr('href')),
                    $item = $(this);

                if (!$item.hasClass('disabled')) {
                    navListItems.removeClass('btn-primary').addClass('btn-default');
                    $item.addClass('btn-primary');
                    allWells.hide();
                    $target.show();
                    $target.find('input:eq(0)').focus();
                }
            });

            allNextBtn.click(function(){
                var curStep = $(this).closest(".setup-content"),
                    curStepBtn = curStep.attr("id"),
                    nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a"),
                    curInputs = curStep.find("input[type='text'],input[type='url']"),
                    isValid = true;
                $(".form-group").removeClass("has-error");
                for(var i=0; i<curInputs.length; i++){
                    if (!curInputs[i].validity.valid){
                        isValid = false;
                        $(curInputs[i]).closest(".form-group").addClass("has-error");
                    }
                }

                if (isValid && curStepBtn === 'step-1'){
                    nextStepWizard.removeAttr('disabled').trigger('click');
                $scope.step2 = false;
                document.getElementById('step2').style.pointerEvents = 'auto';
                    $scope.step3 = false;
                    document.getElementById('step3').style.pointerEvents = 'auto';
                    $scope.invoice = InvoicesFactory.invoice($scope.invoice);
                }
                $scope.invoice = InvoicesFactory.invoice($scope.invoice);

                if (curStepBtn === 'step-2' ){
                    nextStepWizard.removeAttr('disabled').trigger('click');

                }

              /* if (isValid && curStepBtn === 'step-2' && $scope.invoice.items.length > 0 ){
                    nextStepWizard.removeAttr('disabled').trigger('click');
                    $scope.step3 = false;
                    document.getElementById('step3').style.pointerEvents = 'auto';
                    $scope.invoice = InvoicesFactory.invoice($scope.invoice);
                }*/

            });

            $('div.setup-panel div a.btn-primary').trigger('click');

        }])
}());
