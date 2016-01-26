<?php
/**
 * Created by Tim Unger
 * Date: 11/11/2015
 * Uses Slim Framework w/ Composer managed dependencies
 * RESTful CRUD API
 */

require '../vendor/autoload.php';


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
    require_once('../config.php');


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
 * Description:
 * Gets student by registration code. Returns JSON.
 */
$app->get('/getStudentByCode/:codeNumber/:eventNum', function($codeNumber, $eventNum) use($app) {
    try {
        $db = getDB();
        $sql = $db->prepare("SELECT * FROM student WHERE (codeNum = :codeNumber) AND (eventNum = :eventNum) ");
        $sql->bindParam(':codeNumber', $codeNumber, PDO::PARAM_INT);
        $sql->bindParam(':eventNum', $eventNum, PDO::PARAM_INT);
        $sql->execute();
        $student = $sql->fetchAll(PDO::FETCH_ASSOC);
        $db = null;
        echo json_encode($student);
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
 * Gets Employee by registration code. Returns JSON.
 */
$app->get('/getEmployeeByCode/:codeNumber/:eventNum', function($codeNumber, $eventNum) use($app) {
    try {
        $db = getDB();
        $sql = $db->prepare("SELECT * FROM employee WHERE codeNum = :codeNumber AND eventNum = :eventNum");
        $sql->bindParam(':codeNumber', $codeNumber, PDO::PARAM_INT);
        $sql->bindParam(':eventNum', $eventNum, PDO::PARAM_INT);
        $sql->execute();
        $emp = $sql->fetchAll(PDO::FETCH_ASSOC);
        $db = null;
        echo json_encode($emp);
    } catch(PDOException $e) {
        echo '{"error":{"text":'. $e->getMessage() .'}}';
    }

});

/**
 * Tim Unger
 *
 * Gets table by table name, for excel dump.
 */
$app->get('/getTable/:tableName', function($tableName) use($app) {

    try {
        $db = getDB();
        $sql = $db->prepare("SELECT * FROM :tableName");
        $sql->bindParam(':tableName', $tableName, PDO::PARAM_STR);
        $sql->execute();
        $table = $sql->fetchAll(PDO::FETCH_ASSOC);
        $db = null;
        echo json_encode($table);
    } catch(PDOException $e) {
        echo '{"error":{"text":'. $e->getMessage() .'}}';
    }
});

/**
 * Tim Unger
 *
 * Gets table by table name, for excel dump.
 */
$app->get('/getRegistrantForExcel/', function() use($app) {

    try {
        $db = getDB();
        $sql = $db->prepare("SELECT fName, lName, college, major, classStanding,
                             email, regType, checkedIn FROM registrant LEFT JOIN
                             student S ON S.code = R.code LEFT JOIN employees E
                             ON E.code = S.code");
        $sql->execute();
        $table = $sql->fetchAll(PDO::FETCH_ASSOC);
        $db = null;
        echo json_encode($table);
    } catch(PDOException $e) {
        echo '{"error":{"text":'. $e->getMessage() .'}}';
    }
});



/**
 * Tim Unger
 *
 * Gets student by email. For students who forget/lose registration code. Returns JSON.
 */
$app->get('/getStudentByEmail/:email/:eventNum', function($email, $eventNum) use($app) {

    try {
        $db = getDB();
        $sql = $db->prepare("SELECT * FROM student WHERE email = :email AND eventNum = :eventNum");
        $sql->bindParam(':email', $email, PDO::PARAM_STR);
        $sql->bindParam(':eventNum', $eventNum, PDO::PARAM_INT);
        $sql->execute();
        $student = $sql->fetchAll(PDO::FETCH_ASSOC);
        $db = null;
        echo json_encode($student);
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
 * Gets employee by email. For employee who forget/lose registration code. Returns JSON. (May not be needed)
 */
$app->get('/getEmployeeByEmail/:email/:eventNum', function($email, $eventNum) use($app) {

    try {
        $db = getDB();
        $sql = $db->prepare("SELECT * FROM employee WHERE email = :email AND eventNum = :eventNum");
        $sql->bindParam(':email', $email, PDO::PARAM_STR);
        $sql->bindParam(':eventNum', $eventNum, PDO::PARAM_INT);
        $sql->execute();
        $emp = $sql->fetchAll(PDO::FETCH_ASSOC);
        $db = null;
        echo json_encode($emp);
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
$app->put("/updateStudent",
    function() use ($app){
        $request = $app->request();
        $registrant = json_decode($request->getBody());
        try {
            $db = getDB();
            $sql = $db->prepare("UPDATE registrant SET fName = :fName, lName = :lName,
                                 checkedIn = YES, checkInTime = :chkInTime
                                 WHERE codeNum = :codeNumber AND eventNum = :eventNum");
            $sql->bindParam(':fName', $registrant->fName, PDO::PARAM_STR);
            $sql->bindParam(':lName', $registrant->lName, PDO::PARAM_STR);
            $sql->bindParam(':cknInTime', date("Y-m-d H:i:s"), PDO::PARAM_STR);
            $sql->bindParam(':codeNumber', $registrant->code, PDO::PARAM_INT);
            $sql->bindParam(':eventNum', $registrant->eventNum, PDO::PARAM_INT);
            $sql->execute();

            $sql = $db->prepare("UPDATE student SET major = :major, college = :college,
                                 classStanding = :classStanding WHERE codeNum = :codeNumber AND eventNum = :eventNum");
            $sql->bindParam(':major', $registrant->major, PDO::PARAM_STR);
            $sql->bindParam(':college', $registrant->college, PDO::PARAM_STR);
            $sql->bindParam(':classStanding', $registrant->classStanding, PDO::PARAM_STR);
            $sql->bindParam(':codeNumber', $registrant->code, PDO::PARAM_INT);
            $sql->bindParam(':eventNum', $registrant->eventNum, PDO::PARAM_INT);
            $sql->execute();

            $db = null;

        }catch(PDOException $e) {
            echo '{"error":{"text":'. $e->getMessage() .'}}';
        }
});

$app->put("/updateEmployee",
    function() use ($app){
        $request = $app->request();
        $registrant = json_decode($request->getBody());
        try {
            $db = getDB();
            $sql = $db->prepare("UPDATE registrant SET fName = :fName, lName = :lName,
                                 checkedIn = YES, checkInTime = :chkInTime
                                 WHERE codeNum = :codeNumber AND eventNum = :eventNum");
            $sql->bindParam(':fName', $registrant->fName, PDO::PARAM_STR);
            $sql->bindParam(':lName', $registrant->lName, PDO::PARAM_STR);
            $sql->bindParam(':cknInTime', date("Y-m-d H:i:s"), PDO::PARAM_STR);
            $sql->bindParam(':codeNumber', $registrant->code, PDO::PARAM_INT);
            $sql->bindParam(':eventNum', $registrant->eventNum, PDO::PARAM_INT);
            $sql->execute();

            $sql = $db->prepare("UPDATE employee SET company = :company WHERE codeNum = :codeNumber
                                 AND eventNum = :eventNum");
            $sql->bindParam(':company', $registrant->company, PDO::PARAM_STR);
            $sql->bindParam(':codeNumber', $registrant->code, PDO::PARAM_INT);
            $sql->bindParam(':eventNum', $registrant->eventNum, PDO::PARAM_INT);
            $sql->execute();

            if($registrant->jobTitle != "")
            {
                $sql = $db->prepare("UPDATE employee SET jobTitle = :jobTitle WHERE codeNum = :codeNumber
                                     AND eventNum = :eventNum");
                $sql->bindParam(':jobTitle', $registrant->jobTitle, PDO::PARAM_STR);
                $sql->bindParam(':codeNumber', $registrant->code, PDO::PARAM_INT);
                $sql->bindParam(':eventNum', $registrant->eventNum, PDO::PARAM_INT);
                $sql->execute();
            }

            $db = null;

        }catch(PDOException $e) {
            echo '{"error":{"text":'. $e->getMessage() .'}}';
        }
    });

$app->put("/updateRegistrant",
    function() use ($app){
        $request = $app->request();
        $registrant = json_decode($request->getBody());
        try {
            $db = getDB();
            $sql = $db->prepare("UPDATE registrant SET fName = :fName, lName = :lName,
                                 checkedIn = YES, checkInTime = :chkInTime
                                 WHERE codeNum = :codeNumber AND eventNum = :eventNum");
            $sql->bindParam(':fName', $registrant->fName, PDO::PARAM_STR);
            $sql->bindParam(':lName', $registrant->lName, PDO::PARAM_STR);
            $sql->bindParam(':cknInTime', date("Y-m-d H:i:s"), PDO::PARAM_STR);
            $sql->bindParam(':codeNumber', $registrant->code, PDO::PARAM_INT);
            $sql->bindParam(':eventNum', $registrant->eventNum, PDO::PARAM_INT);
            $sql->execute();

            $db = null;

        }catch(PDOException $e) {
            echo '{"error":{"text":'. $e->getMessage() .'}}';
        }
    });


$app->post("/addStudent",
    function() use ($app){
        $request = $app->request();
        $student = json_decode($request->getBody());
        try {
            $db = getDB();
            $invalidCode = false;
            while($invalidCode){
                $code = generateCode(); //Get random code that will be primary key in database
                $invalidCode = CheckCode($code); //Get if code already exists. If it does, find a new one
            }
            $sql = $db->prepare("INSERT INTO registrant (fName, lName, regType, checkedIn, checkInTime, codeNum, eventNum)
                                 VALUES (:fName, :lName, :regType, YES, :chkInTime, :codeNumber, :eventNum)");
            $sql->bindParam(':fName', $student->fName, PDO::PARAM_STR);
            $sql->bindParam(':lName', $student->lName, PDO::PARAM_STR);
            $sql->bindParam(':regType', $student->regType, PDO::PARAM_STR);
            $sql->bindParam(':cknInTime', date("Y-m-d H:i:s"), PDO::PARAM_STR);
            $sql->bindParam(':codeNumber', $code, PDO::PARAM_INT);
            $sql->bindParam(':eventNum', $student->eventNum, PDO::PARAM_INT);
            $sql->execute();

            $sql = $db->prepare("INSERT INTO student (major, college, classStanding, codeNum, eventNum)
                                 VALUES (:major, :college, :classStanding, :codeNumber, :eventNum)");
            $sql->bindParam(':major', $student->major, PDO::PARAM_STR);
            $sql->bindParam(':college', $student->college, PDO::PARAM_STR);
            $sql->bindParam(':classStanding', $student->classStanding, PDO::PARAM_STR);
            $sql->bindParam(':codeNumber', $code, PDO::PARAM_INT);
            $sql->bindParam(':eventNum', $student->eventNum, PDO::PARAM_INT);
            $sql->execute();

            $db = null;

        }catch(PDOException $e) {
            echo '{"error":{"text":'. $e->getMessage() .'}}';
        }
    });
/**
 * Tim Unger
 *
 * Post- Adds Employee. Takes JSON encoded input. Note: Must pass Job Title as empty string
 * if no added in optional field. Could cause issues otherwise.
 */
$app->post("/addEmployee",
    function() use ($app){
        $request = $app->request();
        $employee = json_decode($request->getBody());
        try {
            $db = getDB();
            $invalidCode = false;
            while($invalidCode){
                $code = generateCode(); //Get random code that will be primary key in database
                $invalidCode = CheckCode($code, $employee->eventNum); //Get if code already exists. If it does, find a new one
            }
            $sql = $db->prepare("INSERT INTO registrant (fName, lName, regType, checkedIn, checkInTime, codeNum, eventNum)
                                 VALUES (:fName, :lName, :regType, YES, :chkInTime, :codeNumber, :eventNum)");
            $sql->bindParam(':fName', $employee->fName, PDO::PARAM_STR);
            $sql->bindParam(':lName', $employee->lName, PDO::PARAM_STR);
            $sql->bindParam(':regType', $employee->regType, PDO::PARAM_STR);
            $sql->bindParam(':cknInTime', date("Y-m-d H:i:s"), PDO::PARAM_STR);
            $sql->bindParam(':codeNumber', $code, PDO::PARAM_INT);
            $sql->bindParam(':eventNum', $employee->eventNum, PDO::PARAM_INT);
            $sql->execute();

            $sql = $db->prepare("INSERT INTO employee (company, codeNum, eventNum) VALUES (:company, :codeNumber, :eventNum)");
            $sql->bindParam(':company', $employee->company, PDO::PARAM_STR);
            $sql->bindParam(':codeNumber', $code, PDO::PARAM_INT);
            $sql->bindParam(':eventNum', $employee->eventNum, PDO::PARAM_INT);
            $sql->execute();

            if($employee->jobTitle != "")
            {
                $sql = $db->prepare("UPDATE employee SET jobTitle = :jobTitle WHERE codeNum = :codeNumber AND eventNum = :eventNum");
                $sql->bindParam(':jobTitle', $employee->jobTitle, PDO::PARAM_STR);
                $sql->bindParam(':codeNumber', $code, PDO::PARAM_INT);
                $sql->bindParam(':eventNum', $employee->eventNum, PDO::PARAM_INT);
                $sql->execute();
            }

            $db = null;

        }catch(PDOException $e) {
            echo '{"error":{"text":'. $e->getMessage() .'}}';
        }
    });

$app->post("/addRegistrant",
    function() use ($app){
        $request = $app->request();
        $student = json_decode($request->getBody());
        try {
            $db = getDB();
            $invalidCode = false;
            while($invalidCode){
                $code = generateCode(); //Get random code that will be primary key in database
                $invalidCode = CheckCode($code, $student->eventNum); //Get if code already exists. If it does, find a new one
            }
            $sql = $db->prepare("INSERT INTO registrant (fName, lName, regType, checkedIn, checkInTime, codeNum, eventNum)
                                 VALUES (:fName, :lName, :regType, YES, :chkInTime, :codeNumber, :eventNum)");
            $sql->bindParam(':fName', $student->fName, PDO::PARAM_STR);
            $sql->bindParam(':lName', $student->lName, PDO::PARAM_STR);
            $sql->bindParam(':regType', $student->regType, PDO::PARAM_STR);
            $sql->bindParam(':cknInTime', date("Y-m-d H:i:s"), PDO::PARAM_STR);
            $sql->bindParam(':codeNumber', $code, PDO::PARAM_INT);
            $sql->bindParam(':eventNum', $student->eventNum, PDO::PARAM_INT);
            $sql->execute();

            $db = null;

        }catch(PDOException $e) {
            echo '{"error":{"text":'. $e->getMessage() .'}}';
        }
    });

$app->run();
//Generate a random 6 digit number
function generateCode(){
    $number = rand(1,999999);
    while(strlen($number) < 6){
        $number = '0'.$number;
    }
    return $number;
}

//Checks if code already exists
function CheckCode($code,$eventNum){
    try {
        $db = getDB();
        $sql = $db->prepare("SELECT * FROM registrant WHERE codeNumber = :codeNum AND eventNum = :eventNum");
        $sql->bindParam(':codeNumber', $code, PDO::PARAM_INT);
        $sql->bindParam(':eventNum', $eventNum, PDO::PARAM_INT);
        $sql->execute();
    if($sql->rowCount() > 0){
        return true;
    }
    else{
        return false;
    }$db = null;

    }catch(PDOException $e) {
        echo '{"error":{"text":'. $e->getMessage() .'}}';
    }
}