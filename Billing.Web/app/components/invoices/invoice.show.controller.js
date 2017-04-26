(function () {
    angular
        .module("Billing")
        .controller('InvoiceShowController', ['$scope', '$route', '$routeParams', 'DataFactory', 'InvoicesService', '$uibModal', 'ToasterService' , function ($scope, $route, $routeParams, DataFactory, InvoicesService, $uibModal, ToasterService) {
            var invoiceId = $routeParams.id;
            if (invoiceId) {
                DataFactory.read("invoices", invoiceId, function (data) {
                    $scope.invoice = data;
                    $scope.invoice.date = new Date($scope.invoice.date);
                });
            }

            $scope.download = function () {
                InvoicesService.download(invoiceId);
            };

            $scope.send = function () {

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
                    mail.InvoiceId = invoiceId;
                    DataFactory.insert("invoices/mail", mail, function (data) {
                        ToasterService.pop('success', "Success", data);
                    })
                }, function () {
                });
            };

        }]);
}());
