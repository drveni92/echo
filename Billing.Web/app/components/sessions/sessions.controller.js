(function() {
    angular
        .module("Billing")
        .controller('LoginController', ['$scope', '$rootScope', '$http', '$location', 'SessionService', 'localStorageService', 'ToasterService', function($scope, $rootScope, $http, $location, SessionService, localStorageService, ToasterService) {
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
                            $rootScope.currentUsername = credentials.currentUser.username;
                            $rootScope.currentUserId = credentials.currentUser.id;
                            redirectTo = (redirectTo == "/logout") ? BillingConfig.DefaultRoute : redirectTo;
                            $location.path(redirectTo);
                        }, function(reason) {
                            ToasterService.pop('error', "Error", "Username or password is incorrect");
                        });
                    }
                }, function(reason) {
                    ToasterService.pop('error', "Error", reason);
                });
            $( document ).ready(function() {

                $( ".content" ).hide().fadeIn( "slow" );
                $('#countdown').countdown('2017/05/19 14:00:00', function(event) {
                    $(this).html(event.strftime('%Ddays   %Hh %Mm %Ss'));
                });
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
                        $rootScope.currentUsername = credentials.currentUser.username;
                        $rootScope.currentUserId = credentials.currentUser.id;

                        if ($scope.user.remember) {
                            localStorageService.cookie.set("Billing", credentials.remember, BillingConfig.ExpirationDate);
                        }
                        redirectTo = (redirectTo == "/logout") ? BillingConfig.DefaultRoute : redirectTo;
                        $location.path(redirectTo);
                    },
                    function(reason) {
                        document.getElementById('username').style.border = "2px solid #f00";
                        document.getElementById('password').style.border = "2px solid #f00";
                        
                        ToasterService.pop('error', "Error", "Username or password is incorrect");
                        
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
