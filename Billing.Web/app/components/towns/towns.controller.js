angular
    .module("Billing")
    .controller('TownsController', ['$scope', '$http', '$uibModal', 'DataFactory', function($scope, $http, $uibModal, DataFactory) {

        $scope.regions= REGIONS;

        function ListTowns(){
            DataFactory.list("towns", function(data){ $scope.towns = data});

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
                        return {  id: 0,
                            name: '',
                            zip: '',
                            region: null }
                    },
                    options: function() {

                        return []
                    }
                }
            });

            modalInstance.result.then(function(town) {
                DataFactory.insert("towns", town, function(data) { ListTowns(); });
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
                            return {  id: item.id,
                                name: item.name,
                                zip: item.zip,
                                region: item.region }
                        },
                        options: function() {
                            return []
                        }
                    }
                });

                modalInstance.result.then(function(town) {
                    DataFactory.update("towns", town.id, town, function (data) {
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
                DataFactory.delete("towns", town.id, function(data){
                    ListTowns();

                });
            }, function() {
                console.log('Modal dismissed at: ' + new Date());
            });

        }

}]);