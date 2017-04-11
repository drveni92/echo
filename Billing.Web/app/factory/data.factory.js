(function() {

    var app = angular.module("Billing");

    var DataFactory = function($http, $rootScope, ToasterService) {
        var source = BillingConfig.source;
        $http.defaults.headers.common.Token = credentials.token;
        $http.defaults.headers.common.ApiKey = BillingConfig.apiKey;

        return {
            promise: function(dataSet) {
                return $http.get(source + dataSet);
            },

            list: function(dataSet, callback) {
                $http.get(source + dataSet)
                    .success(function (data, status, headers) {
                        return callback(data);
                    })
                    .error(function (error) {
                        console.log(error);
                        ToasterService.pop('error', "Error", error.message);
                        return callback(false);
                    });
            },

            read: function(dataSet, id, callback) {
                $http.get(source + dataSet + "/" + id)
                    .success(function(data) {
                        return callback(data);
                    })
                    .error(function(error) {
                        ToasterService.pop('error', "error", error);
                        return callback(false);
                    });
            },

            insert: function(dataSet, data, callback) {
                $http({ method:"post", url:source + dataSet, data:data })
                    .success(function(data) {
                        return callback(data);
                    })
                    .error(function(error){
                        ToasterService.pop('error', "error", error);
                        return callback(false);
                    });
            },

            update: function(dataSet, id, data, callback) {
                $http({ method:"put", url:source + dataSet + "/" + id, data: data })
                    .success(function(data) {
                        return callback(data);
                    })
                    .error(function(error){
                        ToasterService.pop('error', "error", error);
                        return callback(false);
                    });
            },

            delete: function(dataSet, id, callback) {
                $http({ method:"delete", url:source + dataSet + "/" + id })
                    .success(function() {
                        return callback(true);
                    })
                    .error(function(error){
                        ToasterService.pop('error', "error", error);
                        return callback(false);
                    });
            }
        };
    };

    app.factory("DataFactory", DataFactory);

}());