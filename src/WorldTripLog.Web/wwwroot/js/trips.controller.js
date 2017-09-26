//Getting existing module
(function () {
  "use strict";

  angular.module("app-trips")
    .controller("tripsController", tripsController);

  function tripsController($http) {
    let vm = this;

    vm.loading = true;

    vm.trips = [];

    $http.get("/api/trips")
      .then(function (response) {
        angular.copy(response.data, vm.trips);
      }, function (err) {
        vm.errorMessage = "Failed to load trips";
      })
      .finally(function () {
        vm.loading = false;
        console.log(response);
      });

    vm.newTrip = {};

    vm.addTrip = function () {
      $http.post("/api/trips", vm.newTrip)
        .then(function (response) {
          vm.trips.push(vm.newTrip);
        }, function (err) {
          vm.errorMessage = "Failed to post new trip";
        })
        .finally(function () {
          vm.newTrip = {};
        });
    };
  }
})();
