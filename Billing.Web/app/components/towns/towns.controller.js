angular
    .module("Billing")
    .controller('TownsController', ['$scope', '$http', '$uibModal', 'DataFactory', 'ToasterService', 'TownsFactory', function($scope, $http, $uibModal, DataFactory, ToasterService, TownsFactory) {

        $scope.regions = BillingConfig.regions;

        $scope.getTown = function(currentTown) {
            $scope.town = currentTown;
        };

        $scope.maxPagination = BillingConfig.maxPagination

        $scope.pageParams = {
            page: 1,
            showPerPage: BillingConfig.showPerPage,
            sortType: 'name',
            sortReverse: false,
            totalItems: 0
        };

        function ListTowns() {
            $scope.pageParams.page = $scope.pageParams.page - 1;

            DataFactory.list("towns", function (data) {
                $scope.towns = data.list;
                $scope.pageParams.totalItems = data.totalItems;
                $scope.pageParams.page = data.currentPage + 1;
            }, $scope.pageParams);
        };



        $scope.sort = function(column) {
            if($scope.pageParams.sortType === column) $scope.pageParams.sortReverse = !$scope.pageParams.sortReverse;
            $scope.pageParams.sortType = column;
            ListTowns();
        };

        $scope.search = function () {
            if ($scope.pageParams.name.toString().length > 2 || $scope.pageParams.name.toString().length == 0) ListTowns();
        };

        $scope.showItems = function () {
            ListTowns();
        };

        $scope.pageChanged = function() {
            ListTowns();
        };

        ListTowns();

        $scope.new = function() {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/towns/templates/new.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function() {
                        return TownsFactory.empty();
                    }
                }
            });
            modalInstance.result.then(function(town) {
                DataFactory.insert("towns", TownsFactory.town(town), function(data) {
                    ToasterService.pop('success', "Success", "Town added");
                    ListTowns();
                });
            }, function() {
                console.log('Modal dismissed at: ' + new Date());
            });
        };

        $scope.edit = function(item) {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/towns/templates/edit.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function() {
                        return TownsFactory.town(item)
                    }
                }
            });

            modalInstance.result.then(function(town) {
                DataFactory.update("towns", town.id, TownsFactory.town(town), function(data) {
                    ToasterService.pop('success', "Success", "Town saved");
                    ListTowns();
                });
            }, function() {
                console.log('Modal dismissed at: ' + new Date());
            });
        };

        $scope.delete = function(town) {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/towns/templates/delete.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function() {
                        return town
                    }
                }
            });

            modalInstance.result.then(function(town) {
                DataFactory.delete("towns", town.id, function(data) {
                    ToasterService.pop('success', "Success", "Town deleted");
                    ListTowns();
                });
            }, function() {
                console.log('Modal dismissed at: ' + new Date());
            });

        }

    }]);
