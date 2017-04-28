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

            list: function(dataSet, callback, params) {
                $http.get(source + dataSet, { params: params })
                    .then(function success(response) {
                        return callback(response.data);
                    }, function error(error) {
                        ToasterService.pop('error', "Error", error.data.message);
                    });

            },

            read: function(dataSet, id, callback) {
                $http.get(source + dataSet + "/" + id)
                    .then(function success(response) {
                        return callback(response.data);
                    }, function error(error) {
                        ToasterService.pop('error', "Error", error.data.message);
                    });
            },

            insert: function(dataSet, data, callback, params) {
                $http({ method: "post", url: source + dataSet, data: data, params: params })
                    .then(function success(response) {
                        return callback(response.data);
                    }, function error(error) {
                        ToasterService.pop('error', "Error", error.data.message);
                    });
            },

            update: function(dataSet, id, data, callback) {
                $http({ method: "put", url: source + dataSet + "/" + id, data: data })
                    .then(function success(response) {
                        return callback(response.data);
                    }, function error(error) {
                        ToasterService.pop('error', "Error", error.data.message);
                    });
            },

            delete: function(dataSet, id, callback) {
                $http({ method: "delete", url: source + dataSet + "/" + id })
                    .then(function success(response) {
                        return callback(response.data);
                    }, function error(error) {
                        ToasterService.pop('error', "Error", error.data.message);
                    });
            }
        };
    };

    app.factory("DataFactory", DataFactory);

}());
