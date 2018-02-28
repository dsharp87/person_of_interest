-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema person_of_interest
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `person_of_interest` ;

-- -----------------------------------------------------
-- Schema person_of_interest
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `person_of_interest` DEFAULT CHARACTER SET utf8 ;
USE `person_of_interest` ;

-- -----------------------------------------------------
-- Table `person_of_interest`.`users`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `person_of_interest`.`users` (
  `UserID` INT NOT NULL AUTO_INCREMENT,
  `FirstName` VARCHAR(255) NULL,
  `LastName` VARCHAR(255) NULL,
  `Email` VARCHAR(255) NULL,
  `Password` VARCHAR(255) NULL,
  `Salt` VARCHAR(255) NULL,
  `ConnectionID` VARCHAR(255) NULL,
  `CreatedAt` DATETIME NULL DEFAULT now(),
  `UpdatedAt` DATETIME NULL DEFAULT now() on update now(),
  PRIMARY KEY (`UserID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `person_of_interest`.`quizes`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `person_of_interest`.`quizes` (
  `QuizID` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(255) NULL,
  `CreatedAt` DATETIME NULL DEFAULT now(),
  `UpdatedAt` DATETIME NULL DEFAULT now() on update now(),
  PRIMARY KEY (`QuizID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `person_of_interest`.`questions`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `person_of_interest`.`questions` (
  `QuestionID` INT NOT NULL AUTO_INCREMENT,
  `QuestionString` VARCHAR(255) NULL,
  `AnswerA` VARCHAR(255) NULL,
  `AnswerB` VARCHAR(255) NULL,
  `AnswerC` VARCHAR(255) NULL,
  `AnswerD` VARCHAR(255) NULL,
  `CreatedAt` DATETIME NULL DEFAULT now(),
  `UpdatedAt` DATETIME NULL DEFAULT now() on update now(),
  `QuizID` INT NOT NULL,
  PRIMARY KEY (`QuestionID`),
  INDEX `fk_quiz1_quizes1_idx` (`QuizID` ASC),
  CONSTRAINT `fk_quiz1_quizes1`
    FOREIGN KEY (`QuizID`)
    REFERENCES `person_of_interest`.`quizes` (`QuizID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `person_of_interest`.`quiz_results`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `person_of_interest`.`quiz_results` (
  `QuizResultID` INT NOT NULL AUTO_INCREMENT,
  `ResultString` VARCHAR(255) NULL,
  `CreatedAt` DATETIME NULL DEFAULT now(),
  `UpdatedAt` DATETIME NULL DEFAULT now() on update now(),
  `QuizID` INT NOT NULL,
  `UserID` INT NOT NULL,
  PRIMARY KEY (`QuizResultID`),
  INDEX `fk_quiz_results_quizes1_idx` (`QuizID` ASC),
  INDEX `fk_quiz_results_users1_idx` (`UserID` ASC),
  CONSTRAINT `fk_quiz_results_quizes1`
    FOREIGN KEY (`QuizID`)
    REFERENCES `person_of_interest`.`quizes` (`QuizID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_quiz_results_users1`
    FOREIGN KEY (`UserID`)
    REFERENCES `person_of_interest`.`users` (`UserID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
