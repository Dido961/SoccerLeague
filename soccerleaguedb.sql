-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jun 12, 2024 at 10:26 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `soccerleaguedb`
--

-- --------------------------------------------------------

--
-- Table structure for table `matches`
--

CREATE TABLE `matches` (
  `MatchId` int(11) NOT NULL,
  `HomeTeamId` int(11) NOT NULL,
  `AwayTeamId` int(11) NOT NULL,
  `HomeGoals` int(11) DEFAULT 0,
  `AwayGoals` int(11) DEFAULT 0,
  `Date` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `teams`
--

CREATE TABLE `teams` (
  `TeamId` int(11) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `IsGrand` tinyint(1) NOT NULL DEFAULT 0,
  `GoalsFor` int(11) DEFAULT 0,
  `GoalsAgainst` int(11) DEFAULT 0,
  `Wins` int(11) DEFAULT 0,
  `Draws` int(11) DEFAULT 0,
  `Losses` int(11) DEFAULT 0,
  `Points` int(11) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `teams`
--

INSERT INTO `teams` (`TeamId`, `Name`, `IsGrand`, `GoalsFor`, `GoalsAgainst`, `Wins`, `Draws`, `Losses`, `Points`) VALUES
(1, 'Team Arsenal', 1, 0, 0, 0, 0, 0, 0),
(2, 'Barnsley', 1, 0, 0, 0, 0, 0, 0),
(3, 'Team Aston Villa', 0, 0, 0, 0, 0, 0, 0),
(4, 'Team Barnsley', 0, 0, 0, 0, 0, 0, 0),
(5, 'Team Brentford', 0, 0, 0, 0, 0, 0, 0),
(6, 'Team Chelsea', 1, 0, 0, 0, 0, 0, 0),
(7, 'Team Everton', 0, 0, 0, 0, 0, 0, 0),
(8, 'Team Liverpool', 1, 0, 0, 0, 0, 0, 0),
(9, 'Team Manchester City', 1, 0, 0, 0, 0, 0, 0),
(10, 'Team Manchester United', 1, 0, 0, 0, 0, 0, 0);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `matches`
--
ALTER TABLE `matches`
  ADD PRIMARY KEY (`MatchId`),
  ADD KEY `HomeTeamId` (`HomeTeamId`),
  ADD KEY `AwayTeamId` (`AwayTeamId`);

--
-- Indexes for table `teams`
--
ALTER TABLE `teams`
  ADD PRIMARY KEY (`TeamId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `matches`
--
ALTER TABLE `matches`
  MODIFY `MatchId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=496;

--
-- AUTO_INCREMENT for table `teams`
--
ALTER TABLE `teams`
  MODIFY `TeamId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=31;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `matches`
--
ALTER TABLE `matches`
  ADD CONSTRAINT `matches_ibfk_1` FOREIGN KEY (`HomeTeamId`) REFERENCES `teams` (`TeamId`) ON DELETE CASCADE,
  ADD CONSTRAINT `matches_ibfk_2` FOREIGN KEY (`AwayTeamId`) REFERENCES `teams` (`TeamId`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
