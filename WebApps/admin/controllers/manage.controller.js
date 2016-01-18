

app.controller( 'manageController', ['$scope', '$http', function($scope, $http) {
    $scope.message = "";
    $scope.welcome = 'Welcome to EWU Career Services Event Management, Username!';
    $scope.getEvents = function () {
        $http({
            method: 'GET',
            url: 'models/webModelAPI.php/getEventsList',
            data: {},
            headers: {'Content-Type': 'application/json'}
        })
            .success(function (data) {
                //Change data back to proper format to populate form
                for (var i = 0; i < data.length; i++) {
                    data[i].startTime = new Date(data[i].eventDate + " " + data[i].startTime);
                    data[i].endTime = new Date(data[i].eventDate + " " + data[i].endTime);
                    data[i].eventDate = new Date(data[i].eventDate);
                    if (data[i].preReg == 'true') {
                        data[i].preReg = true;
                    }
                    else {
                        data[i].preReg = false;
                    }
                    if (data[i].cusQuest == 'true') {
                        data[i].cusQuest = true;
                    }
                    else {
                        data[i].cusQuest = false;
                    }
                }
                $scope.message = data;
            })
    };

    $scope.updateEventForm = {eventNum: 0, eventName: "", eventDate: null, startTime: null, endTime: null,
        preRegistration: false, siteHeader: "", customQuestions: false};

    $scope.updateEvent = function () {
        //Convert Data to Strings that are human read-able before storing.
        $scope.updateEventForm.eventDate = $scope.updateEventForm.eventDate.toDateString();
        $scope.updateEventForm.startTime = $scope.updateEventForm.startTime.toTimeString();
        $scope.updateEventForm.endTime = $scope.updateEventForm.endTime.toTimeString();
        $scope.createEventForm.preRegistration = $scope.createEventForm.preRegistration.toString();
        $scope.createEventForm.customQuestions = $scope.createEventForm.customQuestions.toString();

        $http({
            method: 'POST',
            url: 'models/webModelAPI.php/updateEvent',
            data: JSON.stringify($scope.updateEventForm),
            headers: {'Content-Type': 'application/json'}
        })
            .success(function(data) {

                if (!data.success) {
                    $scope.changed = data.error;
                } else {
                    $scope.changed = 'Event Changed';
                }
            });

    };
    //From UI Bootstrap Website - allows for 12hr clock
    $scope.ismeridian = true;
    $scope.toggleMode = function() {
        $scope.ismeridian = ! $scope.ismeridian;
    };
}]);