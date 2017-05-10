(function() {
    angular
        .module("Billing")
        .directive('toasterContainer', ['$compile', '$timeout', 'ToasterService',
            function($compile, $timeout, ToasterService) {
                return {
                    replace: true,
                    restrict: 'EA',
                    link: function(scope, elm, attrs) {
                        var id = 0;

                        var types = {
                            error: 'toast-error',
                            info: 'toast-info',
                            success: 'toast-success',
                            warning: 'toast-warning'
                        };

                        scope.config = {
                            position: 'toast-bottom-right',
                            title: 'toast-title',
                            message: 'toast-message',
                            tap: true
                        };

                        function addToast(toast) {
                            toast.type = types[toast.type];
                            if (!toast.type)
                                toast.type = 'toast-info';

                            id++;
                            angular.extend(toast, { id: id });
                            setTimeout(toast, 5000);
                            scope.toasters.unshift(toast);
                        }

                        function setTimeout(toast, time) {
                            toast.timeout = $timeout(function() {
                                scope.removeToast(toast.id);
                            }, time);
                        }

                        scope.toasters = [];
                        scope.$on('toaster-newToast', function() {
                            addToast(ToasterService.toast);
                        });
                    },
                    controller: function($scope, $element, $attrs) {

                        $scope.stopTimer = function(toast) {
                            if (toast.timeout)
                                $timeout.cancel(toast.timeout);
                        };

                        $scope.continueTimer = function(toast) {
                            toast.timeout = $timeout(function() {
                                $scope.removeToast(toast.id);
                            }, 5000);
                        };

                        $scope.removeToast = function(id) {
                            var i = 0;
                            for (i; i < $scope.toasters.length; i++) {
                                if ($scope.toasters[i].id === id)
                                    break;
                            }
                            $scope.toasters.splice(i, 1);
                        };

                        $scope.remove = function(id) {
                            if ($scope.config.tap === true) {
                                $scope.removeToast(id);
                            }
                        };
                    },
                    templateUrl: 'app/directives/toaster/toaster.html'
                };
            }
        ]);




}());
