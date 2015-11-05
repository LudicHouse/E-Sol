<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace esol;

/**
 * Represents a save file within the game client.
 *
 * @author Matthew
 */
class save {
    public $language;
    
    public $saveDate;
    
    public $randomSeed;
    public $height;
    
    public $smogTimer;
    public $pestTimer;
    public $canLevel;
    public $hydrationLevel;
    
    public $selectedAccessory;
    public $unlockedAccessories;
    
    public $selectedAnimals;
    public $unlockedAnimals;
}
