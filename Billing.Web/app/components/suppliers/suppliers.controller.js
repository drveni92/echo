angular
    .module("Billing")
    .controller('SuppliersController', ['$scope', '$http', '$uibModal', 'DataFactory', 'ToasterService', 'SuppliersFactory', function($scope, $http, $uibModal, DataFactory, ToasterService, SuppliersFactory) {

        $scope.maxPagination = BillingConfig.maxPagination;

        function ListSuppliers() {
            DataFactory.list("suppliers?page=" + ($scope.currentPage - 1), function(data) {
                $scope.totalItems = data.totalItems;
                $scope.currentPage = data.currentPage + 1;
                $scope.suppliers = data.list;
            });

        }

        $scope.pageChanged = function() {
            ListSuppliers();
        };


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
                    data: function() {
                        return SuppliersFactory.empty();
                    }
                }
            });
            modalInstance.result.then(function(supplier) {
                DataFactory.insert("suppliers", SuppliersFactory.supplier(supplier), function(data) {
                    ToasterService.pop('success', "Success", "Supplier added");
                    ListSuppliers();
                });
            }, function() {
                console.log('Modal dismissed at: ' + new Date());
            });
        };

        $scope.show = function(supplier) {

            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/suppliers/templates/show.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function() {
                        return supplier
                    }
                }
            });

            modalInstance.result.then(function() {}, function() {});

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
                    }
                }
            });

            modalInstance.result.then(function(supplier) {
                DataFactory.update("suppliers", supplier.id, SuppliersFactory.supplier(supplier), function(data) {
                    ToasterService.pop('success', "Success", "Supplier saved");
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
                    }
                }
            });

            modalInstance.result.then(function(supplier) {
                DataFactory.delete("suppliers", supplier.id, function(data) {
                    ToasterService.pop('success', "Success", "Supplier deleted");
                    ListSuppliers();
                });
            }, function() {
                console.log('Modal dismissed at: ' + new Date());
            });
        }

    }]);
