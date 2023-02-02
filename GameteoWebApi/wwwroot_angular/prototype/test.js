import './test.css'
import angular from  'angular' 

const app = angular.module('appCurrencyBoard', [require('angular-route')]);


app.config( ($routeProvider)=> {
    $routeProvider
                .when('/',{
                    templateUrl:'/demo/index.html',
                    controller:'currencyboard-controller'
            })
                .when('/err', {template: 'ERRRR;'})
                .otherwise({redirectTo: "/"});

});


function hi(){
    alert("sdsd")
}