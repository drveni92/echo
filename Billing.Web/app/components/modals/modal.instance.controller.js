(function() {
    angular
        .module("Billing")
        .controller('ModalInstanceController', ['$uibModalInstance', '$scope', 'DataFactory', 'data', '$timeout', function($uibModalInstance, $scope, DataFactory,
            data, $timeout) {

            var $modal = this;

            $modal.data = data;
            $modal.regions = BillingConfig.regions;

            $modal.ok = function() {
                $uibModalInstance.close($modal.data);
            };


            $modal.cancel = function() {
                $uibModalInstance.dismiss('cancel');
            };

            /* Data for autocomplete */
            $modal.getSuppliers = function(value) {
                var url = 'suppliers/' + value;
                return DataFactory.promise(url)
                    .then(function(response) {
                        return response.data.list;
                    });
            };

            $modal.getProducts = function(value) {
                var url = 'products/' + value;
                return DataFactory.promise(url)
                    .then(function(response) {
                        return response.data.list;
                    });
            };

            $modal.getTowns =  function(value) {
                var url = 'towns/' + value;
                return DataFactory.promise(url)
                    .then(function(response) {
                        return response.data.list;
                    });
            };

            $modal.getCategories =  function(value) {
                var url = 'categories/' + value;
                return DataFactory.promise(url)
                    .then(function(response) {
                        return response.data.list;
                    });
            };
        }]);

}());
