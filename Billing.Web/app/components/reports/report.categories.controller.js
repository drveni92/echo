(function () {
    angular
        .module("Billing")
        .controller('ReportCategoriesController', ['$scope', 'DataFactory', 'ToasterService', '$uibModal', '$timeout', function ($scope, DataFactory, ToasterService, $uibModal, $timeout) {

            var response = null;

            $scope.$watch('isMenuOpened', function () {
                if (response !== null) {
                    $timeout(function () {
                        setGraph(response);
                    }, 800);
                }
            });

            $scope.dateOptions = {
                maxDate: new Date(),
                startingDay: 1
            };

            $scope.selectedOption = "0";

            $scope.startDateOpened = false;
            $scope.endDateOpened = false;

            $scope.openStartDate = function () {
                $scope.startDateOpened = true;
            };

            $scope.openEndDate = function () {
                $scope.endDateOpened = true;
            };

            $scope.dates = {
                endDate: new Date(),
                startDate: new Date(),
                id: 0
            };
            $scope.dates.startDate.setMonth($scope.dates.startDate.getMonth() - 5);



            $scope.options = {
                chart: {
                    type: 'discreteBarChart',
                    height: 550,
                    x: function (d) {
                        return d.label1;
                    },
                    y: function (d) {
                        return d.value;
                    },
                    margin: {
                        top: 20,
                        right: 100,
                        bottom: 100,
                        left: 100
                    },
                    showValues: true,
                    duration: 500,
                    yAxis: {
                        tickFormat: function (d) {
                            return d3.format('$,.2f')(d);
                        }
                    },
                    forceY: 200000,
                    staggerLabels: true,
                    callback: function (chart) {
                        chart.discretebar.dispatch.on('elementClick', function (e) {
                            $scope.dates.id = e.data.label2;
                            DataFactory.insert('salesbyproduct', $scope.dates, function (result) {
                                var modalInstance = $uibModal.open({
                                    animation: true,
                                    ariaLabelledBy: 'modal-title',
                                    ariaDescribedBy: 'modal-body',
                                    templateUrl: 'app/components/reports/templates/salesbyproduct.html',
                                    controller: 'ModalInstanceController',
                                    controllerAs: '$modal',
                                    size: 'lg',
                                    resolve: {
                                        data: function () {
                                            return result;
                                        }
                                    }
                                });

                                modalInstance.result.then(function () {
                                }, function () {

                                });
                            });
                        });
                    }
                },
                title: {
                    enable: true,
                    text: 'Sales by categories'
                }
            };

            $scope.createGraph = function () {
                DataFactory.insert('salesbycategory', $scope.dates, function (result) {
                    response = result;
                    setGraph(result);
                });
            };

            var setGraph = function (data) {
                var tempData = [];
                for (var i = data.sales.length - 1; i >= 0; i--) {
                    tempData.push({ label1: data.sales[i].name, label2: data.sales[i].id, value: data.sales[i].total });
                }
                $scope.data = [{ key: "Sales By Cateogry", values: tempData }];
            };

            $scope.updateGraph = function () {
                setGraph(response);
            };

            $scope.createGraph();

        }]);


}());
