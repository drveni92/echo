angular
    .module("Billing")
    .controller('CategoriesController', ['$scope', '$http', '$uibModal', 'DataFactory', 'ToasterService', function($scope, $http, $uibModal, DataFactory, ToasterService) {
        $scope.get = function(currentCategory) {
            $scope.category = currentCategory;
            $scope.showCategory = true;
        };

        $scope.maxPagination = BillingConfig.maxPagination

        function ListCategories(page) {
            DataFactory.list("categories?page=" + page, function(data) {
                $scope.categories = data.list;
                $scope.totalItems = data.totalItems;
                $scope.currentPage = data.currentPage + 1;
            });
        }

        $scope.pageChanged = function() {
            ListCategories($scope.currentPage - 1);
        };

        ListCategories(0);

        $scope.new = function() {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/categories/templates/new.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function () {
                        return {id: 0, name: ''}
                    },
                    options: function () {
                        return []
                    }
                }
            });
            modalInstance.result.then(function (category) {
                DataFactory.insert("categories", category, function (data) {
                    ToasterService.pop('success', "Success", "Category added");
                    ListCategories();
                });
            }, function () {
                console.log('Modal dismissed at: ' + new Date());
            });
        };

        $scope.edit = function(category) {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/categories/templates/edit.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function() {
                        return $.extend(true, {}, category)
                    },
                    options: function() {
                        return []
                    }
                }
            });

            modalInstance.result.then(function(category) {
                DataFactory.update("categories", category.id, category, function(data) {
                    ToasterService.pop('success', "Success", "Category saved");
                    ListCategories();
                });
            }, function() {
                ListCategories();
            });
        }

        $scope.delete = function(category) {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/categories/templates/delete.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function() {
                        return category
                    },
                    options: function() {
                        return []
                    }
                }
            });

            modalInstance.result.then(function(category) {
                DataFactory.delete("categories", category.id, function(data) {
                    ToasterService.pop('success', "Success", "Category deleted");
                    ListCategories();
                });
            }, function() {
                console.log('Modal dismissed at: ' + new Date());
            });
        }
    }]);