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
                    .then(function success(response) {
                        return callback(response.data);
                    }, function error(error) {
                        ToasterService.pop('error', "Error", error.statusText);
                    });

            },

            read: function(dataSet, id, callback) {
                $http.get(source + dataSet + "/" + id)
                    .then(function success(response) {
                        return callback(response.data);
                    }, function error(error) {
                        ToasterService.pop('error', "Error", error.statusText);
                    });
            },

            insert: function(dataSet, data, callback) {
                $http({ method: "post", url: source + dataSet, data: data })
                    .then(function success(response) {
                        return callback(response.data);
                    }, function error(error) {
                        ToasterService.pop('error', "Error", error.statusText);
                    });
            },

            update: function(dataSet, id, data, callback) {
                $http({ method: "put", url: source + dataSet + "/" + id, data: data })
                    .then(function success(response) {
                        return callback(response.data);
                    }, function error(error) {
                        ToasterService.pop('error', "Error", error.statusText);
                    });
            },

            delete: function(dataSet, id, callback) {
                $http({ method: "delete", url: source + dataSet + "/" + id })
                    .then(function success(response) {
                        return callback(response.data);
                    }, function error(error) {
                        ToasterService.pop('error', "Error", error.statusText);
                    });
            }
        };
    };

    app.factory("DataFactory", DataFactory);

}());
