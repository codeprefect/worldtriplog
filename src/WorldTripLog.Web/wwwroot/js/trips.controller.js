//Getting existing module
(function () {
  "use strict";

  angular.module("app-trips")
    .controller("tripsController", tripsController);

  tripsController.$inject = ['$http'];

  function tripsController($http) {
    let vm = this;
    activate();

    function activate() {
      vm.loading = true;
      vm.trips = [];
      vm.newTrip = {};
      getTrips();
    }

    vm.addTrip = addTrip;

    function getTrips() {
      $http.get("/api/trips")
        .then(function (response) {
          angular.copy(response.data, vm.trips);
        }, function (err) {
          vm.errorMessage = `${err.data.message} (${err.data.reason})`;
        })
        .finally(function () {
          vm.loading = false;
        });
    }

    function addTrip() {
      $http.post("/api/trips", vm.newTrip)
        .then(function (response) {
          vm.trips.push(vm.newTrip);
        }, function (err) {
          vm.errorMessage = `${err.data.message} (${err.data.reason})`;
        })
        .finally(function () {
          vm.newTrip = {};
        });
    }
  }
})();
