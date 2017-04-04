angular
    .module("Billing")
    .controller('ModalInstanceController', function($uibModalInstance, data, options) {
        
        this.data = data;
        this.options = options;

        this.ok = function() {
            console.log(data);
            $uibModalInstance.close(this.data);
        };

        this.cancel = function() {
            $uibModalInstance.dismiss('cancel');
        };
    });