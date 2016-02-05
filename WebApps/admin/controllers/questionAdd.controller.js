/**
 * Created by Tim Unger on 1/17/2016.
 */


app.controller( 'questionAddController', ['$scope', '$routeParams', '$http', function($scope, $routeParams, $http) {

    $scope.eventNum = $routeParams.eventNum;
    $scope.questionCounter = 1;
    $scope.question = {question: null, questionID : $scope.questionCounter, eventNum : $scope.eventNum};
    $scope.choice1 = {choice: null, questionID : $scope.questionCounter, eventNum : $scope.eventNum };
    $scope.choice2 = {choice: null, questionID : $scope.questionCounter, eventNum : $scope.eventNum };
    $scope.choice3 = {choice: null, questionID : $scope.questionCounter, eventNum : $scope.eventNum };
    $scope.choice4 = {choice: null, questionID : $scope.questionCounter, eventNum : $scope.eventNum };
    $scope.choice5 = {choice: null, questionID : $scope.questionCounter, eventNum : $scope.eventNum };
    $scope.choice6 = {choice: null, questionID : $scope.questionCounter, eventNum : $scope.eventNum };
    $scope.success = false;

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
                    $scope.addChoices();

                }
            });

    };

    $scope.addChoices = function () {
        try {
            $http({
                method: 'POST',
                url: 'models/webModelAPI.php/addChoice',
                data: JSON.stringify($scope.choice1),
                headers: {'Content-Type': 'application/json'}
            });


            $http({
                method: 'POST',
                url: 'models/webModelAPI.php/addChoice',
                data: JSON.stringify($scope.choice2),
                headers: {'Content-Type': 'application/json'}
            });

            if($scope.choice3 !== "" && $scope.choice3 !== null) {
                $http({
                    method: 'POST',
                    url: 'models/webModelAPI.php/addChoice',
                    data: JSON.stringify($scope.choice3),
                    headers: {'Content-Type': 'application/json'}
                });
            }

            if($scope.choice4 !== "" && $scope.choice4 !== null) {
                $http({
                    method: 'POST',
                    url: 'models/webModelAPI.php/addChoice',
                    data: JSON.stringify($scope.choice4),
                    headers: {'Content-Type': 'application/json'}
                });
            }

            if($scope.choice5 !== "" && $scope.choice5 !== null) {
                $http({
                    method: 'POST',
                    url: 'models/webModelAPI.php/addChoice',
                    data: JSON.stringify($scope.choice5),
                    headers: {'Content-Type': 'application/json'}
                });
            }

            if($scope.choice6 !== "" && $scope.choice6 !== null) {
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

        $scope.questionCounter++;
        $scope.created = $scope.questionCounter;
        $scope.resetForm();



    };

    $scope.resetForm = function(){
        $scope.question = {question: null, questionID : $scope.questionCounter, eventNum : $scope.eventNum};
        $scope.choice1 = {choice: null, questionID : $scope.questionCounter, eventNum : $scope.eventNum };
        $scope.choice2 = {choice: null, questionID : $scope.questionCounter, eventNum : $scope.eventNum };
        $scope.choice3 = {choice: null, questionID : $scope.questionCounter, eventNum : $scope.eventNum };
        $scope.choice4 = {choice: null, questionID : $scope.questionCounter, eventNum : $scope.eventNum };
        $scope.choice5 = {choice: null, questionID : $scope.questionCounter, eventNum : $scope.eventNum };
        $scope.choice6 = {choice: null, questionID : $scope.questionCounter, eventNum : $scope.eventNum };
        $scope.success = false;
    }

}]);