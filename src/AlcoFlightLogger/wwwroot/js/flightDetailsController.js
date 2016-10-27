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

                // prepare coordinates
                var flightCoordinates = vm.flight.fuelPoints.map(function (val) {
                    return { lat: Number(val.latitude), lng: Number(val.longitude) };
                });

                if (typeof google !== 'undefined') {
                    // show map
                    var map = new google.maps.Map(document.getElementById('map_canvas'), {
                        zoom: 15,
                        center: flightCoordinates[0],
                        mapTypeId: 'terrain'
                    });

                    // plot flight
                    var flightPath = new google.maps.Polyline({
                        path: flightCoordinates,
                        geodesic: true,
                        strokeColor: '#FF0000',
                        strokeOpacity: 1.0,
                        strokeWeight: 2
                    });

                    flightPath.setMap(map);
                } else {
                    document.getElementById('map_canvas').innerHTML = "Failed to load map.";
                }

            }, function (error) {
                // Failure
                vm.errorMessage = "Failed to retrieve flight log: " + error.message;
            })
            .finally(function () {
                vm.isBusy = false;
            });

        vm.modifyFlight = function() {
            vm.isBusy = true;
            vm.errorMessage = "Saving...";
            $http.put("/api/Flights/", vm.flight)
                .then(function(response) {
                    //success
                    vm.errorMessage = "Flight modified!";
                }, function(error) {
                    //failure
                    vm.errorMessage = "Failed to modify the flight: " + error.message;
                })
                .finally(function() {
                    vm.isBusy = false;
                });
        };
    }
})();