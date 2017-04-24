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
                    type: 'multiBarHorizontalChart',
                    height: 440,
                    x: function(d){return d.label1;},
                    y: function(d){return d.value;},
                    showControls: true,
                    showValues: true,
                    duration: 500,
                    xAxis: {
                        showMaxMin: false
                    },
                    yAxis: {
                        axisLabel: 'Turnover',
                        tickFormat: function(d) {
                            return d3.format(',.2f')(d);
                        }
                    },
                    callback: function(chart) {
                        chart.multibar.dispatch.on('elementClick', function(e){
                            console.log('elementClick in callback', e.data);
                            var Id = e.data.label2;
                            console.log(Id);

                        });
                    }
                }
            };

            $scope.createGraph = function() {
                DataFactory.insert('salesbycategory', $scope.dates, function(result) {
                    response = result;
                    setGraph(result);
                    console.log(result);
                });
            };

            var setGraph = function(data) {
                var tempData = [];
                for (var i = data.sales.length - 1; i >= 0; i--) {
                    tempData.push({ label1: data.sales[i].name,label2: data.sales[i].id , value: data.sales[i].total })
                }
                $scope.data = [{ key: "Turnover" , color: "#d9534f" , values: tempData }];
            }

            $scope.updateGraph = function() {
                setGraph(response);
            };

            $scope.createGraph();

        }]);


}());