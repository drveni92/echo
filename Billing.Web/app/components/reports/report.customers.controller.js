(function() {
    angular
        .module("Billing")
        .controller('ReportCustomersController', ['$scope', 'DataFactory', 'ToasterService', function($scope, DataFactory, ToasterService) {

            var initializing = false;

            $scope.dates = {
                endDate: new Date(),
                startDate: new Date()
            };

            $scope.dateOptions = {
                maxDate: new Date(),
                startingDay: 1
            };


            function ListSalesCustomer() {
                DataFactory.insert("salesbycustomer", { startDate : $scope.dates.startDate,
                    endDate : $scope.dates.endDate }, function(data) {
                    $scope.salesbycustomer = data;}
                )}

            $scope.dates.startDate.setMonth($scope.dates.startDate.getMonth() - 12);

            ListSalesCustomer();

            $scope.dateOptions = {
                maxDate: new Date(),
                startingDay: 1
            };

            $scope.selectedOption = "0";

            $scope.startDateOpened = false;
            $scope.endDateOpened = false;

            $scope.openStartDate = function() {
                $scope.startDateOpened = true;
            };


            $scope.openEndDate = function() {
                $scope.endDateOpened = true;
            };

            $scope.$watch(
                function(scope) { return scope.dates.startDate },
                function() {
                    if (initializing)
                        ListSalesCustomer();
                }
            );
            $scope.$watch(
                function(scope) { return scope.dates.endDate },
                function() {
                    if (initializing)
                        ListSalesCustomer();
                    else
                        initializing = true;
                }
            );



        }]);

}());





