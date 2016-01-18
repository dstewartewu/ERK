<?php
	echo '
	<html>
		<head>
			<title>Eastern Washington University Pre-Registration</title>
			<link href="jobfair.css" rel="stylesheet">
			<link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css" rel="stylesheet">
			<link rel="icon" type="image/x-icon" href="favicon.ico">
			<link rel="shortcut icon" type="image/x-icon" href="favicon.ico">

		</head>
		<body>

			<img id="bannerImage" src="ewu_banner.jpg" width="40%" class="pull-left" alt=""/>

			<div style="text-align: center;">

			<br/>
				<h1 id="EventText"><b></b></h1>
			</div>

			<hr>

			<br/>

			<div class="col-md-12">
				<div style="text-align: center;">
					<h2 id="SuccessText"></h2>
				</div>
			</div>
			<!-- Start the Javascript -->
			<script>
				window.onload = init;
				function init(){
					var eventText = new XMLHttpRequest();
					eventText.open("GET", \'jobfairname.txt\', false);
					eventText.send(null);

					document.getElementById("EventText").innerHTML = "Pre-Registration Form for ".bold() + eventText.response.bold();

				}
			</script>
		</body>
	</html>';

	//Check if submit button was pressed
	if(isset($_POST['submit']))
	{
		//Get all the information from the html form and sanitize it
		$id = $_POST["id"];
		$Fname = filter_var($_POST["fName"], FILTER_SANITIZE_STRING);
		$Lname = filter_var($_POST["lName"], FILTER_SANITIZE_STRING);
		$registrant = filter_var($_POST["registrantType"], FILTER_SANITIZE_STRING);
		$email = filter_var($_POST["email"],FILTER_VALIDATE_EMAIL);
		
		if($registrant == "Student"){ //Check is user was a student
			$class = filter_var($_POST["Class"],FILTER_SANITIZE_STRING);
			$college = filter_var($_POST["college"],FILTER_SANITIZE_STRING);
			$major = filter_var($_POST["major"], FILTER_SANITIZE_STRING);
			$other = filter_var($_POST["otherUniversity"], FILTER_SANITIZE_STRING);
		}
		if($registrant == "Employer"){ //Check is user was an employer
			$business = filter_var($_POST["business"],FILTER_SANITIZE_STRING);
			$jobTitle = filter_var($_POST["jobTitle"],FILTER_SANITIZE_STRING);
		}
		$attended = "No";
		$invalidCode = true;
		
		//Use variable result to hold all error checking. When there's an error, it will be appended to result and displayed at the end
		$result = '';
		//Check if firstname is only comprised of letters and is not an empty string
		if(preg_match('#[0-9]#',$Fname) || strlen($Fname) == 0){
			//fail
			$result .= 'Firstname cannot be empty and must not contain any numbers or special characters<br/>';
		}
		//Check if lastname is only comprised of letters and is not an empty string
		if(preg_match('#[0-9]#',$Lname)  || strlen($Lname) == 0){
			//fail
			$result .= 'Last name cannot be empty and must not contain any numbers<br/>';
		}
		//Check if registrant type is valid
		if($registrant == null || ($registrant != "Student" && $registrant != "Employer" && $registrant != "General")){
			//fail
			$result .= 'Must select a registrant type<br/>';
		}
		//Check if user entered a valid email
		if($email == null || strlen($email) == 0 || !filter_var($email, FILTER_VALIDATE_EMAIL)){
			//fail
			$result .= 'Enter a valid email address<br/>';
		}
		//If user is an employer
		if($registrant == "Employer"){
			//Check if business is valid
			if($business == null || strlen($business) == 0) {
				//fail
				$result .= 'Enter a business name';
			}
		}
		//User is not an employer
		else{
			//Set employer info to null because user is not an employer
			$business = null;
			$jobTitle = null;
		}
		
		//User is a student
		if($registrant == "Student"){
			//Check if student ID is valid
			if(!is_numeric($id) && strlen($id) != 0){
				$result .= 'Id must contain only numeric values<br/>';
			}
			//Check if class standing is valid
			if($class == null || ($class != "Freshman" && $class != "Sophomore" && $class != "Junior" && $class != "Senior" && $class != "PostBaccalaureate" && $class != "Graduate" && $class != "Alumnus")){
				//fail
				$result .= 'Please select a class standing<br/>';
			}
			//Check if college is valid
			if($college == null || $college == ""){
				//fail
				$result .= 'Please select a university<br/>';
			}
			else{
				if($college == "Other"){
					if($other == "" || $other == null){
						$result .='Please enter a valid college<br/>';
					}
				}
			}
			//Check if major is valid
			if($major == null || $major == ""){
				$result .='Please enter a valid major<br/>';
			}
		}
		//User is not a student
		else{
			//Set student info to null because user is not a student
			$id = null;
			$class = null;
			$college = null;
			$major = null;
		}
		//If there are no errors. All info is valid
		if(empty($result)){
			include 'config.php'; //Holds database information
			$con = new mysqli(__DBHOST, __DBUSER, __DBPWD, __DBNAME); //Create a connection
			if($con->connect_errno > 0){ //If there's an error with the connection
				die('Unable to connect to database [' . $con->connect_error . ']');
			}
			$sqllist = $con->prepare("INSERT INTO registrant (`codeNum`,`Fname`,`Lname`, `Email`, `RegType`,`CheckedIn`) VALUES".
			"(?,?,?,?,?,?)"); //Create a prepared statement
			$code = null;
			while($invalidCode){
				$code = generateCode(); //Get random code that will be primary key in database
				$invalidCode = CheckCode($con,$code); //Get if code already exists. If it does, find a new one
			}
			GetQuestionsAndAnswers($code, $con);
			$sqllist->bind_param("ssssss",$code,$Fname,$Lname, $email,$registrant,$attended); //Bind the parameters for security purposes
			if($sqllist->Execute()){ //This will display if everything goes correctly
					if($registrant == "Employer"){
						isEmployer($con,$code,$business,$jobTitle);
					}
					else if($registrant == "Student"){
						isStudent($con,$code,$major,$college,$class);
					}
					session_start();
					$_SESSION['code'] = $code;
							
					$emailSuccessful = require 'email.php';
					//session_write_close();
					if($emailSuccessful){
						$file = fopen("successRegistrationMessage.txt", 'r');
	    					$pageText = fread($file, 25000);
						echo nl2br($pageText).'<br/>';
						$img = $_SESSION['barcode'];
						echo '<img src="data:image/png;base64,'.$img.'">';
					}
					
					session_destroy();
			}else{ //There was an error inserting into the database
				die('There was an error preregistering [' . $con->error . ']');
		}
			
        }
		else{
			echo $result;
		}
	}
	else{
		echo "Please go back to the previous page and submit your information";
	}
	
	function CheckDatabaseRowSize($con) //Counts how many questions are in the database
	{		
		$sql = "SELECT * from questions";
		$result = mysqli_query($con, $sql);
		$rows = 0;
		if($result->num_rows > 0){
			while($row = $result->fetch_assoc()){
				$rows++;
			}
		}
		return $rows;
		
	}
	
	function GetQuestionsAndAnswers($code,$con){
		$rows = CheckDatabaseRowSize($con); //Gets number of questions
		
		for ($x = 1; $x <= $rows; $x++) { //For each question, grab the answer the user selected and insert it
			$answer = $_POST['A'.$x];
			$sql = $con->prepare("INSERT INTO answers (`codeNum`,`questionID`,`answer`) VALUES".
			"(?,?,?)"); //Create a prepared statement
			$sql->bind_param("sss", $code, $x, $answer);
			if(!$sql->Execute()){ //This will display if there is an error
				die('There was an error preregistering [' . $con->error . ']');
			}
			
		}
		
	}
	
	//Checks if code already exists
	function CheckCode($con,$code){
		$sql = "SELECT * FROM registrant WHERE codeNum = '$code'";
		$query = mysqli_query($con,$sql);
		if(mysqli_num_rows($query) > 0){
			return true;
		}
		else{
			return false;
		}
	}
	
	function isStudent($con,$code,$major,$college,$class){
		$sqllist = $con->prepare("INSERT INTO student (`codeNum`, `Major`, `College`, `ClassStanding`) VALUES".
			"(?,?,?,?)"); //Create a prepared statement
			
			$sqllist->bind_param("ssss",$code,$major, $college,$class); //Bind the parameters for security purposes
			if(!$sqllist->Execute()){ //This will display if there is an error
				die('There was an error preregistering [' . $con->error . ']');
			}
	}
	
	function isEmployer($con,$code,$business,$jobTitle){
		$sqllist = $con->prepare("INSERT INTO employee (`codeNum`,`Business`,`Job`) VALUES".
			"(?,?,?)"); //Create a prepared statement
			
			$sqllist->bind_param("sss",$code,$business,$jobTitle); //Bind the parameters for security purposes
			if(!$sqllist->Execute()){ //This will display if there is an error
				die('There was an error preregistering [' . $con->error . ']');
			}
	}
	//Generate a random 6 digit number
	function generateCode(){
		$number = rand(1,999999);
		while(strlen($number) < 6){
			$number = '0'.$number;
		}
		return $number;
	}
	
?>