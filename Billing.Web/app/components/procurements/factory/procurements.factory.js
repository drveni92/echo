(function() {
    angular
        .module("Billing")
        .factory('ProcurementsFactory', function() {
            return {
                empty: function() {
                    return {
                        id: 0,
                        document: '',
                        date: new Date(),
                        product: { id: null, name: '' },
                        supplier: { id: null, name: '' },
                        quantity: null,
                        price: null
                    }
                },
                procurement: function(procurement) {
                    return {
                        id: procurement.id,
                        document: procurement.document,
                        date: new Date(procurement.date),
                        product: { id: procurement.product.id },
                        supplier: { id: procurement.supplier.id },
                        quantity: procurement.quantity,
                        price: procurement.price
                    }
                }
            };
        })
}());
