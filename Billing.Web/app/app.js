(function() {

    REGIONS = ["Banja Luka", "Bihac", "Doboj", "Mostar", "Sarajevo", "Trebinje", "Tuzla", "Zenica"];

    credentials = {
        token: "",
        expiration: "",
        currentUser: {
            id: 0,
            name: "",
            roles: []
        }
    };

    redirectTo = '/';

    function authenticated() {
        if (credentials === null) return false;
        return (credentials.currentUser.id != 0);
    };

    var app = angular.module("Billing", ["ngRoute", "ui.bootstrap", "LocalStorageModule", 'oi.select']);
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
            .when("/agents/:id", {
                templateUrl: "app/components/agents/templates/show.html",
                controller: "AgentShowController"
            })
            .when("/products", {
                templateUrl: "app/components/products/templates/product.html",
                controller: "ProductsController"
            })
            .when("/customers", {
                templateUrl: "app/components/customers/templates/customer.html",
                controller: "CustomersController"
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
            .when("/invoice/:id", {
                templateUrl: "app/components/invoices/templates/show_invoice.html",
                controller: "InvoiceShowController"
            })
            .otherwise({ redirectTo: "/invoices" });
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

    app.directive('adminAccess', function() {
        return {
            scope: { owner: '=' },
            restrict: 'A',
            link: function(scope, element, attr) {
                var access = (credentials.currentUser.roles.indexOf("admin") > -1);
                if (!access) {
                    var user = credentials.currentUser.id;
                    var owner = scope.owner;
                    if (owner != user) element.remove();
                }
            }
        };
    });

}());
