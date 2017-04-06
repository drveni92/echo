(function() {

    var app = angular.module("Billing");

    var SessionsController = function($scope, $rootScope, $http, $location, SessionService) {

        $http.get("config.json")
            .then(function(response) {
                BillingConfig = response.data;
            });

        $scope.login = function() {
            $http.defaults.headers.common.Authorization = "Basic " + SessionService.encode($scope.user.name + ":" + $scope.user.pass);
            var promise = $http({
                method: "post",
                url: BillingConfig.source + "login",
                data: {
                    "apiKey": BillingConfig.apiKey,
                    "signature": BillingConfig.signature
                }
            });
            promise.then(
                function(response) {
                    credentials = response.data;
                    $rootScope.currentUser = credentials.currentUser.name;
                    $location.path("/agents");

                },
                function(reason) {
                    $scope.loginError = "Username or password is incorrect";
                    credentials = null;
                    $location.path("/login");
                });
        };

        $rootScope.logout = function() {
            $http({
                method: "get",
                url: BillingConfig.source + "logout"
            }).then(
                function(response) {
                    console.log(response);
                    credentials = null;
                    $location.path("/login");
                },
                function(reason) {
                    console.log(reason);
                });
        };
    };
    app.controller("SessionsController", SessionsController);

}());
