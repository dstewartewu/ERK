/**
 * Created by Tim Unger on 1/10/2016.
 */

app.controller( 'usersController', ['$scope', '$http', function($scope, $http) {

    $scope.key = document.getElementById('key').value;

    $scope.adminAdd = {key: ""};
    $scope.adminAdd.key = $scope.key;

    $scope.adminObj = {userName:"", key: ""};
    $scope.adminObj.key = $scope.key;

    $scope.addAdmin = function () {
        $http({
            method: 'POST',
            url: 'models/webModelAPI.php/addAdmin',
            data: $scope.adminAdd,
            headers: {'Content-Type': 'application/json'}
        })

            .success(function (data) {
                $scope.adminAdd.userName = "";
                $scope.getAdmins();
            });

};

    $scope.getAdmins = function () {
        $http({
            method: 'POST',
            url: 'models/webModelAPI.php/getAdmin',
            data: $scope.adminAdd,
            headers: {'Content-Type': 'application/json'}
        })

            .success(function (data) {
                $scope.message = data;
            });
    };

    $scope.deleteAdmin = function () {
        $scope.adminObj.key = $scope.key;
        $http({
            method: 'POST',
            url: 'models/webModelAPI.php/deleteAdmin',
            data: $scope.adminObj,
            headers: {'Content-Type': 'application/json'}
        })

            .success(function (data) {
                $scope.getAdmins();
            });

    };

}]);