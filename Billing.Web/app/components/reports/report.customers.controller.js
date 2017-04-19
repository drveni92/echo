(function() {
    angular
        .module("Billing")
        .controller('ReportCustomersController', ['$scope', 'DataFactory', 'ToasterService', function($scope, DataFactory, ToasterService) {

            var response = null;

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

            $scope.dates = {
                endDate: new Date(),
                startDate: new Date()
            };
            $scope.dates.startDate.setMonth($scope.dates.startDate.getMonth() - 1);

            $scope.options = {
                chart: {
                    type: 'discreteBarChart',
                    height: 650,
                    margin: {
                        top: 20,
                        right: 200,
                        bottom: 250,
                        left: 200
                    },
                    useInteractiveGuideline: true,
                    x: function(d) {
                        return d.label;
                    },
                    y: function(d) {
                        if ($scope.selectedOption === "0") return d.value.turnover + (1e-10);
                        else return d.value.percent;
                    },
                    showValues: true,
                    valueFormat: function(d) {
                        return d3.format(',.2f')(d);
                    },
                    duration: 500,
                    xAxis: {
                        rotateLabels: 30
                    },
                    yAxis: {
                        axisLabelDistance: 50
                    }
                }
            };

            $scope.createGraph = function() {
                DataFactory.insert('salesbycustomer', $scope.dates, function(result) {
                	response = result;
                	setGraph(result);
                });
            };

			var setGraph = function(data) {
                tempData = [];
                for (var i = data.customers.length - 1; i >= 0; i--) {
                    tempData.push({ label: data.customers[i].name, value: { turnover: data.customers[i].turnover, percent: data.customers[i].percent } })
                }
                $scope.data = [{ key: "Sales by Customer", values: tempData }];
            }

            $scope.updateGraph = function() {
                setGraph(response);
            };

            $scope.createGraph();

        }]);

}());
