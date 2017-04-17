angular
    .module("Billing")
    .controller('CustomersController', ['$scope', '$http', '$uibModal', 'DataFactory', 'ToasterService', 'CustomersFactory', function($scope, $http, $uibModal, DataFactory, ToasterService, CustomersFactory) {

        $scope.maxPagination = BillingConfig.maxPagination
        
        function ListCustomers() {
            DataFactory.list("customers?page=" + ($scope.currentPage - 1), function(data) {
                $scope.customers = data.list;
                $scope.totalItems = data.totalItems;
                $scope.currentPage = data.currentPage + 1;
            });
        }

        $scope.pageChanged = function() {
            ListCustomers();
        };

        ListCustomers();

        $scope.new = function() {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/customers/templates/new.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function() {
                        return CustomersFactory.empty();
                    }
                }
            });

            modalInstance.result.then(function(customer) {
                DataFactory.insert("customers", CustomersFactory.customer(customer), function(data) {
                    ToasterService.pop('success', "Success", "Customer added");
                    ListCustomers();
                });
            }, function() {
                console.log('Modal dismissed at: ' + new Date());
            });
        };

        $scope.show = function(customer) {

            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/customers/templates/show.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function() {
                        return customer
                    }
                }
            });

            modalInstance.result.then(function() {}, function() {});

        };

        $scope.edit = function(customer) {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/customers/templates/edit.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function() {
                        return $.extend(true, {}, customer)
                    }
                }
            });

            modalInstance.result.then(function(customer) {
                DataFactory.update("customers", customer.id, CustomersFactory.customer(customer), function(data) {
                    ToasterService.pop('success', "Success", "Customer saved");
                    ListCustomers();
                });
            }, function() {
                ListCustomers();
            });
        }


        $scope.delete = function(customer) {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/customers/templates/delete.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function() {
                        return customer
                    }
                }
            });

            modalInstance.result.then(function(customer) {
                DataFactory.delete("customers", customer.id, function(data) {
                    ToasterService.pop('success', "Success", "Customer deleted");
                    ListCustomers();
                });
            }, function() {
                console.log('Modal dismissed at: ' + new Date());
            });
        }

    }]);
