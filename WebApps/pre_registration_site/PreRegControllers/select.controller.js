app.controller( 'selectController', ['$scope', '$http', function($scope, $http) {

    $scope.eventObj = {
        eventNum: 0, eventName: "", eventDate: null, startTime: null, endTime: null,
        preReg: false, siteHeader: "", cusQuest: false
    };

    $scope.getEvents = function () {
        $http({
            method: 'GET',
            url: 'admin/models/webModelAPI.php/getEventsWithPreRegList',
            data: {},
            headers: {'Content-Type': 'application/json'}
        })
            .success(function (data) {
                $scope.message = data;
            })
    };
    //Sets Event Header and redirects to the pre-reg form.
    $scope.redirectPreReg = function(){
        document.getElementById('EventText').innerText = $scope.eventObj.siteHeader;

        window.location = '#PreRegister/' + $scope.eventObj.eventNum;
    }
}]);
