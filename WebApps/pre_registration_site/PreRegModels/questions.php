<?php
	include '../../admin/models/config.php'; //Holds database information
	include '../../admin/models/DB.php';

    header('Content-Type: application/json');
	try {
		$db = getDB(); //Create a connection

		$eventNum = $_GET['eventNum'];

		$sql = $db->prepare("SELECT * from questions WHERE eventNum = :eventNum");
		$sql->bindParam(':eventNum', $eventNum, PDO::PARAM_INT);
		$sql->execute();
		$questions = $sql->fetchAll(PDO::FETCH_ASSOC);

		$db = null;

		echo json_encode($questions);

	} catch (PDOException $e) {
		echo '{"error":{"text":' . $e->getMessage() . '}}';
	}
?>