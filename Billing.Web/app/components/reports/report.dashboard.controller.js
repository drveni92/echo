(function() {
    angular
        .module("Billing")
        .controller('ReportDashboardController', ['$scope', 'DataFactory', 'ToasterService', 'DashboardFactory', function($scope, DataFactory, ToasterService, DashboardFactory) {
            
            $scope.title = '';

            DataFactory.list('dashboard', function(data) {
                console.log(data);
                $scope.title = data.title;
                setAgentsSales(data);
                setBurningItems(data);
                setCategoriesYear(data);
                setCustomers(data);
                setInvoices(data);
                setProducts(data);
            });

            function setProducts(data) {
                result = DashboardFactory.products(data.top5Products);
                $scope.products = {
                    data: result.data,
                    options: result.options
                };
            };

            function setInvoices(data) {
                result = DashboardFactory.invoices(data.invoices);
                $scope.invoices = {
                    data: result.data,
                    options: result.options
                };
            };

            function setCustomers(data) {
                result = DashboardFactory.customers(data.customers);
                $scope.customers = {
                    data: result.data,
                    options: result.options
                };
            };

            function setCategoriesYear(data) {
                result = DashboardFactory.categoriesyear(data.categoriesYear);
                $scope.categoriesYear = {
                    data: result.data,
                    options: result.options
                };
            };

            function setAgentsSales(data) {
                result = DashboardFactory.agentsales(data.agentsSales);
                $scope.agentsales = {
                    data: result.data,
                    options: result.options
                };
            };

            function setBurningItems(data) {
                result = DashboardFactory.burningitems(data.burningItems);
                $scope.burningitems = {
                    data: result.data,
                    options: result.options
                };
            };

        }]);

}());
