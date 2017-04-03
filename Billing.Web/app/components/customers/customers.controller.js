angular
    .module("Billing")
    .controller('CustomersController', ['$scope', '$http', 'DataFactory',  function($scope, $http, DataFactory) {
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
            $scope.showAgent = false;
            $scope.showCustomer = false;

        };

        $scope.new = function() {
            $scope.customer = {
                id: 0,
                name: "",
                address: "",
                town: { id: 1 }
            };
        };

        $scope.open = function() {
            $scope.showModal = true;
        };

        $scope.ok = function() {
            $scope.showModal = false;
        };

        $scope.cancel = function() {
            $scope.showModal = false;
        };

        function ListCustomers() {
            DataFactory.list("customers", function(data) { $scope.customers = data });
        }

        ListCustomers();
    }]);
