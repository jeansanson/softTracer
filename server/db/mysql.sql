CREATE TABLE `users` (
  `username` varchar(130) NOT NULL,
  `displayName` varchar(255) DEFAULT NULL,
  `password` varchar(255) DEFAULT NULL,
  `email` varchar(255) DEFAULT NULL,
  `token` varchar(45) NOT NULL,
  PRIMARY KEY (`username`,`token`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `projects_users` (
  `projectId` int NOT NULL,
  `username` varchar(130) NOT NULL,
  `role` int DEFAULT NULL,
  PRIMARY KEY (`projectId`,`username`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `projects` (
  `projectId` int NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `resume` varchar(4092) DEFAULT NULL,
  `openingDate` datetime DEFAULT NULL,
  `token` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`projectId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


CREATE TABLE `requirements` (
  `projectId` INT NOT NULL,
  `requirementId` INT NOT NULL,
  `name` VARCHAR(45) NULL,
  `description` VARCHAR(45) NULL,
  `completed` BIT(1) NULL,
  `parentId` INT NULL,
  PRIMARY KEY (`projectId`, `requirementId`));
