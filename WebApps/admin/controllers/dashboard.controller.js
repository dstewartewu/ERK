
app.controller( 'dashboardController', ['$scope', '$http', function($scope, $http) {
    $scope.message = "";
    $scope.welcome = 'Welcome to EWU Career Services Event Management!';
    $scope.key = {key: null};
    $scope.getEvents = function () {
        $scope.key.key = document.getElementById('key').value;
        $http({
            method: 'POST',
            url: 'models/webModelAPI.php/getEventsList',
            data: $scope.key,
            headers: {'Content-Type': 'application/json'}
        })
            .success(function (data) {

                $scope.message = data;
                if($scope.message.length == 0){
                    $scope.message = [{ eventName: 'No Events are Currently Scheduled!' , eventDate: ''}];
                }
            })
    };

    $scope.getEvents();
}]);