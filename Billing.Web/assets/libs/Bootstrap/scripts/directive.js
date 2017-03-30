(function() {

    var app = angular.module('bootDemo', []);

    var helloWorld = function() {

        return {
            restrict: "AE",
            replace: true,
            template: "<h3 style='background-color:{{ color }}'>Hello World!</h3>",
            link: function(scope, elem, attrs) {
                elem.bind("click", function() {
                    elem.css("background-color", "white");
                    scope.$apply(function(){ scope.color = "white"; });
                });
                elem.bind("mouseover", function() { elem.css("cursor", "pointer") });
            }
        };
    };

    app.directive("helloWorld", helloWorld);

    var MainCtrl = function($scope) {
        $scope.name = "World";
    };

    app.controller("MainCtrl", MainCtrl);

}());