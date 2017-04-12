angular
    .module("Billing")
    .controller('CustomersController', ['$scope', '$http', '$uibModal', 'DataFactory', 'ToasterService', function($scope, $http, $uibModal, DataFactory, ToasterService) {

        $scope.maxPagination = BillingConfig.maxPagination
        
        function ListCustomers(page) {
            DataFactory.list("customers?page=" + page, function(data) {
                $scope.customers = data.list;
                $scope.totalItems = data.totalItems;
                $scope.currentPage = data.currentPage + 1;
            });
        }

        $scope.pageChanged = function() {
            ListCustomers($scope.currentPage - 1);
        };

        ListCustomers(0);

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
                        return { id: 0, name: '', address: '', town: { id: null } }
                    },
                    options: function() {
                        return ["towns"]
                    }
                }
            });

            modalInstance.result.then(function(customer) {
                DataFactory.insert("customers", customer, function(data) {
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
                    },
                    options: function() {
                        return []
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
                    },
                    options: function() {
                        return ["towns"]
                    }
                }
            });

            modalInstance.result.then(function(customer) {
                DataFactory.update("customers", customer.id, customer, function(data) {
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
                    },
                    options: function() {
                        return []
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
