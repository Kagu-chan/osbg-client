angular.module("osbgApp")
	.controller("HeaderController", ($scope) => {
		$scope.isAlpha = true;
		$scope.alphaMessage = "This is an alpha release - this means restricted functionallity!"
		$scope.test = "This is a test";
	});