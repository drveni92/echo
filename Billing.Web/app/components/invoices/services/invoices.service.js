(function() {
    angular
        .module("Billing")
        .service('InvoicesService', ['$http', 'ToasterService', function($http, ToasterService) {
            var source = BillingConfig.source;
            $http.defaults.headers.common.Token = credentials.token;
            $http.defaults.headers.common.ApiKey = BillingConfig.apiKey;

            return {
                next: function(dataSet, callback) {
                    $http.get(source + dataSet)
                        .then(function success(data) {
                            return callback(data);
                        }, function error(error) {
                            ToasterService.pop('error', "Error", error.data.message);
                        });
                }
            };
        }]);
}());
