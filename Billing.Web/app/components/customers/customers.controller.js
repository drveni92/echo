angular
    .module("Billing")
    .controller('CustomersController', ['$scope', '$http', '$uibModal', 'DataFactory', function($scope, $http, $uibModal, DataFactory) {
        $scope.get = function(currentCustomer) {
            $scope.customer = currentCustomer;
            $scope.showCustomer = true;
        };

        $scope.save = function() {
            if ($scope.customer.id === 0)
                DataFactory.insert("customers", $scope.customer, function(data) { ListCustomers(); });
            else
                DataFactory.update("customers", $scope.customer.id, $scope.customer, function(data) { ListCustomers(); });
        };
        $scope.delete = function() {
            DataFactory.delete("customers", $scope.customer.id, function(data) { ListCustomers(); });
        };

        function ListCustomers() {
            DataFactory.list("customers", function(data) { $scope.customers = data });
        }

        ListCustomers();

        $scope.new = function() {
            DataFactory.list("towns", function(data) {
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
                            return { towns: data }
                        }
                    }
                });

                modalInstance.result.then(function(customer) {
                    DataFactory.insert("customers", customer, function(data) { ListCustomers(); });
                }, function() {
                    console.log('Modal dismissed at: ' + new Date());
                });
            });
        };
    }]);
