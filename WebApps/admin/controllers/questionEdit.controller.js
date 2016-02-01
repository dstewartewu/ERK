/**
 * Created by Tim Unger on 1/18/2016.
 */


app.controller( 'questionEditController', ['$scope', '$http', function($scope, $http) {
    $scope.eventsList = {eventNum: 0, eventName: "", eventDate: null, startTime: null, endTime: null,
        preReg: false, siteHeader: "", cusQuest: false};

    $scope.questionsList = {eventNum: 0, question : '', questionID : 0};

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

    $scope.getQuestions = function($eventNum){
        $http({
            method: 'GET',
            url: 'models/webModelAPI.php/getQuestions/' + $eventNum,
            data: {},
            headers: {'Content-Type': 'application/json'}
        })
            .success(function (data) {
                $scope.questions = data;
            })


    };

    $scope.choices = {};
    $scope.newChoice = {choice: "", eventNum: 0, questionID: 0};

    $scope.loadChoices = function($eventNum, $questionNum) {
        $http({
            method: 'GET',
            url: 'models/webModelAPI.php/getChoices/' + $eventNum  + '/' + $questionNum,
            data: {},
            headers: {'Content-Type': 'application/json'}
        })

            .success(function (data) {
                $scope.choices = data;
            });
        $scope.newChoice.eventNum = $eventNum;
        $scope.newChoice.questionID = $questionNum;
        $scope.newChoice.choice = "";
    };

    $scope.updateQuestion = function() {
        $http({
            method: 'POST',
            url: 'models/webModelAPI.php/updateQuestion',
            data: $scope.questionsList,
            headers: {'Content-Type': 'application/json'}
        });
        if($scope.newChoice.choice !== ""){
            $scope.choices.push($scope.newChoice);
        }
        $scope.choices.forEach(function($choice) {
            $http({
                method: 'POST',
                url: 'models/webModelAPI.php/updateChoice',
                data: $choice,
                headers: {'Content-Type': 'application/json'}
            })
        });

    };
}]);