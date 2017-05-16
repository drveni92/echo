(function () {
    angular
        .module("Billing")
        .controller('CategoriesController', ['$scope', '$http', '$uibModal', 'DataFactory', 'ToasterService', 'CategoriesFactory', function ($scope, $http, $uibModal, DataFactory, ToasterService, CategoriesFactory) {
            $scope.get = function (currentCategory) {
                $scope.category = currentCategory;
                $scope.showCategory = true;
            };

            $scope.maxPagination = BillingConfig.maxPagination;

            $scope.pageParams = {
                page: 1,
                showPerPage: BillingConfig.showPerPage,
                sortType: 'name',
                sortReverse: false,
                totalItems: 0
            };

            function ListCategories() {
                $scope.pageParams.page = $scope.pageParams.page - 1;

                DataFactory.list("categories", function (data) {
                    $scope.categories = data.list;
                    $scope.pageParams.totalItems = data.totalItems;
                    $scope.pageParams.page = data.currentPage + 1;
                }, $scope.pageParams);
            }



            $scope.sort = function (column) {
                if ($scope.pageParams.sortType === column) $scope.pageParams.sortReverse = !$scope.pageParams.sortReverse;
                $scope.pageParams.sortType = column;
                ListCategories();
            };

            $scope.search = function () {
                if ($scope.pageParams.name.toString().length > 2 || $scope.pageParams.name.toString().length === 0) ListCategories();
            };

            $scope.showItems = function () {
                ListCategories();
            };

            $scope.pageChanged = function () {
                ListCategories();
            };

            ListCategories();

            $scope.new = function () {
                var modalInstance = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/view/categories/new.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    resolve: {
                        data: function () {
                            return CategoriesFactory.empty();
                        }
                    }
                });
                modalInstance.result.then(function (category) {
                    DataFactory.insert("categories", CategoriesFactory.category(category), function (data) {
                        ToasterService.pop('success', "Success", "Category added");
                        ListCategories();
                    });
                }, function () {
                });
            };

            $scope.edit = function (category) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/view/categories/edit.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    resolve: {
                        data: function () {
                            return $.extend(true, {}, category);
                        }
                    }
                });

                modalInstance.result.then(function (category) {
                    DataFactory.update("categories", category.id, CategoriesFactory.category(category), function (data) {
                        ToasterService.pop('success', "Success", "Category saved");
                        ListCategories();
                    });
                }, function () {
                    ListCategories();
                });
            };

            $scope.delete = function (category) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/view/categories/delete.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    resolve: {
                        data: function () {
                            return category;
                        }
                    }
                });

                modalInstance.result.then(function (category) {
                    DataFactory.delete("categories", category.id, function (data) {
                        ToasterService.pop('success', "Success", "Category deleted");
                        ListCategories();
                    });
                }, function () {
                });
            };
        }]);

}());