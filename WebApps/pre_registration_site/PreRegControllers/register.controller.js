
app.controller( 'registerController', ['$scope', '$routeParams', '$http', function($scope, $routeParams, $http) {
    $scope.eventNum = $routeParams.eventNum;


    <!-- Will show the student section and hide the employer section -->
    $scope.IfStudent = function(){
        document.getElementById("studentInfo").style.display = 'block';

    };
    <!-- Will hide the employer and student section. Usually called when page is first loaded -->
    $scope.IfGeneral = function(){
        document.getElementById("studentInfo").style.display = 'none';
        resetStudent();
    };

    <!-- Will check to see what university is selected. If other is selected, it will display the 'other' input box. If not, it will hide it -->
    function CheckUniversity(){
        list = document.getElementById("college");
        var university = list.options[list.selectedIndex].value;
        if(university == "Other"){
            document.getElementById("otherUniversity").style.display = 'block';
        }
        else{
            document.getElementById("otherUniversity").style.display = 'none';
        }
    }

    $scope.questionsResponse={};
    $scope.getData = function(){
        jQuery.support.cors = true;

        $http({
            method: 'GET',
            url: '/pre_registration_site/PreRegModels/questions.php?eventNum='+$scope.eventNum,
                data: null,
                headers: {'Content-Type': 'application/json'}
        })
            .success(function(data) {

                angular.forEach(data, function (val) {

                    //If there is a question, show the legend and fieldset
                    document.getElementById("questions").style.display = 'block';
                    document.getElementById("questionsLegend").style.display = 'block';

                    var string = '<input type="hidden" name=Q' + val.questionID + ' value="' + val.question + '"><div class="form-group">' + val.question;

                    getAnswers(val.questionID, string);

                });
            });

    };



    function getAnswers(questionID, string){
        jQuery.support.cors = true;
        $http({
            method: 'GET',
            url: '/pre_registration_site/PreRegModels/answers.php?eventNum='+$scope.eventNum+'&questionID='+questionID,
            data: null,
            headers: {'Content-Type': 'application/json'}
        })
            .success(function(data) {
                string += "<select class='form-control' name=A" + questionID + ">";

                angular.forEach(data, function (val) {
                    string += '<option>' + val.choice + '</option>';
                });
                string += "</select></div>";
                //document.getElementById('divButton').appendChild(string);
                var question = $('#divButton').append(string);

            });
    }

    <!-- Will check if the info entered is valid. If valid, it will submit the info. If not it will stay on page. -->
    $scope.validate = function()
    {
        var isValid = true;
        var hasAlerted = false;

        //Start general info validation

        //Check if user has entered a valid firstname
        fName = document.getElementById("fName");
        if(fName == null || fName.value == "" || !isNaN(fName.value)) //If true, info is invalid
        {
            fName.style.backgroundColor = "red";
            alert("Please enter your first name using only letters");
            hasAlerted = true;
            isValid = false;
        }
        else
        {	//info is valid, change color back to white and capitalize first character

            fName.style.backgroundColor = "white";
        }

        //Check if user has entered a valid lastname
        lName = document.getElementById("lName");
        if(lName == null || lName.value == "" || !isNaN(lName.value)) //If true, info is invalid
        {
            lName.style.backgroundColor = "red";
            isValid = false;
            if(!hasAlerted) {
                alert("Please enter your last name using only letters");
                hasAlerted = true;
            }
        }
        else
        {   //info is valid, change color back to white and capitalize first character

            lName.style.backgroundColor = "white";
        }

        var radios = document.getElementsByName("registrantType");
        if(!checkRadios(radios)) //if user hasn't selected a registrant type (i.e. Student)
        {
            isValid = false;
            if(!hasAlerted)
            {
                alert("Please select your registrant type");
                hasAlerted = true;
            }
        }

        //Check if user has entered a valid email
        email = document.getElementById("email");
        if(email == null || email.value == "" || !checkEmail(email)) //if true, info is invalid
        {
            email.style.backgroundColor = "red";
            isValid = false;
            if(!hasAlerted) { alert("Please enter a valid email");
                hasAlerted = true; }
        }
        else
        {   //info is valid, change color back to white and capitalize first character
            email.value = capitalizeFirstLetter(email.value);
            email.style.backgroundColor = "white";
        }

        //End general info validation

        //Start Student/Alumnus info validation

        //Check if user has selected a class standing
        if(document.getElementById('Student').checked){
            var classStanding = document.getElementsByName("Class");
            if(!checkRadios(classStanding)) //If true, user hasn't selected a class standing (i.e. Freshman)
            {
                isValid = false;
                if(!hasAlerted)
                {
                    alert("Please select your class standing");
                    hasAlerted = true;
                }
            }

            //Check if user has entered a valid college
            list = document.getElementById("college");
            var university = list.options[list.selectedIndex].value;
            if(university == "Other"){
                var other = document.getElementById("otherUniversity");
                if(other.value == "" || other.value == "Please type in your college here") //If true, users college isn't listed and the user hasn't entered in their college
                {
                    isValid = false;
                    other.style.backgroundColor = "red";
                    if(!hasAlerted)
                    {
                        alert("Please type in your college");
                        hasAlerted = true;
                    }
                }
                else
                {
                    other.value = toTitleCase(other.value);
                    other.style.backgroundColor = "white";
                }
            }

            //Check if user has entered a valid major
            var major = document.getElementById("major");
            if(major == null || major.value == "" || !isNaN(major.value)) //If true, users college isn't listed and the user hasn't entered in their college
            {
                isValid = false;
                major.style.backgroundColor = "red";
                if(!hasAlerted)
                {
                    alert("Please enter your major");
                    hasAlerted = true;
                }
            }
            else
            {
                major.value = toTitleCase(major.value);
                major.style.backgroundColor = "white";
            }


            //Check is user has entered a valid student ID
            studentID = document.getElementById("id");
            if(!studentID.value == "" && isNaN(studentID.value))
            {
                studentID.style.backgroundColor = "red";
                isValid = false;
                if(!hasAlerted)
                {
                    alert("Please enter student ID using only numeric values");
                    hasAlerted = true;
                }
            }
            else
            {
                //info is valid, change color back to white
                studentID.style.backgroundColor = "white";
            }

            //Check if user has entered a valid graduation year
            graduation = document.getElementById("graduationYear");
            if(graduation == null || graduation.value == "" || isNaN(graduation.value) || graduation.value.length != 4) //If true, info is invalid
            {
                graduation.style.backgroundColor = "red";
                isValid = false;
                if(!hasAlerted)
                {
                    alert("Please enter your graduation year using only four numeric values");
                    hasAlerted = true;
                }
            }
            else
            {
                //info is valid, change color back to white
                graduation.style.backgroundColor = "white";
            }
        }

        //End Student/Alumnus info validation

        return isValid;
    };

}]);


function resetStudent(){
}

function checkEmail(email)
{
    if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(email.value))
    {
        return (true)
    }
    return (false)
}


function checkRadios(radios){
    for(var i = 0, len = radios.length; i < len; i++){
        if(radios[i].checked){
            return true;
        }
    }
    return false;
}


function toTitleCase(str)
{
    return str.replace(/\w\S*/g, function(txt){return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase();});
}

function capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.substr(1).toLowerCase();
    //return str.replace(/\w\S*/g, function(txt){return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase();});
}
