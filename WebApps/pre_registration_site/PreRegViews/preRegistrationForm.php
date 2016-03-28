<?php
echo '
	<html>
		<head>
			<link href="../PreRegCSS/jobfair.css" rel="stylesheet">
		</head>
		<body>
				<img src="../PreRegImages/ewu_banner.jpg" width="40%" alt=""/><hr/>
		</body>
	</html>';


init();

function init()
{
    //Check if submit button was pressed
    if (isset($_POST['submit'])) {
        //Get all the information from the html form and sanitize it
        $Fname = filter_var($_POST["fName"], FILTER_SANITIZE_STRING);
        $Lname = filter_var($_POST["lName"], FILTER_SANITIZE_STRING);
        $registrant = filter_var($_POST["registrantType"], FILTER_SANITIZE_STRING);
        $email = filter_var($_POST["email"], FILTER_VALIDATE_EMAIL);
        $eventNum = filter_var($_POST["eventNum"], FILTER_SANITIZE_NUMBER_INT);

        if ($registrant == "Student") { //Check is user was a student
            $class = filter_var($_POST["Class"], FILTER_SANITIZE_STRING);
            $college = filter_var($_POST["college"], FILTER_SANITIZE_STRING);
            $major = filter_var($_POST["major"], FILTER_SANITIZE_STRING);
            $other = filter_var($_POST["otherUniversity"], FILTER_SANITIZE_STRING);
        } else {
            //Set student info to null because user is not a student
            $id = null;
            $class = null;
            $college = null;
            $major = null;
            $other = null;
        }
        $attended = "No";
        $invalidCode = true;

        //Use variable result to hold all error checking. When there's an error, it will be appended to result and displayed at the end
        $result = '';
        //Check if firstname is only comprised of letters and is not an empty string
        if (preg_match('#[0-9]#', $Fname) || strlen($Fname) == 0) {
            //fail
            $result .= 'Firstname cannot be empty and must not contain any numbers or special characters<br/>';
        }
        //Check if lastname is only comprised of letters and is not an empty string
        if (preg_match('#[0-9]#', $Lname) || strlen($Lname) == 0) {
            //fail
            $result .= 'Last name cannot be empty and must not contain any numbers<br/>';
        }
        //Check if registrant type is valid
        if ($registrant == null || ($registrant != "Student" && $registrant != "General")) {
            //fail
            $result .= 'Must select a registrant type<br/>';
        }
        //Check if user entered a valid email
        if ($email == null || strlen($email) == 0 || !filter_var($email, FILTER_VALIDATE_EMAIL)) {
            //fail
            $result .= 'Enter a valid email address<br/>';
        }

        //User is a student
        if ($registrant == "Student") {
            //Check if student ID is valid
            if (!is_numeric($id) && strlen($id) != 0) {
                $result .= 'Id must contain only numeric values<br/>';
            }
            //Check if class standing is valid
            if ($class == null || ($class != "Freshman" && $class != "Sophomore" && $class != "Junior" && $class != "Senior" && $class != "PostBaccalaureate" && $class != "Graduate" && $class != "Alumnus")) {
                //fail
                $result .= 'Please select a class standing<br/>';
            }
            //Check if college is valid
            if ($college == null || $college == "") {
                //fail
                $result .= 'Please select a university<br/>';
            } else {
                if ($college == "Other") {
                    if ($other == "" || $other == null) {
                        $result .= 'Please enter a valid college<br/>';
                    }
                }
            }
            //Check if major is valid
            if ($major == null || $major == "") {
                $result .= 'Please enter a valid major<br/>';
            }
        }
        //If there are no errors. All info is valid
        if (empty($result)) {
            try {
                include '../../admin/models/config.php'; //Holds database information
                include '../../admin/models/DB.php';

            } catch (exception $e) {
                echo $e->getMessage();
            }

            try {
                while ($invalidCode) {
                    $code = generateCode(); //Get random code that will be primary key in database
                    $invalidCode = CheckCode($code, $eventNum); //Get if code already exists. If it does, find a new one
                }
                $time = date("Y-m-d H:i:s");
                $db = getDB(); //Create a connection
                $sql = $db->prepare("INSERT INTO registrant (fName, lName, email, checkedIn, registerDate, regType, codeNum, eventNum, major, college, classStanding)
                                 VALUES (:fName, :lName, :email, :checkedIn, :registerDate, :regType, :codeNum,  :eventNum, :major, :college, :classStanding)");
                $sql->bindValue(':codeNum', $code, PDO::PARAM_STR);
                $sql->bindValue(':registerDate', $time, PDO::PARAM_STR);
                $sql->bindValue(':checkedIn', $attended, PDO::PARAM_STR);
                $sql->bindParam(':fName', $Fname, PDO::PARAM_STR);
                $sql->bindParam(':lName', $Lname, PDO::PARAM_STR);
                $sql->bindParam(':email', $email, PDO::PARAM_STR);
                $sql->bindParam(':regType', $registrant, PDO::PARAM_STR);
                $sql->bindParam(':eventNum', $eventNum, PDO::PARAM_INT);
                $sql->bindParam(':major', $major, PDO::PARAM_STR);
                $sql->bindParam(':college', $college, PDO::PARAM_STR);
                $sql->bindParam(':classStanding', $class, PDO::PARAM_STR);

                $added = $sql->execute();

            } catch (exception $e) {
                return $e->getMessage();
            }
            //
            try {
                AddQuestionsAndAnswers($code, $eventNum);

                    //Bind the parameters for security purposes
                if ($added) { //This will display if everything goes correctly

                    session_start();
                    $_SESSION['code'] = $code;
                    $_SESSION['email'] = $email;

                   $emailSuccessful = require '../PreRegModels/email.php';
                    //session_write_close();
                    if ($emailSuccessful) {

                        $file = fopen('../temp/successRegistrationMessage.txt', 'r');
                        $pageText = fread($file, 25000);
                        echo nl2br($pageText) . '<br/>';
                        echo '<h1>'.$code.'</h1>';
                    }
                    session_destroy();
                } else { //There was an error inserting into the database
                    //die('There was an error preregistering [' . $con->error . ']');
                }
            } catch (exception $e) {
                echo $e->getMessage();
            }

        } else {
            echo $result;
        }
    } else {
        echo "Please go back to the previous page and submit your information";
    }
    return 0;
}


    function GetQuestionIDs($eventNum) //Counts how many questions are in the database
    {
        $db = getDB();
        try {
            $sql = $db->prepare("SELECT questionID from questions WHERE eventNum = :eventNum");
            $sql->bindParam(':eventNum', $eventNum, PDO::PARAM_INT);
            $sql->execute();
            $questionIDs = $sql->fetchAll(PDO::FETCH_ASSOC);
            $db = null;
            return $questionIDs;
        }
        catch(exception $e)
        {
            return  $e->getMessage();
        }
    }

    function AddQuestionsAndAnswers($code, $eventNum){
        $rows =  GetQuestionIDs($eventNum);
        $eventN = $eventNum;


        for ($x = 0; $x < sizeof($rows); $x++) { //For each question, grab the answer the user selected and insert it
            $answer = $_POST['A'.($rows[$x]['questionID'])];
            try {
                $db = getDB();
                $questID = ($rows[$x]['questionID']);
                $sql = $db->prepare("SELECT choiceID FROM choices WHERE eventNum = :eventNum AND questionID = :questionID AND choice = :choice");
                $sql->bindValue(':eventNum', $eventN, PDO::PARAM_INT);
                $sql->bindParam(':questionID', $questID, PDO::PARAM_INT);
                $sql->bindParam(':choice', $answer, PDO::PARAM_STR);

                $sql->execute();

                $choiceID = $sql->fetch();

                $choiceNum = $choiceID['choiceID'];

                $sql = $db->prepare("INSERT INTO answers (codeNum, questionID, choiceID, eventNum) VALUES
                (:codeNum, :questionID, :choiceID, :eventNum)"); //Create a prepared statement
                $sql->bindParam(':codeNum', $code, PDO::PARAM_STR);
                $sql->bindValue(':questionID', $rows[$x]['questionID'], PDO::PARAM_INT);
                $sql->bindValue(':choiceID', $choiceNum, PDO::PARAM_INT);
                $sql->bindValue(':eventNum', $eventNum, PDO::PARAM_INT);

                if(!$sql->execute()){ //This will display if there is an error
                    $db = null;
                //die();
                }
                $db = null;
            }  catch(Exception $e) {
            echo 'error:'. $e->getMessage();
        }

        }

    }

    //Checks if code already exists
//Checks if code already exists
function CheckCode($code,$eventNum){

    try {
        $db = getDB();
        $sql = $db->prepare("SELECT * FROM registrant WHERE codeNum = :codeNum AND eventNum = :eventNum");
        $sql->bindParam(':codeNum', filter_var($code,FILTER_SANITIZE_STRING), PDO::PARAM_STR);
        $sql->bindParam(':eventNum', filter_var($eventNum,FILTER_SANITIZE_NUMBER_INT), PDO::PARAM_INT);
        $sql->execute();
        $db = null;
        if($sql->rowCount() > 0){
            return true;
        }
        else{
            return false;
        }

    }catch(PDOException $e) {
        echo '{"error":{"text":'. $e->getMessage() .'}}';
    }
    return 0;
};
//Generate a random 6 digit number
function generateCode(){
    $number = rand(1,999999);
    while(strlen($number) < 6){
        $number = '0'.$number;
    }
    return $number;
}
?>