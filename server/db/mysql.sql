CREATE TABLE `users` (
  `username` varchar(130) NOT NULL,
  `displayName` varchar(255) DEFAULT NULL,
  `password` varchar(255) DEFAULT NULL,
  `email` varchar(255) DEFAULT NULL,
  `token` varchar(45) NOT NULL,
  PRIMARY KEY (`username`,`token`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `softracer`.`projects_users` (
  `projectId` INT NOT NULL,
  `username` INT NOT NULL,
  PRIMARY KEY (`projectId`, `username`));

CREATE TABLE `softracer`.`projects` (
  `projectId` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(255) NULL,
  `resume` VARCHAR(4092) NULL,
  `openingDate` DATE NULL,
  PRIMARY KEY (`projectId`));

