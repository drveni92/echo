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
                            quantity: 0,
                            price: 0
                        }
                    },
                    options: function() {
                        return  [ "products", "suppliers" ]
                    }
                }
            });

            modalInstance.result.then(function(procurement) {
                DataFactory.insert("procurements", procurement, function(data) { ListProcurements(); });
            }, function() {
                console.log('Modal dismissed at: ' + new Date());
            });

        };

        $scope.show = function() {

        };

        $scope.edit = function() {

        };

        $scope.delete = function() {

        };

    }]);
