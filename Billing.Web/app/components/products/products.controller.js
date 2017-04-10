angular
    .module("Billing")
    .controller('ProductsController', ['$scope', '$http', '$uibModal', 'DataFactory', function($scope, $http, $uibModal, DataFactory) {

        function ListProducts() {
            DataFactory.list("products", function(data) { $scope.products = data });
        }

        ListProducts();

        $scope.new = function() {

            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/products/templates/new.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function() {
                        return {
                            id: 0,
                            name: '',
                            unit: '',
                            price: null,
                            category: { id: null }
                        }
                    },
                    options: function() {
                        return ["categories"]
                    }
                }
            });

            modalInstance.result.then(function(product) {
                console.log(product);
                DataFactory.insert("products", product, function(data) { ListProducts(); });
            }, function() {
                console.log('Modal dismissed at: ' + new Date());
            });

        };

        $scope.show = function(product) {

            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/products/templates/show.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function() {
                        return product
                    },
                    options: function() {
                        return []
                    }
                }
            });

            modalInstance.result.then(function() {
            }, function() {
            });

        };

        $scope.edit = function(product) {

            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/products/templates/edit.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function() {
                        return $.extend(true, {}, product)
                    },
                    options: function() {
                        return ["categories"]
                    }
                }
            });

            modalInstance.result.then(function(product) {
                DataFactory.update("products", product.id, product, function(data) {
                    ListProducts();
                });
            }, function() {
                ListProducts();
            });

        };

        $scope.delete = function(product) {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/products/templates/delete.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function() {
                        return product
                    },
                    options: function() {
                        return []
                    }
                }
            });

            modalInstance.result.then(function(product) {
                DataFactory.delete("products", product.id, function(data) {
                    ListProducts();
                    //message success missing
                });
            }, function() {
                console.log('Modal dismissed at: ' + new Date());
            });
        };

    }]);