(function() {

    var app = angular.module('bootDemo', ['ui.bootstrap']);

    CollapseCtrl = function($scope) {

        $scope.isCollapsed = false;

    };

    app.controller("CollapseCtrl", CollapseCtrl);

}());


