(function(){

    var app = angular.module("Billing");

    var TownsController = function($scope, $http, DataFactory) {

        $scope.regions= REGIONS;
        $scope.showTown = false;
        var idx = 0;

        ListTowns();
        $scope.getTown = function(currentTown){
            idx = 0;
            $scope.town = currentTown;
            $scope.showTown = true;
        };
        $scope.makeChanged = function(selectedMakeCode) {

            idx = selectedMakeCode;
            idx++;
        };
        $scope.save = function(){
            if($scope.town.id === 0)
            {

                $scope.town.region = idx;

                DataFactory.insert("towns", $scope.town, function(data){ ListTowns();} );
            }
            else {
                $scope.town.region = idx;

                DataFactory.update("towns", $scope.town.id, $scope.town, function (data) {
                    ListTowns();
                });
            }
        };
        $scope.delete = function(){

            DataFactory.delete("towns", $scope.town.id, function(data){ListTowns();});
            $scope.showTown = false;

        };

        $scope.new = function(){
            $scope.town = {
                id: 0,
                name: "",
                zip: "",
                Region: 1
            };
            $scope.showTown = true;
        };

        function ListTowns(){
            DataFactory.list("towns", function(data){ $scope.towns = data});

        }
    };

    app.controller("TownsController", TownsController);

}());