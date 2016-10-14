// app-trips.js
(function () {
    "use strict";

    angular.module("app-flights", ["ngRoute"])
    .config(function ($routeProvider) {
        
        $routeProvider.when("/", {
            controller: "fuelController",
            controllerAs: "vm",
            templateUrl: "/views/logFuelView.html"
        });

        $routeProvider.when("/flights", {
            controller: "flightsController",
            controllerAs: "vm",
            templateUrl: "/views/flightsView.html"
        });

        $routeProvider.when("/details/:flightId", {
            controller: "flightDetailsController",
            controllerAs: "vm",
            templateUrl: "/views/flightDetailsView.html"
        });

        $routeProvider.otherwise({ redirectTo: "/" });

    });
})();