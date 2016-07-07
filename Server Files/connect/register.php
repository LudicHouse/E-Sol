<?php

/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

include '../lib/database.php';

$db = new esol\database();
$token = $db->register($_GET['region'], $_GET['language']);

echo $token;