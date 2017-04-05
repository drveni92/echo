angular
    .module("Billing")
    .controller('SuppliersController', ['$scope', '$http', '$uibModal', 'DataFactory', function($scope, $http, $uibModal, DataFactory) {
        $scope.get = function(currentSupplier) {
            $scope.supplier = currentSupplier;
            $scope.showSupplier = true;
        };

        $scope.save = function(){
            if($scope.supplier.id === 0)
                DataFactory.insert("supliers", $scope.supplier, function(data){ ListSuppliers();} );
            else
                DataFactory.update("supliers", $scope.supplier.id, $scope.supplier, function(data){ListSuppliers();});
        };
        $scope.delete = function(){

            DataFactory.delete("suppliers", $scope.supplier.id, function(data){ListSuppliers();});
            $scope.showAgent = false;

        };
        function ListSuppliers() {
            DataFactory.list("suppliers", function(data) { $scope.suppliers = data });
        }

        ListSuppliers();

        $scope.new = function() {
            DataFactory.list("towns", function(data) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/components/suppliers/templates/new.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    resolve: {
                        data: function() {
                            return { id: 0, name: '', address: '', town: { id: null } }
                        },
                        options: function() {
                            return { towns: data }
                        }
                    }
                });
                modalInstance.result.then(function(supplier) {
                    DataFactory.insert("suppliers", supplier, function(data) { ListSuppliers(); });
                }, function() {
                    console.log('Modal dismissed at: ' + new Date());
                });

            });
        };
    }]);