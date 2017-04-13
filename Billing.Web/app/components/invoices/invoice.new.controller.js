(function() {
    angular
        .module("Billing")
        .controller('InvoicesNewController', ['$scope', '$uibModal', '$route', '$routeParams', 'DataFactory', 'ToasterService', '$location', function($scope, $uibModal, $route, $routeParams, DataFactory, ToasterService, $location) {
            $scope.states = BillingConfig.states;

            $scope.active = 0;

            $scope.invoice = {
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
            };

            var invoiceId = $routeParams.id;

            if (invoiceId) {
                DataFactory.read("invoices", invoiceId, function(data) {
                    $scope.invoice = data;
                    $scope.invoice.date = new Date($scope.invoice.date);
                });
            }

            $scope.add_new_item = function(item) {
                /* check if item exists */
                var exists = false;
                for (var i = $scope.invoice.items.length - 1; i >= 0; i--) {
                    if ($scope.invoice.items[i].product.name === item.name && $scope.invoice.items[i].price === item.price) {
                        exists = true;
                        if ($scope.invoice.items[i].quantity + item.quantity > item.stock.inventory) {
                            console.log(item.stock.inventory - $scope.invoice.items[i].quantity);
                        } else {
                            $scope.invoice.items[i].quantity += item.quantity;
                        }
                    }
                }
                if (!exists) {
                    $scope.invoice.items.unshift({
                        id: 0,
                        product: {
                            id: item.id,
                            name: item.name,
                            unit: item.unit,
                        },
                        quantity: item.quantity,
                        price: item.price,
                        subTotal: item.quantity * item.price
                    });
                }
                ToasterService.pop('info', "Info", "Item added");
            };

            $scope.deleteItem = function(item) {
                for (var i = $scope.invoice.items.length - 1; i >= 0; i--) {
                    if ($scope.invoice.items[i].product.name === item.product.name && $scope.invoice.items[i].price === item.price) {
                        $scope.invoice.items.splice(i, 1);
                    }
                }
                ToasterService.pop('success', "Success", "Item deleted");
            };

            $scope.open = function() {
                $scope.popup.opened = true;
            };

            $scope.popup = {
                opened: false
            };

            $scope.setActieveTab = function(index) {
                if (index === 2) $scope.invoicePrepare();
                $scope.active = index;
            };

            $scope.getProducts = function(value) {
                var url = 'products/' + value;
                return DataFactory.promise(url)
                    .then(function(response) {
                        return response.data.list;
                    });
            };

            $scope.getCustomers = function(value) {
                var url = 'customers/' + value;
                return DataFactory.promise(url)
                    .then(function(response) {
                        return response.data.list;
                    });
            };

            $scope.getShippers = function(value) {
                var url = 'shippers/' + value;
                return DataFactory.promise(url)
                    .then(function(response) {
                        return response.data.list;
                    });
            }

            $scope.invoicePrepare = function() {
                var invoice = $scope.invoice;
                if ($scope.invoice.agent.id != 0) {
                    invoice.agent.id = $scope.invoice.agent.id;
                    invoice.agent.name = $scope.invoice.agent.name;
                } else {
                    invoice.agent.id = credentials.currentUser.id;
                    invoice.agent.name = credentials.currentUser.name;
                }
                invoice.shipper = {
                    id: $scope.invoice.shipper.id,
                    name: $scope.invoice.shipper.name
                };
                invoice.customer = {
                    id: $scope.invoice.customer.id,
                    name: $scope.invoice.customer.name,
                    address: $scope.invoice.customer.address,
                    town: {
                        name: $scope.invoice.customer.town.name
                    }
                };
                invoice.status = 0;
                invoice.vat = 17;
                invoice.subTotal = 0;
                for (var i = invoice.items.length - 1; i >= 0; i--) {
                    invoice.subTotal += invoice.items[i].subTotal;
                }
                invoice.vatAmount = invoice.subTotal * invoice.vat / 100;
                invoice.total = invoice.subTotal + invoice.vatAmount + invoice.shipping;
                $scope.invoice = invoice;
            };

            $scope.save = function() {
                if (invoiceId == null) {
                    DataFactory.insert("invoices", $scope.invoice, function(data) {
                        if (data) {
                            $scope.invoice = data;
                            $scope.invoice.date = new Date(data.date);
                            ToasterService.pop('success', "Success", "Invoice added");
                            $location.path ('/invoice/'+ data.id);
                        }
                    });
                } else {
                    DataFactory.update("invoices", $scope.invoice.id, $scope.invoice, function(data) {
                        if (data) {
                            $scope.invoice = data;
                            $scope.invoice.date = new Date(data.date);
                            ToasterService.pop('success', "Success", "Invoice saved");
                            $location.path ('/invoice/'+ data.id);
                        }
                    });
                }

            };
        }])
}());
