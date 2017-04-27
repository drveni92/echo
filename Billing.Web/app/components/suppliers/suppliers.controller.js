angular
    .module("Billing")
    .controller('SuppliersController', ['$scope', '$http', '$uibModal', 'DataFactory', 'ToasterService', 'SuppliersFactory', function($scope, $http, $uibModal, DataFactory, ToasterService, SuppliersFactory) {

        $scope.maxPagination = BillingConfig.maxPagination;

        $scope.pageParams = {
            page: 1,
            showPerPage: BillingConfig.showPerPage,
            sortType: 'name',
            sortReverse: false,
            totalItems: 0
        };

        function ListSuppliers() {
            $scope.pageParams.page = $scope.pageParams.page - 1;

            DataFactory.list("suppliers", function (data) {
                $scope.suppliers = data.list;
                $scope.pageParams.totalItems = data.totalItems;
                $scope.pageParams.page = data.currentPage + 1;
            }, $scope.pageParams);
        };



        $scope.sort = function(column) {
            if($scope.pageParams.sortType === column) $scope.pageParams.sortReverse = !$scope.pageParams.sortReverse;
            $scope.pageParams.sortType = column;
            ListSuppliers();
        };

        $scope.search = function () {
            if ($scope.pageParams.name.toString().length > 2 || $scope.pageParams.name.toString().length == 0) ListSuppliers();
        };

        $scope.showItems = function () {
            ListSuppliers();
        };

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
