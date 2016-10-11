// flightsController.js
(function () {
    "use strict";
    // Getting the existing module
    angular.module("app-flights")
        .controller("flightsController", flightsController);

    function flightsController($http) {
        var vm = this;
        vm.flights = [];
        vm.errorMessage = "";
        vm.isBusy = true;
        $http.get("/api/Flights")
            .then(function (response) {
                // Success
                angular.copy(response.data, vm.flights);
            }, function (error) {
                // Failure
                vm.errorMessage = "Failed to retrieve flight log: " + error;
            })
            .finally(function () {
                vm.isBusy = false;
            });

        vm.postFlight = function() {
            vm.isBusy = true;
            var flight = { Name: "test", Longitude: 1.2345, Latitude: 6.7890 };
            $http.post("/api/Flights", flight)
                .then(function(response) {
                    vm.flights.push(response.data);
                }, function(error) {
                    vm.errorMessage = "Failsed to post new flight: " + error;
                })
                .finally(function() {
                    vm.isBusy = false;
                });
        }
    }
})();