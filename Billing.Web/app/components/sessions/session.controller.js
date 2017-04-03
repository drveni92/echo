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
                    $rootScope.billing = true;
                    authenticated = true;
                    $rootScope.logoutbutton = true;
                    $scope.Lerror = false;
                    $scope.loginError = "";
                    document.body.style.backgroundColor = "#fff";
                    currentUser = response.data.name;
                    $rootScope.token = response.data.token;
                    $location.path("/agents");

                },
                function(reason){
                    authntication = false;
                    $scope.Lerror = true;
                    $scope.loginError = "Username or password is incorrect";
                    currentUser = "";
                    $location.path("/login");
                });
        };

        $scope.logout = function() {
            var request = $http({
                method: "get",
                url: "http://localhost:9000/api/logout"
            });
            request.then(
                function (response) {
                    $rootScope.logout2 = true;
                    $rootScope.billing = false;
                    $rootScope.message="";
                    authenticated = false;
                    $rootScope.logoutbutton = false;
                    $rootScope.message1 = response.data;
                    return true;
                },
                function (reason) {
                    return false;
                });
        };
    };
    app.controller("SessionsController", SessionsController);
    
}());