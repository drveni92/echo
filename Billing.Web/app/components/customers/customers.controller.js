/**
 * Created by User on 31.03.2017..
 */
(function(){

    var app = angular.module("Billing");

    var CustomersController = function($scope, $http, DataService) {

        $scope.showCustomer = false;
        ListCustomers();

        $scope.getCustomer = function(currentCustomer){
            $scope.customer = currentCustomer;
            $scope.showCustomer = true;
        };

        $scope.saveC = function(){
            if($scope.customer.id === 0)
                DataService.insert("customers", $scope.customer, function(data){ ListCustomers();} );
            else
                DataService.update("customers", $scope.customer.id, $scope.customer, function(data){ListCustomers();});
        };
        $scope.deleteC = function(){

            DataService.delete("customers", $scope.customer.id, function(data){ListCustomers();});
            $scope.showAgent = false;
            $scope.showCustomer = false;

        };

        $scope.new = function(){
            $scope.customer = {
                id: 0,
                name: "",
                address:"",
                town : {id:1}
            };
            $scope.showCustomer = true;
        };

        function ListCustomers(){
            DataService.list("customers", function(data){ $scope.customers = data});
        }
    };

    app.controller("CustomersController", CustomersController);

}());