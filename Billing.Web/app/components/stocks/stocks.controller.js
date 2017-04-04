(function(){

    var app = angular.module("Billing");

    var StocksController = function($scope, $http, DataFactory) {

        $scope.showCategory = false;
        ListStock();

        $scope.getStock = function(currentStock){
            $scope.stock = currentStock;
            $scope.showStock = true;
        };

        $scope.save = function(){
            if($scope.stock.id === 0)
                DataFactory.insert("stocks", $scope.stock, function(data){ ListStock();} );
            else
                DataFactory.update("stocks", $scope.stock.id, $scope.stock, function(data){ListStock();});
        };
        $scope.delete = function(){

            DataFactory.delete("stocks", $scope.stock.id, function(data){ListStock();});
            $scope.showStock = false;

        };

        $scope.new = function(){
            $scope.stock = {
                id: 0,
                name: ""
            };
            $scope.showStock = true;
        };

        function ListStock(){
            DataFactory.list("stocks", function(data){ $scope.stocks = data});
        }
    };

    app.controller("StocksController", StocksController);

}());