<!DOCTYPE html>
<!--
To change this license header, choose License Headers in Project Properties.
To change this template file, choose Tools | Templates
and open the template in the editor.
-->
<?php
include 'lib/database.php';

$db = new \esol\database();
?>

<html>
    <head>
        <meta charset="UTF-8">
        <title></title>
    </head>
    <body>
        <h1>E-Sol Server Statistics</h1>
        
        <?php
        if (file_exists("./install") == true)
        {
            ?>
        <p style="color:red">Warning: You do not appear to have deleted the /install directory. Please do this immediately as leaving it poses a security risk.</p>
            <?php
        }
        ?>
        
        <div>
            <h2>Basic Stats</h2>
            <ul>
                <li>Total Plants: <?=$db->getNumUsers()?></li>
                <li>Total Saves: <?=$db->getNumSaves()?></li>
                <li>Average Saves Per Plant: <?=$db->getNumSaves() / $db->getNumUsers()?></li>
            </ul>
        </div>
        
        <div>
            <h2>Region Plants Breakdown</h2>
            <ul>
                <?php
                $regions = $db->getNumUsersGrouped();
                foreach ($regions as $region)
                {
                    echo "<li>" . $region['region'] . ": " . $region['COUNT( * )'] . "</li>\n\r";
                }
                ?>
            </ul>
        </div>
        
        <div>
            <h2>Region Saves Breakdown</h2>
            <ul>
                <?php
                $regions = $db->getNumSavesGrouped();
                foreach ($regions as $region)
                {
                    echo "<li>" . $region['region'] . ": " . $region['COUNT( * )'] . "</li>\n\r";
                }
                ?>
            </ul>
        </div>
    </body>
</html>
