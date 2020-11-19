-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema ancla
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema ancla
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `ancla` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci ;
USE `ancla` ;

-- -----------------------------------------------------
-- Table `ancla`.`productos`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `ancla`.`productos` (
  `ID` INT NOT NULL AUTO_INCREMENT,
  `NOMBRE` VARCHAR(20) NOT NULL,
  `PRECIO` DECIMAL(10,2) NOT NULL,
  `DESCONTINUADO` TINYINT(1) NOT NULL,
  PRIMARY KEY (`ID`))
ENGINE = InnoDB
AUTO_INCREMENT = 4
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `ancla`.`inventario`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `ancla`.`inventario` (
  `ID` INT NOT NULL AUTO_INCREMENT,
  `NOMBRE` VARCHAR(20) NOT NULL,
  `UNIDAD` VARCHAR(12) NOT NULL,
  `STOCK` INT NOT NULL,
  `DESCONTINUADO` TINYINT(1) NOT NULL,
  PRIMARY KEY (`ID`))
ENGINE = InnoDB
AUTO_INCREMENT = 6
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `ancla`.`detalle_productos`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `ancla`.`detalle_productos` (
  `ID_PROD` INT NOT NULL,
  `ID_INV` INT NOT NULL,
  `CANTIDAD` INT NOT NULL,
  PRIMARY KEY (`ID_PROD`, `ID_INV`),
  INDEX `ID_INV` (`ID_INV` ASC) VISIBLE,
  CONSTRAINT `detalle_productos_ibfk_1`
    FOREIGN KEY (`ID_PROD`)
    REFERENCES `ancla`.`productos` (`ID`),
  CONSTRAINT `detalle_productos_ibfk_2`
    FOREIGN KEY (`ID_INV`)
    REFERENCES `ancla`.`inventario` (`ID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `ancla`.`usuarios`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `ancla`.`usuarios` (
  `ID` INT NOT NULL AUTO_INCREMENT,
  `NOMBRE` VARCHAR(25) NOT NULL,
  `NOMBRE_USUARIO` VARCHAR(15) NOT NULL,
  `CONTRASENA` VARCHAR(15) NOT NULL,
  `ES_ADMIN` TINYINT(1) NOT NULL,
  `ACTIVO` TINYINT(1) NOT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE INDEX `NOMBRE_USUARIO` (`NOMBRE_USUARIO` ASC) VISIBLE)
ENGINE = InnoDB
AUTO_INCREMENT = 8
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `ancla`.`ventas`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `ancla`.`ventas` (
  `ID` INT NOT NULL AUTO_INCREMENT,
  `TOTAL` DECIMAL(10,2) NOT NULL,
  `FECHA` DATETIME NOT NULL,
  `ID_CAJERO` INT NOT NULL,
  PRIMARY KEY (`ID`),
  INDEX `ID_CAJERO` (`ID_CAJERO` ASC) VISIBLE,
  CONSTRAINT `ventas_ibfk_1`
    FOREIGN KEY (`ID_CAJERO`)
    REFERENCES `ancla`.`usuarios` (`ID`))
ENGINE = InnoDB
AUTO_INCREMENT = 41
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `ancla`.`detalle_ventas`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `ancla`.`detalle_ventas` (
  `ID_PROD` INT NOT NULL,
  `ID_VENT` INT NOT NULL,
  `CANTIDAD` INT NOT NULL,
  `PRECIO_VENTA` DECIMAL(10,2) NOT NULL,
  PRIMARY KEY (`ID_PROD`, `ID_VENT`),
  INDEX `ID_VENT` (`ID_VENT` ASC) VISIBLE,
  CONSTRAINT `detalle_ventas_ibfk_1`
    FOREIGN KEY (`ID_PROD`)
    REFERENCES `ancla`.`productos` (`ID`),
  CONSTRAINT `detalle_ventas_ibfk_2`
    FOREIGN KEY (`ID_VENT`)
    REFERENCES `ancla`.`ventas` (`ID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `ancla`.`proveedores`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `ancla`.`proveedores` (
  `ID` INT NOT NULL AUTO_INCREMENT,
  `NOMBRE_EMPRESA` VARCHAR(35) NOT NULL,
  `NOMBRE_CONTACTO` VARCHAR(35) NOT NULL,
  `TELEFONO` VARCHAR(15) NOT NULL,
  `CORREO` VARCHAR(30) NOT NULL,
  `DIRECCION` VARCHAR(40) NOT NULL,
  PRIMARY KEY (`ID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `ancla`.`reportes_caja`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `ancla`.`reportes_caja` (
  `ID_REPORTE` INT NOT NULL AUTO_INCREMENT,
  `ID_USUARIO` INT NOT NULL,
  `FECHA_APERTURA` DATETIME NOT NULL,
  `FECHA_CIERRE` DATETIME NOT NULL,
  `IMPORTE_APERTURA` DECIMAL(10,2) NOT NULL,
  `IMPORTE_CIERRE` DECIMAL(10,2) NOT NULL,
  `TOTAL_VENTAS` DECIMAL(10,2) NOT NULL,
  `FALTANTE` DECIMAL(10,2) NOT NULL,
  PRIMARY KEY (`ID_REPORTE`),
  INDEX `ID_USUARIO` (`ID_USUARIO` ASC) VISIBLE,
  CONSTRAINT `reportes_caja_ibfk_1`
    FOREIGN KEY (`ID_USUARIO`)
    REFERENCES `ancla`.`usuarios` (`ID`))
ENGINE = InnoDB
AUTO_INCREMENT = 15
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
