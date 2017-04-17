(function() {
    angular
        .module("Billing")
        .factory('ShippersFactory', function() {
            return {
                empty: function() {
                    return {
                        id: 0,
                        name: '',
                        address: '',
                        town: { id: null, name: '' }
                    }
                },
                shipper: function(shipper) {
                    return {
                        id: shipper.id,
                        name: shipper.name,
                        address: shipper.address,
                        town: { id: shipper.town.id }
                    }
                }
            };
        })
}());
