<?php
/**
 * Created by Tim Unger
 * Date: 11/11/2015
 * Uses Slim Framework w/ Composer managed dependencies
 * RESTful CRUD API
 */

require '../vendor/autoload.php';

date_default_timezone_set('America/Los_Angeles');
/**
 * Tim Unger
 * 11/13/15
 *
 * Description:
 * Opens DB connection. Currently set up for MySQL. Should also work for MariaDB.
 * Uses vars stored in config.php to build the connection string.
 *
 */
function getDB()
{
    require_once('../admin/models/config.php');


    $dbhost = __DBHOST;//ipAddress;
    $dbport = __DBPORT;//port
    $dbuser = __DBUSER;//user
    $dbpass = __DBPWD;//pass
    $dbname = __DBNAME;//db



    $mysql_conn_string = "mysql:host=$dbhost;port=$dbport;dbname=$dbname";
    $dbConnection = new PDO($mysql_conn_string, $dbuser, $dbpass);
    $dbConnection->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    return $dbConnection;
}

/**
 * Instantiates Slim framework.
 */
$app = new Slim\Slim();


/**
 * From SLIM website tutorial
 *
 * GET
 * tests connection to API
 */
$app->get('/', function() use($app) {
    echo "Welcome to EWU Career Services Pre-Registration API. This is a test function. ";
});
/**
 * Tim Unger
 * 11/13/15
 *
 * Description:
 * Gets row count in Questions table and returns that count.
 */
$app->get('/getQuestionCount/:eventNum', function($eventNum) use($app){

    try {
        $db = getDB();
        $sql = $db->prepare("SELECT * FROM questions WHERE eventNum = :eventNum");
        $sql->bindParam(':eventNum', $eventNum, PDO::PARAM_INT);
        $sql->execute();
        $count = array();

        $count['count'] = $sql->rowCount();

        echo json_encode($count);
    } catch(PDOException $e) {
        echo '{"error":{"text":'. $e->getMessage() .'}}';
    }
});

/**
 * Tim Unger
 *
 * Gets registrant by registration code. Returns JSON.
 */
$app->get('/getRegistrantByCode/:codeNumber/:eventNum', function($codeNumber, $eventNum) use($app) {

    try {
        $db = getDB();

        $sql = $db->prepare("SELECT * FROM registrant WHERE codeNum = :codeNum AND eventNum = :eventNum");
        $sql->bindParam(':codeNum', $codeNumber, PDO::PARAM_INT);
        $sql->bindParam(':eventNum', $eventNum, PDO::PARAM_INT);
        $sql->execute();
        $reg = $sql->fetchAll(PDO::FETCH_ASSOC);
        $db = null;
        echo json_encode($reg);
    } catch(PDOException $e) {
        echo '{"error":{"text":'. $e->getMessage() .'}}';
    }

});
/**
 * Tim Unger
 *
 * Gets registrant by email. For registrants who forget/lose registration code. Returns JSON.
 */
$app->get('/getRegistrantByEmail/:email/:eventNum', function($email, $eventNum) use($app) {

    try {
        $db = getDB();
        $sql = $db->prepare("SELECT * FROM registrant WHERE email = :email AND eventNum = :eventNum");
        $sql->bindParam(':email', $email, PDO::PARAM_STR);
        $sql->bindParam(':eventNum', $eventNum, PDO::PARAM_INT);
        $sql->execute();
        $reg= $sql->fetchAll(PDO::FETCH_ASSOC);
        $db = null;
        echo json_encode($reg);
    } catch(PDOException $e) {
        echo '{"error":{"text":'. $e->getMessage() .'}}';
    }

});


/**
 * Tim Unger
 *
 * Updates student by registration code. Updates both registrant and student tables. Must pass all information
 * that is stored other than email.
 */
$app->post("/updateRegistrant",
    function() use ($app){
        $request = $app->request();
        $registrant = json_decode($request->getBody());

        try {
            $time = date("Y-m-d H:i:s");
            $checkedIn = 'Yes';

            $db = getDB();
            $sql = $db->prepare("UPDATE registrant
                                 SET fName= :fName , lName= :lName , checkedIn= :checkedIn , checkInTime= :checkInTime ,
                                 regType= :regType , major= :major , college= :college , classStanding= :classStanding
                                 WHERE (codeNum = :codeNum AND eventNum = :eventNum)");
            $sql->bindValue(':codeNum', $registrant->code, PDO::PARAM_STR);
            $sql->bindValue(':checkInTime', $time, PDO::PARAM_STR);
            $sql->bindValue(':checkedIn', $checkedIn, PDO::PARAM_STR);
            $sql->bindParam(':fName', $registrant->fName, PDO::PARAM_STR);
            $sql->bindParam(':lName', $registrant->lName, PDO::PARAM_STR);
            $sql->bindParam(':regType', $registrant->regType, PDO::PARAM_STR);
            $sql->bindParam(':eventNum', $registrant->eventNum, PDO::PARAM_INT);
            $sql->bindParam(':major', $registrant->major, PDO::PARAM_STR);
            $sql->bindParam(':college', $registrant->college, PDO::PARAM_STR);
            $sql->bindParam(':classStanding', $registrant->classStanding, PDO::PARAM_STR);

            $sql->execute();

            $db = null;

        }catch(PDOException $e) {
            echo '{"error":{"text":'. $e->getMessage() .'}}';
        }
    });


$app->post("/addRegistrant", function() use ($app) {
    $request = $app->request();
    $student = json_decode($request->getBody());

    try {
        $db = getDB();
        $invalidCode = true;
        while($invalidCode){
            $code = generateCode(); //Get random code that will be primary key in database
            $event = $student->eventNum;
            $invalidCode = CheckCode($code, $event); //Get if code already exists. If it does, find a new one
        }
        $time = date("Y-m-d H:i:s");
        $checkedIn = 'yes';

        $sql = $db->prepare("INSERT INTO registrant (fName, lName, checkedIn, checkInTime, regType, codeNum, eventNum, major, college, classStanding)
                                 VALUES (:fName, :lName, :checkedIn, :checkInTime, :regType, :codeNum, :eventNum, :major, :college, :classStanding)");
        $sql->bindValue(':codeNum', $code, PDO::PARAM_STR);
        $sql->bindValue(':checkInTime', $time, PDO::PARAM_STR);
        $sql->bindValue(':checkedIn', $checkedIn, PDO::PARAM_STR);
        $sql->bindParam(':fName', $student->fName, PDO::PARAM_STR);
        $sql->bindParam(':lName', $student->lName, PDO::PARAM_STR);
        $sql->bindParam(':regType', $student->regType, PDO::PARAM_STR);
        $sql->bindParam(':eventNum', $student->eventNum, PDO::PARAM_INT);
        $sql->bindParam(':major', $student->major, PDO::PARAM_STR);
        $sql->bindParam(':college', $student->college, PDO::PARAM_STR);
        $sql->bindParam(':classStanding', $student->classStanding, PDO::PARAM_STR);
        $sql->execute();

        $db = null;

    }catch(PDOException $e) {
        echo '{"error":{"text":'. $e->getMessage() .'}}'.$app->request()->getBody();
    }
});


//Generate a random 6 digit number
function generateCode(){
    $number = rand(1,999999);
    while(strlen($number) < 6){
        $number = '0'.$number;
    }
    return $number;
};

//Checks if code already exists
function CheckCode($code,$eventNum){

    try {
        $db = getDB();
        $sql = $db->prepare("SELECT * FROM registrant WHERE codeNum = :codeNum AND eventNum = :eventNum");
        $sql->bindParam(':codeNum', $code, PDO::PARAM_STR);
        $sql->bindParam(':eventNum', $eventNum, PDO::PARAM_INT);
        $sql->execute();
        $db = null;
        if($sql->rowCount() > 0){
            return true;
        }
        else{
            return false;
        }$db = null;

    }catch(PDOException $e) {
        echo '{"error":{"text":'. $e->getMessage() .'}}';
    }
    return 0;
};

$app->run();