<?php
	require 'vendor/phpmailer/phpmailer/PHPMailerAutoload.php';
	include 'config.php';
	$postCode = $_SESSION['code'];
	$to = $_POST['email'];
	$img = base64_encode(require_once('imageRender.php'));
	$_SESSION['barcode'] = $img; //Store barcode
	$file = fopen("emailMessage.txt", 'r');
	$pageText = fread($file, 25000);
	$string = nl2br($pageText) . '<br/>';
	$content = '
	<html>
	<head>
		<title>
			Barcode
		</title>
	</head>
	
	<body>		
		<p>'.$string.'</p>
		<img src="data:image/png;base64,'.$img.'">			
	</body>
</html>
	';

	$img = chunk_split($img);

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
	
	$mail->addAttachment($img,"Barcode");
	
	if(!$mail->Send()){
		echo "<br/>There was a problem sending the email. Please try again.<br/>";
		echo "If you continue to have issues please contact Career Services.<br/>";
		return false;
	}
	else{
		return true;
	}

?>