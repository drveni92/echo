angular
    .module("Billing")
    .controller('ShippersController', ['$scope', '$http', '$uibModal', 'DataFactory', 'ToasterService', 'ShippersFactory', function($scope, $http, $uibModal, DataFactory, ToasterService, ShippersFactory) {

        $scope.maxPagination = BillingConfig.maxPagination;

        $scope.pageParams = {
            page: 1,
            showPerPage: BillingConfig.showPerPage,
            sortType: 'name',
            sortReverse: false,
            totalItems: 0
        };

        function ListShippers() {
            $scope.pageParams.page = $scope.pageParams.page - 1;

            DataFactory.list("shippers", function (data) {
                $scope.shippers = data.list;
                $scope.pageParams.totalItems = data.totalItems;
                $scope.pageParams.page = data.currentPage + 1;
            }, $scope.pageParams);
        };



        $scope.sort = function(column) {
            if($scope.pageParams.sortType === column) $scope.pageParams.sortReverse = !$scope.pageParams.sortReverse;
            $scope.pageParams.sortType = column;
            ListShippers();
        };

        $scope.search = function () {
            if ($scope.pageParams.name.toString().length > 2 || $scope.pageParams.name.toString().length == 0) ListShippers();
        };

        $scope.showItems = function () {
            ListShippers();
        };

        $scope.pageChanged = function() {
            ListShippers();
        };

        ListShippers();

        $scope.new = function() {

                var modalInstance = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/components/shippers/templates/new.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    resolve: {
                        data: function() {
                            return ShippersFactory.empty();
                        }
                    }
                });

                modalInstance.result.then(function(shipper) {
                    DataFactory.insert("shippers", ShippersFactory.shipper(shipper), function(data) { 
                        ToasterService.pop('success', "Success", "Shipper added");
                        ListShippers(); });
                }, function() {
                    console.log('Modal dismissed at: ' + new Date());
                });
        };

        $scope.show = function(shipper) {

            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/shippers/templates/show.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function() {
                        return shipper
                    }
                }
            });

            modalInstance.result.then(function() {
            }, function() {
            });

        };


        $scope.edit = function(shipper) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/components/shippers/templates/edit.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    resolve: {
                        data: function() {
                            return $.extend(true, {}, shipper)
                        }
                    }
                });

                modalInstance.result.then(function(shipper) {
                    DataFactory.update("shippers", shipper.id, ShippersFactory.shipper(shipper), function(data) {
                        ToasterService.pop('success', "Success", "Shipper saved");
                        ListShippers();
                    });
                }, function() {
                    console.log('Modal dismissed at: ' + new Date());
                });
        }


        $scope.delete = function(shipper) {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/shippers/templates/delete.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function() {
                        return shipper
                    }
                }
            });

            modalInstance.result.then(function(shipper) {
                DataFactory.delete("shippers", shipper.id, function(data) {
                    ToasterService.pop('success', "Success", "Shipper deleted");
                    ListShippers();
                });
            }, function() {
                console.log('Modal dismissed at: ' + new Date());
            });
        }

    }]);