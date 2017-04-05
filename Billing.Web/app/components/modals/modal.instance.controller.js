angular
    .module("Billing")
    .controller('ModalInstanceController', ['$uibModalInstance', '$scope', 'DataFactory', function($uibModalInstance, $scope, DataFactory, data, options){
        
        this.data = data;
        this.options = options;

        DataFactory.list("suppliers", function(data){
            $scope.suppliers =  data;
        });

        this.ok = function() {
            $uibModalInstance.close(this.data);
        };

        this.cancel = function() {
            $uibModalInstance.dismiss('cancel');
        };
    }]);