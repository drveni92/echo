(function () {
    angular
        .module("Billing")
        .controller('LoginController', ['$scope', '$rootScope', '$http', '$location', 'SessionService', 'localStorageService', 'ToasterService', '$timeout', function ($scope, $rootScope, $http, $location, SessionService, localStorageService, ToasterService, $timeout) {
            $http.get("config.json")
                .then(function (response) {
                    BillingConfig = response.data;
                    $rootScope.invoicesCount = 0;
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
                        }).then(function (response) {
                            credentials = response.data;
                            localStorageService.cookie.set("Billing", credentials.remember, BillingConfig.ExpirationDate);
                            $rootScope.currentUser = credentials.currentUser.name;
                            $rootScope.currentUsername = credentials.currentUser.username;
                            $rootScope.currentUserId = credentials.currentUser.id;
                            redirectTo = (redirectTo == "/logout") ? BillingConfig.DefaultRoute : redirectTo;
                            $location.path(redirectTo);

                            getCountInvoices();

                        }, function (reason) {
                            ToasterService.pop('error', "Error", "Username or password is incorrect");
                        });
                    }
                }, function (reason) {
                    ToasterService.pop('error', "Error", reason);
                });
            $(document).ready(function () {

                $(".content").hide().fadeIn("slow");
                $('#countdown').countdown('2017/05/19 14:00:00', function (event) {
                    $(this).html(event.strftime('%Ddays   %Hh %Mm %Ss'));
                });
            });
            $scope.login = function () {
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
                    function (response) {
                        credentials = response.data;
                        $rootScope.currentUser = credentials.currentUser.name;
                        $rootScope.currentUsername = credentials.currentUser.username;
                        $rootScope.currentUserId = credentials.currentUser.id;

                        if ($scope.user.remember) {
                            localStorageService.cookie.set("Billing", credentials.remember, BillingConfig.ExpirationDate);
                        }
                        redirectTo = (redirectTo == "/logout") ? BillingConfig.DefaultRoute : redirectTo;
                        getCountInvoices();
                        $location.path(redirectTo);
                    },
                    function (reason) {
                        document.getElementById('username').style.border = "2px solid #f00";
                        document.getElementById('password').style.border = "2px solid #f00";

                        ToasterService.pop('error', "Error", "Username or password is incorrect");

                        credentials = null;
                    });
            };


            function getCountInvoices() {
                $http.defaults.headers.common.Token = credentials.token;
                $http.defaults.headers.common.ApiKey = BillingConfig.apiKey;

                $http.get(BillingConfig.source + "invoices/automatic/nocheck").then(
                    function (response) {
                        $rootScope.invoicesCount = response.data;
                        $timeout(getCountInvoices, 10000);
                    },
                    function (reason) {
                    }
                );
            };

            $rootScope.getClass = function (path) {
                return ($location.path() === path) ? 'active-menu' : '';
            };
            
        }])
        .controller('LogoutController', ['$http', 'localStorageService', function ($http, localStorageService) {
            $http({
                method: "get",
                url: BillingConfig.source + "logout",
                async: false
            }).then(function (response) {
                localStorageService.cookie.clearAll("Billing");
                credentials = null;
                window.location.reload();
                return true;
            },
                function (reason) {
                    return false;
                });

        }]);
}());
