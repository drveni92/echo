(function() {
    angular
        .module("Billing")
        .controller('ProcurementsController', ['$scope', '$http', '$uibModal', 'DataFactory', 'ToasterService', 'ProcurementsFactory', function ($scope, $http, $uibModal, DataFactory, ToasterService, ProcurementsFactory) {
            $scope.maxPagination = BillingConfig.maxPagination;
            $scope.pageParams = {
                page: 1,
                showPerPage: BillingConfig.showPerPage,
                sortType: 'date',
                sortReverse: true,
                totalItems: 0
            };

            function ListProcurements() {
                $scope.pageParams.page = $scope.pageParams.page - 1;

                DataFactory.list("procurements", function (data) {
                    $scope.procurements = data.list;
                    $scope.pageParams.totalItems = data.totalItems;
                    $scope.pageParams.page = data.currentPage + 1;
                }, $scope.pageParams);
            }



            $scope.sort = function (column) {
                if ($scope.pageParams.sortType === column) $scope.pageParams.sortReverse = !$scope.pageParams.sortReverse;
                $scope.pageParams.sortType = column;
                ListProcurements();
            };

            $scope.search = function () {
                if ($scope.pageParams.product.toString().length > 2 || $scope.pageParams.product.toString().length === 0) ListProcurements();
            };

            $scope.showItems = function () {
                ListProcurements();
            };

            $scope.pageChanged = function () {
                ListProcurements();
            };

            ListProcurements();

            $scope.new = function () {
                var modalInstance = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/components/procurements/templates/new.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    resolve: {
                        data: function () {
                            return ProcurementsFactory.empty();
                        }
                    }
                });

                modalInstance.result.then(function (procurement) {
                    DataFactory.insert("procurements", ProcurementsFactory.procurement(procurement), function (data) {
                        ToasterService.pop('success', "Success", "Procurement added");
                        ListProcurements();
                    });
                }, function () {
                });

            };

            $scope.show = function (procurement) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/components/procurements/templates/show.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    resolve: {
                        data: function () {
                            return procurement;
                        }
                    }
                });

                modalInstance.result.then(function () { }, function () { });

            };

            $scope.edit = function (procurement) {
                procurement.date = new Date(procurement.date);
                var modalInstance = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/components/procurements/templates/edit.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    resolve: {
                        data: function () {
                            return $.extend(true, {}, procurement);
                        }
                    }
                });

                modalInstance.result.then(function (procurement) {
                    DataFactory.update("procurements", procurement.id, ProcurementsFactory.procurement(procurement), function (data) {
                        ToasterService.pop('success', "Success", "Procurement saved");
                        ListProcurements();
                    });
                }, function () {
                    ListProcurements();
                });

            };

            $scope.delete = function (procurement) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/components/procurements/templates/delete.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    resolve: {
                        data: function () {
                            return procurement;
                        }
                    }
                });

                modalInstance.result.then(function (procurement) {
                    DataFactory.delete("procurements", procurement.id, function (data) {
                        ToasterService.pop('success', "Success", "Procurement deleted");
                        ListProcurements();
                    });
                }, function () {
                });
            };

        }]);
}());