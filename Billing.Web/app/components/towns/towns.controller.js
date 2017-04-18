angular
    .module("Billing")
    .controller('TownsController', ['$scope', '$http', '$uibModal', 'DataFactory', 'ToasterService', 'TownsFactory', function($scope, $http, $uibModal, DataFactory, ToasterService, TownsFactory) {

        $scope.regions = BillingConfig.regions;

        $scope.getTown = function(currentTown) {
            $scope.town = currentTown;
        };

        $scope.maxPagination = BillingConfig.maxPagination

        function ListTowns() {
            DataFactory.list("towns?page=" + ($scope.currentPage - 1), function(data) {
                $scope.towns = data.list;
                $scope.totalItems = data.totalItems;
                $scope.currentPage = data.currentPage + 1;
            });
        }

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
