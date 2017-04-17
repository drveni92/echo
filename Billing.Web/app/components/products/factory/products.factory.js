(function() {
    angular
        .module("Billing")
        .factory('ProductsFactory', function() {
            return {
                empty: function() {
                    return {
                        id: 0,
                        name: '',
                        unit: '',
                        price: null,
                        category: { id: null, name: '' }
                    }
                },
                product: function(product) {
                    return {
                        id: product.id,
                        name: product.name,
                        unit: product.unit,
                        price: product.price,
                        category: { id: product.category.id }
                    }
                }
            };
        })
}());
