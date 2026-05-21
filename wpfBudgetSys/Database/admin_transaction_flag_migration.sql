-- Adds suspicious-transaction flag for admin oversight.
ALTER TABLE `transactions`
  ADD COLUMN IF NOT EXISTS `is_flagged` tinyint(1) NOT NULL DEFAULT 0 AFTER `date`;
