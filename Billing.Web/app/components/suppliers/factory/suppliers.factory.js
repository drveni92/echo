(function() {
    angular
        .module("Billing")
        .factory('SuppliersFactory', function() {
            return {
                empty: function() {
                    return {
                        id: 0,
                        name: '',
                        address: '',
                        town: { id: null, name: '' }
                    }
                },
                supplier: function(supplier) {
                    return {
                        id: supplier.id,
                        name: supplier.name,
                        address: supplier.address,
                        town: { id: supplier.town.id }
                    }
                }
            };
        })
}());
