(function () {

    var app = angular.module("Billing", ["ngRoute", "ui.bootstrap", "ngIdle", "ngAnimate", "LocalStorageModule", 'oi.select', 'nvd3']);

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
        return (credentials.currentUser.id !== 0);
    }

    app.config(function ($routeProvider) {
        $routeProvider
            .when("/dashboard", {
                templateUrl: "app/view/reports/dashboard.html",
                controller: "ReportDashboardController"
            })
            .when("/login", {
                templateUrl: "app/view/sessions/login.html",
                controller: "LoginController"
            })
            .when("/logout", {
                template: "",
                controller: "LogoutController"
            })
            .when("/agents", {
                templateUrl: "app/view/agents/agents.html",
                controller: "AgentsController"
            })
            .when("/agents/:id", {
                templateUrl: "app/view/agents/show.html",
                controller: "AgentShowController"
            })
            .when("/products", {
                templateUrl: "app/view/products/product.html",
                controller: "ProductsController"
            })
            .when("/customers", {
                templateUrl: "app/view/customers/customer.html",
                controller: "CustomersController"
            })
            .when("/report/customers", {
                templateUrl: "app/view/reports/customer.html",
                controller: "ReportCustomersController"
            })
            .when("/report/categories", {
                templateUrl: "app/view/reports/category.html",
                controller: "ReportCategoriesController"
            })
            .when("/report/agents/regions", {
                templateUrl: "app/view/reports/agentsregions.html",
                controller: "ReportAgentsRegionsController"
            })
            .when("/report/customers/categories", {
                templateUrl: "app/view/reports/customerscategories.html",
                controller: "ReportCustomersCategoriesController"
            })
            .when("/report/maps", {
                templateUrl: "app/view/reports/maps.html",
                controller: "ReportsMapController"
            })
            .when("/towns", {
                templateUrl: "app/view/towns/town.html",
                controller: "TownsController"
            })
            .when("/categories", {
                templateUrl: "app/view/categories/category.html",
                controller: "CategoriesController"
            })
            .when("/stocks", {
                templateUrl: "app/view/stocks/stock.html",
                controller: "StocksController"

            })
            .when("/suppliers", {
                templateUrl: "app/view/suppliers/supplier.html",
                controller: "SuppliersController"
            })
            .when("/shippers", {
                templateUrl: "app/view/shippers/shipper.html",
                controller: "ShippersController"
            })
            .when("/procurements", {
                templateUrl: "app/view/procurements/procurements.html",
                controller: "ProcurementsController"
            })
            .when("/invoices", {
                templateUrl: "app/view/invoices/invoices.html",
                controller: "InvoicesController"
            })
            .when("/invoices/new", {
                templateUrl: "app/view/invoices/new.html",
                controller: "InvoicesNewController"
            })
            .when("/invoices/:id", {
                templateUrl: "app/view/invoices/new.html",
                controller: "InvoicesNewController"
            })
            .when("/invoice/:id", {
                templateUrl: "app/view/invoices/show_invoice.html",
                controller: "InvoiceShowController"
            })
            .when("/report/regions", {
                templateUrl: "app/view/reports/salesbyregion.html",
                controller: "SalesByRegionController"
            })
            .when("/invoicesreview", {
                templateUrl: "app/view/reports/invoicesreview.html",
                controller: "InvoicesReviewController"
            })
            .otherwise({ redirectTo: "/dashboard" });
    }).run(function ($rootScope, $location) {
        $rootScope.$on("$routeChangeStart", function (event, next, current) {
            if (!authenticated()) {
                if (next.templateUrl != "app/view/sessions/login.html") {
                    redirectTo = $location.path();
                    $location.path("/login");
                }
            }
        });
        $rootScope.authenticated = authenticated;
    });
}());
