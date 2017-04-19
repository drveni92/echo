(function() {
    angular
        .module("Billing")
        .controller('ReportAgentsRegionsController', ['$scope', 'DataFactory', 'ToasterService', function($scope, DataFactory, ToasterService) {

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
                    type: 'multiBarChart',
                    height: 650,
                    margin: {
                        top: 20,
                        right: 100,
                        bottom: 250,
                        left: 100
                    },
                    x: function(d) {
                        return d.x.split(/(?=[A-Z])/).join(" ").toLowerCase();
                    },
                    valueFormat: function(d) {
                        return d3.format('$,.2f')(d);
                    },
                    clipEdge: true,
                    duration: 500,
                    stacked: false,
                    xAxis: {
                        rotateLabels: 30
                    },
                    yAxis: {
                        axisLabelDistance: 50
                    }
                }
            };

            $scope.data = [];

            $scope.createGraph = function() {
                DataFactory.insert('salesbyagentsregions', $scope.dates, function(result) {
                    response = result;
                    setGraph(result);
                });
            };

            var setGraph = function(data) {
                $scope.data = [];
                for (var i = data.agents.length - 1; i >= 0; i--) {
                    var temp = { key: data.agents[i].name, values: [] };
                    for (var j = data.regions.length - 1; j >= 0; j--) {
                        var reg = String(data.regions[j].region);
                        temp.values.push({ x: reg, y: data.agents[i].sales[reg] });
                    }
                    $scope.data.push(temp);
                }
            }

            $scope.updateGraph = function() {
                setGraph(response);
            };

            $scope.createGraph();

        }]);

}());
