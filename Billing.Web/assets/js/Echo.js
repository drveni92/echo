(function(){
    angular.module("Billing")
    .controller("Echo", ['$scope', 'Idle', '$uibModal', function ($scope, Idle, $uibModal) {

        Idle.watch();
        $scope.started = true;

        function closeModals() {
            if ($scope.warning) {
                $scope.warning.close();
                $scope.warning = null;
            }
        };

        $scope.$on('IdleStart', function () {
            closeModals();
            $scope.warning = $uibModal.open({
                templateUrl: 'app/components/keepalive/warning.html',
                windowClass: 'modal-danger',
                countdownBar: true,
                animation: true,


            });
        });

        $scope.$on('IdleEnd', function () {
            closeModals();
        });

        $scope.$on('IdleTimeout', function () {
            closeModals();
            window.location.reload();
        });

    }])

        .config(['IdleProvider', 'KeepaliveProvider', function (IdleProvider, KeepaliveProvider) {
            IdleProvider.idle(200);
            IdleProvider.timeout(20);
            KeepaliveProvider.interval(20);
    }]);

}());