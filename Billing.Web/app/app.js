(function(){

    var app = angular.module("Billing", ["ngRoute"]);

    app.config(function($routeProvider){
        $routeProvider
            .when("/agents", {
                templateUrl: "app/components/agents/templates/agents.html",
                controller: "AgentsController" })
            .otherwise({ redirectTo: "/agents" });
    });
}());