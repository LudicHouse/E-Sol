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
?>

<html>
    <head>
        <meta charset="UTF-8">
        <title></title>
    </head>
    <body>
        <h1>Pre-Install Checks</h1>
        
        <div>
            <h2>PHP</h2>
            <ul>
                <?php
                if ($installer->checkPhpVersion() == true)
                {
                    ?>
                <li>PHP Version: <span style="color:green"><?=phpversion()?></span></li>
                    <?php
                }
                else
                {
                    ?>
                <li>PHP Version: <span style="color:red"><?=phpversion()?> (Should be at least <?=$installer->targetPhpVersion?>)</span></li>
                    <?php
                }
                ?>
            </ul>
        </div>
        
        <div>
            <h2>Database</h2>
            <ul>
                <?php
                if ($installer->canLogin($dbhost, $dbname, $dbuser, $dbpwd) == true)
                {
                    ?>
                <li>Connect to database: <span style="color:green">Success</span></li>
                    <?php
                    if ($installer->hasPrivileges($dbhost, $dbname, $dbuser, $dbpwd) == true)
                    {
                        ?>
                <li>User has appropriate access privileges: <span style="color:green">Success</span></li>
                        <?php
                    }
                    else
                    {
                        ?>
                <li>User has appropriate access privileges: <span style="color:red">Failed</span></li>
                        <?php
                    }
                }
                else
                {
                    ?>
                <li>Connect to database: <span style="color:red">Failed</span></li>
                <li>User has appropriate access privileges: <span style="color:red">Failed</span></li>
                    <?php
                }
                ?>
            </ul>
        </div>
        
        <div>
            <h2>Summary</h2>
            <p>
                <?php
                if ($installer->canInstall($dbhost, $dbname, $dbuser, $dbpwd) == true)
                {
                    ?>
                All checks have been passed, you can now install the server on this device!
                    <?php
                }
                else
                {
                    ?>
                Some of the checks failed - you can still attempt to install the server, but it may not do so correctly.
                    <?php
                }
                ?>
            </p>
            <form action="./install.php" method="POST">
                <input type="hidden" name="dbhost" value="<?=$dbhost?>" />
                <input type="hidden" name="dbname" value="<?=$dbname?>" />
                <input type="hidden" name="dbuser" value="<?=$dbuser?>" />
                <input type="hidden" name="dbpwd" value="<?=$dbpwd?>" />
                <input type="submit" value="Begin Installation"/>
            </form>
        </div>
    </body>
</html>
