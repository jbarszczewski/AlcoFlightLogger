// flightDetailsController.js
(function () {
    "use strict";
    // Getting the existing module
    angular.module("app-flights")
        .controller("flightDetailsController", flightDetailsController);

    function flightDetailsController($routeParams, $http) {
        var vm = this;
        vm.errorMessage = "";
        vm.isBusy = true;
        vm.flight = "";
        $http.get("/api/Flights/" + $routeParams.flightId)
            .then(function (response) {
                // Success
                vm.flight = angular.copy(response.data);
            }, function (error) {
                // Failure
                vm.errorMessage = "Failed to retrieve flight log: " + error.message;
            })
            .finally(function () {
                vm.isBusy = false;
            });
    }
})();