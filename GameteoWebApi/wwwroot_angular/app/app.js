
import angular from  'angular' 
import currencyBoardController from './currency-board/currency-board.js'
import currencyRowController from './currency-row/currency-row.js'
import mainMenuContrller from './main-menu/main-menu.js'

import "./app.css"


const app = angular.module('app', [require('angular-route')]);


app.config( ($routeProvider)=> {
    $routeProvider
                .when('/',{
                    templateUrl:'/app/currency-board/currency-board.html',
                    controller:'currencyboard-controller'
            })
                .when('/cnb-news', {template: 'cnb-news'})
                .when('/cnb-archive', {template: 'cnb-archive'})
                .when('/cnb-contacts', {template: 'cnb-contacts'})
                .otherwise({redirectTo: "/"});

});

app.controller('currencyboard-controller',currencyBoardController); 
app.directive('currencyrow',['$http', function($http){
    return {
        templateUrl:'/app/currency-row/currency-row.html',
        link:currencyRowController
    }
}])

app.directive('mainmenu',function(){
    return {
        templateUrl:'/app/main-menu/main-menu.html',
        link:mainMenuContrller
    }
})


