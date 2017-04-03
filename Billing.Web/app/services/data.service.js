(function() {

    var app = angular.module("Billing");

    var DataService = function($http, $rootScope) {
        var source = "http://localhost:9000/api/";
        $http.defaults.headers.common.Token = $rootScope.token;
        $http.defaults.headers.common.ApiKey = "RWNoby1CaWxsaW5n";

        return {
            promise: function(dataSet) {
                return $http.get(source + dataSet);
            },

            list: function(dataSet, callback) {
                $rootScope.message = "Please wait...";
                console.log($http.defaults.headers);
                $http.get(source + dataSet)
                    .success(function (data, status, headers) {
                        $rootScope.message = "";
                        return callback(data);
                    })
                    .error(function (error) {
                        return callback(false);
                    });
            },

            read: function(dataSet, id, callback) {
                $http.get(source + dataSet + "/" + id)
                    .success(function(data) {
                        return callback(data);
                    })
                    .error(function(error) {
                        return callback(false);
                    });
            },

            insert: function(dataSet, data, callback) {
                $http({ method:"post", url:source + dataSet, data:data })
                    .success(function(data) {
                        return callback(data);
                    })
                    .error(function(error){
                        return callback(false);
                    });
            },

            update: function(dataSet, id, data, callback) {
                $http({ method:"put", url:source + dataSet + "/" + id, data: data })
                    .success(function(data) {
                        return callback(data);
                    })
                    .error(function(error){
                        return callback(false);
                    });
            },

            delete: function(dataSet, id, callback) {
                $http({ method:"delete", url:source + dataSet + "/" + id })
                    .success(function() {
                        return callback(true);
                    })
                    .error(function(error){
                        return callback(false);
                    });
            }
        };
    };

    app.factory("DataService", DataService);

}());