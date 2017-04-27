angular
    .module("Billing")
    .controller('CustomersController', ['$scope', '$http', '$uibModal', 'DataFactory', 'ToasterService', 'CustomersFactory', function($scope, $http, $uibModal, DataFactory, ToasterService, CustomersFactory) {

        $scope.maxPagination = BillingConfig.maxPagination

        $scope.pageParams = {
            page: 1,
            showPerPage: BillingConfig.showPerPage,
            sortType: 'name',
            sortReverse: false,
            totalItems: 0
        };

        function ListCustomers() {
            $scope.pageParams.page = $scope.pageParams.page - 1;

            DataFactory.list("customers", function (data) {
                $scope.customers = data.list;
                $scope.pageParams.totalItems = data.totalItems;
                $scope.pageParams.page = data.currentPage + 1;
            }, $scope.pageParams);
        };



        $scope.sort = function(column) {
            if($scope.pageParams.sortType === column) $scope.pageParams.sortReverse = !$scope.pageParams.sortReverse;
            $scope.pageParams.sortType = column;
            ListCustomers();
        };

        $scope.search = function () {
            if ($scope.pageParams.name.toString().length > 2 || $scope.pageParams.name.toString().length == 0) ListCustomers();
        };

        $scope.showItems = function () {
            ListCustomers();
        };

        $scope.pageChanged = function() {
            ListCustomers();
        };

        ListCustomers();

        $scope.new = function() {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/customers/templates/new.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function() {
                        return CustomersFactory.empty();
                    }
                }
            });

            modalInstance.result.then(function(customer) {
                DataFactory.insert("customers", CustomersFactory.customer(customer), function(data) {
                    ToasterService.pop('success', "Success", "Customer added");
                    ListCustomers();
                });
            }, function() {
                console.log('Modal dismissed at: ' + new Date());
            });
        };

        $scope.show = function(customer) {

            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/customers/templates/show.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function() {
                        return customer
                    }
                }
            });

            modalInstance.result.then(function() {}, function() {});

        };

        $scope.edit = function(customer) {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/customers/templates/edit.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function() {
                        return $.extend(true, {}, customer)
                    }
                }
            });

            modalInstance.result.then(function(customer) {
                DataFactory.update("customers", customer.id, CustomersFactory.customer(customer), function(data) {
                    ToasterService.pop('success', "Success", "Customer saved");
                    ListCustomers();
                });
            }, function() {
                ListCustomers();
            });
        }


        $scope.delete = function(customer) {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/customers/templates/delete.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function() {
                        return customer
                    }
                }
            });

            modalInstance.result.then(function(customer) {
                DataFactory.delete("customers", customer.id, function(data) {
                    ToasterService.pop('success', "Success", "Customer deleted");
                    ListCustomers();
                });
            }, function() {
                console.log('Modal dismissed at: ' + new Date());
            });
        }

    }]);
