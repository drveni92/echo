(function() {

    var app = angular.module("Billing");

    var SessionsController = function($scope, $rootScope, $http, $location, SessionService) {

        $scope.login = function() {
            $http.defaults.headers.common.Authorization = "Basic " + SessionService.encode($scope.user.name + ":" + $scope.user.pass);
            $http.defaults.headers.common.Signature = "rxNzHu3jm6ubUy5sSHiHoyYo3I3Jt9rsRXQFsmTCNIM=";
            $http.defaults.headers.common.ApiKey = "RWNoby1CaWxsaW5n";
            var promise = $http({
                method: "post",
                url: "http://localhost:9000/api/login",
                data: {
                    "apiKey": "RWNoby1CaWxsaW5n",
                    "signature": "rxNzHu3jm6ubUy5sSHiHoyYo3I3Jt9rsRXQFsmTCNIM="
                }});
            promise.then(
                function(response) {
                    $rootScope.authenticated = true;
                    $scope.loginError = null;
                    $rootScope.currentUser = response.data;
                    $location.path("/agents");

                },
                function(reason){
                    $rootScope.authenticated = true;
                    $scope.loginError = "Username or password is incorrect";
                    currentUser = null;
                    $location.path("/login");
                });
        };

        $rootScope.logout = function() {
            var request = $http({
                method: "get",
                url: "http://localhost:9000/api/logout"
            });
            request.then(
                function (response) {
                    $rootScope.authenticated = false;
                    $rootScope.currentUser = null;
                    $location.path("/login");
                },
                function (reason) {
                    console.log(reason);
                });
        };
    };
    app.controller("SessionsController", SessionsController);
    
}());