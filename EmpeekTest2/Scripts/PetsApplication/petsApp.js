(function () {
    'use strict';

    angular.module('petsApp', ['ngRoute'])
        .config(function ($routeProvider) {
             
            $routeProvider.when("/owners", {
                controller: "OwnersController",
                controllerAs: "vm",
                templateUrl: "/public/templates/Owners.Controller.html"
            });

            $routeProvider.when("/:owner/pets/:ownerName", {
                controller: "PetsController",
                controllerAs: "vm",
                templateUrl: "/public/templates/Pets.Controller.html"
            });

            $routeProvider.otherwise({
                redirectTo: "/owners"
            });
        });

    
})();
