(function() {
    angular
        .module("Billing")
        .factory('httpInterceptor', ['$q', '$rootScope', function($q, $rootScope) {

            var skip = "invoices/automatic/nocheck";

            var numLoadings = 0;

            return {
                request: function(config) {

                    numLoadings++;

                    if(config.url.indexOf(skip) === -1) $rootScope.$broadcast("loader_show");
                    return (config || $q.when(config));

                },
                response: function(response) {

                    //console.log(response);

                    if ((--numLoadings) === 0) {
                        // Hide loader
                        $rootScope.$broadcast("loader_hide");
                    }

                    return response || $q.when(response);

                },
                responseError: function(response) {

                    if (!(--numLoadings)) {
                        // Hide loader
                        $rootScope.$broadcast("loader_hide");
                    }

                    return $q.reject(response);
                }
            };
        }])
        .config(function($httpProvider) {
            $httpProvider.interceptors.push('httpInterceptor');
        });
}());
