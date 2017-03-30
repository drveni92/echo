(function() {

    var app = angular.module("Billing");

    var SessionsController = function($scope, $rootScope, $http, $location, SessionService) {

        $scope.login = function() {

            console.log("Login....");

            $http.defaults.headers.common.Authorization = "Basic " + SessionService.encode($scope.user.name + ":" + $scope.user.pass);
            $http.defaults.headers.common.Signature = "gC0xdV8gLD2cU0lzeDxFGZoZhxd78iz+6KojPZR5Wh4=";
            $http.defaults.headers.common.ApiKey = "R2lnaVNjaG9vbA==";
            var promise = $http({
                method: "post",
                url: "http://localhost:9000/api/login",
                data: {
                    "apiKey": "R2lnaVNjaG9vbA==",
                    "signature": "gC0xdV8gLD2cU0lzeDxFGZoZhxd78iz+6KojPZR5Wh4="
                }});
            promise.then(
                function(response) {
                    authenticated = true;
                    console.log(response);
                    currentUser = response.data.name;
                    $location.path("/agents");
                },
                function(reason){
                    authntication = false;
                    currentUser = "";
                    $location.path("/login");
                });
        };

        $scope.logout = function() {
            var request = $http({
                method: "get",
                url: source + "logout"
            });
            request.then(
                function (response) {
                    authenticated = false;
                    return true;
                },
                function (reason) {
                    return false;
                });
        };
    };
    app.controller("SessionsController", SessionsController);
    
}());