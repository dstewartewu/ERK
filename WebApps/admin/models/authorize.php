<?php
//forces SSO on the current page

require_once realpath(dirname(__FILE__)) . '/config.php';
require_once 'DB.php';


if(!__DEBUG) {
    //DO SSO AUTHORIZATION

    require_once '\\\\NETSTORAGE.EASTERN.EWU.EDU\SHOME2\tunger\website\vendor\jasig\phpcas\CAS.php';

    if(__DEBUG)
    {
        phpCAS::setDebug();
    }

    phpCAS::client(CAS_VERSION_2_0, "login.ewu.edu", 443, "/cas");


    if(__DEBUG || __CERT === FALSE)
    {
        phpCAS::setNoCasServerValidation();
    }
    else
    {
        phpCAS::setCasServerCACert(__CERT);
    }

    phpCAS::setDebug();

    //echo "I AM AT 1";
    phpCAS::forceAuthentication();
    //echo "I AM AT 2";

    $user = phpCAS::getUser();

    $db = getDB();
    $sql = $db->prepare("SELECT * FROM admins WHERE userName = :userName");
    $sql->bindParam(':userName', $user, PDO::PARAM_STR);
    $sql->execute();
    $valid = $sql->rowCount();

    $db = null;

    if ($valid != 1)
    {
        echo "UNAUTHORIZED";
        die();
    }
}
else {
    //DO NO AUTHORIZATION
    $user = "DEBUG_ONLY";
}

define('__USER', $user);
