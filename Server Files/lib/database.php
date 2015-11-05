<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace esol;

include 'config.php';
include 'save.php';
include 'selectedanimal.php';

use PDO;

/**
 * Handles all input and output functionality for the database.
 *
 * @author Matthew
 */
class database {
    private $db;
    
    public function __construct() {
        $this->config = new config();
        
        $this->db = new PDO('mysql:host=' . config::$dbhost 
                . ';dbname=' . config::$dbname 
                . ';charset=utf8', 
                config::$dbuser, 
                config::$dbpwd);
    }
    
    /**
     * Registers a new device on the database.
     * 
     * @param type $region The country code for the new device.
     * @return type The access token the device should use for uploading saves.
     */
    public function register($region, $language) {
        $token = $this->randomString(20);
        $hashed = crypt($token, config::$salt);
        $countQuery = $this->db->prepare("SELECT COUNT(*) FROM users "
                . "WHERE token = :hashed");
        $countQuery->execute(array('hashed' => $hashed));
        
        while ($countQuery->fetchColumn() > 0)
        {
            $token = $this->randomString(20);
            $hashed = crypt($token, config::$salt);
            $countQuery->execute(array('hashed' => $hashed));
        }
        
        $query = $this->db->prepare("INSERT INTO users (token, "
                . "region, "
                . "language)"
                . " VALUES (:hashed, "
                . ":region, "
                . ":language)");
        $query->execute(array('hashed' => $hashed, 
            'region' => $region, 
            'language' => $language));
        return $token;
    }
    
    /**
     * Uploads a new save file.
     * 
     * @param type $token The user's access token.
     * @param \esol\save $save The save file to upload.
     * @return boolean True if signed in correctly, false otherwise.
     */
    public function uploadSave($token, save $save)
    {
        $id = $this->getUserId($token);
        
        if ($id != false)
        {
            $saveQuery = $this->db->prepare("INSERT INTO saves (userid, "
                    . "timestamp, "
                    . "randomseed, "
                    . "height, "
                    . "smogtimer, "
                    . "pesttimer, "
                    . "canlevel, "
                    . "hydrationlevel, "
                    . "selectedaccessory) "
                    . "VALUES (:userid, "
                    . ":timestamp, "
                    . ":randomseed, "
                    . ":height, "
                    . ":smogtimer, "
                    . ":pesttimer, "
                    . ":canlevel, "
                    . ":hydrationlevel, "
                    . ":selectedaccessory)");
            $saveQuery->execute(array('userid' => $id, 
                'timestamp' => $save->saveDate, 
                'randomseed' => $save->randomSeed, 
                'height' => $save->height, 
                'smogtimer' => $save->smogTimer, 
                'pesttimer' => $save->pestTimer, 
                'canlevel' => $save->canLevel, 
                'hydrationlevel' => $save->hydrationLevel, 
                'selectedaccessory' => $save->selectedAccessory));
            
            $saveId = $this->db->lastInsertId();
            //echo $saveId;
            
            $accQuery = $this->db->prepare("INSERT INTO unlockedaccessories "
                    . "(saveid, "
                    . "accessory) "
                    . "VALUES (:saveid, "
                    . ":accessory)");
            foreach ($save->unlockedAccessories as $accessory)
            {
                //echo $accessory . " ";
                $accQuery->execute(array('saveid' => $saveId, 
                    'accessory' => $accessory));
                //echo $accQuery->errorInfo()[2];
            }
            ///echo "\r\n";
            
            $selectAnimalsQuery = $this->db->prepare("INSERT INTO selectedanimals "
                    . "(saveid, "
                    . "branch, "
                    . "animal) "
                    . "VALUES (:saveid, "
                    . ":branch, "
                    . ":animal)");
            foreach ($save->selectedAnimals as $animal)
            {
                //echo $animal->branch . " " . $animal->animal . " ";
                $selectAnimalsQuery->execute(array('saveid' => $saveId, 
                    'branch' => $animal->branch,
                    'animal' => $animal->animal));
                //echo $selectAnimalsQuery->errorInfo()[2];
            }
            //echo "\r\n";
            
            $unlockedAnimalsQuery = $this->db->prepare("INSERT INTO unlockedanimals "
                    . "(saveid, "
                    . "animal) "
                    . "VALUES (:saveid, "
                    . ":animal)");
            foreach ($save->unlockedAnimals as $animal)
            {
                //echo $animal;
                $unlockedAnimalsQuery->execute(array('saveid' => $saveId,
                    'animal' => $animal));
                //echo $unlockedAnimalsQuery->errorInfo()[2];
            }
            //echo "\r\n";
            
            return true;
        }
        else
        {
            return false;
        }
    }
    
    /**
     * Downloads a save at random from the specified region.
     * 
     * @param type $region The country code to download a save from.
     * @return \esol\save|boolean The downloaded save, or false if no saves in that country.
     */
    public function downloadSave($region)
    {
        $numSaves = $this->getNumSaves($region);
        //echo $numSaves;
        
        if ($numSaves == 0)
        {
            //echo "No saves.";
            return false;
        }
        
        $saveNumber = rand(0, $numSaves - 1);
        $saveQuery = $this->db->prepare("SELECT *, saves.id AS saveid FROM saves, users "
                . "WHERE saves.userid = users.id "
                . "AND region = :region");
        $saveQuery->execute(array('region' => $region));
        $rows = $saveQuery->fetchAll();
        $row = $rows[$saveNumber];
        
        //echo $saveQuery->errorInfo()[2];
        
        //print_r(array_keys($row));
        //print_r(array_values($row));
        
        $saveId = $row['saveid'];
        //echo $saveId;
        
        $save = new save();
        $save->language = $row['language'];
        $save->saveDate = $row['timestamp'];
        $save->randomSeed = $row['randomseed'];
        $save->height = $row['height'];
        $save->smogTimer = $row['smogtimer'];
        $save->pestTimer = $row['pesttimer'];
        $save->canLevel = $row['canlevel'];
        $save->hydrationLevel = $row['hydrationlevel'];
        $save->selectedAccessory = $row['selectedaccessory'];
        
        $accQuery = $this->db->prepare("SELECT * FROM unlockedaccessories "
                . "WHERE saveid = :saveid");
        $accQuery->execute(array('saveid' => $saveId));
        $accRows = $accQuery->fetchAll();
        //echo "<" . $accQuery->errorInfo()[2] . ">";
        //echo "<" . count($accRows) . ">";
        for ($loop = 0; $loop < count($accRows); $loop++)
        {
            $row = $accRows[$loop];
            $save->unlockedAccessories[$loop] = $row['accessory'];
        }
        
        $selAnimQuery = $this->db->prepare("SELECT * FROM selectedanimals "
                . "WHERE saveid = :saveid");
        $selAnimQuery->execute(array('saveid' => $saveId));
        $selAnimRows = $selAnimQuery->fetchAll();
        for ($loop = 0; $loop < count($selAnimRows); $loop++)
        {
            $row = $selAnimRows[$loop];
            $animal = new selectedanimal();
            $animal->branch = $row['branch'];
            $animal->animal = $row['animal'];
            $save->selectedAnimals[$loop] = $animal;
        }
        
        $unAnimQuery = $this->db->prepare("SELECT * FROM unlockedanimals "
                . "WHERE saveid = :saveid");
        $unAnimQuery->execute(array('saveid' => $saveId));
        $unAnimRows = $unAnimQuery->fetchAll();
        for ($loop = 0; $loop < count($unAnimRows); $loop++)
        {
            $row = $unAnimRows[$loop];
            $save->unlockedAnimals[$loop] = $row['animal'];
        }
        
        return $save;
    }
    
    /**
     * Gets the number of users currently registered.
     * @return type The total number of users.
     */
    public function getNumUsers()
    {
        $countQuery = $this->db->prepare("SELECT COUNT( * ) FROM users");
        $countQuery->execute();
        return $countQuery->fetchColumn();
    }
    
    /**
     * Gets the number of users for each region.
     * 
     * @return type An array containing the number of users per region.
     */
    public function getNumUsersGrouped()
    {
        $countQuery = $this->db->prepare("SELECT region, COUNT( * ) FROM users "
                . "GROUP BY region");
        $countQuery->execute();
        return $countQuery->fetchAll();
    }
    
    /**
     * Gets the number of save files in the database.
     * 
     * @param type $region The region to search, or false to count all.
     * @return type The number of saves.
     */
    public function getNumSaves($region = false)
    {
        if ($region != false)
        {
            $countQuery = $this->db->prepare("SELECT COUNT( * ) FROM saves, users "
                    . "WHERE saves.userid = users.id "
                    . "AND region = :region");
        }
        else
        {
            $countQuery = $this->db->prepare("SELECT COUNT( * ) FROM saves");
        }
        $countQuery->execute(array('region' => $region));
        return $countQuery->fetchColumn();
    }
    
    /**
     * Gets the number of saves for each region.
     * 
     * @return type An array containing the number of users per region.
     */
    public function getNumSavesGrouped()
    {
        $countQuery = $this->db->prepare("SELECT users.region, COUNT( * ) FROM saves, users "
                . "WHERE saves.userid = users.id "
                . "GROUP BY users.region");
        $countQuery->execute();
        return $countQuery->fetchAll();
    }
    
    /**
     * Gets the database ID for the user with the specified token.
     * 
     * @param type $token The user's access token.
     * @return boolean The user's ID, or false if no user with that token was found.
     */
    private function getUserId($token)
    {
        $hashed = crypt($token, config::$salt);
        
        $countQuery = $this->db->prepare("SELECT COUNT(*) FROM users WHERE token = :token");
        $countQuery->execute(array('token' => $hashed));
        
        if ($countQuery->fetchColumn() > 0)
        {
            $query = $this->db->prepare("SELECT * FROM users WHERE token = :token");
            $query->execute(array('token' => $hashed));
            
            $row = $query->fetch();
            return $row['id'];
        }
        
        return false;
    }
    
    /**
     * Generates a random string.
     * 
     * @param type $length The length of the string to generate.
     * @return string The random string.
     */
    private function randomString($length = 10) {
        $characters = '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ';
        $charactersLength = strlen($characters);
        
        $randomString = '';
        for ($i = 0; $i < $length; $i++) {
            $randomString .= $characters[rand(0, $charactersLength - 1)];
        }
        
        return $randomString;
    }
}
