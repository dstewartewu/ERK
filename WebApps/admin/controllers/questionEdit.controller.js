/**
 * Created by Tim Unger on 1/18/2016.
 */


app.controller( 'questionEditController', ['$scope', '$http', function($scope, $http) {
    $scope.eventsList = {eventNum: 0, eventName: "", eventDate: null, startTime: null, endTime: null,
        preReg: false, siteHeader: "", cusQuest: false};

    $scope.key={key: null};

    $scope.questionsList = {eventNum: 0, question : '', questionID : 0};

    // Loads Events into a dropdown menu
    $scope.getEvents = function () {

        $http({
            method: 'GET',
            url: 'models/webModelAPI.php/getEventsWithPreRegList',
            data: null,
            headers: {'Content-Type': 'application/json'}
        })
            .success(function (data) {
                $scope.message = data;
            })
    };
    // Loads questions into a dropdown menu
    $scope.getQuestions = function(){
        $scope.key = {key: null, eventNum: 0};
        $scope.key.key = document.getElementById('key').value;
        $scope.key.eventNum = $scope.eventsList.eventNum;

        $http({
            method: 'POST',
            url: 'models/webModelAPI.php/getQuestions',
            data: $scope.key,
            headers: {'Content-Type': 'application/json'}
        })
            .success(function (data) {
                $scope.questions = data;
            })


    };

    $scope.choices = [];
    $scope.newChoice = {choice: "Click Add for New Choice", eventNum: 0, questionID: 0, choiceNum:0, key: null};
    $scope.error = "";

    $scope.loadChoices = function() {
        $scope.key = {key: null, eventNum: 0, questionNum: 0};
        $scope.key.key = document.getElementById('key').value;
        $scope.key.eventNum = $scope.eventsList.eventNum;
        $scope.key.questionNum = $scope.questionsList.questionID;
        $scope.newChoice.eventNum = $scope.eventsList.eventNum;
        $scope.newChoice.questionID = $scope.questionsList.questionID;
        $scope.newChoice.key = document.getElementById('key').value;
        $scope.newChoice.choice = "Click Add for New Choice";

        $http({
            method: 'POST',
            url: 'models/webModelAPI.php/getChoices',
            data: $scope.key,
            headers: {'Content-Type': 'application/json'}
        })

            .success(function (data) {
                $scope.choices = data;
            });
    };

    $scope.updateQuestion = function() {
        $scope.questionsList.key = document.getElementById('key').value;
        $http({
            method: 'POST',
            url: 'models/webModelAPI.php/updateQuestion',
            data: $scope.questionsList,
            headers: {'Content-Type': 'application/json'}
        });

    };


    $scope.deleteQuestion = function(){
        $scope.questionsList.key = document.getElementById('key').value;
        $http({
            method: 'POST',
            url: 'models/webModelAPI.php/deleteQuestion',
            data: $scope.questionsList,
            headers: {'Content-Type': 'application/json'}
        })
        .success(function(data){
            $scope.getQuestions();
            $scope.loadChoices();
        })
    };

    $scope.editing = null;
    $scope.editItem = function(item) {
        $scope.editing = item;
    };

    $scope.deleteChoice = function(item){
        item.key = document.getElementById('key').value;
        $http({
            method: 'POST',
            url: 'models/webModelAPI.php/deleteChoice',
            data: item,
            headers: {'Content-Type': 'application/json'}
        })
            .success(function (data) {
                $scope.loadChoices();
                $scope.error = data;
            });
    };

    $scope.submitChange = function(){
        if($scope.editing.choiceNum !== 0) {
            $scope.editing.key = document.getElementById('key').value;
            $http({
                method: 'POST',
                url: 'models/webModelAPI.php/updateChoice',
                data: $scope.editing,
                headers: {'Content-Type': 'application/json'}
            })
                .success(function (data) {
                    $scope.loadChoices();
                });
        }
        else {
            $http({
                method: 'POST',
                url: 'models/webModelAPI.php/addChoice',
                data: $scope.editing,
                headers: {'Content-Type': 'application/json'}
            })
                .success(function (data) {
                    $scope.loadChoices();
                });
        }
    };

    $scope.addQuestions = function(){
        if($scope.eventsList.eventNum !== 0) {
            window.location = "#/PreRegistrationSetup/" + $scope.eventsList.eventNum;
        }
    }
}]);