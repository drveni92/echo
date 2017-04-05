angular
	.module("Billing")
	.controller("CustomerShowController", ['$scope', '$http', '$route', '$routeParams', 'DataFactory', function($scope, $http, $route, $routeParams, DataFactory) {
		var customerId = $routeParams.id;

		DataFactory.read("customers", customerId, function(data) {
			$scope.customer = data;
		});

	 }]);