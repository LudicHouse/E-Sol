<?php

/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

include '../lib/database.php';

$save = new \esol\save();
$save->saveDate = $_GET['savedate'];
//echo $save->saveDate . " ";
$save->randomSeed = $_GET['randomseed'];
//echo $save->randomSeed . " ";
$save->height = $_GET['height'];
//echo $save->height . " ";
$save->smogTimer = $_GET['smogtimer'];
//echo $save->smogTimer . " ";
$save->pestTimer = $_GET['pesttimer'];
//echo $save->pestTimer . " ";
$save->canLevel = $_GET['canlevel'];
//echo $save->canLevel . " ";
$save->hydrationLevel = $_GET['hydrationlevel'];
//echo $save->hydrationLevel . " ";
$save->selectedAccessory = $_GET['selectedaccessory'];
//echo $save->selectedAccessory . " ";
//echo "\n\r";

$numAccessories = $_GET['numaccessories'];
for ($loop = 0; $loop < $numAccessories; $loop++)
{
    $save->unlockedAccessories[$loop] = $_GET['accessory' . $loop];
}

$numSelAnim = $_GET['numselanim'];
for ($loop = 0; $loop < $numSelAnim; $loop++)
{
    $anim = new esol\selectedanimal();
    $anim->branch = $_GET['selbranch' . $loop];
    $anim->animal = $_GET['selanimal' . $loop];
    $save->selectedAnimals[$loop] = $anim;
}

$numUnAnim = $_GET['numunanim'];
for ($loop = 0; $loop < $numUnAnim; $loop++)
{
    $save->unlockedAnimals[$loop] = $_GET['unanimal' . $loop];
}

$db = new esol\database();
$result = $db->uploadSave($_GET['token'], $save);

if ($result == true)
{
    echo "true";
}
elseif ($result == false)
{
    echo "false";
}
 else
{
    echo "Something went wrong!";
}