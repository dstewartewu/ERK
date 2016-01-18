<?php
	include 'config.php'; //Holds database information
	$con = mysqli_connect(__DBHOST, __DBUSER, __DBPWD, __DBNAME); //Create a connection
	if($con->connect_errno > 0){ //If there's an error with the connection
		die('Unable to connect to database [' . $con->connect_error . ']');
	}
	
	$id = $_GET["id"]; //Get question ID

	$sql = "SELECT * FROM choices WHERE questionID = " + $id;
	$result = mysqli_query($con, $sql);
	if($result->num_rows > 0){
		$rows = array();
		while($row = $result->fetch_assoc()){
			$rows[] = $row;
		}
		echo json_encode($rows);
	}
?>