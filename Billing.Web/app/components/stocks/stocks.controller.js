(function () {
    angular
        .module("Billing")
        .controller('StocksController', ['$scope', '$http', '$uibModal', 'DataFactory', 'ToasterService', function ($scope, $http, $uibModal, DataFactory, ToasterService) {


            $scope.maxPagination = BillingConfig.maxPagination

            $scope.pageParams = {
                page: 1,
                showPerPage: BillingConfig.showPerPage,
                sortType: 'product.name',
                sortReverse: false,
                totalItems: 0
            };

            function ListStocks() {
                $scope.pageParams.page = $scope.pageParams.page - 1;

                DataFactory.list("stocks", function (data) {
                    $scope.stocks = data.list;
                    $scope.pageParams.totalItems = data.totalItems;
                    $scope.pageParams.page = data.currentPage + 1;
                }, $scope.pageParams);
            };



            $scope.sort = function (column) {
                if ($scope.pageParams.sortType === column) $scope.pageParams.sortReverse = !$scope.pageParams.sortReverse;
                $scope.pageParams.sortType = column;
                ListStocks();
            };

            $scope.search = function () {
                if ($scope.pageParams.name.toString().length > 2 || $scope.pageParams.name.toString().length == 0) ListStocks();
            };

            $scope.showItems = function () {
                ListStocks();
            };

            $scope.pageChanged = function () {
                ListStocks();
            };

            ListStocks();

        }]);
})