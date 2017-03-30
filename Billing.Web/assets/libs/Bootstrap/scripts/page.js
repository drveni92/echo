(function() {

    var app = angular.module('bootDemo', ['ui.bootstrap']);

    PageCtrl = function($scope) {

        $scope.totalItems = 64;
        $scope.currentPage = 4;

        $scope.setPage = function (pageNo) {
            $scope.currentPage = pageNo;
        };

        $scope.pageChanged = function() {
            $log.log('Page changed to: ' + $scope.currentPage);
        };

        $scope.maxSize = 5;
        $scope.bigTotalItems = 175;
        $scope.bigCurrentPage = 1;

    };

    app.controller("PageCtrl", PageCtrl);

}());


