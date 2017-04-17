(function() {
    angular
        .module("Billing")
        .factory('CustomersFactory', function() {
            return {
                empty: function() {
                    return {
                        id: 0,
                        name: '',
                        address: '',
                        town: { id: null, name: '' }
                    }
                },
                customer: function(customer) {
                    return {
                        id: customer.id,
                        name: customer.name,
                        address: customer.address,
                        town: { id: customer.town.id }
                    }
                }
            };
        })
}());
