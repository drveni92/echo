(function() {
    angular
        .module('Billing')
        .service('ToasterService', function($rootScope) {
            this.pop = function(type, title, body) {
                this.toast = {
                    type: type,
                    title: title,
                    body: body
                };
                $rootScope.$broadcast('toaster-newToast');
            };
        })
}());
