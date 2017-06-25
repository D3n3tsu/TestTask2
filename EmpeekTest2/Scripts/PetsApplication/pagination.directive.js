(function () {
    'use strict';

    angular.module('petsApp')
        .directive('pagination', function () {
            return {
                restrict: 'E',
                template: `
                    <div>
                        <a ng-click="vm.GoToPage(1)"
                           ng-class="{currentPage: vm.currentPage === 1}" >1</a>
                        <span ng-if="vm.pages.length>0" ng-repeat="item in vm.pages track by $index">
                            , <a ng-click="vm.GoToPage($index+2)" 
                                 ng-class="{currentPage: vm.currentPage === $index+2}">{{ $index+2 }}</a>
                        </span>
                    </div>
                `
            };
        });


})();