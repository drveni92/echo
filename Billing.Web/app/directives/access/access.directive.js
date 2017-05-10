(function () {
    angular
        .module("Billing")
        .directive('adminAccess', function () {
            return {
                scope: { owner: '=' },
                restrict: 'A',
                link: function (scope, element, attr) {
                    var access = (credentials.currentUser.roles.indexOf("admin") > -1);
                    if (!access) {
                        var user = credentials.currentUser.id;
                        var owner = scope.owner;
                        if (owner != user) element.remove();
                    }
                }
            };
        });
}());
