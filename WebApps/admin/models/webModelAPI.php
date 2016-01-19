<?php
require_once('DB.php');
require_once('../scripts/config.php');
require '../../vendor/autoload.php';
/**
 * Created by PhpStorm.
 * User: Tim
 * Date: 1/10/2016
 */

$app = new Slim\Slim();

$app->get('/', function() use($app) {
    echo "Welcome to EWU Career Services Events Administrative API. This is a test function. ";
});

$app->get('/getEventsList', function() use ($app) {

        try {

            $db = getDB();
            $sql = $db->prepare("SELECT * FROM eventInfo");
            $sql->execute();
            $events = $sql->fetchAll(PDO::FETCH_ASSOC);
            $db = null;
            echo json_encode($events);
        }
        catch(PDOException $e) {
            echo '{"error":{"text":'. $e->getMessage() .'}}';
        }
});

$app->get('/getEvent/:eventNum', function($eventNum) use ($app) {
    try {
        $db = getDB();
        $sql = $db->prepare("SELECT * FROM eventInfo WHERE eventNum = :eventNum");
        $sql->bindParam(':eventNum', $eventNum, PDO::PARAM_INT);
        $sql->execute();
        $events = $sql->fetchAll(PDO::FETCH_ASSOC);
        $db = null;

        echo json_encode($events);
    }
    catch(PDOException $e) {
        echo '{"error":{"text":'. $e->getMessage() .'}}';
    }
});

$app->post('/createEvent', function() use ($app) {

    $request = $app->request();
    $validate = json_decode($request->getBody());
    $data = array();
    try {
        $db = getDB();
        $sql = $db->prepare("INSERT INTO eventInfo (eventName, eventDate, startTime,
                                 endTime, siteHeader, preReg, cusQuest)
                                 VALUES (:eventName, :eventDate, :startTime, :endTime,
                                         :siteHeader, :preReg, :cusQuest)");
        $sql->bindParam(':eventName', $validate->eventName, PDO::PARAM_STR);
        $sql->bindParam(':eventDate', $validate->eventDate, PDO::PARAM_STR);
        $sql->bindParam(':startTime', $validate->startTime, PDO::PARAM_STR);
        $sql->bindParam(':endTime', $validate->endTime, PDO::PARAM_STR);
        $sql->bindParam(':siteHeader', $validate->siteHeader, PDO::PARAM_STR);
        $sql->bindParam(':preReg', $validate->preRegistration, PDO::PARAM_STR);
        $sql->bindParam(':cusQuest', $validate->customQuestions, PDO::PARAM_STR);

        $sql->execute();

        $db = null;
        $data['success'] = true;

        }
        catch (PDOException $e) {
            $data['success'] = false;
            $data['error']= 'text'.$e->getMessage();
        }
    echo json_encode($data);
});

$app->post('/deleteEvent', function() use ($app) {

    $request = $app->request();
    $validate = json_decode($request->getBody());

    $key = (string)$validate->key;
    $eventNum = $validate->eventNum;
    if(isValid($key) == 0) {
        try {
            $db = getDB();
            $sql = $db->prepare("DELETE FROM eventInfo WHERE eventNum = :eventNum");
            $sql->bindParam(':eventNum', $eventNum, PDO::PARAM_INT);
            $sql->execute();

            $sql = $db->prepare("DELETE FROM student WHERE eventNum = :eventNum");
            $sql->bindParam(':eventNum', $eventNum, PDO::PARAM_INT);
            $sql->execute();

            $sql = $db->prepare("DELETE FROM registrant WHERE eventNum = :eventNum");
            $sql->bindParam(':eventNum', $eventNum, PDO::PARAM_INT);
            $sql->execute();

            $sql = $db->prepare("DELETE FROM employees WHERE eventNum = :eventNum");
            $sql->bindParam(':eventNum', $eventNum, PDO::PARAM_INT);
            $sql->execute();

            $sql = $db->prepare("DELETE FROM questions WHERE eventNum = :eventNum");
            $sql->bindParam(':eventNum', $eventNum, PDO::PARAM_INT);
            $sql->execute();

            $sql = $db->prepare("DELETE FROM choices WHERE eventNum = :eventNum");
            $sql->bindParam(':eventNum', $eventNum, PDO::PARAM_INT);
            $sql->execute();

            $sql = $db->prepare("DELETE FROM answers WHERE eventNum = :eventNum");
            $sql->bindParam(':eventNum', $eventNum, PDO::PARAM_INT);
            $sql->execute();

            $db = null;

        } catch (PDOException $e) {
            echo '{"error":{"text":' . $e->getMessage() . '}}';
        }
    }
    else {
        echo "Unauthorized";
    }
});

$app->post('/updateEvent', function() use ($app) {

    $request = $app->request();
    $validate = json_decode($request->getBody());
    $data = array();
    try {
        $db = getDB();
        $sql = $db->prepare("UPDATE eventInfo SET eventName = :eventName, eventDate = :eventDate,
                             startTime = :startTime, endTime = :endTime, siteHeader = :siteHeader,
                             preReg = :preReg, cusQuest = :cusQuest WHERE eventNum = :eventNum");
        $sql->bindParam(':eventName', $validate->eventName, PDO::PARAM_STR);
        $sql->bindParam(':eventDate', $validate->eventDate, PDO::PARAM_STR);
        $sql->bindParam(':startTime', $validate->startTime, PDO::PARAM_STR);
        $sql->bindParam(':endTime', $validate->endTime, PDO::PARAM_STR);
        $sql->bindParam(':siteHeader', $validate->siteHeader, PDO::PARAM_STR);
        $sql->bindParam(':preReg', $validate->preReg, PDO::PARAM_STR);
        $sql->bindParam(':cusQuest', $validate->cusQuest, PDO::PARAM_STR);
        $sql->bindParam(':eventNum', $validate->eventNum, PDO::PARAM_INT);

        $sql->execute();

        $db = null;
        $data['success'] = true;

    }
    catch (PDOException $e) {
        $data['success'] = false;
        $data['error']= 'text'.$e->getMessage();
    }
    echo json_encode($data);
});

    function updateQuestion(){

    }



$app->run();

function isValid($key){
    $test = (string)__ADMINKEY;
    return strcmp($key, $test);
}