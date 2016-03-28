/**
 * Created by Tim Unger on 1/17/2016.
 */


app.controller( 'questionAddController', ['$scope', '$routeParams', '$http', function($scope, $routeParams, $http) {

    $scope.eventNum = $routeParams.eventNum;
    $scope.question = {question: null, eventNum : $scope.eventNum, key: null};
    $scope.choice1 = {choice: null, eventNum : $scope.eventNum, key: null};
    $scope.choice2 = {choice: null, eventNum : $scope.eventNum, key: null};
    $scope.choice3 = {choice: null, eventNum : $scope.eventNum, key: null};
    $scope.choice4 = {choice: null, eventNum : $scope.eventNum, key: null};
    $scope.choice5 = {choice: null, eventNum : $scope.eventNum, key: null};
    $scope.choice6 = {choice: null, eventNum : $scope.eventNum, key: null};
    $scope.success = false;
    $scope.question.key = document.getElementById('key').value;
    $scope.choice1.key = document.getElementById('key').value;
    $scope.choice2.key = document.getElementById('key').value;
    $scope.choice3.key = document.getElementById('key').value;
    $scope.choice4.key = document.getElementById('key').value;
    $scope.choice5.key = document.getElementById('key').value;
    $scope.choice6.key = document.getElementById('key').value;
    $scope.lastQuestionAdded = 0;
    $scope.addQuestion = function () {
        $http({
            method: 'POST',
            url: 'models/webModelAPI.php/addQuestion',
            data: JSON.stringify($scope.question),
            headers: {'Content-Type': 'application/json'}
        })
            .success(function (data) {

                if (!data.success) {
                    $scope.created = data.error;
                } else {
                    $scope.success = true;
                    $scope.lastQuestionAdded = data.questionID;
                    $scope.addChoices();
                    $scope.created = "Question Added Successfully!"
                }
            });

    };

    $scope.addChoices = function () {

        try {
            $scope.choice1.questionID = $scope.lastQuestionAdded;
            $http({
                method: 'POST',
                url: 'models/webModelAPI.php/addChoice',
                data: JSON.stringify($scope.choice1),
                headers: {'Content-Type': 'application/json'}
            });

            $scope.choice2.questionID = $scope.lastQuestionAdded;
            $http({
                method: 'POST',
                url: 'models/webModelAPI.php/addChoice',
                data: JSON.stringify($scope.choice2),
                headers: {'Content-Type': 'application/json'}
            });

            if($scope.choice3.choice !== "" && $scope.choice3.choice !== null) {
                $scope.choice3.questionID = $scope.lastQuestionAdded;
                $http({
                    method: 'POST',
                    url: 'models/webModelAPI.php/addChoice',
                    data: JSON.stringify($scope.choice3),
                    headers: {'Content-Type': 'application/json'}
                });
            }

            if($scope.choice4.choice !== "" && $scope.choice4.choice !== null) {
                $scope.choice4.questionID = $scope.lastQuestionAdded;
                $http({
                    method: 'POST',
                    url: 'models/webModelAPI.php/addChoice',
                    data: JSON.stringify($scope.choice4),
                    headers: {'Content-Type': 'application/json'}
                });
            }

            if($scope.choice5.choice !== "" && $scope.choice5.choice !== null) {
                $scope.choice5.questionID = $scope.lastQuestionAdded;
                $http({
                    method: 'POST',
                    url: 'models/webModelAPI.php/addChoice',
                    data: JSON.stringify($scope.choice5),
                    headers: {'Content-Type': 'application/json'}
                });
            }

            if($scope.choice6.choice !== "" && $scope.choice6.choice !== null) {
                $scope.choice6.questionID = $scope.lastQuestionAdded;
                $http({
                    method: 'POST',
                    url: 'models/webModelAPI.php/addChoice',
                    data: JSON.stringify($scope.choice6),
                    headers: {'Content-Type': 'application/json'}
                });
            }

        }
        catch(err){
            $scope.created = err;
        }

        $scope.resetForm();



    };

    $scope.resetForm = function(){
        $scope.question = {question: null, eventNum : $scope.eventNum, key: document.getElementById('key').value};
        $scope.choice1 = {choice: null, eventNum : $scope.eventNum, key: document.getElementById('key').value};
        $scope.choice2 = {choice: null, eventNum : $scope.eventNum, key: document.getElementById('key').value};
        $scope.choice3 = {choice: null, eventNum : $scope.eventNum, key: document.getElementById('key').value};
        $scope.choice4 = {choice: null, eventNum : $scope.eventNum, key: document.getElementById('key').value};
        $scope.choice5 = {choice: null, eventNum : $scope.eventNum, key: document.getElementById('key').value};
        $scope.choice6 = {choice: null, eventNum : $scope.eventNum, key: document.getElementById('key').value};
        $scope.success = false;
    }

}]);