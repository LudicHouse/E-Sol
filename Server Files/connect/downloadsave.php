<?php

/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

include '../lib/database.php';

$db = new esol\database();
$save = $db->downloadSave($_GET['region']);

if ($save != false)
{
    echo $save->language . "\n\r";
    echo $save->saveDate . "\n\r";
    echo $save->randomSeed . "\n\r";
    echo $save->height . "\n\r";
    echo $save->smogTimer . "\n\r";
    echo $save->pestTimer . "\n\r";
    echo $save->canLevel . "\n\r";
    echo $save->hydrationLevel . "\n\r";
    echo $save->selectedAccessory . "\n\r";
    
    echo count($save->unlockedAccessories) . "\n\r";
    foreach ($save->unlockedAccessories as $accessory)
    {
        echo $accessory . "\n\r";
    }
    
    echo count($save->selectedAnimals) . "\n\r";
    foreach ($save->selectedAnimals as $selAnim)
    {
        echo $selAnim->branch . "\n\r";
        echo $selAnim->animal . "\n\r";
    }
    
    echo count($save->unlockedAnimals) . "\n\r";
    foreach ($save->unlockedAnimals as $animal)
    {
        echo $animal . "\n\r";
    }
}
else
{
    echo "false";
}