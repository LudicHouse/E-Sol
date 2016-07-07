-- phpMyAdmin SQL Dump
-- version 3.5.8.2
-- http://www.phpmyadmin.net
--
-- Host: localhost
-- Generation Time: Jul 30, 2015 at 09:40 AM
-- Server version: 5.5.42-37.1-log
-- PHP Version: 5.4.23

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Database: `gkorguk1_esol`
--

-- --------------------------------------------------------

--
-- Table structure for table `saves`
--

DROP TABLE IF EXISTS `saves`;
CREATE TABLE IF NOT EXISTS `saves` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userid` int(11) NOT NULL,
  `timestamp` text NOT NULL,
  `randomseed` int(11) NOT NULL,
  `height` float NOT NULL,
  `smogtimer` float NOT NULL,
  `pesttimer` float NOT NULL,
  `canlevel` float NOT NULL,
  `hydrationlevel` float NOT NULL,
  `selectedaccessory` text NOT NULL,
  PRIMARY KEY (`id`),
  KEY `userid` (`userid`)
) ENGINE=MyISAM  DEFAULT CHARSET=latin1 AUTO_INCREMENT=28 ;

-- --------------------------------------------------------

--
-- Table structure for table `selectedanimals`
--

DROP TABLE IF EXISTS `selectedanimals`;
CREATE TABLE IF NOT EXISTS `selectedanimals` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `saveid` int(11) NOT NULL,
  `branch` int(11) NOT NULL,
  `animal` text NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=latin1 AUTO_INCREMENT=14 ;

-- --------------------------------------------------------

--
-- Table structure for table `unlockedaccessories`
--

DROP TABLE IF EXISTS `unlockedaccessories`;
CREATE TABLE IF NOT EXISTS `unlockedaccessories` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `saveid` int(11) NOT NULL,
  `accessory` text NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=latin1 AUTO_INCREMENT=77 ;

-- --------------------------------------------------------

--
-- Table structure for table `unlockedanimals`
--

DROP TABLE IF EXISTS `unlockedanimals`;
CREATE TABLE IF NOT EXISTS `unlockedanimals` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `saveid` int(11) NOT NULL,
  `animal` text NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=latin1 AUTO_INCREMENT=79 ;

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
CREATE TABLE IF NOT EXISTS `users` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `token` text NOT NULL,
  `region` text NOT NULL,
  `language` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=latin1 AUTO_INCREMENT=9 ;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
