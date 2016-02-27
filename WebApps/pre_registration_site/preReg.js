
var app = angular.module("PreRegistration", ['ui.bootstrap', 'ngRoute']);

app.config(['$routeProvider', function($routeProvider) {
    $routeProvider
    // route for the index page
        .when('/', {
            templateUrl : 'pre_registration_site/PreRegViews/selectEvent.html',
            controller : 'selectController'
        })

        .when('/PreRegister', {
            templateUrl : 'pre_registration_site/PreRegViews/PreRegistrationForm.html',
            controller : 'registerController'
        })


}]);