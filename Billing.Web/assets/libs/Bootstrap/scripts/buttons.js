(function() {

    var app = angular.module('bootDemo', ['ui.bootstrap']);

    ButtonsCtrl = function($scope) {

        $scope.singleModel = 1;

        $scope.radioModel = 'Middle';

        $scope.checkModel = {
            left: false,
            middle: true,
            right: false
        };
    };

    app.controller("ButtonsCtrl", ButtonsCtrl);

}());


