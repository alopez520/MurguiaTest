-- Crear la base de datos
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'TodoDB')
BEGIN
    CREATE DATABASE TodoDB;
END
GO

-- Esperar a que la base de datos esté lista (opcional, más confiable)
-- Cambiar al contexto de la base de datos
USE TodoDB;
GO

-- Crear tabla
CREATE TABLE dbo.Tareas (
	Id int IDENTITY(1,1) NOT NULL,
	Titulo nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Descripcion nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Completada bit NOT NULL,
	Important bit NOT NULL,
	FechaCreacion datetime2 NOT NULL,
	CONSTRAINT PK_Tareas PRIMARY KEY (Id)
);
GO

-- Insertar una fila de ejemplo
INSERT INTO dbo.Tareas
(Titulo, Descripcion, Completada, Important, FechaCreacion)
VALUES('Tarea ejemplo', 'Esta es una tarea de ejemplo', 0, 1, CURRENT_TIMESTAMP);
GO