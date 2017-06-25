(function () {
    'use strict';

    angular.module('petsApp')
        .controller('OwnersController', OwnersController);

        function OwnersController($http) {
            var vm = this;
            vm.first = {};
            vm.second = {};
            vm.third = {};
            vm.message = '';
            vm.totalCount = 0;
            vm.owners = [];
            //Pages array must be one page smaller then number of pages. First page is always shown
            vm.pages = [];
            vm.currentPage = 1;
            

            vm.GetOwners = GetOwners;
            function GetOwners() {
                vm.message = 'Loading owners. Please wait.';
                $http.get('api/owner')
                    .then(function (responce) {
                        //success
                        
                        ApplyData(responce);
                    },
                    function () {
                        //failure
                        vm.message = 'Error while loading owners';
                    });
            }

            vm.AddOwner = AddOwner;
            function AddOwner() {
                vm.message = 'Updating database';
                var data = { newOwner: vm.newOwner };
                $http.post('api/owner', data)
                    .then(function (responce) {
                        //success

                        //clearing input field
                        vm.newOwner = '';
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
                vm.first = {};
                vm.second = {};
                vm.third = {};
                var firstIdx = (vm.currentPage - 1) * 3;
                var secondIdx = (vm.currentPage - 1) * 3 + 1;
                var thirdIdx = (vm.currentPage - 1) * 3 + 2;
                if (firstIdx < vm.totalCount)
                    vm.first = vm.owners[firstIdx];
                if (vm.currentPage!==1&&firstIdx === vm.totalCount) vm.GoToPage(page - 1);
                if (secondIdx < vm.totalCount)
                    vm.second = vm.owners[secondIdx];
                if (thirdIdx < vm.totalCount)
                    vm.third = vm.owners[thirdIdx];
            }

            vm.DeleteOwner = DeleteOwner;
            function DeleteOwner(id) {
                $http.delete('api/owner/' + id)
                    .then(function () {
                        //success 
                        
                        vm.GetOwners();
                    },
                    function () {
                        //failure
                        vm.message = 'Error while deleting owner.';
                    });
            }
             
            vm.GetOwners();
            vm.GoToPage(vm.currentPage);

            function ApplyData(responce) {
                angular.copy(responce.data.Owners, vm.owners);
                vm.totalCount = responce.data.NumberOfOwners;
                vm.pages = vm.totalCount < 3 ? [] : new Array(Math.ceil(vm.totalCount / 3) - 1);
                vm.message = '';
                vm.GoToPage(vm.currentPage);
            }
        }


    
    

})();
