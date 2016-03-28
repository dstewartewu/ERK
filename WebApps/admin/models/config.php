<?php
//////////////////////////////
//    Main configuration    //
//////////////////////////////
//Sets the application into debug mode - boolean
//****WARNING: THIS MUST BE FALSE FOR PRODUCTION MODE****
    define('__DEBUG', FALSE);

    //The path of the certificate that gets passed to phpCAS setCasServerCACert()
    //If set to FALSE the setNoCasServerValidation() is called instead
    //string or FALSE
    define('__CERT', FALSE);
	//This files holds the info necessary for connecting to databases and Email settings

	define('__DBHOST', 'DB-IP_ADDRESS'); //server location. Change as needed
	
	//Change this as needed
	define('__DBNAME', 'DB_NAME'); //database name
	define('__DBUSER', 'DB_USER'); //username
	define('__DBPWD', 'DB_PWD'); //password
	define('__DBPORT', 'DB_PORT'); //port
	
	$emailHost = 'EMAIL_HOST'; //SMTP Server
	$port = 25;
	$fromEmail = 'FROM_EMAIL_ADDRESS'; //Email address which sends out the emails
	$emailPass = 'EMAIL_PASS'; //Password for Email
	$emailSubject = 'EMAIL_SUBJECT'; //Subject of email

	//Should match a hidden input field located in admin/index.php - used to authorize WebModelAPI.php calls.
	define('__ADMINKEY', "HIDDEN_KEY");

?>