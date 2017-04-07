(function() {
    angular
        .module("Billing")
        .controller('InvoicesNewController', ['$scope', '$uibModal', 'DataFactory', function($scope, $uibModal, DataFactory) {
            $scope.states = BillingConfig.states;

            $scope.invoice = {
                id: 0,
                invoiceNo: '',
                date: new Date(),
                shipping: 0,
                agnent: { id: null },
                shipper: { id: null },
                customer: { id: null },
                items: [],
            };

            (function() {
                DataFactory.list("shippers", function(data) {
                    $scope.shippers = data;
                });
            }());

            (function() {
                DataFactory.list("customers", function(data) {
                    $scope.customers = data;
                });
            }());

            $scope.open = function() {
                $scope.popup.opened = true;
            };

            $scope.popup = {
                opened: false
            };



        }])
}());
