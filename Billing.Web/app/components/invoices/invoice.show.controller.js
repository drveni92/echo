(function() {
    angular
        .module("Billing")
        .controller('InvoiceShowController', ['$scope','$route', '$routeParams', 'DataFactory', 'InvoicesService', function($scope, $route, $routeParams, DataFactory, InvoicesService) {
            var invoiceId = $routeParams.id;
            if(invoiceId) {
                DataFactory.read("invoices", invoiceId, function(data) {
                    $scope.invoice = data;
                    $scope.invoice.date = new Date($scope.invoice.date);
                });
            }
        }]);
}());
