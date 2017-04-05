angular
    .module("Billing")
    .controller('CategoriesController', ['$scope', '$http', '$uibModal', 'DataFactory', function($scope, $http, $uibModal, DataFactory) {
        $scope.get = function(currentCategory) {
            $scope.category = currentCategory;
            $scope.showCategory = true;
        };

        $scope.save = function(){
            if($scope.category.id === 0)
                DataFactory.insert("categories", $scope.category, function(data){ ListCategories();} );
            else
                DataFactory.update("categories", $scope.category.id, $scope.category, function(data){ListCategories();});
        };
        $scope.delete = function(){

            DataFactory.delete("categories", $scope.category.id, function(data){ListCategories();});
            $scope.showCategory = false;

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
                            return null
                        }
                    }
                });
                modalInstance.result.then(function(category) {
                    DataFactory.insert("categories", category, function(data) { ListCategories(); });
                }, function() {
                    console.log('Modal dismissed at: ' + new Date());
                });


        };
    }]);