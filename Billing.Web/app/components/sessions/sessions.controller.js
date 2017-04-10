(function() {
    angular
        .module("Billing")
        .controller('LoginController', ['$scope', '$rootScope', '$http', '$location', 'SessionService', 'localStorageService', function($scope, $rootScope, $http, $location, SessionService, localStorageService) {
            $http.get("config.json")
                .then(function(response) {
                    BillingConfig = response.data;
                    var rememberMeToken = localStorageService.cookie.get("Billing");
                    if (rememberMeToken != null) {
                        $http({
                            method: "post",
                            url: BillingConfig.source + "remember",
                            data: {
                                "apiKey": BillingConfig.apiKey,
                                "signature": BillingConfig.signature,
                                "remember": rememberMeToken
                            }
                        }).then(function(response) {
                            credentials = response.data;
                            localStorageService.cookie.set("Billing", credentials.remember, BillingConfig.ExpirationDate);
                            $rootScope.currentUser = credentials.currentUser.name;
                            redirectTo = (redirectTo == "/logout") ? BillingConfig.DefaultRoute : redirectTo;
                            $location.path(redirectTo);
                        }, function(reason) {
                            console.log(reason);
                        });
                    }
                }, function(reason) {
                    console.log(reason);
                });

            $scope.login = function() {

                $http.defaults.headers.common.Authorization = "Basic " + SessionService.encode($scope.user.name + ":" + $scope.user.pass);
                var promise = $http({
                    method: "post",
                    url: BillingConfig.source + "login",
                    data: {
                        "apiKey": BillingConfig.apiKey,
                        "signature": BillingConfig.signature,
                        "remember": $scope.user.remember
                    }
                });
                promise.then(
                    function(response) {
                        credentials = response.data;
                        $rootScope.currentUser = credentials.currentUser.name;
                        if ($scope.user.remember) {
                            localStorageService.cookie.set("Billing", credentials.remember, BillingConfig.ExpirationDate);
                        }
                        redirectTo = (redirectTo == "/logout") ? BillingConfig.DefaultRoute : redirectTo;
                        $location.path(redirectTo);
                    },
                    function(reason) {
                        $scope.loginError = "Username or password is incorrect";
                        credentials = null;
                    });
            };
        }])
        .controller('LogoutController', ['$http', 'localStorageService', function($http, localStorageService) {
            $http({
                method: "get",
                url: BillingConfig.source + "logout",
                async: false
            }).then(function(response) {
                    localStorageService.cookie.clearAll("Billing");
                    credentials = null;
                    window.location.reload();
                    return true;
                },
                function(reason) {
                    console.log(reason);
                    return false;
                });

        }]);
}());