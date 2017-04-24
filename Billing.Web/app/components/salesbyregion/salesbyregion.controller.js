angular
    .module("Billing")
    .controller('SalesByRegionController', ['$scope','$rootScope', '$http', '$uibModal', 'DataFactory','$filter', function($scope, $rootScope, $http, $uibModal, DataFactory,$filter ) {

        function ListSales() {
            DataFactory.insert("salesbyregion", { startDate : $scope.dates.startDate,
                endDate : $scope.dates.endDate }, function(data) {
                $scope.salesbyregion = data;}

        )};
        $scope.dates = {
            startDate: new Date(),
            endDate: new Date()
        };
        var idAgent;


        $rootScope.openStartDateModal = function() {
            DataFactory.insert("salesbyagent/"+idAgent, { startDate : $rootScope.SalesByAgents.startDate,
                endDate :  $rootScope.SalesByAgents.endDate , id : idAgent}, function(data) {
                $rootScope.SalesByAgents=data;
                $rootScope.SalesByAgents.startDate =  new Date ($rootScope.SalesByAgents.startDate);
                $rootScope.SalesByAgents.endDate = new Date ($rootScope.SalesByAgents.endDate);
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
                $rootScope.SalesByAgents = data;
                $rootScope.SalesByAgents.startDate = new Date($scope.dates.startDate);
                $rootScope.SalesByAgents.endDate = new Date($scope.dates.endDate);

                var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/salesbyregion/templates/show.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
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
