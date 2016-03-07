/**
 * Created by Tim Unger on 1/13/2016.
 */

app.controller( 'kioskController', ['$scope', '$http', function($scope, $http) {

    $scope.eventObj = {eventNum: 0, eventName: "", eventDate: null, startTime: null, endTime: null,
        preReg: false, siteHeader: "", cusQuest: false, key: null};

    $scope.getEvents = function () {
        $scope.eventObj.key = document.getElementById('key').value;
        $http({
            method: 'POST',
            url: 'models/webModelAPI.php/getEventsWithPreRegList',
            data: $scope.eventObj,
            headers: {'Content-Type': 'application/json'}
        })
            .success(function (data) {
                $scope.message = data;
            })
    };

    $scope.regKey = '';

    $scope.eventReg = {eventNum: 0, regKey: '', expire: '', key: null};

    $scope.generateRegistrationCode = function()
    {
        $scope.eventReg.eventNum = $scope.eventObj.eventNum;
        $scope.eventReg.key = document.getElementById('key').value;
        $http({
            method: 'POST',
            url: 'models/webModelAPI.php/RegisterKiosk',
            data: $scope.eventReg,
            headers: {'Content-Type': 'application/json'}
        })
            .success(function (data) {
                $scope.eventReg = data;

            })
    };
    $scope.deleteEvent = function()
    {
        $scope.eventObj.key = document.getElementById('key').value;
        $http({
            method: 'POST',
            url: 'models/webModelAPI.php/deleteEvent',
            data: $scope.eventObj,
            headers: {'Content-Type': 'application/json'}
        })
            .success(function (data) {
                $scope.message = data;
            })
    };
}]);