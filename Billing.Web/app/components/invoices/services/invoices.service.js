(function() {
    angular
        .module("Billing")
        .service('InvoicesService', ['$http', 'ToasterService', function($http, ToasterService) {
            var source = BillingConfig.source;
            $http.defaults.headers.common.Token = credentials.token;
            $http.defaults.headers.common.ApiKey = BillingConfig.apiKey;

            return {
                next: function(dataSet, callback, failed) {
                    $http.get(source + dataSet)
                        .then(function success(response) {
                            return callback(response.data);
                        }, function error(reason) {
                            return failed(reason);
                        });
                }
            };
        }]);
}());
