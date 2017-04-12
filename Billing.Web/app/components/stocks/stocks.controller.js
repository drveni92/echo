  angular
      .module("Billing")
      .controller ('StocksController', ['$scope', '$http', '$uibModal', 'DataFactory', 'ToasterService', function($scope, $http, $uibModal, DataFactory, ToasterService) {


          $scope.maxPagination = BillingConfig.maxPagination

          function ListStocks(page) {
              DataFactory.list("stocks?page=" + page, function(data) {
                  $scope.stocks = data.list;
                  $scope.totalItems = data.totalItems;
                  $scope.currentPage = data.currentPage + 1;
              });
          }

          $scope.pageChanged = function() {
              ListStocks($scope.currentPage - 1);
          };

          ListStocks(0);

      }]);