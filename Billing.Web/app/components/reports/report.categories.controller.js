(function() {
    angular
        .module("Billing")
        .controller('ReportCategoriesController', ['$scope', 'DataFactory', 'ToasterService', function($scope, DataFactory, ToasterService) {

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
            $scope.dates.startDate.setMonth($scope.dates.startDate.getMonth() - 5);



            $scope.options = {
                chart: {
                    type: 'pieChart',
                    height: 500,
                    x: function(d){
                        return d.key;
                    },
                    y: function(d){
                        return d.y;
                    },
                    showLabels: true,
                    duration: 500,
                    labelThreshold: 0.01,
                    labelSunbeamLayout: true,
                    legend: {
                        margin: {
                            top: 5,
                            right: 35,
                            bottom: 5,
                            left: 0
                        }
                    }
                }
            };

            $scope.createGraph = function() {
                DataFactory.insert('salesbycategory', $scope.dates, function(result) {
                    response = result;
                    setGraph(result);
                });
            };

            var setGraph = function(data) {
                var tempData = [];
                for (var i = data.sales.length - 1; i >= 0; i--) {
                    tempData.push({ key: data.sales[i].name, y: data.sales[i].total })
                }
                $scope.data = tempData;
            }

            $scope.updateGraph = function() {
                setGraph(response);
            };

            $scope.createGraph();

        }]);


}());