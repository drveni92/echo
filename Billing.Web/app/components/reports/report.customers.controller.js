(function() {
    angular
        .module("Billing")
        .controller('ReportCustomersController', ['$scope', 'DataFactory', 'ToasterService', function($scope, DataFactory, ToasterService) {

            var data = null;

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
                    x: function(d) {
                        return d.label;
                    },
                    y: function(d) {
                        return d.value + (1e-10);
                    },
                    showValues: true,
                    valueFormat: function(d) {
                        return d3.format(',.2f')(d);
                    },
                    duration: 500,
                    xAxis: {
                        axisLabel: 'Customers',
                        rotateLabels: 30
                    },
                    yAxis: {
                        axisLabel: 'Turnovers',
                        axisLabelDistance: 50
                    }
                }
            };


            DataFactory.insert('salesbycustomer', { startDate: '10.10.2010', endDate: '11.11.2016' }, function(result) {
                data = result;
                temp = [];
                var tempData = [];
                for (var i = data.customers.length - 1; i >= 0; i--) {
                    tempData.push({ label: data.customers[i].name, value: data.customers[i].turnover })
                }
                $scope.data = [{ key: "Sales by Customer", values: tempData }];
            });

        }]);

}());
