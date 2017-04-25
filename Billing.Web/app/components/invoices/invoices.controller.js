(function () {
    angular
        .module("Billing")
        .controller('InvoicesController', ['$scope', '$uibModal', 'DataFactory', 'InvoicesService', 'ToasterService', function ($scope, $uibModal, DataFactory, InvoicesService, ToasterService) {
            $scope.states = BillingConfig.states;
            $scope.maxPagination = BillingConfig.maxPagination;
            $scope.userId = credentials.currentUser.id;
            $scope.currentPage = 1;
            $scope.searchInvoiceNo = "";

            function ListInvoices() {
                var url = "invoices";
                url += "?invoiceno=" + (($scope.searchInvoiceNo !== undefined) ? $scope.searchInvoiceNo.toString() : "");
                url += "&page=" + (($scope.currentPage !== undefined) ? ($scope.currentPage - 1) : 0);
                DataFactory.list(url, function (data) {
                    $scope.invoices = data.list;
                    $scope.totalItems = data.totalItems;
                    $scope.currentPage = data.currentPage + 1;
                });
            };

            $scope.search = function search() {
                if($scope.searchInvoiceNo.toString().length > 2) ListInvoices();
            };

            $scope.pageChanged = function () {
                $scope.listInvoices();
            };

            ListInvoices();

            $scope.download = function (id) {
                InvoicesService.download(id);
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
                        console.log(invoice);
                        mail.InvoiceId = invoice.id;
                        DataFactory.insert("invoices/mail", mail, function (data) {
                            ToasterService.pop('success', "Success", data);
                        })
                    }, function () {
                        console.log('Modal dismissed at: ' + new Date());
                    });
                }, function () { });
            }

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
            }



            $scope.nextState = function (invoice, cancel = false) {
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
                        console.log('Modal dismissed at: ' + new Date());
                    });
                });
            }


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
                    console.log('Modal dismissed at: ' + new Date());
                });
            };
        }])
}());
