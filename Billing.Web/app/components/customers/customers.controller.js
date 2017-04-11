angular
    .module("Billing")
    .controller('CustomersController', ['$scope', '$http', '$uibModal', 'DataFactory', function($scope, $http, $uibModal, DataFactory) {

        function ListCustomers() {
            DataFactory.list("customers", function(data) { $scope.customers = data });
        }

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
                        return { id: 0, name: '', address: '', town: { id: null } }
                    },
                    options: function() {
                        return ["towns"]
                    }
                }
            });

            modalInstance.result.then(function(customer) {
                DataFactory.insert("customers", customer, function(data) { ListCustomers(); });
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

            modalInstance.result.then(function() {
            }, function() {
            });

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
                    ListCustomers();
                    //message success missing
                });
            }, function() {
                console.log('Modal dismissed at: ' + new Date());
            });
        }

    }]);
