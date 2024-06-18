--CREATE DATABASE Examen1
CREATE TABLE Usuarios(
IdUsuario VARCHAR(20) PRIMARY KEY,
Contraseña VARCHAR(8),
Cedula VARCHAR(20),
Nombre VARCHAR(50),
--Son los puntos totales que tiene el usuario.
PuntosTot INT 
)


--Catálogo de lugares para canjear puntos (futuro)
CREATE TABLE ComerciosCanje(
IdComercio INT IDENTITY PRIMARY KEY,
Nombre VARCHAR(50)
)


--Catálogo de materiales
CREATE TABLE Materiales(
IdMaterial INT IDENTITY PRIMARY KEY,
Descripcion VARCHAR(50),
PuntosKilo INT
)


CREATE TABLE Registros(
IdRegistro INT IDENTITY (1,1) PRIMARY KEY,
IdUsuario VARCHAR(20),
IdMaterial INT,
CantidadKg  DECIMAL(18,3),
Fecha DATETIME,
--Estos puntos son los que se dan por este registro.
Puntos INT
)

ALTER TABLE Reciclaje ADD CONSTRAINT FK_Reciclaje_Usuarios 
FOREIGN KEY (IdUsuario) 
REFERENCES Usuarios(IdUsuario)

ALTER TABLE Reciclaje ADD CONSTRAINT FK_Reciclaje_Materiales 
FOREIGN KEY (IdMaterial) 
REFERENCES Materiales(IdMaterial)


