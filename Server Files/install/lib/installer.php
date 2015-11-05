<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace esol;

use PDO;

/**
 * Description of installer
 *
 * @author Matthew
 */
class installer {
    public $targetPhpVersion = "5.3.0";
    
    public function checkPhpVersion()
    {
        $comp = version_compare(phpversion(), $this->targetPhpVersion);
        
        if ($comp < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    
    public function canLogin($dbhost, $dbname, $dbuser, $dbpwd)
    {
        try
        {
            $db = new PDO('mysql:host=' . $dbhost 
                    . ';dbname=' . $dbname 
                    . ';charset=utf8', 
                    $dbuser, 
                    $dbpwd);
            
            return true;
        }
        catch (\PDOException $e)
        {
            return false;
        }
    }
    
    public function hasPrivileges($dbhost, $dbname, $dbuser, $dbpwd)
    {
        $db = new PDO('mysql:host=' . $dbhost 
                . ';dbname=' . $dbname 
                . ';charset=utf8', 
                $dbuser, 
                $dbpwd);
        
        $query = $db->prepare("SHOW GRANTS FOR CURRENT_USER");
        $query->execute();
        $rows = $query->fetchAll();
        
        for ($loop = 0; $loop < count($rows); $loop++)
        {
            $row = $rows[$loop];
            //echo $row[0];
            
            if ($row[0] == "GRANT ALL PRIVILEGES ON `" . str_replace("_", "\_", $dbname) . "`.* TO '" . $dbuser . "'@'" . $dbhost . "'")
            {
                return true;
            }
        }
        
        return false;
    }
    
    public function canInstall($dbhost, $dbname, $dbuser, $dbpwd)
    {
        if ($this->checkPhpVersion() == true 
                && $this->canLogin($dbhost, $dbname, $dbuser, $dbpwd) == true
                && $this->hasPrivileges($dbhost, $dbname, $dbuser, $dbpwd) == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public function install($dbhost, $dbname, $dbuser, $dbpwd)
    {
        try
        {
            $db = new PDO('mysql:host=' . $dbhost 
                    . ';dbname=' . $dbname 
                    . ';charset=utf8', 
                    $dbuser, 
                    $dbpwd);
            
            $sql = file_get_contents("./database.sql");
            $db->exec($sql);
        }
        catch (\PDOException $e)
        {
            return false;
        }
        
        try
        {
            $config = "<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace esol;

/**
 * Description of config
 *
 * @author Matthew
 */
class config {
    public static \$dbhost = '$dbhost';
    public static \$dbname = '$dbname';
    public static \$dbuser = '$dbuser';
    public static \$dbpwd = '$dbpwd';
    
    public static \$salt = '" . $this->randomString(20) . "';
}";
            file_put_contents("../lib/config.php", $config);
        }
        catch (Exception $e)
        {
            return false;
        }
        
        return true;
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
