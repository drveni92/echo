(function () {
    angular
        .module("Billing")
        .controller('InvoicesReviewController', ['$scope', 'DataFactory', '$uibModal', 'ToasterService', '$rootScope',
            function ($scope, DataFactory, $uibModal, ToasterService, $rootScope) {
                $scope.states = BillingConfig.states;

                $scope.dates = {
                    startDate: new Date(),
                    endDate: new Date()
                };

                function ListCustomers() {
                    DataFactory.insert("customers/all", {
                    }, function (data) {
                        $scope.Customers = data;
                    });
                }

                $scope.update = function () {
                    ListInvoicesReview();
                }

                ListCustomers();
                function ListInvoicesReview() {
                    DataFactory.insert("invoicereviewpost", {
                        startDate: $scope.dates.startDate,
                        endDate: $scope.dates.endDate,
                        id: $scope.CustomerId
                    }, function (result) {
                        $scope.invoicereview = result;
                    })
                }

                var initializing = false;

                $scope.dates.startDate.setMonth($scope.dates.startDate.getMonth() - 12);

                $scope.startDateOpened = false;
                $scope.endDateOpened = false;

                $scope.openStartDate = function () {
                    $scope.startDateOpened = true;
                };

                $scope.openEndDate = function () {
                    $scope.endDateOpened = true;
                };

                $scope.$watch(
                    function (scope) { return scope.dates.startDate },
                    function () {
                        if (initializing)
                            ListInvoicesReview();
                    }
                );
                $scope.$watch(
                    function (scope) { return scope.dates.endDate },
                    function () {
                        if (initializing)
                            ListInvoicesReview();
                        else
                            initializing = true;
                    }
                );

                $scope.show = function (id) {
                    DataFactory.list("invoicereview/" + (id), function (data) {
                        $scope.InvoiceCustomer = data;

                        var modalInstance = $uibModal.open({
                            animation: true,
                            ariaLabelledBy: 'modal-title',
                            ariaDescribedBy: 'modal-body',
                            templateUrl: 'app/view/reports/invoicereviewshow.html',
                            controller: 'ModalInstanceController',
                            controllerAs: '$modal',
                            scope: $scope,
                            size: 'lg',
                            resolve: {
                                data: function () {
                                    return data;
                                }

                            }
                        });
                    });
                }


            }]);

}());

