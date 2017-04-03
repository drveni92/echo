(function(){

    var app = angular.module("Billing");

    var AgentsController = function($scope, $http, DataService) {

        $scope.showAgent = false;
        ListAgents();

        $scope.getAgent = function(currentAgent){
            $scope.agent = currentAgent;
            $scope.showAgent = true;
        };

        $scope.save = function(){
            if($scope.agent.id === 0)
                DataService.insert("agents", $scope.agent, function(data){ ListAgents();} );
            else
                DataService.update("agents", $scope.agent.id, $scope.agent, function(data){ListAgents();});
        };
        $scope.delete = function(){

            DataService.delete("agents", $scope.agent.id, function(data){ListAgents();});
            $scope.showAgent = false;

        };

        $scope.new = function(){
            $scope.agent = {
                id: 0,
                name: "",
                username:""
            };
            $scope.showAgent = true;
        };

        function ListAgents(){
            DataService.list("agents", function(data){ $scope.agents = data});
        }
    };

    app.controller("AgentsController", AgentsController);

}());