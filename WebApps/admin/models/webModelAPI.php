<?php
require_once('DB.php');
require_once('config.php');
require '../../vendor/autoload.php';
/**
 * Created by PhpStorm.
 * User: Tim
 * Date: 1/10/2016
 */

date_default_timezone_set('America/Los_Angeles');

$app = new Slim\Slim();

//  Welcome GET route to test accessibility

$app->get('/', function() use($app) {
    echo "Welcome to EWU Career Services Events Administrative API. This is a test function. ";
});

//  CREATE API calls

$app->post('/addQuestion', function() use ($app) {

    $request = $app->request();
    $validate = json_decode($request->getBody());
    $data = array();
    if(isValid($validate->key) === 0) {
        try {
            $db = getDB();
            $sql = $db->prepare("INSERT INTO questions (questionID, question, eventNum)
                             VALUES (:questionID, :question, :eventNum)");
            $sql->bindParam(':questionID', $validate->questionID, PDO::PARAM_INT);
            $sql->bindParam(':question', $validate->question, PDO::PARAM_STR);
            $sql->bindParam(':eventNum', $validate->eventNum, PDO::PARAM_INT);

            $sql->execute();
            $questionNum = $db->lastInsertId();
            $db = null;
            $data['success'] = true;
            $data['questionID'] = $questionNum;
        } catch (PDOException $e) {
            $data['success'] = false;
            $data['error'] = 'text' . $e->getMessage();
        }
    }
    echo json_encode($data);
});

$app->post('/addAdmin', function() use ($app) {

    $request = $app->request();
    $validate = json_decode($request->getBody());
    $data = array();
    if(isValid($validate->key) === 0) {
        try {
            $db = getDB();
            $sql = $db->prepare("INSERT INTO admins (userName, adminPrivilege)
                                 VALUES (:userName, true)");
            $sql->bindParam(':userName', $validate->userName, PDO::PARAM_STR);

            $sql->execute();
            $db = null;
            $data['success'] = true;
        } catch (PDOException $e) {
            $data['success'] = false;
            $data['error'] = 'text' . $e->getMessage();
        }
    }
    echo json_encode($data);
});

$app->post('/addChoice', function() use ($app) {

    $request = $app->request();
    $validate = json_decode($request->getBody());
    $data = array();
    if(isValid($validate->key) ===0) {
        try {
            $db = getDB();
            $sql = $db->prepare("INSERT INTO choices (questionID, choice, eventNum)
                             VALUES (:questionID, :choice, :eventNum)");
            $sql->bindParam(':questionID', $validate->questionID, PDO::PARAM_INT);
            $sql->bindParam(':choice', $validate->choice, PDO::PARAM_STR);
            $sql->bindParam(':eventNum', $validate->eventNum, PDO::PARAM_INT);

            $sql->execute();
            $db = null;
            $data['success'] = true;
        } catch (PDOException $e) {
            $data['success'] = false;
            $data['error'] = 'text' . $e->getMessage();
        }
    }
    echo json_encode($data);
});

$app->post('/createEvent', function() use ($app) {

    $request = $app->request();
    $validate = json_decode($request->getBody());
    $key = $validate->key;
    $data = array();
    if(isValid($key) === 0) {
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
            $eventNum = $db->lastInsertId();
            $db = null;
            $data['success'] = true;
            $data['eventNum'] = $eventNum;
        } catch (PDOException $e) {
            $data['success'] = false;
            $data['error'] = 'text' . $e->getMessage();
        }

        echo json_encode($data);
    }
    else {
        echo "error";
    }
});

//  READ API Calls

$app->post('/getAdmin', function() use ($app) {
    $request = $app->request();
    $validate = json_decode($request->getBody());
    if(isValid($validate->key) === 0) {
        try {

            $db = getDB();
            $sql = $db->prepare("SELECT * FROM admins");
            $sql->execute();
            $admins = $sql->fetchAll(PDO::FETCH_ASSOC);
            $db = null;
            echo json_encode($admins);
        } catch (PDOException $e) {
            echo '{"error":{"text":' . $e->getMessage() . '}}';
        }
    }
});

$app->get('/getEventsWithPreRegList', function() use ($app) {
    $request = $app->request();
    $validate = json_decode($request->getBody());
        try {

            $db = getDB();
            $sql = $db->prepare("SELECT * FROM eventInfo WHERE cusQuest = :cusQuest");
            $cusQuest = 'true';
            $sql->bindParam(':cusQuest', $cusQuest, PDO::PARAM_STR);
            $sql->execute();
            $events = $sql->fetchAll(PDO::FETCH_ASSOC);
            $db = null;
            echo json_encode($events);
        } catch (PDOException $e) {
            echo '{"error":{"text":' . $e->getMessage() . '}}';
        }
});

$app->post('/getEventsList', function() use ($app) {
    $request = $app->request();
    $validate = json_decode($request->getBody());
    if(isValid($validate->key) === 0) {
        try {

            $db = getDB();
            $sql = $db->prepare("SELECT * FROM eventInfo");
            $sql->execute();
            $events = $sql->fetchAll(PDO::FETCH_ASSOC);
            $db = null;
            echo json_encode($events);
        } catch (PDOException $e) {
            echo '{"error":{"text":' . $e->getMessage() . '}}';
        }
    }
});

$app->post('/getEvent', function() use ($app) {
    $request = $app->request();
    $validate = json_decode($request->getBody());
    if(isValid($validate->key) === 0) {
        try {
            $db = getDB();
            $sql = $db->prepare("SELECT * FROM eventInfo WHERE eventNum = :eventNum");
            $sql->bindParam(':eventNum', $validate->eventNum, PDO::PARAM_INT);
            $sql->execute();
            $events = $sql->fetchAll(PDO::FETCH_ASSOC);
            $db = null;

            echo json_encode($events);
        } catch (PDOException $e) {
            echo '{"error":{"text":' . $e->getMessage() . '}}';
        }
    }
});

$app->post('/getQuestions', function() use ($app) {
    $request = $app->request();
    $validate = json_decode($request->getBody());
    if(isValid($validate->key) === 0) {
        try {
            $db = getDB();
            $sql = $db->prepare("SELECT * FROM questions WHERE eventNum = :eventNum");
            $sql->bindParam(':eventNum', $validate->eventNum, PDO::PARAM_INT);
            $sql->execute();
            $events = $sql->fetchAll(PDO::FETCH_ASSOC);
            $db = null;

            echo json_encode($events);
        } catch (PDOException $e) {
            echo '{"error":{"text":' . $e->getMessage() . '}}';
        }
    }
});

$app->post('/getChoices', function() use ($app) {
    $request = $app->request();
    $validate = json_decode($request->getBody());
        try {
            $db = getDB();
            $sql = $db->prepare("SELECT * FROM choices WHERE eventNum = :eventNum && questionID = :questionNum");
            $sql->bindParam(':eventNum', $validate->eventNum, PDO::PARAM_INT);
            $sql->bindParam(':questionNum', $validate->questionNum, PDO::PARAM_INT);
            $sql->execute();
            $events = $sql->fetchAll(PDO::FETCH_ASSOC);
            $db = null;

            echo json_encode($events);
        } catch (PDOException $e) {
            echo '{"error":{"text":' . $e->getMessage() . '}}';
        }
});

$app->post('/getRegistrantNameEmail', function() use ($app) {
    $request = $app->request();
    $validate = json_decode($request->getBody());
    if(isValid($validate->key) === 0) {
        try {

            $db = getDB();
            $sql = $db->prepare("SELECT fname, lname, email FROM registrant WHERE eventNum = :eventNum AND email <> 'null'");
            $sql->bindParam(':eventNum', $validate->eventNum, PDO::PARAM_STR);
            $sql->execute();
            $emaillist = $sql->fetchAll(PDO::FETCH_ASSOC);
            $db = null;
            echo json_encode($emaillist);
        } catch (PDOException $e) {
            echo '{"error":{"text":' . $e->getMessage() . '}}';
        }
    }
});

$app->post('/getRegistrantCSVDump', function() use ($app) {
    $request = $app->request();
    $validate = json_decode($request->getBody());
    if(isValid($validate->key) === 0) {
        try {
            //Query is ugly, but it works, gets all registrants by event number while concat each answer to tupple by codeNum
            $db = getDB();
            $sql = $db->prepare("SELECT registrant.fname, registrant.lname, registrant.registerDate, registrant.regType, registrant.major,
                             registrant.college, registrant.classStanding, registrant.company, registrant.employeePosition,
                             registrant.checkedIn, registrant.checkInTime, GROUP_CONCAT(choices.choice SEPARATOR ', ') FROM registrant
                             LEFT JOIN answers ON registrant.codeNum = answers.codeNum AND registrant.eventNum = answers.eventNum
                             LEFT JOIN choices ON answers.questionID = choices.questionID AND registrant.eventNum = choices.eventNum
                             WHERE registrant.eventNum = :eventNum GROUP BY registrant.codeNum");
            $sql->bindParam(':eventNum', $validate->eventNum, PDO::PARAM_STR);
            $sql->execute();
            $reglist = $sql->fetchAll(PDO::FETCH_ASSOC);
            $db = null;
            echo json_encode($reglist);
        } catch (PDOException $e) {
            echo '{"error":{"text":' . $e->getMessage() . '}}';
        }
    }
});

//  UPDATE API Calls

$app->post('/updateEvent', function() use ($app) {

    $request = $app->request();
    $validate = json_decode($request->getBody());
    $data = array();
    if(isValid($validate->key)===0){
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

        } catch (PDOException $e) {
            $data['success'] = false;
            $data['error'] = 'text' . $e->getMessage();
        }
    }
    echo json_encode($data);
});

$app->post('/updateQuestion', function() use ($app) {

    $request = $app->request();
    $validate = json_decode($request->getBody());
    $data = array();
    if(isValid($validate->key) ===0) {
        try {
            $db = getDB();
            $sql = $db->prepare("UPDATE questions SET question = :question
                             WHERE questionID = :questionID AND eventNum = :eventNum");
            $sql->bindParam(':question', $validate->question, PDO::PARAM_STR);
            $sql->bindParam(':questionID', $validate->questionID, PDO::PARAM_INT);
            $sql->bindParam(':eventNum', $validate->eventNum, PDO::PARAM_INT);

            $sql->execute();
            $db = null;
            $data['success'] = true;
        } catch (PDOException $e) {
            $data['success'] = false;
            $data['error'] = 'text' . $e->getMessage();
        }
    }
    echo json_encode($data);
});

$app->post('/updateChoice', function() use ($app) {

    $request = $app->request();
    $validate = json_decode($request->getBody());
    $data = array();
    try {
        $db = getDB();
        $sql = $db->prepare("UPDATE choices SET choice = :choice
                             WHERE choiceID = :choiceID AND questionID = :questionID AND eventNum = :eventNum");
        $sql->bindParam(':choiceID', $validate->choiceID, PDO::PARAM_INT);
        $sql->bindParam(':choice', $validate->choice, PDO::PARAM_STR);
        $sql->bindParam(':questionID', $validate->questionID, PDO::PARAM_INT);
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

//  DELETE API Calls

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

            $sql = $db->prepare("DELETE FROM registrant WHERE eventNum = :eventNum");
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
            $data['success'] = true;

        } catch (PDOException $e) {
            $data['success'] = false;
            $data['error'] = 'text' . $e->getMessage();
        }
        echo json_encode($data);
    }
    else {
        echo "Unauthorized";
    }
});

$app->post('/deleteAdmin', function() use ($app) {

    $request = $app->request();
    $validate = json_decode($request->getBody());

    $key = (string)$validate->key;
    if (isValid($key) == 0) {
        try {
            $db = getDB();
            $sql = $db->prepare("DELETE FROM admins WHERE userName = :userName");
            $sql->bindParam(':userName', $validate->userName, PDO::PARAM_STR);
            $sql->execute();

            $db = null;
        } catch (PDOException $e) {
            echo '{"error":{"text":' . $e->getMessage() . '}}';
        }
    }
});

$app->post('/deleteChoice', function() use ($app) {

    $request = $app->request();
    $validate = json_decode($request->getBody());

    $key = (string)$validate->key;
    if (isValid($key) == 0) {
        try {
            $db = getDB();
            $sql = $db->prepare("DELETE FROM choices WHERE choiceID = :choiceID");
            $sql->bindParam(':choiceID', $validate->choiceID, PDO::PARAM_INT);
            $sql->execute();

            $db = null;
        } catch (PDOException $e) {
            echo '{"error":{"text":' . $e->getMessage() . '}}';
        }
    }
});

$app->post('/deleteQuestion', function() use ($app) {

    $request = $app->request();
    $validate = json_decode($request->getBody());

    $key = $validate->key;
    $eventNum = $validate->eventNum;
    $questionID = $validate->questionID;

    if (isValid($key) == 0) {
        try {
            $db = getDB();

            $sql = $db->prepare("DELETE FROM questions WHERE eventNum = :eventNum AND questionID = :questionID");
            $sql->bindParam(':eventNum', $eventNum, PDO::PARAM_INT);
            $sql->bindParam(':questionID', $questionID, PDO::PARAM_INT);
            $sql->execute();

            $sql = $db->prepare("DELETE FROM choices WHERE eventNum = :eventNum AND questionID = :questionID");
            $sql->bindParam(':eventNum', $eventNum, PDO::PARAM_INT);
            $sql->bindParam(':questionID', $questionID, PDO::PARAM_INT);
            $sql->execute();

            $sql = $db->prepare("DELETE FROM answers WHERE eventNum = :eventNum AND questionID = :questionID");
            $sql->bindParam(':eventNum', $eventNum, PDO::PARAM_INT);
            $sql->bindParam(':questionID', $questionID, PDO::PARAM_INT);
            $sql->execute();

            $db = null;

        } catch (PDOException $e) {
            echo '{"error":{"text":' . $e->getMessage() . '}}';
        }
    }
});

//  KIOSK REGISTRATION

$app->post('/RegisterKiosk', function() use ($app){
    $request = $app->request();
    $validate = json_decode($request->getBody());
    if(isValid($validate->key) === 0) {

        $regKey = openssl_random_pseudo_bytes(24);
        $regKey = base64_encode($regKey);
        $regKey = preg_replace("/[^A-Za-z0-9 ]/", 'a', $regKey);
        $date = new DateTime();
        $date->add(new DateInterval('P2D'));

        $expire = $date->format('M-d-Y h:i:s');
        $request = $app->request();
        $validate = json_decode($request->getBody());
        $data = array();
        try {
            $db = getDB();
            $sql = $db->prepare("INSERT INTO kiosks (kioskReg, eventNum, expire)
                             VALUES (:kioskReg, :eventNum, :expire)");
            $sql->bindValue(':kioskReg', $regKey, PDO::PARAM_STR);
            $sql->bindValue(':expire', $expire, PDO::PARAM_STR);
            $sql->bindParam(':eventNum', $validate->eventNum, PDO::PARAM_INT);

            $sql->execute();
            $db = null;
            $data['success'] = true;
            $data['regKey'] = $regKey;
            $data['expire'] = $expire;
        } catch (PDOException $e) {
            $data['success'] = false;
            $data['error'] = 'text' . $e->getMessage();
        }
    }
    echo json_encode($data);

});


$app->run();

function isValid($key){
    $test = (string)__ADMINKEY;
    return strcmp($key, $test);
}