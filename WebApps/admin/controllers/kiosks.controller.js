/**
 * Created by Tim Unger on 1/13/2016.
 */

app.controller( 'kioskController', ['$scope', '$http', function($scope, $http) {

    $scope.eventObj = {eventNum: 0, eventName: "", eventDate: null, startTime: null, endTime: null,
        preReg: false, siteHeader: "", cusQuest: false};

    $scope.getEvents = function () {
        $http({
            method: 'GET',
            url: 'models/webModelAPI.php/getEventsWithPreRegList',
            data: {},
            headers: {'Content-Type': 'application/json'}
        })
            .success(function (data) {
                $scope.message = data;
            })
    };

    $scope.regKey = '';

    $scope.eventReg = {eventNum: 0, key: '', expire: ''};

    $scope.generateRegistrationCode = function()
    {
        $scope.eventReg.eventNum = $scope.eventObj.eventNum;
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
}]);