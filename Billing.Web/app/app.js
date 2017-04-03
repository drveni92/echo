(function(){

    var app = angular.module("Billing", ["ngRoute"]);

    app.config(function($routeProvider){
        $routeProvider
            .when("/agents", {
                templateUrl: "app/components/agents/templates/agents.html",
                controller: "AgentsController" })
            .when("/customers", {
                templateUrl: "app/components/customers/templates/customer.html",
                controller: "CustomersController" })
            .when("/login", {
                templateUrl: "app/components/sessions/templates/login.html",
                controller: "SessionsController" })
            .when("/logout", {
                templateUrl: "app/components/sessions/templates/logout.html",
                controller: "SessionsController" })
            .otherwise({ redirectTo: "/agents" });
    }).run(function($rootScope, $location){
        $rootScope.$on("$routeChangeStart", function(event, next, current){
            console.log($rootScope.authenticated);
            if(!$rootScope.authenticated){
                if(next.templateUrl != "app/components/sessions/templates/login.html"){
                    $location.path("/login");
                }
            }
        })
    });
}());