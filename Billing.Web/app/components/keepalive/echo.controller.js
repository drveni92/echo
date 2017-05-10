(function () {
    angular.module("Billing")
        .controller("EchoController", ['$scope', 'Idle', '$uibModal', '$location', function ($scope, Idle, $uibModal, $location) {

            Idle.watch();
            $scope.started = true;

            function closeModals() {
                if ($scope.warning) {
                    $scope.warning.close();
                    $scope.warning = null;
                }
            }

            $scope.$on('IdleStart', function () {
                if ($location.path() !== "/login") {
                    closeModals();
                    $scope.warning = $uibModal.open({
                        templateUrl: 'app/components/keepalive/warning.html',
                        windowClass: 'modal-danger',
                        countdownBar: true,
                        animation: true
                    });
                }
            });

            $scope.$on('IdleEnd', function () {
                if ($location.path() !== "/login") {
                    closeModals();
                }
            });

            $scope.$on('IdleTimeout', function () {
                if ($location.path() !== "/login") {
                    closeModals();
                    window.location.reload();
                }
            });

        }])

        .config(['IdleProvider', 'KeepaliveProvider', function (IdleProvider, KeepaliveProvider) {
            IdleProvider.idle(60*60);
            IdleProvider.timeout(20);
            KeepaliveProvider.interval(20);
        }]);

}());