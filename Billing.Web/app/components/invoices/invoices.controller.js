(function() {
    angular
        .module("Billing")
        .controller('InvoicesController', ['$scope', '$uibModal', 'DataFactory', 'InvoicesService', function($scope, $uibModal, DataFactory, InvoicesService) {
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
            		/* success message */
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
                    ListInvoices();
                    //message success missing
                });
            }, function() {
                console.log('Modal dismissed at: ' + new Date());
            });
        };
        }])
}());
