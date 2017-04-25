(function () {
    angular
        .module("Billing")
        .controller('ReportDashboardController', ['$scope', 'DataFactory', 'ToasterService', 'DashboardFactory', '$timeout', function ($scope, DataFactory, ToasterService, DashboardFactory, $timeout) {

            $scope.title = '';
            var results = null;

            $scope.$watch('isMenuOpened', function () {
                if (results != null) {
                    $timeout(function () {
                        setReports();
                    }, 800);
                }
            });

            function setReports() {
                setAgentsSales(results.agentsSales);
                setBurningItems(results.burningItems);
                setCategoriesYear(results.categoriesYear);
                setCategoriesMonth(results.categoriesMonth);
                setCustomers(results.customers);
                setInvoices(results.invoices);
                setProducts(results.top5Products);
                setRegionsYear(results.regionsYear);
                setRegionsMonth(results.regionsMonth);
            };

            DataFactory.list('dashboard', function (data) {
                $scope.title = data.title;
                results = data;
                setReports();
            });

            function setCategoriesMonth(data) {
                result = DashboardFactory.monthSale(data);
                $scope.categoriesmonth = {
                    data: result.data,
                    options: result.options
                }
            };

            function setRegionsMonth(data) {
                result = DashboardFactory.monthSale(data);
                $scope.regionsmonth = {
                    data: result.data,
                    options: result.options
                };
            };

            function setRegionsYear(data) {
                result = DashboardFactory.regionsYear(data);
                $scope.regionsyear = {
                    data: result.data,
                    options: result.options
                };
            };

            function setProducts(data) {
                result = DashboardFactory.products(data);
                $scope.products = {
                    data: result.data,
                    options: result.options
                };
            };

            function setInvoices(data) {
                result = DashboardFactory.invoices(data);
                $scope.invoices = {
                    data: result.data,
                    options: result.options
                };
            };

            function setCustomers(data) {
                result = DashboardFactory.customers(data);
                $scope.customers = {
                    data: result.data,
                    options: result.options
                };
            };

            function setCategoriesYear(data) {
                result = DashboardFactory.categoriesyear(data);
                $scope.categoriesYear = {
                    data: result.data,
                    options: result.options
                };
            };

            function setAgentsSales(data) {
                result = DashboardFactory.agentsales(data);
                $scope.agentsales = {
                    data: result.data,
                    options: result.options
                };
            };

            function setBurningItems(data) {
                result = DashboardFactory.burningitems(data);
                $scope.burningitems = {
                    data: result.data,
                    options: result.options
                };
            };

        }]);

}());
