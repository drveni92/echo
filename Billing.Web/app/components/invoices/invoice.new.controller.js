(function() {
    angular
        .module("Billing")
        .controller('InvoicesNewController', ['$scope', '$uibModal', '$route', '$routeParams', 'DataFactory', 'ToasterService', '$location', 'InvoicesFactory', function($scope, $uibModal, $route, $routeParams, DataFactory, ToasterService, $location, InvoicesFactory) {
            $scope.states = BillingConfig.states;

            $scope.active = 0;

            $scope.invoice = InvoicesFactory.empty();

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
                        $scope.invoice.items[i].quantity += item.quantity;
                    }
                }
                if (!exists) {
                    $scope.invoice.items.unshift(InvoicesFactory.item(item));
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
                if (index === 2) $scope.invoice = InvoicesFactory.invoice($scope.invoice);
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

            $scope.save = function() {
                if (invoiceId == null) {
                    DataFactory.insert("invoices", $scope.invoice, function(data) {
                        if (data) {
                            $scope.invoice = InvoicesFactory.invoice(data);
                            ToasterService.pop('success', "Success", "Invoice added");
                            $location.path('/invoice/' + data.id);
                        }
                    });
                } else {
                    DataFactory.update("invoices", $scope.invoice.id, $scope.invoice, function(data) {
                        if (data) {
                            $scope.invoice = InvoicesFactory.invoice(data);
                            ToasterService.pop('success', "Success", "Invoice saved");
                            $location.path('/invoice/' + data.id);
                        }
                    });
                }

            };
        }])
}());
