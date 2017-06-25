(function () {
    'use strict';

    angular.module('petsApp', ['ngRoute']).controller('testctrl', testctrl)
        .config(function ($routeProvider, $locationProvider) {
             
            $routeProvider.when("/owners", {
                controller: "OwnersController",
                controllerAs: "vm",
                templateUrl: "/public/templates/Owners.Controller.html"
            });

            $routeProvider.when("/:owner/pets", {
                controller: "PetsController",
                controllerAs: "vm",
                templateUrl: "/public/templates/Pets.Controller.html"
            });

            $routeProvider.otherwise({
                redirectTo: "/owners"
            });
        });

    function testctrl($scope) {
        $scope.asd = 10;
    }
})();
