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

    $scope.getAnswerCount = function () {

    };

    $scope.getPreRegister = function () {

    };
    $emailList = {};
    $scope.getEmailList = function ($eventNum) {
        $http({
            method: 'GET',
            url: 'models/webModelAPI.php/getRegistrantNameEmail/' + $eventNum,
            data:{},
            headers: {'Content-Type': 'application/json'}
        })

            .success(function (data) {

                var csv = JSON2CSV(data);
                var downloadLink = document.createElement("a");
                var blob = new Blob(["\ufeff", csv]);
                var url = URL.createObjectURL(blob);
                downloadLink.href = url;
                downloadLink.download = $scope.eventObj.eventName + "emailList.csv";

                document.body.appendChild(downloadLink);
                downloadLink.click();
                document.body.removeChild(downloadLink);
            });
    };
    $scope.getRegList = function ($eventNum) {
        $http({
            method: 'GET',
            url: 'models/webModelAPI.php/getRegistrantCSVDump/' + $eventNum,
            data:{},
            headers: {'Content-Type': 'application/json'}
        })

            .success(function (data) {

                var csv = JSON2CSV(data);
                var downloadLink = document.createElement("a");
                var blob = new Blob(["\ufeff", csv]);
                var url = URL.createObjectURL(blob);
                downloadLink.href = url;
                downloadLink.download = $scope.eventObj.eventName + "RegList.csv";

                document.body.appendChild(downloadLink);
                downloadLink.click();
                document.body.removeChild(downloadLink);
            });
    };

function JSON2CSV(objArray) {
    var array = objArray;
    var str = '';
    var line = '';

    if ($("#labels").is(':checked')) {
        var head = array[0];
        if ($("#quote").is(':checked')) {
            for (var index in array[0]) {
                var value = index + "";
                line += '"' + value.replace(/"/g, '""') + '",';
            }
        } else {
            for (var index in array[0]) {
                line += index + ',';
            }
        }

        line = line.slice(0, -1);
        str += line + '\r\n';
    }

    for (var i = 0; i < array.length; i++) {
        var line = '';

        if ($("#quote").is(':checked')) {
            for (var index in array[i]) {
                var value = array[i][index] + "";
                line += '"' + value.replace(/"/g, '""') + '",';
            }
        } else {
            for (var index in array[i]) {
                line += array[i][index] + ',';
            }
        }

        line = line.slice(0, -1);
        str += line + '\r\n';
    }
    return str;
}

}]);