/**
 * Created by Tim Unger on 1/13/2016.
 */

app.controller( 'createEventsController', ['$scope', '$http', function($scope, $http) {

    $scope.createEventForm = {eventName: "", eventDate: new Date(), startTime: null, endTime: null,
                              preRegistration: 'false', siteHeader: "", customQuestions: 'false'};

    $scope.createEvent = function () {

        $scope.createEventForm.eventDate = $scope.createEventForm.eventDate.toDateString();
        $scope.createEventForm.startTime = $scope.createEventForm.startTime.toTimeString();
        $scope.createEventForm.endTime = $scope.createEventForm.endTime.toTimeString();
        $scope.createEventForm.preRegistration = $scope.createEventForm.preRegistration.toString();
        $scope.createEventForm.customQuestions = $scope.createEventForm.customQuestions.toString();

        $http({
            method: 'POST',
            url: 'models/webModelAPI.php/createEvent',
            data: JSON.stringify($scope.createEventForm),
            headers: {'Content-Type': 'application/json'}
        })
            .success(function(data) {

                if (!data.success) {
                    $scope.created = data.error;
                } else {
                    $scope.created = 'Event Created';
                    if($scope.createEventForm.preRegistration == "true" && $scope.createEventForm.customQuestions == "true") {
                        window.location = "#/PreRegistrationSetup";
                    }
                    $scope.createEventForm = {
                        eventName: "", eventDate: new Date(),
                        startTime: null, endTime: null,
                        preRegistration: 'false', siteHeader: "", customQuestions: 'false'
                    };

                }
            });

    };
    //From UI Bootstrap Website
    $scope.ismeridian = true;
    $scope.toggleMode = function() {
        $scope.ismeridian = ! $scope.ismeridian;
    };
}]);

