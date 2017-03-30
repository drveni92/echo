(function() {

    var app = angular.module('bootDemo', ['ui.bootstrap']);

    AlertCtrl = function($scope) {

        $scope.alerts = [
            {type: 'danger', msg: 'Your time evidence is rejected!'},
            {type: 'success', msg: 'Your time evidence is approved :-)'}
        ];

        $scope.addAlert = function () {
            $scope.alerts.push({msg: 'This is still on pending...'});
        };

        $scope.closeAlert = function (index) {
            $scope.alerts.splice(index, 1);

        };
    };

    app.controller("AlertCtrl", AlertCtrl);

}());


