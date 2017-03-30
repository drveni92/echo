(function() {

    var app = angular.module('bootDemo', ['ui.bootstrap']);

    TimePickerCtrl = function($scope, $log) {
        $scope.mytime = new Date();

        $scope.hstep = 1;
        $scope.mstep = 15;

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };

        $scope.ismeridian = true;
        $scope.toggleMode = function() {
            $scope.ismeridian = ! $scope.ismeridian;
        };

        $scope.update = function() {
            var d = new Date();
            d.setHours( 14 );
            d.setMinutes( 0 );
            $scope.mytime = d;
        };

        $scope.changed = function () {
            if ($scope.mytime.getHours() < 9) $scope.mytime.setHours(9);
            if ($scope.mytime.getHours() > 17) $scope.mytime.setHours(17);
            $log.log('Time changed to: ' + $scope.mytime);
        };

        $scope.clear = function() {
            $scope.mytime = null;
        };
    };

    app.controller("TimePickerCtrl", TimePickerCtrl);

}());



