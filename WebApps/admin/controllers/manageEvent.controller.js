

app.controller( 'manageController', ['$scope', '$http', function($scope, $http) {
    $scope.message = "";
    $scope.key = {key: null};
    $scope.welcome = 'Welcome to EWU Career Services Event Management, Username!';
    $scope.getEvents = function () {
        $scope.key.key = document.getElementById('key').value;
        $http({
            method: 'POST',
            url: 'models/webModelAPI.php/getEventsList',
            data: $scope.key,
            headers: {'Content-Type': 'application/json'}
        })
            .success(function (data) {
                //Change data back to proper format to populate form
                for (var i = 0; i < data.length; i++) {
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
        preReg: false, siteHeader: "", cusQuest: false, key: null};

    $scope.updateEvent = function () {
        $scope.updateEventForm.key = document.getElementById('key').value;

        $scope.submitUpdate = jQuery.extend(true, {}, $scope.updateEventForm);
        //Convert Data to Strings that are human read-able before storing.
        $scope.submitUpdate.eventDate = $scope.submitUpdate.eventDate.toDateString();

        if($scope.submitUpdate.preReg){
            $scope.submitUpdate.preReg = 'true';
        }
        else{
            $scope.submitUpdate.preReg = 'false';
        }
        if($scope.submitUpdate.cusQuest){
            $scope.submitUpdate.cusQuest = 'true';
        }
        else{
            $scope.submitUpdate.cusQuest = 'false';
        }

        $http({
            method: 'POST',
            url: 'models/webModelAPI.php/updateEvent',
            data: $scope.submitUpdate,
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

    $scope.deleteEvent = function() {
        $scope.updateEventForm.key = $scope.updateEventForm.key = document.getElementById('key').value;

        $http({
            method: 'POST',
            url: 'models/webModelAPI.php/deleteEvent',
            data: $scope.updateEventForm,
            headers: {'Content-Type': 'application/json'}
        })
            .success(function(data) {

                if (!data.success) {
                    $scope.changed = data.error;
                } else {
                    $scope.changed = 'Event Deleted';
                }
            });
        $scope.getEvents();
    }
}]);

