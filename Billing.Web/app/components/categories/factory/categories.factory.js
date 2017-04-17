(function() {
    angular
        .module("Billing")
        .factory('CategoriesFactory', function() {
            return {
                empty: function() {
                    return {
                        id: 0,
                        name: ''
                    }
                },
                category: function(category) {
                    return {
                        id: category.id,
                        name: category.name
                    }
                }
            };
        })
}());
