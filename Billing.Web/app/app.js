(function() {

    credentials = {
        token: "",
        expiration: "",
        currentUser: {
            id: 0,
            name: "",
            role: ""
        }
    };

    function authenticated() {
        return (credentials.currentUser.id != 0);
    };

    var app = angular.module("Billing", ["ngRoute", "ui.bootstrap"]);

    app.config(function($routeProvider) {
        $routeProvider
          .when("/login", {
                templateUrl: "app/components/sessions/templates/login.html",
                controller: "SessionsController"
            })
            .when("/agents", {
                templateUrl: "app/components/agents/templates/agents.html",
                controller: "AgentsController"
            })
            .when("/customers", {
                templateUrl: "app/components/customers/templates/customer.html",
                controller: "CustomersController"
            })
            .when("/categories", {
                templateUrl: "app/components/categories/templates/category.html",
                controller: "CategoriesController"
            })
            .when("/stocks", {
                templateUrl: "app/components/stocks/templates/stock.html",
                controller: "StocksController"
            })
            .otherwise({ redirectTo: "/agents" });
    }).run(function($rootScope, $location) {
        $rootScope.$on("$routeChangeStart", function(event, next, current) {
            if (!authenticated()) {
                if (next.templateUrl != "app/components/sessions/templates/login.html") {
                    $location.path("/login");
                }
            }
        });
        $rootScope.authenticated = authenticated;
    });
}());
