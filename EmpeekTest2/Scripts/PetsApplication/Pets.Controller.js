(function () {
    'use strict';

    angular.module('petsApp')
        .controller('PetsController', PetsController);

        function PetsController($http, $routeParams) {
            var vm = this;

            //getting owners data from url
            vm.ownerId = $routeParams.owner;
            vm.ownerName = $routeParams.ownerName;

            //if owners name ends with letter s we don't apply s-edning to it
            if (vm.ownerName.length > 0 && vm.ownerName.slice(-1) !== 's') {
                vm.ownerName += '\'s';
            } else {
                vm.ownerName += '\'';
            }

            //this variables are my alternative to a ng-repeat command
            //they allow to maintain three-rows table even when there is not enough items to show on page
            vm.first = {};
            vm.second = {};
            vm.third = {};

            //message to show while operations are in process or failed
            vm.message = '';

            vm.totalCount = 0;
            vm.pets = [];

            //Pages array must be one page smaller then number of pages. First page is always shown
            vm.pages = [];

            vm.currentPage = 1;

            vm.GetPets = GetPets;
            function GetPets() {
                vm.message = 'Loading pets. Please wait. :)';
                $http.get('api/pet/' + vm.ownerId)
                    .then(function (responce) {
                        //success

                        ApplyData(responce);
                    },
                    function () {
                        //failure
                        vm.message = 'Error while loading pets';
                    });
            }

            vm.AddPet = AddPet;
            function AddPet() {
                vm.message = 'Updating database';
                var data = { ownerId: vm.ownerId, newPet: vm.newPet };
                $http.post('api/pet', data)
                    .then(function (responce) {
                        //success

                        //clearing input field
                        vm.newPet = '';
                        ApplyData(responce);
                    },
                    function (data) {
                        //failure
                        console.log(data);
                        vm.message = 'Failed to update database';
                    });
            }


            vm.GoToPage = GoToPage;
            function GoToPage(page) {
                vm.currentPage = page;
                //clear variables
                vm.first = {};
                vm.second = {};
                vm.third = {};
                var firstIdx = (vm.currentPage - 1) * 3;
                var secondIdx = (vm.currentPage - 1) * 3 + 1;
                var thirdIdx = (vm.currentPage - 1) * 3 + 2;
                if (firstIdx < vm.totalCount)
                    vm.first = vm.pets[firstIdx];

                //if there is no first item on the current page go to previous page
                if (vm.currentPage !== 1 && firstIdx === vm.totalCount) vm.GoToPage(page - 1);

                if (secondIdx < vm.totalCount)
                    vm.second = vm.pets[secondIdx];
                if (thirdIdx < vm.totalCount)
                    vm.third = vm.pets[thirdIdx];
            }

            vm.DeletePet = DeletePet;
            function DeletePet(id) {
                $http.delete('api/pet/' + id)
                    .then(function () {
                        //success 

                        vm.GetPets();
                    },
                    function () {
                        //failure
                        vm.message = 'Error while deleting pet.';
                    });
            }

            vm.GetPets();
            vm.GoToPage(vm.currentPage);

            //function to wrap all data processing in get or post actions
            function ApplyData(responce) {
                angular.copy(responce.data.Pets, vm.pets);
                vm.totalCount = responce.data.NumberOfPets;
                vm.pages = vm.totalCount < 3 ? [] : new Array(Math.ceil(vm.totalCount / 3) - 1);
                vm.message = '';
                vm.GoToPage(vm.currentPage);
            }
        }
})();