(function() {

    REGIONS = ["Banja Luka", "Bihac", "Doboj", "Mostar", "Sarajevo", "Trebinje", "Tuzla", "Zenica"];

    credentials = {
        token: "",
        expiration: "",
        currentUser: {
            id: 0,
            name: "",
            role: ""
        }
    };

    redirectTo = '/';

    function authenticated() {
        if (credentials == null) return false;
        return (credentials.currentUser.id != 0);
    };

    var app = angular.module("Billing", ["ngRoute", "ui.bootstrap", "LocalStorageModule"]);

    app.config(function($routeProvider) {
        $routeProvider
            .when("/login", {
                templateUrl: "app/components/sessions/templates/login.html",
                controller: "LoginController"
            })
            .when("/logout", {
                template: "",
                controller: "LogoutController"
            })
            .when("/agents", {
                templateUrl: "app/components/agents/templates/agents.html",
                controller: "AgentsController"
            })
            .when("/customers", {
                templateUrl: "app/components/customers/templates/customer.html",
                controller: "CustomersController"
            })
            .when("/customer/:id", {
                templateUrl: "app/components/customers/templates/show.html",
                controller: "CustomerShowController"
            })
            .when("/towns", {
                templateUrl: "app/components/towns/templates/town.html",
                controller: "TownsController"
            })
            .when("/categories", {
                templateUrl: "app/components/categories/templates/category.html",
                controller: "CategoriesController"
            })
            .when("/stocks", {
                templateUrl: "app/components/stocks/templates/stock.html",
                controller: "StocksController"

            })
            .when("/suppliers", {
                templateUrl: "app/components/suppliers/templates/supplier.html",
                controller: "SuppliersController"
            })
            .when("/shippers", {
                templateUrl: "app/components/shippers/templates/shipper.html",
                controller: "ShippersController"
            })
            .when("/shipper/:id", {
                templateUrl: "app/components/shippers/templates/show.html",
                controller: "ShipperShowController"
            })
            .when("/procurements", {
                templateUrl: "app/components/procurements/templates/procurements.html",
                controller: "ProcurementsController"
            })
            .when("/invoices", {
                templateUrl: "app/components/invoices/templates/invoices.html",
                controller: "InvoicesController"
            })
            .when("/invoices/new", {
                templateUrl: "app/components/invoices/templates/new.html",
                controller: "InvoicesNewController"
            })
            .when("/invoices/:id", {
                templateUrl: "app/components/invoices/templates/new.html",
                controller: "InvoicesNewController"
            })
            .otherwise({ redirectTo: "/agents" });
    }).run(function($rootScope, $location) {
        $rootScope.$on("$routeChangeStart", function(event, next, current) {
            if (!authenticated()) {
                if (next.templateUrl != "app/components/sessions/templates/login.html") {
                    redirectTo = $location.path();
                    $location.path("/login");
                }
            }
        });
        $rootScope.authenticated = authenticated;
    });
}());
