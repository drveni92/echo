(function () {
    angular
        .module("Billing")
        .controller('InvoicesController', ['$scope', '$uibModal', 'DataFactory', 'InvoicesService', 'ToasterService', '$rootScope', function ($scope, $uibModal, DataFactory, InvoicesService, ToasterService, $rootScope) {
            $scope.states = BillingConfig.states;
            $scope.userId = credentials.currentUser.id;
            $scope.maxPagination = BillingConfig.maxPagination;
            $scope.showAdvancedSearch = false;
            $scope.searchParams = {
                agent: '',
                customer: '',
                status: ''
            };
            $scope.pageParams = {
                invoiceno: '',
                page: 1,
                showPerPage: BillingConfig.showPerPage,
                sortType: 'total',
                sortReverse: false,
                totalItems: 0
            };

            function ListInvoices() {
                $scope.pageParams.page = $scope.pageParams.page - 1;
                if ($scope.showAdvancedSearch) {
                    DataFactory.insert("invoices/search", $scope.searchParams, function (data) {
                        $scope.invoices = data.list;
                        $scope.pageParams.totalItems = data.totalItems;
                        $scope.pageParams.page = data.currentPage + 1;
                    }, $scope.pageParams);
                }
                else {
                    DataFactory.list("invoices", function (data) {
                        $scope.invoices = data.list;
                        $scope.pageParams.totalItems = data.totalItems;
                        $scope.pageParams.page = data.currentPage + 1;
                    }, $scope.pageParams);
                }
            };

            $scope.advancedSearch = function () {
                $scope.showAdvancedSearch = !$scope.showAdvancedSearch
                if ($scope.showAdvancedSearch == false) ListInvoices();
            };

            $scope.sort = function (column) {
                if ($scope.pageParams.sortType === column) $scope.pageParams.sortReverse = !$scope.pageParams.sortReverse;
                $scope.pageParams.sortType = column;
                ListInvoices();
            };

            $scope.advancedSearchSubmit = function () {
                ListInvoices();
            };

            $scope.search = function () {
                if ($scope.pageParams.invoiceno.toString().length > 2 || $scope.pageParams.invoiceno.toString().length == 0) ListInvoices();
            };

            $scope.showItems = function () {
                ListInvoices();
            };

            $scope.pageChanged = function () {
                ListInvoices();
            };

            ListInvoices();

            $scope.download = function (id) {
                InvoicesService.download(id);
            };

            $scope.checkUpdates = function () {
                DataFactory.list("invoices/automatic/check", function (result) {
                    $rootScope.invoicesCount = 0;
                    var modalInstance = $uibModal.open({
                        animation: true,
                        ariaLabelledBy: 'modal-title',
                        ariaDescribedBy: 'modal-body',
                        templateUrl: 'app/components/invoices/templates/updated.html',
                        controller: 'ModalInstanceController',
                        controllerAs: '$modal',
                        size: 'lg',
                        resolve: {
                            data: function () {
                                return result
                            }
                        }
                    });

                    modalInstance.result.then(function () { }, function () { });
                });
            };

            $scope.show = function (invoice) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/components/invoices/templates/show.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    scope: $scope,
                    size: 'lg',
                    resolve: {
                        data: function () {
                            return invoice
                        }
                    }
                });

                modalInstance.result.then(function (invoice) {
                    var modalInstance = $uibModal.open({
                        animation: true,
                        ariaLabelledBy: 'modal-title',
                        ariaDescribedBy: 'modal-body',
                        templateUrl: 'app/components/invoices/templates/mail.html',
                        controller: 'ModalInstanceController',
                        controllerAs: '$modal',
                        resolve: {
                            data: function () {
                                return null;
                            }
                        }
                    });

                    modalInstance.result.then(function (mail) {
                        mail.InvoiceId = invoice.id;
                        DataFactory.insert("invoices/mail", mail, function (data) {
                            ToasterService.pop('success', "Success", data);
                        })
                    }, function () {
                    });
                }, function () { });
            };

            $scope.show_history = function (invoice) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/components/invoices/templates/show_histories.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    scope: $scope,
                    size: 'lg',
                    resolve: {
                        data: function () {
                            return invoice
                        }
                    }
                });

                modalInstance.result.then(function () { }, function () { });
            };

            $scope.nextState = function (invoice, cancel) {
                if(cancel == null) cancel = false;
                var url = "invoices/" + invoice.id + "/next/" + cancel;
                InvoicesService.next(url, function (data) {
                    ToasterService.pop('info', "Invoice " + invoice.invoiceNo, "New state of the invoice is " + $scope.states[data.status + 1]);
                    ListInvoices();
                }, function (error) {
                    ToasterService.pop('error', "Error", error.data.message);
                    var modalInstance = $uibModal.open({
                        animation: true,
                        ariaLabelledBy: 'modal-title',
                        ariaDescribedBy: 'modal-body',
                        templateUrl: 'app/components/invoices/templates/automatic.html',
                        controller: 'ModalInstanceController',
                        controllerAs: '$modal',
                        resolve: {
                            data: function () {
                                return invoice
                            }
                        }
                    });

                    modalInstance.result.then(function (invoice) {
                        DataFactory.read("invoices/automatic", invoice.id, function (data) {
                            ToasterService.pop('success', "Success", data);
                        });
                    }, function () {
                    });
                });
            };


            $scope.delete = function (invoice) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/components/invoices/templates/delete.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    resolve: {
                        data: function () {
                            return invoice
                        }
                    }
                });

                modalInstance.result.then(function (invoice) {
                    DataFactory.delete("invoices", invoice.id, function (data) {
                        ToasterService.pop('success', "Success", "Invoice deleted");
                        ListInvoices();
                    });
                }, function () {
                });
            };
        }])
}());
