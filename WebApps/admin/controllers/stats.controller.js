app.controller( 'statsController', ['$scope', '$http', function($scope, $http) {

    $scope.eventObj = {
        eventNum: 0, eventName: "", eventDate: null, startTime: null, endTime: null,
        preReg: false, siteHeader: "", cusQuest: false
    };

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
}]);
