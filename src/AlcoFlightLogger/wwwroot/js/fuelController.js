// flightsController.js
(function () {
    "use strict";
    // Getting the existing module
    angular.module("app-flights")
        .controller("fuelController", fuelController);

    function fuelController($http) {
        var vm = this;
        vm.errorMessage = "";
        vm.isBusy = false;

        vm.postFlight = function () {
            var successPosition = function (position) {
                var fuelPoint = {
                    Date: new Date(),
                    Latitude: position.coords.latitude,
                    Longitude: position.coords.longitude
                };
                $http.post("/api/Flights/FuelPoint", fuelPoint)
                    .then(function (response) {
                        vm.flights.push(response.data);
                    }, function (error) {
                        vm.errorMessage = "Failed to post new flight: " + error.message;
                    })
                    .finally(function () {
                        vm.isBusy = false;
                    });
            }

            var errorPosition = function (err) {

            }

            if (navigator.geolocation) {
                vm.isBusy = true;
                navigator.geolocation.getCurrentPosition(successPosition, errorPosition, { enableHighAccuracy: true, timeout: 20000 });
            }
        }
    }
})();