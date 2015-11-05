<!DOCTYPE html>
<!--
To change this license header, choose License Headers in Project Properties.
To change this template file, choose Tools | Templates
and open the template in the editor.
-->
<?php
include './lib/installer.php';

$installer = new esol\installer();
$dbhost = $_POST['dbhost'];
$dbname = $_POST['dbname'];
$dbuser = $_POST['dbuser'];
$dbpwd = $_POST['dbpwd'];

$result = $installer->install($dbhost, $dbname, $dbuser, $dbpwd);
?>

<html>
    <head>
        <meta charset="UTF-8">
        <title></title>
    </head>
    <body>
        <?php
        if ($result == true)
        {
            ?>
        <p>Installation successful! Please delete the /install directory immediately, to prevent security risks.</p>
            <?php
        }
        else
        {
            ?>
        <p>Something went wrong, please check /install/error_log for details.</p>
            <?php
        }
        ?>
    </body>
</html>
