angular
    .module("Billing")
    .controller('ShippersController', ['$scope', '$http', '$uibModal', 'DataFactory', function($scope, $http, $uibModal, DataFactory) {

        function ListShippers() {
            DataFactory.list("shippers", function(data) { $scope.shippers = data });
        }

        ListShippers();

        $scope.new = function() {
            DataFactory.list("towns", function(data) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/components/shippers/templates/new.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    resolve: {
                        data: function() {
                            return { id: 0, name: '', address: '', town: { id: null } }
                        },
                        options: function() {
                            return { towns: data }
                        }
                    }
                });

                modalInstance.result.then(function(shipper) {
                    DataFactory.insert("shippers", shipper, function(data) { ListShippers(); });
                }, function() {
                    console.log('Modal dismissed at: ' + new Date());
                });
            });
        };

        $scope.edit = function(shipper) {
            DataFactory.list("towns", function(data) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/components/shippers/templates/edit.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    resolve: {
                        data: function() {
                            return shipper
                        },
                        options: function() {
                            return { towns: data }
                        }
                    }
                });

                modalInstance.result.then(function(shipper) {
                    DataFactory.update("shippers", shipper.id, shipper, function(data) {
                        ListShippers();
                        //message success missing
                    });
                }, function() {
                    console.log('Modal dismissed at: ' + new Date());
                });
            });
        }


        $scope.delete = function(shipper) {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/shippers/templates/delete.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function() {
                        return shipper
                    },
                    options: function() {
                        return null
                    }
                }
            });

            modalInstance.result.then(function(shipper) {
                DataFactory.delete("shippers", shipper.id, function(data) {
                    ListShippers();
                    //message success missing
                });
            }, function() {
                console.log('Modal dismissed at: ' + new Date());
            });
        }

    }]);