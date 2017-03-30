(function(){

	authenticated = false;

    var app = angular.module("Billing", ["ngRoute"]);

    app.config(function($routeProvider){
        $routeProvider
            .when("/agents", {
                templateUrl: "app/components/agents/templates/agents.html",
                controller: "AgentsController" })
            .when("/login", {
                templateUrl: "app/components/sessions/templates/login.html",
                controller: "SessionsController" })
            .otherwise({ redirectTo: "/agents" });
    }).run(function($rootScope, $location){
        $rootScope.$on("$routeChangeStart", function(event, next, current){
            if(!authenticated){
                if(next.templateUrl != "app/components/sessions/templates/login.html"){
                    $location.path("/login");
                }
            }
        })
    });
}());