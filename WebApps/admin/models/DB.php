<?php
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
    require_once('../../config.php');


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
