(function() {
    angular
        .module("Billing")
        .controller('InvoicesController', ['$scope', '$uibModal', 'DataFactory', 'InvoicesService', 'ToasterService', function($scope, $uibModal, DataFactory, InvoicesService, ToasterService) {
            $scope.states = BillingConfig.states;

            function ListInvoices() {
                DataFactory.list("invoices", function(data) { $scope.invoices = data; });
            };

            ListInvoices();

            $scope.show = function(invoice) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/components/invoices/templates/show.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    size: 'lg',
                    resolve: {
                        data: function() {
                            return invoice
                        },
                        options: function() {
                            return []
                        }
                    }
                });

                modalInstance.result.then(function() {}, function() {});
            }

            $scope.show_history = function(invoice) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/components/invoices/templates/show_histories.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    size: 'lg',
                    resolve: {
                        data: function() {
                            return invoice
                        },
                        options: function() {
                            return []
                        }
                    }
                });

                modalInstance.result.then(function() {}, function() {});
            }



            $scope.nextState = function(invoice, cancel = false) {
            	var url = "invoices/" + invoice.id + "/next/" + cancel;
            	InvoicesService.next(url, function(data) {
            		ToasterService.pop('info', "Invoice " + invoice.invoiceNo, "New state of the invoice is " + $scope.states[data.status + 1]);
            		ListInvoices();
            	});
            }


            $scope.delete = function(invoice) {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/invoices/templates/delete.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function() {
                        return invoice
                    },
                    options: function() {
                        return []
                    }
                }
            });

            modalInstance.result.then(function(invoice) {
                DataFactory.delete("invoices", invoice.id, function(data) {
                    ToasterService.pop('success', "Success", "Invoice deleted");
                    ListInvoices();
                });
            }, function() {
                console.log('Modal dismissed at: ' + new Date());
            });
        };
        }])
}());
