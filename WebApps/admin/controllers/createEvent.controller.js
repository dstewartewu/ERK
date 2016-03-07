/**
 * Created by Tim Unger on 1/13/2016.
 */

app.controller( 'createEventsController', ['$scope', '$http', function($scope, $http) {

    $scope.createEventForm = {eventName: null, eventDate: new Date(), startTime: null, endTime: null,
                              preRegistration: false, siteHeader: null, customQuestions: false, key: null};

    $scope.createEvent = function () {

        //Creates a deep copy of the createEventForm object in order to keep the 2-way binding from breaking
        $scope.createEventForm.key = document.getElementById('key').value;
        $scope.submitEvent = jQuery.extend(true, {}, $scope.createEventForm);

        //Below, form values are converted to human readable input for db.

        $scope.submitEvent.eventDate = $scope.submitEvent.eventDate.toDateString();
        $scope.submitEvent.startTime = $scope.submitEvent.startTime.toTimeString();
        $scope.submitEvent.endTime = $scope.submitEvent.endTime.toTimeString();
        if($scope.submitEvent.preRegistration){
            $scope.submitEvent.preRegistration = 'true';
        }
        else{
            $scope.submitEvent.preRegistration = 'false';
        }
        if($scope.submitEvent.customQuestions){
            $scope.submitEvent.customQuestions = 'true';
        }
        else{
            $scope.submitEvent.customQuestions = 'false';
        }
        $http({
            method: 'POST',
            url: 'models/webModelAPI.php/createEvent',
            data: $scope.submitEvent,
            headers: {'Content-Type': 'application/json'}
        })
            .success(function(data) {

                if (!data.success) {
                    $scope.created = data.error;
                } else {
                    $scope.created = 'Event Created';
                    if($scope.createEventForm.preRegistration && $scope.createEventForm.customQuestions) {
                        window.location = "#/PreRegistrationSetup/" + data.eventNum;
                    }
                    $scope.createEventForm = {
                        eventName: "", eventDate: new Date(),
                        startTime: null, endTime: null,
                        preRegistration: false, siteHeader: "", customQuestions: false
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

