(function(){

    var app = angular.module("Billing");

    var StocksController = function($scope, $http, DataFactory) {

        $scope.showStock = false;
        ListStock();

        $scope.getStock = function(currentStock){
            $scope.stock = currentStock;
            $scope.showStock = true;
        };

        function ListStock(){
            DataFactory.list("stocks", function(data){ $scope.stocks = data});
        }
    };

    app.controller("StocksController", StocksController);

}());