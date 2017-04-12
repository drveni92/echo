(function() {
    angular
        .module("Billing")
        .service('InvoicesService', ['$http', function($http) {
            var source = BillingConfig.source;
            $http.defaults.headers.common.Token = credentials.token;
            $http.defaults.headers.common.ApiKey = BillingConfig.apiKey;

            return {
                next: function(dataSet, callback) {
                    $http.get(source + dataSet)
                        .success(function(data, status, headers) {
                            return callback(data);
                        })
                        .error(function(error) {
                            return callback(false);
                        });
                }
            };
        }]);
}());
