(function () {
    angular
        .module("Billing")
        .factory('InvoicesFactory', function () {
            return {
                invoice: function (invoice) {
                    var temp_subTotal = 0;
                    var temp_vat = 17;
                    for (var i = invoice.items.length - 1; i >= 0; i--) {
                        temp_subTotal += invoice.items[i].subTotal;
                    }
                    var temp_vatAmout = temp_subTotal * temp_vat / 100;

                    return {
                        id: invoice.id,
                        invoiceNo: invoice.invoiceNo,
                        date: invoice.date,
                        items: invoice.items,
                        agent: {
                            id: credentials.currentUser.id,
                            name: credentials.currentUser.name
                        },
                        shipper: {
                            id: invoice.shipper.id,
                            name: invoice.shipper.name
                        },
                        customer: {
                            id: invoice.customer.id,
                            name: invoice.customer.name,
                            address: invoice.customer.address,
                            town: { name: invoice.customer.town.name }
                        },
                        status: invoice.status,
                        shipping: invoice.shipping,
                        vat: temp_vat,
                        subTotal: temp_subTotal,
                        vatAmount: temp_vatAmout,
                        total: temp_subTotal + temp_vatAmout + invoice.shipping
                    }
                },

                empty: function () {
                    return {
                        id: 0,
                        invoiceNo: '',
                        date: new Date(),
                        shipping: 0,
                        agent: { id: 0, name: '' },
                        shipper: { id: 0, name: '' },
                        customer: { id: 0, name: '', address: '', town: { id: 0, name: '' } },
                        items: [],
                        subTotal: 0,
                        vat: 17,
                        vatAmout: 0
                    }
                },

                item: function (item) {
                    return {
                        id: 0,
                        product: {
                            id: item.id,
                            name: item.name,
                            unit: item.unit,
                        },
                        quantity: item.quantity,
                        price: item.price,
                        subTotal: item.quantity * item.price
                    }
                }
            };
        })
}());
