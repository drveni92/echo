angular
    .module("Billing")
    .controller('ProcurementsController', ['$scope', '$http', '$uibModal', 'DataFactory', 'ToasterService', function($scope, $http, $uibModal, DataFactory, ToasterService) {

        $scope.maxPagination = BillingConfig.maxPagination

        function ListProcurements() {
            DataFactory.list("procurements?page=" + ($scope.currentPage - 1), function(data) {
                $scope.procurements = data.list;
                $scope.totalItems = data.totalItems;
                $scope.currentPage = data.currentPage + 1;
            });
        }

        $scope.pageChanged = function() {
            ListProcurements();
        };

        ListProcurements();

        $scope.new = function() {

            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/procurements/templates/new.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function() {
                        return {
                            id: 0,
                            document: '',
                            date: new Date(),
                            product: { id: null, name: '' },
                            supplier: { id: null, name: '' },
                            quantity: null,
                            price: null
                        }
                    }
                }
            });

            modalInstance.result.then(function(procurement) {
                DataFactory.insert("procurements", procurement, function(data) {
                    ToasterService.pop('success', "Success", "Procurement added");
                    ListProcurements();
                });
            }, function() {
                console.log('Modal dismissed at: ' + new Date());
            });

        };

        $scope.show = function(procurement) {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/procurements/templates/show.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function() {
                        return procurement
                    }
                }
            });

            modalInstance.result.then(function() {}, function() {});

        };

        $scope.edit = function(procurement) {

            procurement.date = new Date(procurement.date);

            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/procurements/templates/edit.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function() {
                        return $.extend(true, {}, procurement)
                    }
                }
            });

            modalInstance.result.then(function(procurement) {
                DataFactory.update("procurements", procurement.id, procurement, function(data) {
                    ToasterService.pop('success', "Success", "Procurement saved");
                    ListProcurements();
                });
            }, function() {
                ListProcurements();
            });

        };

        $scope.delete = function(procurement) {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/procurements/templates/delete.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function() {
                        return procurement
                    }
                }
            });

            modalInstance.result.then(function(procurement) {
                DataFactory.delete("procurements", procurement.id, function(data) {
                    ToasterService.pop('success', "Success", "Procurement deleted");
                    ListProcurements();
                });
            }, function() {
                console.log('Modal dismissed at: ' + new Date());
            });
        };

    }]);
