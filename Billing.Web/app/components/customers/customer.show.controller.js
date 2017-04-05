angular
	.module("Billing")
	.controller("CustomerShowController", ['$scope', '$http', '$routeParams', 'DataFactory', function($scope, $http, $routeParams, DataFactory) {
		console.log($routeParams.id);
	}