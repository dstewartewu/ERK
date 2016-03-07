<?php

	require 'vendor/phpmailer/phpmailer/PHPMailerAutoload.php';
	include '../../admin/models/config.php';
	$postCode = $_SESSION['code'];
	$to = $_SESSION['email'];

	$file = fopen("../temp/emailMessage.txt", 'r');
	$pageText = fread($file, 25000);
	$string = nl2br($pageText) . '<br/>';
	$content = '
	<html>
	<head>
		<title>
			Registration Code
		</title>
	</head>

	<body>
		<p>'.$string.'</p>
		<h1><b>$postCode</b></h1>
	</body>
</html>
	';

	$mail = new PHPMailer;

	$mail->isSMTP();
	$mail->Host = $emailHost;
	
	$mail->SMTPAuth = true;
	$mail->Port = $port;
	$mail->Username = $fromEmail;
	$mail->Password = $emailPass;
	
	$mail->setFrom($fromEmail, '');
	
	$mail->Subject = $emailSubject;
	
	$mail->AltBody = "To view the message, please use an HTML compatible email viewer!";
	
	$mail->Body = $content;
	
	$mail->addAddress($to, "");
	
	if(!$mail->Send()){
		echo "<br/>There was a problem sending the email. Please try again.<br/>";
		echo "If you continue to have issues please contact Career Services.<br/>";
		return false;
	}
	else{
		return true;
	}

?>