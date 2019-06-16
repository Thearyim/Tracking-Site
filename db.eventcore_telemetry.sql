-- phpMyAdmin SQL Dump
-- version 4.8.3
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: Jun 14, 2019 at 05:07 PM
-- Server version: 5.7.24-log
-- PHP Version: 7.2.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `eventcore_telemetry`
--

-- --------------------------------------------------------

--
-- Table structure for table `telemetryevents`
--

CREATE TABLE `telemetryevents` (
  `Id` bigint(20) NOT NULL,
  `Timestamp` datetime NOT NULL,
  `EventName` varchar(256) NOT NULL,
  `CorrelationId` char(36) NOT NULL,
  `Context` text
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `telemetryevents`
--

INSERT INTO `telemetryevents` (`Id`, `Timestamp`, `EventName`, `CorrelationId`, `Context`) VALUES
(2, '2019-06-14 16:20:56', 'WebsiteAvailabilityCheck', 'e915e981-92ae-4968-84c1-52437f3c265a', '{\"uri\":\"www.eventcore.com\",\"status\":200}'),
(3, '2019-06-14 16:20:56', 'WebsiteAvailabilityCheck', 'e915e981-92ae-4968-84c1-52437f3c265a', '{\"uri\":\"www.google.com\",\"status\":200}'),
(4, '2019-06-14 16:22:50', 'WebsiteAvailabilityCheck', '9d3ae827-a217-474f-a722-da60cf1f3885', '{\"uri\":\"www.eventcore.com\",\"status\":200}'),
(5, '2019-06-14 16:22:50', 'WebsiteAvailabilityCheck', '9d3ae827-a217-474f-a722-da60cf1f3885', '{\"uri\":\"www.google.com\",\"status\":200}'),
(6, '2019-06-14 16:23:45', 'WebsiteAvailabilityCheck', '3cb5aef8-bb8a-443c-9116-a991bebb1588', '{\"uri\":\"www.eventcore.com\",\"status\":200}'),
(7, '2019-06-14 16:23:45', 'WebsiteAvailabilityCheck', '3cb5aef8-bb8a-443c-9116-a991bebb1588', '{\"uri\":\"www.google.com\",\"status\":200}');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `telemetryevents`
--
ALTER TABLE `telemetryevents`
  ADD PRIMARY KEY (`Id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `telemetryevents`
--
ALTER TABLE `telemetryevents`
  MODIFY `Id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
