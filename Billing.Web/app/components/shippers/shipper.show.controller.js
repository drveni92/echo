angular
    .module("Billing")
    .controller("ShipperShowController", ['$scope', '$http', '$route', '$routeParams', 'DataFactory', function($scope, $http, $route, $routeParams, DataFactory) {
        var shipperId = $routeParams.id;

        DataFactory.read("shippers", shipperId, function(data) {
            $scope.shipper = data;
        });

    }]);