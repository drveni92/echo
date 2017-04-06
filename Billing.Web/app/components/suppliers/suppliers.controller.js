angular
    .module("Billing")
    .controller('SuppliersController', ['$scope', '$http', '$uibModal', 'DataFactory', function($scope, $http, $uibModal, DataFactory) {

        function ListSuppliers() {
            DataFactory.list("suppliers", function(data) { $scope.suppliers = data });
        }

        ListSuppliers();

        $scope.new = function() {
                var modalInstance = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/components/suppliers/templates/new.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    resolve: {
                        data: function () {
                            return {id: 0, name: '', address: '', town: {id: null}}
                        },
                        options: function () {
                            return ["towns"]
                        }
                    }
                });
                modalInstance.result.then(function (supplier) {
                    DataFactory.insert("suppliers", supplier, function (data) {
                        ListSuppliers();
                    });
                }, function () {
                    console.log('Modal dismissed at: ' + new Date());
                });
        };

            $scope.edit = function(supplier) {
                    var modalInstance = $uibModal.open({
                        animation: true,
                        ariaLabelledBy: 'modal-title',
                        ariaDescribedBy: 'modal-body',
                        templateUrl: 'app/components/suppliers/templates/edit.html',
                        controller: 'ModalInstanceController',
                        controllerAs: '$modal',
                        resolve: {
                            data: function() {
                                return $.extend(true, {}, supplier)
                            },
                            options: function() {
                                return ["towns"]
                            }
                        }
                    });

                    modalInstance.result.then(function(supplier) {
                        DataFactory.update("suppliers", supplier.id, supplier, function(data) {
                            ListSuppliers();
                        });
                    }, function() {
                        ListSuppliers();
                    });
            }

        $scope.delete = function(supplier) {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/suppliers/templates/delete.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function() {
                        return supplier
                    },
                    options: function() {
                        return []
                    }
                }
            });

            modalInstance.result.then(function(supplier) {
                DataFactory.delete("suppliers", supplier.id, function(data) {
                    ListSuppliers();
                });
            }, function() {
                console.log('Modal dismissed at: ' + new Date());
            });
        }

}]);