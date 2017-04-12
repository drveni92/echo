angular
    .module("Billing")
    .controller('CategoriesController', ['$scope', '$http', '$uibModal', 'DataFactory', function($scope, $http, $uibModal, DataFactory) {
        $scope.get = function(currentCategory) {
            $scope.category = currentCategory;
            $scope.showCategory = true;
        };

        function ListCategories() {
            DataFactory.list("categories", function(data) { $scope.categories = data });
        }

        ListCategories();

        $scope.new = function() {

                var modalInstance = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/components/categories/templates/new.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    resolve: {
                        data: function() {
                            return { id: 0, name: '' }
                        },
                        options: function() {
                            return []
                        }
                    }
                });
                modalInstance.result.then(function(category) {
                    DataFactory.insert("categories", category, function(data) { ListCategories(); });
                }, function() {
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
                            return category
                        },
                        options: function() {
                            return []
                        }
                    }
                });

                modalInstance.result.then(function(category) {
                    DataFactory.update("categories", category.id, category, function(data) {
                        //message success missing
                    });
                }, function() {
                    console.log('Modal dismissed at: ' + new Date());
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
                    ListCategories();
                    //message success missing
                });
            }, function() {
                console.log('Modal dismissed at: ' + new Date());
            });
        }
    }]);