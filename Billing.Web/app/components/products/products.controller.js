(function () {
    angular
        .module("Billing")
        .controller('ProductsController', ['$scope', '$http', '$uibModal', 'DataFactory', 'ToasterService', 'ProductsFactory', function ($scope, $http, $uibModal, DataFactory, ToasterService, ProductsFactory) {

            $scope.maxPagination = BillingConfig.maxPagination;

            $scope.pageParams = {
                page: 1,
                showPerPage: BillingConfig.showPerPage,
                sortType: 'name',
                sortReverse: false,
                totalItems: 0
            };

            function ListProducts() {
                $scope.pageParams.page = $scope.pageParams.page - 1;

                DataFactory.list("products", function (data) {
                    $scope.products = data.list;
                    $scope.pageParams.totalItems = data.totalItems;
                    $scope.pageParams.page = data.currentPage + 1;
                }, $scope.pageParams);
            }



            $scope.sort = function (column) {
                if ($scope.pageParams.sortType === column) $scope.pageParams.sortReverse = !$scope.pageParams.sortReverse;
                $scope.pageParams.sortType = column;
                ListProducts();
            };

            $scope.search = function () {
                if ($scope.pageParams.name.toString().length > 2 || $scope.pageParams.name.toString().length === 0) ListProducts();
            };

            $scope.showItems = function () {
                ListProducts();
            };

            $scope.pageChanged = function () {
                ListProducts();
            };

            ListProducts();

            $scope.new = function () {
                var modalInstance = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/view/products/new.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    resolve: {
                        data: function () {
                            return ProductsFactory.empty();
                        }
                    }
                });

                modalInstance.result.then(function (product) {
                    DataFactory.insert("products", ProductsFactory.product(product), function (data) {
                        ToasterService.pop('success', "Success", "Product added");
                        ListProducts();
                    });
                }, function () {
                });

            };

            $scope.show = function (product) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/view/products/show.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    resolve: {
                        data: function () {
                            return product;
                        }
                    }
                });

                modalInstance.result.then(function () { }, function () { });

            };

            $scope.edit = function (product) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/view/products/edit.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    resolve: {
                        data: function () {
                            return $.extend(true, {}, product);
                        }
                    }
                });
                modalInstance.result.then(function (product) {
                    DataFactory.update("products", product.id, ProductsFactory.product(product), function (data) {
                        ToasterService.pop('success', "Success", "Product saved");
                        ListProducts();
                    });
                }, function () {
                    ListProducts();
                });

            };

            $scope.delete = function (product) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/view/products/delete.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    resolve: {
                        data: function () {
                            return product;
                        }
                    }
                });

                modalInstance.result.then(function (product) {
                    DataFactory.delete("products", product.id, function (data) {
                        ToasterService.pop('success', "Success", "Product deleted");
                        ListProducts();
                    });
                }, function () {
                });
            };

        }]);
}());