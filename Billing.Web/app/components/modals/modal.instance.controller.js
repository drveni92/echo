angular
    .module("Billing")
    .controller('ModalInstanceController', ['$uibModalInstance', '$scope', 'DataFactory', 'data', 'options','$timeout', function($uibModalInstance, $scope, DataFactory,
                                                                                                                                 data, options,$timeout) {

        var $modal = this;

        $modal.data = data;
        $modal.options = {};
        $modal.counter = options.length - 1;
        $modal.options.regions = REGIONS;

        for (var i = options.length - 1; i >= 0; i--) {
            DataFactory.list(options[i], function(data) {
                $modal.options[options[$modal.counter]] = data;
                $modal.counter -= 1;
            });

        }

        $modal.ok = function() {
            $uibModalInstance.close($modal.data);

        };


        $modal.cancel = function() {
            $uibModalInstance.dismiss('cancel');
        };
    }]);