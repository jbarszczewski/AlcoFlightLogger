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
                vm.errorMessage = "Failed to retrieve flight log: " + error.message;
            })
            .finally(function () {
                vm.isBusy = false;
            });

        vm.postFlight = function() {
            if (navigator.geolocation) {
                vm.isBusy = true;
                navigator.geolocation.getCurrentPosition(function(position) {
                    var fuelPoint = {
                        Date: new Date(),
                        Latitude: position.coords.latitude,
                        Longitude: position.coords.longitude
                    };
                    $http.post("/api/Flights/FuelPoint", fuelPoint)
                        .then(function(response) {
                            vm.flights.push(response.data);
                        }, function(error) {
                            vm.errorMessage = "Failed to post new flight: " + error.message;
                        })
                        .finally(function() {
                            vm.isBusy = false;
                        });
                });
            }
        }
    }
})();