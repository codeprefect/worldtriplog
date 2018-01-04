//Creating a module
(function() {
  "use strict";

  angular.module("app-trips", ['ngRoute']).config(config);

  config.$inject = ['$routeProvider'];

  function config($routeProvider) {
    $routeProvider.when("/", {
      controller: "tripsController",
      controllerAs: "vm",
      templateUrl: "/views/trips.html"
    });

    $routeProvider.when("/editor/:tripId", {
      controller: "tripEditorController",
      controllerAs: "vm",
      templateUrl: "/views/tripEditor.html"
    });

    $routeProvider.otherwise({
      redirectTo: "/"
    });
  }
})();
