angular
    .module("Billing")
    .controller('TownsController', ['$scope', '$http', '$uibModal', 'DataFactory', 'ToasterService', function($scope, $http, $uibModal, DataFactory, ToasterService) {

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
                                region: null
                            }
                        },
                        options: function() {

                            return []

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
                            return []
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
                        return []
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
