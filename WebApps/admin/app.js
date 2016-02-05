/**
 * Created by Tim Unger
 */

var app = angular.module("AdminPanel", ['ui.bootstrap', 'ngRoute']);

app.config(['$routeProvider', function($routeProvider) {

    $routeProvider
    // route for the index page
        .when('/', {
            templateUrl : 'templates/index.html',
            controller : 'dashboardController'
        })

        // route for the CreateEvent page
        .when('/CreateEvent', {
            templateUrl : 'templates/createEvent.html',
            controller : 'createEventsController'
        })

        .when('/PreRegistrationSetup/:eventNum', {
            templateUrl : 'templates/preRegistrationSetup.html',
            controller : 'questionAddController'
        })

        // route for the CreateEvent page
        .when('/RegisterKiosk', {
            templateUrl : 'templates/registerKiosk.html',
            controller : 'kioskController'
        })

        // route for the CreateEvent page
        .when('/ManageEvents', {
            templateUrl : 'templates/manageEvents.html',
            controller : 'manageController'
        })

        .when('/EditQuestions', {
            templateUrl : 'templates/editQuestion.html',
            controller : 'questionEditController'
        })

        // route for the CreateEvent page
        .when('/EventReports', {
            templateUrl : 'templates/statsGraphs.html',
            controller : 'statsController'
        })

}]);