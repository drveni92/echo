(function () {
    angular
        .module("Billing")
        .controller('SalesByRegionController', ['$scope', '$http', '$uibModal', 'DataFactory', '$filter', function ($scope, $http, $uibModal, DataFactory, $filter) {

            function ListSales() {
                tmp = 0;
                DataFactory.insert("salesbyregion", {
                    startDate: $scope.dates.startDate,
                    endDate: $scope.dates.endDate
                }, function (data) {
                    $scope.salesbyregion = data;
                    $scope.sales = data.sales;
                    for (var i = 0; i < $scope.sales.length; i++) {
                        if ($scope.selectedOption != null)
                            if ($scope.sales[i].name == $scope.selectedOption.name) {
                                index = i;
                                $scope.sale = $scope.sales[i]
                                tmp = 1;
                                $scope.selectedOption = $scope.sales[i]
                                break;
                            }
                    }
                    if (tmp === 0)
                        $scope.sale = null;

                }

                );

            };
            $scope.dates = {
                startDate: new Date(),
                endDate: new Date()
            };
            var idAgent;
            var x;
            var tmp = 0;

            $scope.openStartDateModal = function () {
                DataFactory.insert("salesbyagent/" + idAgent, {
                    startDate: $scope.SalesByAgents.startDate,
                    endDate: $scope.SalesByAgents.endDate, id: idAgent
                }, function (data) {
                    $scope.SalesByAgents = data;
                    $scope.SalesByAgents.startDate = new Date($scope.SalesByAgents.startDate);
                    $scope.SalesByAgents.endDate = new Date($scope.SalesByAgents.endDate);
                })
            };

            $scope.update = function () {
                ListSales();
            }

            var initializing = false;

            $scope.dates.startDate.setMonth($scope.dates.startDate.getMonth() - 12);
            ListSales();

            $scope.startDateOpened = false;
            $scope.endDateOpened = false;

            $scope.openStartDate = function () {
                $scope.startDateOpened = true;
            };

            $scope.openEndDate = function () {
                $scope.endDateOpened = true;
            };

            $scope.$watch(
                function (scope) { return scope.dates.startDate },
                function () {
                    if (initializing) {
                        x = $scope.selectedOption;
                        ListSales();
                    }
                }
            );
            $scope.$watch(
                function (scope) { return scope.dates.endDate },
                function () {
                    if (initializing) {
                        x = $scope.selectedOption;
                        ListSales();
                    }
                    else
                        initializing = true;
                }
            );

            $scope.show = function (salesbyagent) {

                DataFactory.insert("salesbyagent/" + salesbyagent.id, {
                    startDate: $scope.dates.startDate,
                    endDate: $scope.dates.endDate, id: salesbyagent.id
                }, function (data) {
                    idAgent = salesbyagent.id;
                    $scope.SalesByAgents = data;
                    $scope.SalesByAgents.startDate = new Date($scope.dates.startDate);
                    $scope.SalesByAgents.endDate = new Date($scope.dates.endDate);

                    var modalInstance = $uibModal.open({
                        animation: true,
                        ariaLabelledBy: 'modal-title',
                        ariaDescribedBy: 'modal-body',
                        templateUrl: 'app/components/reports/templates/salesbyregionshow.html',
                        controller: 'ModalInstanceController',
                        controllerAs: '$modal',
                        scope: $scope,
                        size: 'lg',
                        resolve: {
                            data: function () {
                                return data;
                            }

                        }
                    });

                    modalInstance.result.then(function () { }, function () { });
                });
            };

        }]);
})
