angular
    .module("Billing")
    .controller('ProcurementsController', ['$scope', '$http', '$uibModal', 'DataFactory', function($scope, $http, $uibModal, DataFactory) {

        function ListProcurements() {
            DataFactory.list("procurements", function(data) { $scope.procurements = data });
        }

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
                            date: '',
                            product: { id: null },
                            supplier: { id: null },
                            quantity: null,
                            price: null
                        }
                    },
                    options: function() {
                        return ["products", "suppliers"]
                    }
                }
            });

            modalInstance.result.then(function(procurement) {
                console.log(procurement);
                DataFactory.insert("procurements", procurement, function(data) { ListProcurements(); });
            }, function() {
                console.log('Modal dismissed at: ' + new Date());
            });

        };

        $scope.show = function(procurement) {
            procurement.date = new Date(procurement.date);
            
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
                    },
                    options: function() {
                        return []
                    }
                }
            });

            modalInstance.result.then(function() {
            }, function() {
            });

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
                    },
                    options: function() {
                        return ["products", "suppliers"]
                    }
                }
            });

            modalInstance.result.then(function(procurement) {
                DataFactory.update("procurements", procurement.id, procurement, function(data) {
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
                    },
                    options: function() {
                        return []
                    }
                }
            });

            modalInstance.result.then(function(procurement) {
                DataFactory.delete("procurements", procurement.id, function(data) {
                    ListProcurements();
                    //message success missing
                });
            }, function() {
                console.log('Modal dismissed at: ' + new Date());
            });
        };

    }]);
