<?php

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
    public static $dbhost = "Enter your database host here (usually localhost)";
    public static $dbname = "Enter your database name here";
    public static $dbuser = "Enter your database login username here";
    public static $dbpwd = "Enter your database login password here";
    
    public static $salt = "Enter a long, random string here (used for secure password generation)";
}
