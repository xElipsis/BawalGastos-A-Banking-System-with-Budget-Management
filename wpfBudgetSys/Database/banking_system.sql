-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: May 22, 2026 at 01:39 AM
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
-- Database: `banking_system`
--

-- --------------------------------------------------------

--
-- Table structure for table `accounts`
--

CREATE TABLE `accounts` (
  `account_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `account_number` varchar(255) NOT NULL,
  `account_type` set('Savings','Checking') NOT NULL,
  `balance` decimal(10,0) NOT NULL,
  `status` set('Active','Frozen','Closed') NOT NULL DEFAULT 'Active',
  `created_at` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `accounts`
--

INSERT INTO `accounts` (`account_id`, `user_id`, `account_number`, `account_type`, `balance`, `status`, `created_at`) VALUES
(1, 12, 'ACC-20260519-25260', 'Savings', 1070, 'Active', '2026-05-19 23:56:16'),
(2, 13, 'ACC-20260520-28058', 'Savings', 7000, 'Active', '2026-05-20 23:06:36'),
(3, 18, 'ACC-20260521-46462', 'Savings', 5600, 'Active', '2026-05-21 10:51:11'),
(4, 19, 'ACC-20260521-83838', 'Savings', 20000, 'Active', '2026-05-21 11:45:18'),
(5, 23, 'ACC-20260521-65384', 'Savings', 23000, 'Active', '2026-05-22 00:00:35');

-- --------------------------------------------------------

--
-- Table structure for table `alerts`
--

CREATE TABLE `alerts` (
  `alert_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `category_id` int(11) NOT NULL,
  `alert_type` varchar(50) NOT NULL,
  `message` varchar(255) NOT NULL,
  `is_read` tinyint(1) NOT NULL DEFAULT 0,
  `triggered_at` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `alerts`
--

INSERT INTO `alerts` (`alert_id`, `user_id`, `category_id`, `alert_type`, `message`, `is_read`, `triggered_at`) VALUES
(1, 12, 1, '50%', 'You have reached 50% of your monthly budget for this category (₱3,000.00 of ₱5,000.00).', 1, '2026-05-21 01:01:40'),
(2, 12, 1, '75%', 'You have reached 75% of your monthly budget for this category (₱4,500.00 of ₱5,000.00).', 1, '2026-05-21 01:03:10'),
(3, 12, 1, '90%', 'You have reached 90% of your monthly budget for this category (₱4,500.00 of ₱5,000.00).', 1, '2026-05-21 01:03:10'),
(4, 12, 1, '100%', 'You have reached 100% of your monthly budget for this category (₱6,200.00 of ₱5,000.00).', 1, '2026-05-21 10:37:56'),
(5, 18, 1, '50%', 'You have reached 50% of your monthly budget for this category (₱2,500.00 of ₱5,000.00).', 1, '2026-05-21 10:52:18'),
(6, 19, 4, '50%', 'You have reached 50% of your monthly budget for this category (₱8,000.00 of ₱10,000.00).', 1, '2026-05-21 11:46:28'),
(7, 19, 4, '75%', 'You have reached 75% of your monthly budget for this category (₱8,000.00 of ₱10,000.00).', 1, '2026-05-21 11:46:28'),
(8, 12, 6, '50%', 'You have reached 50% of your monthly budget for this category (₱7,000.00 of ₱12,000.00).', 1, '2026-05-21 15:06:00'),
(9, 12, 6, '75%', 'You have reached 75% of your monthly budget for this category (₱9,000.00 of ₱12,000.00).', 1, '2026-05-21 20:51:59'),
(10, 12, 6, '90%', 'You have reached 90% of your monthly budget for this category (₱12,200.00 of ₱12,000.00).', 1, '2026-05-22 01:41:49'),
(11, 12, 6, '100%', 'You have reached 100% of your monthly budget for this category (₱12,200.00 of ₱12,000.00).', 1, '2026-05-22 01:41:49');

-- --------------------------------------------------------

--
-- Table structure for table `expense_categories`
--

CREATE TABLE `expense_categories` (
  `category_id` int(11) NOT NULL,
  `user_id` int(11) DEFAULT NULL,
  `category_name` varchar(255) NOT NULL,
  `is_default` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `expense_categories`
--

INSERT INTO `expense_categories` (`category_id`, `user_id`, `category_name`, `is_default`) VALUES
(1, NULL, 'Food & Dining', 1),
(2, NULL, 'Transportation', 1),
(3, NULL, 'Bills & Utilities', 1),
(4, NULL, 'Shopping', 1),
(5, NULL, 'Health & Medical', 1),
(6, NULL, 'Education', 1),
(7, NULL, 'Entertainment', 1),
(8, NULL, 'Savings Goal', 1),
(9, NULL, 'Remittance', 1),
(11, 12, 'Car Expenses', 0),
(12, 13, 'Tech Expenses', 0);

-- --------------------------------------------------------

--
-- Table structure for table `logins`
--

CREATE TABLE `logins` (
  `login_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `username` varchar(255) NOT NULL,
  `password_hash` varchar(64) NOT NULL,
  `last_login` datetime DEFAULT NULL,
  `failed_attempts` int(11) NOT NULL DEFAULT 0,
  `is_locked` tinyint(1) NOT NULL DEFAULT 0,
  `locked_until` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `logins`
--

INSERT INTO `logins` (`login_id`, `user_id`, `username`, `password_hash`, `last_login`, `failed_attempts`, `is_locked`, `locked_until`) VALUES
(3, 12, 'Babayaga', '1718c24b10aeb8099e3fc44960ab6949ab76a267352459f203ea1036bec382c2', '2026-05-22 01:56:42', 0, 0, NULL),
(4, 13, 'Elipsinigger', 'ef797c8118f02dfb649607dd5d3f8c7623048c9c063d532cc95c5ed7a898a64f', '2026-05-22 00:03:55', 0, 0, NULL),
(5, 15, 'Ayawton1234', '56d37b1dea780e1c6a6d77c117c96e6e2a4a5693aaa4258e94aabb39d5c6b2c8', NULL, 0, 0, NULL),
(6, 18, 'trdt', '932f3c1b56257ce8539ac269d7aab42550dacf8818d075f0bdf1990562aae3ef', NULL, 0, 0, NULL),
(7, 19, 'Vanixno', '4a9ca4596692e94f9d2912b06a0d007564a22ee750339a6021c2392149b25d6d', NULL, 0, 0, NULL),
(8, 22, 'xX_AdMinisTrator_Xx', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', '2026-05-22 01:48:13', 1, 0, NULL),
(9, 23, 'Aniggerballs', '9a66e85977348ff3f80eabfe3e7fc7c1492f48c70a238881b27ee408d1e1a0ef', '2026-05-22 00:07:23', 0, 0, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `notifications`
--

CREATE TABLE `notifications` (
  `notification_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `title` varchar(100) NOT NULL,
  `message` varchar(255) NOT NULL,
  `type` enum('Transaction','Account','System','Admin') NOT NULL,
  `is_read` tinyint(1) NOT NULL DEFAULT 0,
  `created_at` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `notifications`
--

INSERT INTO `notifications` (`notification_id`, `user_id`, `title`, `message`, `type`, `is_read`, `created_at`) VALUES
(1, 19, 'Deposit Successful', 'You deposited ₱12,000.00 to your account.', 'Transaction', 1, '2026-05-21 11:46:55'),
(2, 12, 'Deposit Successful', 'You deposited ₱2,000.00 to your account.', 'Transaction', 1, '2026-05-21 12:00:56'),
(3, 12, 'Withdraw Successful', 'You withdrawed ₱50.00 to your account.', 'Transaction', 1, '2026-05-21 12:01:09'),
(4, 12, 'Transfer Successful', 'You transferred ₱1,000.00 from your account to account number ACC-20260520-28058.', 'Transaction', 1, '2026-05-21 12:05:25'),
(5, 12, 'Deposit Successful', 'You deposited ₱20,000.00 to your account.', 'Transaction', 1, '2026-05-21 12:07:24'),
(6, 12, 'Deposit Successful', 'You deposited ₱1,000.00 to your account.', 'Transaction', 1, '2026-05-21 15:04:55'),
(7, 12, 'Payment Successful', 'You payed ₱6,000.00 to Education.', 'Transaction', 1, '2026-05-21 15:06:00'),
(8, 12, 'Payment Successful', 'You payed ₱1,000.00 to Education.', 'Transaction', 1, '2026-05-21 15:09:27'),
(9, 12, 'Deposit Successful', 'You deposited ₱2,000.00 to your account.', 'Transaction', 1, '2026-05-21 15:19:35'),
(10, 12, 'Deposit Successful', 'You deposited ₱1,000.00 to your account.', 'Transaction', 1, '2026-05-21 20:38:36'),
(11, 12, 'Deposit Successful', 'You deposited ₱1,000.00 to your account.', 'Transaction', 1, '2026-05-21 20:42:32'),
(12, 12, 'Withdraw Successful', 'You withdrawed ₱2,000.00 from your account.', 'Transaction', 1, '2026-05-21 20:43:04'),
(13, 12, 'Withdraw Successful', 'You withdrawed ₱1,000.00 from your account.', 'Transaction', 1, '2026-05-21 20:43:10'),
(14, 12, 'Transfer Successful', 'You transferred ₱2,000.00 from your account to account number ACC-20260521-46462.', 'Transaction', 1, '2026-05-21 20:44:25'),
(15, 12, 'Transfer Successful', 'You transferred ₱100.00 from your account to account number ACC-20260521-46462.', 'Transaction', 1, '2026-05-21 20:44:31'),
(16, 12, 'Deposit Successful', 'You deposited ₱1,000.00 to your account.', 'Transaction', 1, '2026-05-21 20:49:49'),
(17, 12, 'Withdraw Successful', 'You withdrawed ₱1,000.00 from your account.', 'Transaction', 1, '2026-05-21 20:49:57'),
(18, 12, 'Transfer Successful', 'You transferred ₱1,000.00 from your account to account number ACC-20260521-46462.', 'Transaction', 1, '2026-05-21 20:51:29'),
(19, 12, 'Payment Successful', 'You payed ₱1,000.00 to Education.', 'Transaction', 1, '2026-05-21 20:51:59'),
(20, 12, 'Deposit Successful', 'You deposited ₱1,000.00 to your account.', 'Transaction', 1, '2026-05-21 20:59:53'),
(21, 12, 'Deposit Successful', 'You deposited ₱1,000.00 to your account.', 'Transaction', 1, '2026-05-21 21:00:43'),
(22, 12, 'Test Notification', 'Testing new system-wide notification', 'System', 1, '2026-05-21 21:04:09'),
(23, 13, 'Test Notification', 'Testing new system-wide notification', 'System', 1, '2026-05-21 21:04:09'),
(24, 15, 'Test Notification', 'Testing new system-wide notification', 'System', 0, '2026-05-21 21:04:09'),
(25, 18, 'Test Notification', 'Testing new system-wide notification', 'System', 0, '2026-05-21 21:04:09'),
(26, 19, 'Test Notification', 'Testing new system-wide notification', 'System', 0, '2026-05-21 21:04:09'),
(29, 12, 'Account reactivated', 'Your bank account has been reopened. You can use all banking features again.', 'Account', 1, '2026-05-22 00:02:47'),
(30, 13, 'Account closed', 'Your bank account has been closed. You can still sign in, but deposits, withdrawals, transfers, and payments are disabled. Contact an administrator to reopen your account — you do not need to register again.', 'Account', 1, '2026-05-22 00:03:23'),
(31, 13, 'Payment Successful', 'You payed ₱1,000.00 to Food & Dining.', 'Transaction', 1, '2026-05-22 00:04:45'),
(32, 12, 'Deposit Successful', 'You deposited ₱1,000.00 to your account.', 'Transaction', 1, '2026-05-22 00:06:15'),
(33, 12, 'Withdraw Successful', 'You withdrawed ₱1,000.00 from your account.', 'Transaction', 1, '2026-05-22 00:06:28'),
(34, 12, 'Transfer Successful', 'You transferred ₱2,000.00 from your account to account number ACC-20260521-65384.', 'Transaction', 1, '2026-05-22 00:07:04'),
(35, 12, 'Deposit Successful', 'You deposited ₱100.00 to your account.', 'Transaction', 1, '2026-05-22 00:17:45'),
(36, 12, 'Deposit Successful', 'You deposited ₱500.00 to your account.', 'Transaction', 1, '2026-05-22 00:18:50'),
(37, 12, 'Withdraw Successful', 'You withdrawed ₱1,000.00 from your account.', 'Transaction', 1, '2026-05-22 00:19:55'),
(38, 12, 'Deposit Successful', 'You deposited ₱1,000.00 to your account.', 'Transaction', 1, '2026-05-22 00:20:07'),
(39, 12, 'Payment Successful', 'You payed ₱200.00 to Education.', 'Transaction', 1, '2026-05-22 00:22:19'),
(40, 12, 'Payment Successful', 'You payed ₱1,000.00 to Car Expenses.', 'Transaction', 1, '2026-05-22 00:22:59'),
(41, 12, 'Payment Successful', 'You payed ₱1,000.00 to Car Expenses.', 'Transaction', 1, '2026-05-22 00:23:29'),
(42, 12, 'Payment Successful', 'You payed ₱1,231.00 to Car Expenses.', 'Transaction', 1, '2026-05-22 00:23:34'),
(43, 12, 'Deposit Successful', 'You deposited ₱1.00 to your account.', 'Transaction', 1, '2026-05-22 00:23:48'),
(44, 13, 'Account reactivated', 'Your bank account has been reopened. You can use all banking features again.', 'Account', 0, '2026-05-22 00:53:30'),
(45, 12, 'Deposit Successful', 'You deposited ₱1,000.00 to your account.', 'Transaction', 1, '2026-05-22 01:09:34'),
(46, 12, 'Deposit Successful', 'You deposited ₱1,000.00 to your account.', 'Transaction', 1, '2026-05-22 01:10:50'),
(47, 12, 'Deposit Successful', 'You deposited ₱2,000.00 to your account.', 'Transaction', 1, '2026-05-22 01:17:19'),
(48, 12, 'Transfer Successful', 'You transferred ₱1,000.00 from your account to account number ACC-20260521-65384.', 'Transaction', 1, '2026-05-22 01:25:32'),
(49, 12, 'Transfer Successful', 'Your have received ₱1,000.00 from account number ACC-20260519-25260.', 'Transaction', 1, '2026-05-22 01:25:32'),
(50, 12, 'Transfer Successful', 'You transferred ₱2,000.00 from your account to account number ACC-20260521-65384.', 'Transaction', 1, '2026-05-22 01:35:37'),
(51, 12, 'Transfer Successful', 'Your have received ₱2,000.00 from account number ACC-20260519-25260.', 'Transaction', 1, '2026-05-22 01:35:37'),
(52, 12, 'Payment Successful', 'You payed ₱3,000.00 to Education.', 'Transaction', 1, '2026-05-22 01:41:49'),
(53, 12, 'Transfer Successful', 'You transferred ₱3,000.00 from your account to account number ACC-20260521-65384.', 'Transaction', 1, '2026-05-22 01:44:53'),
(54, 12, 'Transfer Successful', 'Your have received ₱3,000.00 from account number ACC-20260519-25260.', 'Transaction', 1, '2026-05-22 01:44:53'),
(55, 12, 'Withdraw Successful', 'You withdrawed ₱1,000.00 from your account.', 'Transaction', 1, '2026-05-22 01:45:26'),
(56, 12, 'Payment Successful', 'You payed ₱2,000.00 to Education.', 'Transaction', 1, '2026-05-22 01:45:43');

-- --------------------------------------------------------

--
-- Table structure for table `otp_codes`
--

CREATE TABLE `otp_codes` (
  `otp_id` int(11) NOT NULL,
  `email` varchar(100) NOT NULL,
  `otp_code` varchar(6) NOT NULL,
  `expires_at` datetime NOT NULL,
  `is_used` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `otp_codes`
--

INSERT INTO `otp_codes` (`otp_id`, `email`, `otp_code`, `expires_at`, `is_used`) VALUES
(1, 'thealtiam@gmail.com', '878608', '2026-05-22 00:02:06', 1);

-- --------------------------------------------------------

--
-- Table structure for table `roles`
--

CREATE TABLE `roles` (
  `role_id` int(11) NOT NULL,
  `role_name` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `roles`
--

INSERT INTO `roles` (`role_id`, `role_name`) VALUES
(1, 'Administrator'),
(2, 'User');

-- --------------------------------------------------------

--
-- Table structure for table `spending_limits`
--

CREATE TABLE `spending_limits` (
  `limit_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `category_id` int(11) NOT NULL,
  `monthly_limit` decimal(10,0) NOT NULL,
  `daily_limit` decimal(10,0) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `spending_limits`
--

INSERT INTO `spending_limits` (`limit_id`, `user_id`, `category_id`, `monthly_limit`, `daily_limit`) VALUES
(2, 12, 11, 10000, 0),
(3, 13, 1, 5000, 150),
(4, 13, 2, 1500, 50),
(5, 13, 12, 5000, 0),
(6, 12, 6, 12000, 0),
(7, 18, 1, 5000, 100),
(8, 19, 4, 10000, 0),
(10, 23, 3, 7500, 0),
(11, 23, 1, 5000, 150);

-- --------------------------------------------------------

--
-- Table structure for table `transactions`
--

CREATE TABLE `transactions` (
  `transaction_id` int(11) NOT NULL,
  `account_id` int(11) NOT NULL,
  `category_id` int(11) DEFAULT NULL,
  `related_account_id` int(11) DEFAULT NULL,
  `type` enum('Debit','Credit') NOT NULL,
  `amount` decimal(10,0) NOT NULL,
  `description` varchar(255) NOT NULL,
  `reference_number` varchar(255) NOT NULL,
  `date` datetime NOT NULL DEFAULT current_timestamp(),
  `is_flagged` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `transactions`
--

INSERT INTO `transactions` (`transaction_id`, `account_id`, `category_id`, `related_account_id`, `type`, `amount`, `description`, `reference_number`, `date`, `is_flagged`) VALUES
(1, 1, NULL, NULL, 'Credit', 1500, 'Deposit', 'TXN-20260520-958333', '2026-05-20 20:20:08', 0),
(2, 1, NULL, NULL, 'Credit', 3000, 'Deposit', 'TXN-20260520-465964', '2026-05-20 20:20:26', 0),
(3, 1, NULL, NULL, 'Debit', 500, 'Withdraw', 'TXN-20260520-257067', '2026-05-20 22:44:29', 0),
(4, 1, NULL, NULL, 'Credit', 500, 'Deposit', 'TXN-20260520-921475', '2026-05-20 22:44:38', 0),
(5, 1, NULL, 2, 'Debit', 500, 'Transfer', 'TXN-20260520-547387', '2026-05-20 23:14:11', 0),
(6, 2, NULL, 1, 'Debit', 3500, 'Transfer', 'TXN-20260520-916716', '2026-05-20 23:18:54', 0),
(7, 1, 1, NULL, 'Debit', 500, 'Jollibee', 'TXN-20260521-960973', '2026-05-21 00:25:49', 0),
(8, 1, 11, NULL, 'Debit', 1250, 'Change Oil', 'TXN-20260521-857323', '2026-05-21 00:28:08', 0),
(9, 1, 1, NULL, 'Debit', 2500, 'Test', 'TXN-20260521-701646', '2026-05-21 01:01:40', 0),
(10, 1, 1, NULL, 'Debit', 1500, 'Test', 'TXN-20260521-872444', '2026-05-21 01:03:10', 0),
(11, 1, 6, NULL, 'Debit', 1000, 'Test', 'TXN-20260521-864938', '2026-05-21 01:05:31', 0),
(12, 1, 1, NULL, 'Debit', 200, 'McDonalds', 'TXN-20260521-285592', '2026-05-21 01:06:25', 0),
(13, 1, 1, NULL, 'Debit', 1500, 'Saygex', 'TXN-20260521-698377', '2026-05-21 10:37:56', 0),
(14, 3, NULL, NULL, 'Credit', 2000, 'Deposit', 'TXN-20260521-523589', '2026-05-21 10:51:57', 0),
(15, 3, 1, NULL, 'Debit', 2500, 'McDonalds', 'TXN-20260521-992195', '2026-05-21 10:52:18', 0),
(16, 4, 4, NULL, 'Debit', 8000, 'Shopping May', 'TXN-20260521-377388', '2026-05-21 11:46:28', 0),
(17, 4, NULL, NULL, 'Credit', 12000, 'Deposit', 'TXN-20260521-848314', '2026-05-21 11:46:55', 0),
(18, 4, NULL, NULL, 'Debit', 5000, 'Withdraw', 'TXN-20260521-297309', '2026-05-21 11:47:34', 0),
(19, 1, NULL, NULL, 'Credit', 2000, 'Deposit', 'TXN-20260521-748646', '2026-05-21 12:00:56', 0),
(20, 1, NULL, NULL, 'Debit', 50, 'Withdraw', 'TXN-20260521-740680', '2026-05-21 12:01:09', 0),
(21, 1, NULL, 2, 'Debit', 1000, 'Transfer', 'TXN-20260521-188019', '2026-05-21 12:05:25', 0),
(22, 1, NULL, NULL, 'Credit', 20000, 'Deposit', 'TXN-20260521-863546', '2026-05-21 12:07:24', 0),
(23, 1, NULL, NULL, 'Credit', 1000, 'Deposit', 'TXN-20260521-465151', '2026-05-21 15:04:55', 0),
(24, 1, 6, NULL, 'Debit', 6000, 'Tuition', 'TXN-20260521-959740', '2026-05-21 15:06:00', 0),
(25, 1, 6, NULL, 'Debit', 1000, 'Books', 'TXN-20260521-674763', '2026-05-21 15:09:27', 0),
(26, 1, NULL, NULL, 'Credit', 2000, 'Deposit', 'TXN-20260521-130854', '2026-05-21 15:19:35', 0),
(27, 1, NULL, NULL, 'Credit', 1000, 'Deposit', 'TXN-20260521-994597', '2026-05-21 20:38:36', 0),
(28, 1, NULL, NULL, 'Credit', 1000, 'Deposit', 'TXN-20260521-381004', '2026-05-21 20:42:32', 0),
(29, 1, NULL, NULL, 'Debit', 2000, 'Withdraw', 'TXN-20260521-203431', '2026-05-21 20:43:04', 0),
(30, 1, NULL, NULL, 'Debit', 1000, 'Withdraw', 'TXN-20260521-864268', '2026-05-21 20:43:10', 0),
(31, 1, NULL, 3, 'Debit', 2000, 'Transfer', 'TXN-20260521-892014', '2026-05-21 20:44:25', 0),
(32, 1, NULL, 3, 'Debit', 100, 'Transfer', 'TXN-20260521-171010', '2026-05-21 20:44:31', 0),
(33, 1, NULL, NULL, 'Credit', 1000, 'Deposit', 'TXN-20260521-560801', '2026-05-21 20:49:49', 0),
(34, 1, NULL, NULL, 'Debit', 1000, 'Withdraw', 'TXN-20260521-999881', '2026-05-21 20:49:57', 0),
(35, 1, NULL, 3, 'Debit', 1000, 'Transfer', 'TXN-20260521-403170', '2026-05-21 20:51:29', 0),
(36, 1, 6, NULL, 'Debit', 1000, 'Project', 'TXN-20260521-395232', '2026-05-21 20:51:59', 0),
(37, 1, NULL, NULL, 'Credit', 1000, 'Deposit', 'TXN-20260521-729759', '2026-05-21 20:59:53', 0),
(38, 1, NULL, NULL, 'Credit', 1000, 'Deposit', 'TXN-20260521-107724', '2026-05-21 21:00:43', 0),
(39, 2, 1, NULL, 'Debit', 1000, '', 'TXN-20260522-178406', '2026-05-22 00:04:45', 0),
(40, 1, NULL, NULL, 'Credit', 1000, 'Deposit', 'TXN-20260522-463753', '2026-05-22 00:06:15', 0),
(41, 1, NULL, NULL, 'Debit', 1000, 'Withdraw', 'TXN-20260522-223915', '2026-05-22 00:06:28', 0),
(42, 1, NULL, 5, 'Debit', 2000, 'Transfer', 'TXN-20260522-635275', '2026-05-22 00:07:04', 0),
(43, 1, NULL, NULL, 'Credit', 100, 'Deposit', 'TXN-20260522-850424', '2026-05-22 00:17:45', 0),
(44, 1, NULL, NULL, 'Credit', 500, 'Deposit', 'TXN-20260522-719756', '2026-05-22 00:18:50', 0),
(45, 1, NULL, NULL, 'Debit', 1000, 'Withdraw', 'TXN-20260522-560264', '2026-05-22 00:19:55', 0),
(46, 1, NULL, NULL, 'Credit', 1000, 'Deposit', 'TXN-20260522-169091', '2026-05-22 00:20:07', 0),
(47, 1, 6, NULL, 'Debit', 200, 'School Supplies', 'TXN-20260522-374610', '2026-05-22 00:22:19', 0),
(48, 1, 11, NULL, 'Debit', 1000, 'Change Wheel Rims', 'TXN-20260522-766932', '2026-05-22 00:22:59', 0),
(49, 1, 11, NULL, 'Debit', 1000, 'test', 'TXN-20260522-959118', '2026-05-22 00:23:29', 0),
(50, 1, 11, NULL, 'Debit', 1231, 'asdfa', 'TXN-20260522-445541', '2026-05-22 00:23:34', 0),
(51, 1, NULL, NULL, 'Credit', 1, 'Deposit', 'TXN-20260522-881216', '2026-05-22 00:23:48', 0),
(52, 1, NULL, NULL, 'Credit', 1000, 'Deposit', 'TXN-20260522-105965', '2026-05-22 01:09:34', 0),
(53, 1, NULL, NULL, 'Credit', 1000, 'Deposit', 'TXN-20260522-946922', '2026-05-22 01:10:50', 0),
(54, 1, NULL, NULL, 'Credit', 2000, 'Deposit', 'TXN-20260522-194473', '2026-05-22 01:17:19', 0),
(55, 1, NULL, 5, 'Debit', 1000, 'Transfer', 'TXN-20260522-545408', '2026-05-22 01:25:32', 0),
(56, 1, NULL, 5, 'Debit', 2000, 'Transfer', 'TXN-20260522-273360', '2026-05-22 01:35:37', 0),
(57, 1, 6, NULL, 'Debit', 3000, 'Tuition Fees', 'TXN-20260522-779810', '2026-05-22 01:41:49', 0),
(58, 1, NULL, 5, 'Debit', 3000, 'Transfer', 'TXN-20260522-646284', '2026-05-22 01:44:53', 0),
(59, 1, NULL, NULL, 'Debit', 1000, 'Withdraw', 'TXN-20260522-830637', '2026-05-22 01:45:26', 0),
(60, 1, 6, NULL, 'Debit', 2000, 'Tuition Fees', 'TXN-20260522-705806', '2026-05-22 01:45:43', 0);

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `user_id` int(11) NOT NULL,
  `role_id` int(11) NOT NULL,
  `full_name` varchar(255) NOT NULL,
  `email` varchar(255) NOT NULL,
  `phone` varchar(20) NOT NULL,
  `status` set('Active','Inactive','Suspended') NOT NULL DEFAULT 'Active',
  `created_at` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`user_id`, `role_id`, `full_name`, `email`, `phone`, `status`, `created_at`) VALUES
(12, 2, 'Xian Reel Bag-o Lines', 'xianreel2006@gmail.com', '09541563069', 'Active', '2026-05-17 12:56:55'),
(13, 2, 'Julius Bannix Guerrerow', 'juliusow@gmail.com', '0912341234', 'Active', '2026-05-17 12:59:07'),
(15, 2, 'Antonio Bannix Lentejas', 'antonigger@gmail.com', '0912344321', 'Active', '2026-05-17 13:03:43'),
(18, 2, 'k dq s', 'd@we.mn', '09876543221', 'Active', '2026-05-21 10:49:40'),
(19, 2, 'Van Lander Negrow Quiapo', 'vanixno@gmail.com', '43214321', 'Active', '2026-05-21 11:44:19'),
(22, 1, 'Ad Minis Trator', 'AdMinisTrator@gmail.com', '09123432567', 'Active', '2026-05-21 12:26:18'),
(23, 2, 'John Maxim Amigable', 'thealtiam@gmail.com', '09123456789', 'Active', '2026-05-21 23:59:13');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `accounts`
--
ALTER TABLE `accounts`
  ADD PRIMARY KEY (`account_id`),
  ADD UNIQUE KEY `account_account_number` (`account_number`),
  ADD KEY `account_user_id` (`user_id`);

--
-- Indexes for table `alerts`
--
ALTER TABLE `alerts`
  ADD PRIMARY KEY (`alert_id`),
  ADD KEY `alert_category_id` (`category_id`),
  ADD KEY `alert_user_id` (`user_id`);

--
-- Indexes for table `expense_categories`
--
ALTER TABLE `expense_categories`
  ADD PRIMARY KEY (`category_id`),
  ADD KEY `category_user_id` (`user_id`);

--
-- Indexes for table `logins`
--
ALTER TABLE `logins`
  ADD PRIMARY KEY (`login_id`),
  ADD UNIQUE KEY `login_username` (`username`),
  ADD KEY `login_user_id` (`user_id`);

--
-- Indexes for table `notifications`
--
ALTER TABLE `notifications`
  ADD PRIMARY KEY (`notification_id`),
  ADD KEY `user_id` (`user_id`);

--
-- Indexes for table `otp_codes`
--
ALTER TABLE `otp_codes`
  ADD PRIMARY KEY (`otp_id`);

--
-- Indexes for table `roles`
--
ALTER TABLE `roles`
  ADD PRIMARY KEY (`role_id`);

--
-- Indexes for table `spending_limits`
--
ALTER TABLE `spending_limits`
  ADD PRIMARY KEY (`limit_id`),
  ADD KEY `limit_user_id` (`user_id`),
  ADD KEY `limit_category_id` (`category_id`);

--
-- Indexes for table `transactions`
--
ALTER TABLE `transactions`
  ADD PRIMARY KEY (`transaction_id`),
  ADD KEY `transaction_account_id` (`account_id`),
  ADD KEY `transaction_category_id` (`category_id`),
  ADD KEY `related_account_id` (`related_account_id`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`user_id`),
  ADD UNIQUE KEY `user_email` (`email`),
  ADD UNIQUE KEY `user_phone` (`phone`),
  ADD KEY `user_role_id` (`role_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `accounts`
--
ALTER TABLE `accounts`
  MODIFY `account_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `alerts`
--
ALTER TABLE `alerts`
  MODIFY `alert_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT for table `expense_categories`
--
ALTER TABLE `expense_categories`
  MODIFY `category_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT for table `logins`
--
ALTER TABLE `logins`
  MODIFY `login_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT for table `notifications`
--
ALTER TABLE `notifications`
  MODIFY `notification_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=57;

--
-- AUTO_INCREMENT for table `otp_codes`
--
ALTER TABLE `otp_codes`
  MODIFY `otp_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `roles`
--
ALTER TABLE `roles`
  MODIFY `role_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `spending_limits`
--
ALTER TABLE `spending_limits`
  MODIFY `limit_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT for table `transactions`
--
ALTER TABLE `transactions`
  MODIFY `transaction_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=61;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `user_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=24;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `accounts`
--
ALTER TABLE `accounts`
  ADD CONSTRAINT `account_user_id` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`);

--
-- Constraints for table `alerts`
--
ALTER TABLE `alerts`
  ADD CONSTRAINT `alert_category_id` FOREIGN KEY (`category_id`) REFERENCES `expense_categories` (`category_id`),
  ADD CONSTRAINT `alert_user_id` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`);

--
-- Constraints for table `expense_categories`
--
ALTER TABLE `expense_categories`
  ADD CONSTRAINT `category_user_id` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`);

--
-- Constraints for table `logins`
--
ALTER TABLE `logins`
  ADD CONSTRAINT `login_user_id` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`);

--
-- Constraints for table `notifications`
--
ALTER TABLE `notifications`
  ADD CONSTRAINT `notifications_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`);

--
-- Constraints for table `spending_limits`
--
ALTER TABLE `spending_limits`
  ADD CONSTRAINT `limit_category_id` FOREIGN KEY (`category_id`) REFERENCES `expense_categories` (`category_id`),
  ADD CONSTRAINT `limit_user_id` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`);

--
-- Constraints for table `transactions`
--
ALTER TABLE `transactions`
  ADD CONSTRAINT `transaction_account_id` FOREIGN KEY (`account_id`) REFERENCES `accounts` (`account_id`),
  ADD CONSTRAINT `transaction_category_id` FOREIGN KEY (`category_id`) REFERENCES `expense_categories` (`category_id`),
  ADD CONSTRAINT `transactions_ibfk_1` FOREIGN KEY (`related_account_id`) REFERENCES `accounts` (`account_id`);

--
-- Constraints for table `users`
--
ALTER TABLE `users`
  ADD CONSTRAINT `user_role_id` FOREIGN KEY (`role_id`) REFERENCES `roles` (`role_id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
