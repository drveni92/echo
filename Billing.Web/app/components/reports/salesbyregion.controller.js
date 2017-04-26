angular
    .module("Billing")
    .controller('SalesByRegionController', ['$scope', '$http', '$uibModal', 'DataFactory','$filter', function($scope, $http, $uibModal, DataFactory,$filter ) {

        function ListSales() {
            DataFactory.insert("salesbyregion", { startDate : $scope.dates.startDate,
                endDate : $scope.dates.endDate }, function(data) {
                $scope.salesbyregion = data;
            }

        )};
        $scope.dates = {
            startDate: new Date(),
            endDate: new Date()
        };
        var idAgent;


        $scope.openStartDateModal = function() {
            DataFactory.insert("salesbyagent/"+idAgent, { startDate : $scope.SalesByAgents.startDate,
                endDate :  $scope.SalesByAgents.endDate , id : idAgent}, function(data) {
                $scope.SalesByAgents=data;
                $scope.SalesByAgents.startDate =  new Date ($scope.SalesByAgents.startDate);
                $scope.SalesByAgents.endDate = new Date ($scope.SalesByAgents.endDate);
            })
        };


        var initializing = false;

        $scope.dates.startDate.setMonth($scope.dates.startDate.getMonth() - 12);
        ListSales();

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
                     ListSales();
            }
        );
        $scope.$watch(
            function(scope) { return scope.dates.endDate },
            function() {
                if (initializing)
                     ListSales();
                else
                    initializing = true;
            }
        );

        $scope.show = function(salesbyagent) {

            DataFactory.insert("salesbyagent/"+salesbyagent.id, { startDate : $scope.dates.startDate,
                endDate : $scope.dates.endDate , id : salesbyagent.id}, function(data) {
                idAgent=salesbyagent.id;
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
                    data: function() {
                        return data;
                    }

                }
            });

            modalInstance.result.then(function() {}, function() {});
            });
        };

    }]);
