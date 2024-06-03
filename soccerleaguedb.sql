-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jun 03, 2024 at 09:08 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.0.30

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
(1, 'Team Arsenal', 1, 32, 10, 16, 9, 14, 23),
(2, 'Barnsley', 1, 95, 7, 15, 6, 1, 37),
(3, 'Team Aston Villa', 0, 68, 29, 12, 3, 13, 84),
(4, 'Team Barnsley', 0, 17, 36, 8, 5, 10, 75),
(5, 'Team Brentford', 0, 25, 99, 6, 2, 0, 73),
(6, 'Team Chelsea', 1, 59, 76, 0, 16, 19, 52),
(7, 'Team Everton', 0, 65, 69, 14, 7, 9, 20),
(8, 'Team Liverpool', 1, 58, 32, 26, 7, 4, 8),
(9, 'Team Manchester City', 1, 70, 28, 9, 13, 9, 22),
(10, 'Team Manchester United', 1, 79, 28, 1, 7, 14, 58);

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
  MODIFY `MatchId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=181;

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
