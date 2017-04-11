angular
    .module("Billing")
    .controller('TownsController', ['$scope', '$http', '$uibModal', 'DataFactory', 'ToasterService', function($scope, $http, $uibModal, DataFactory, ToasterService) {

        $scope.regions = REGIONS;
        ListTowns();

        $scope.getTown = function(currentTown) {
            $scope.town = currentTown;
        };

        $scope.delete = function() {
            DataFactory.delete("towns", $scope.town.id, function(data) {
                ToasterService.pop('success', "Success", "Town deleted");
                ListTowns();
            });
        };

        function ListTowns() {
            DataFactory.list("towns", function(data) { $scope.towns = data });

        };


        $scope.new = function() {
            DataFactory.list("towns", function(data) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/components/towns/templates/new.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    resolve: {
                        data: function() {
                            return {
                                id: 0,
                                name: '',
                                zip: '',
                                region: 1
                            }
                        },
                        options: function() {

                            return { regions: REGIONS }
                        }
                    }
                });

                modalInstance.result.then(function(town) {
                    DataFactory.insert("towns", town, function(data) {
                        ToasterService.pop('success', "Success", "Town added");
                        ListTowns();
                    });
                }, function() {
                    console.log('Modal dismissed at: ' + new Date());
                });
            });
        };

        $scope.edit = function(item) {
            DataFactory.list("towns", function(data) {

                var modalInstance = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/components/towns/templates/edit.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    resolve: {
                        data: function() {
                            return {
                                id: item.id,
                                name: item.name,
                                zip: item.zip,
                                region: item.region
                            }
                        },
                        options: function() {
                            return { regions: REGIONS }
                        }
                    }
                });

                modalInstance.result.then(function(town) {
                    DataFactory.update("towns", town.id, town, function(data) {
                        ToasterService.pop('success', "Success", "Town saved");
                        ListTowns();
                    });
                }, function() {
                    console.log('Modal dismissed at: ' + new Date());
                });
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
                    },
                    options: function() {
                        return null
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

        };

    }]);
