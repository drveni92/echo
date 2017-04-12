(function() {
    angular
        .module("Billing")
        .directive('spinner', ['$rootScope', function($rootScope) {
            return function($scope, element, attrs) {
                $scope.$on("loader_show", function() {
                    return element.css("display", "block");
                });
                return $scope.$on("loader_hide", function() {
                    return element.css("display", "none");
                });
            };
        }]);
}());
