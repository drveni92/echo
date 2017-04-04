(function() {

    REGIONS = [ "Banja Luka", "Bihac", "Doboj", "Mostar", "Sarajevo", "Trebinje", "Tuzla", "Zenica" ];

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

    var app = angular.module("Billing", ["ngRoute"]);

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
            .when("/towns", {
                templateUrl: "app/components/towns/templates/town.html",
                controller: "TownsController"
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
