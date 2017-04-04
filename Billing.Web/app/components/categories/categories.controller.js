(function(){

    var app = angular.module("Billing");

    var CategoriesController = function($scope, $http, DataFactory) {

        $scope.showCategory = false;
        ListCategories();

        $scope.getCategory = function(currentCategory){
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

        $scope.new = function(){
            $scope.category = {
                id: 0,
                name: ""
            };
            $scope.showCategory = true;
        };

        function ListCategories(){
            DataFactory.list("categories", function(data){ $scope.categories = data});
        }
    };

    app.controller("CategoriesController", CategoriesController);

}());