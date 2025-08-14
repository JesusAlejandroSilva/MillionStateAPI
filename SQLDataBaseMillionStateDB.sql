/* =========================================================
   RealEstate Technical Test – SQL Server Schema (v2)
   Model aligned to: Owner, Property, PropertyImage, PropertyTrace
   .NET 5+ / EF Core friendly (snake-free names, PK int identity)
   ========================================================= */

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

/* ---- Create DB (optional) ---- */
IF DB_ID('MillionStateDB') IS NULL
BEGIN
    CREATE DATABASE MillionStateDB;
END
GO
USE MillionStateDB;
GO

/* ---- Safety drops for re-runs ---- */
IF OBJECT_ID('dbo.PropertyImage','U') IS NOT NULL DROP TABLE dbo.PropertyImage;
IF OBJECT_ID('dbo.PropertyTrace','U') IS NOT NULL DROP TABLE dbo.PropertyTrace;
IF OBJECT_ID('dbo.Property','U')      IS NOT NULL DROP TABLE dbo.Property;
IF OBJECT_ID('dbo.Owner','U')         IS NOT NULL DROP TABLE dbo.Owner;
GO

/* =========================================================
   OWNER
   ---------------------------------------------------------
   IdOwner (PK), Name, Address, Photo, Birthday
   ========================================================= */
CREATE TABLE dbo.Owner
(
    IdOwner     INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Owner PRIMARY KEY,
    Name        NVARCHAR(150)     NOT NULL,
    Address     NVARCHAR(250)     NULL,
    Photo       NVARCHAR(500)     NULL, -- URL o ruta
    Birthday    DATE              NULL,
    CreatedAt   DATETIME2         NOT NULL CONSTRAINT DF_Owner_CreatedAt DEFAULT (SYSDATETIME()),
    UpdatedAt   DATETIME2         NULL
);
GO

/* =========================================================
   PROPERTY
   ---------------------------------------------------------
   IdProperty (PK), Name, Address, Price, CodeInternal, Year, IdOwner (FK)
   + Goodies: IsActive, timestamps, constraints, indices
   ========================================================= */
CREATE TABLE dbo.Property
(
    IdProperty   INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Property PRIMARY KEY,
    Name         NVARCHAR(200)     NOT NULL,
    Address      NVARCHAR(250)     NOT NULL,
    Price        DECIMAL(18,2)     NOT NULL CONSTRAINT CK_Property_Price_Positive CHECK (Price > 0),
    CodeInternal NVARCHAR(30)      NOT NULL,
    Year         SMALLINT          NULL CONSTRAINT CK_Property_Year CHECK (Year BETWEEN 1800 AND 2100),
    IdOwner      INT               NOT NULL,
    IsActive     BIT               NOT NULL CONSTRAINT DF_Property_IsActive DEFAULT(1),
    CreatedAt    DATETIME2         NOT NULL CONSTRAINT DF_Property_CreatedAt DEFAULT (SYSDATETIME()),
    UpdatedAt    DATETIME2         NULL,
    CONSTRAINT FK_Property_Owner FOREIGN KEY (IdOwner)
        REFERENCES dbo.Owner(IdOwner)
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);
GO

/* Uniqueness for business key */
CREATE UNIQUE INDEX UX_Property_CodeInternal ON dbo.Property(CodeInternal);

/* Useful filters */
CREATE INDEX IX_Property_Owner   ON dbo.Property(IdOwner);
CREATE INDEX IX_Property_Price   ON dbo.Property(Price);
CREATE INDEX IX_Property_Active  ON dbo.Property(IsActive) WHERE IsActive = 1;
GO

/* =========================================================
   PROPERTYIMAGE
   ---------------------------------------------------------
   IdPropertyImage (PK), IdProperty (FK), File, Enabled
   ========================================================= */
CREATE TABLE dbo.PropertyImage
(
    IdPropertyImage INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_PropertyImage PRIMARY KEY,
    IdProperty      INT               NOT NULL,
    [File]          NVARCHAR(500)     NOT NULL, -- nombre de archivo o URL
    Enabled         BIT               NOT NULL CONSTRAINT DF_PropertyImage_Enabled DEFAULT (1),
    CreatedAt       DATETIME2         NOT NULL CONSTRAINT DF_PropertyImage_CreatedAt DEFAULT (SYSDATETIME()),
    CONSTRAINT FK_PropertyImage_Property FOREIGN KEY (IdProperty)
        REFERENCES dbo.Property(IdProperty)
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);
GO

CREATE INDEX IX_PropertyImage_Property ON dbo.PropertyImage(IdProperty);
CREATE INDEX IX_PropertyImage_Enabled  ON dbo.PropertyImage(Enabled) INCLUDE ([File]);
GO

/* =========================================================
   PROPERTYTRACE
   ---------------------------------------------------------
   IdPropertyTrace (PK), DateSale, Name, Value, Tax, IdProperty (FK)
   ========================================================= */
CREATE TABLE dbo.PropertyTrace
(
    IdPropertyTrace INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_PropertyTrace PRIMARY KEY,
    DateSale        DATE              NOT NULL,
    Name            NVARCHAR(200)     NOT NULL, -- Ej: comprador o evento
    Value           DECIMAL(18,2)     NOT NULL CONSTRAINT CK_PropertyTrace_Value_Pos CHECK (Value > 0),
    Tax             DECIMAL(18,2)     NOT NULL CONSTRAINT CK_PropertyTrace_Tax_NonNeg CHECK (Tax >= 0),
    IdProperty      INT               NOT NULL,
    CreatedAt       DATETIME2         NOT NULL CONSTRAINT DF_PropertyTrace_CreatedAt DEFAULT (SYSDATETIME()),
    CONSTRAINT FK_PropertyTrace_Property FOREIGN KEY (IdProperty)
        REFERENCES dbo.Property(IdProperty)
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);
GO

CREATE INDEX IX_PropertyTrace_Property        ON dbo.PropertyTrace(IdProperty);
CREATE INDEX IX_PropertyTrace_PropertyDate    ON dbo.PropertyTrace(IdProperty, DateSale);
GO

/* =========================================================
   Seed mínimo (opcional para probar rápido)
   ========================================================= */
INSERT INTO dbo.Owner (Name, Address, Photo, Birthday)
VALUES
('John Smith', '742 Evergreen Ter, Springfield', NULL, '1979-05-21'),
('Mary Johnson', '12 Ocean Dr, Miami Beach',    NULL, '1985-08-09');

INSERT INTO dbo.Property (Name, Address, Price, CodeInternal, Year, IdOwner)
VALUES
('Modern Condo', '123 Ocean Dr, Miami FL 33139', 450000.00, 'ML-1001', 2016, 2),
('Townhouse',    '789 Congress Ave, Austin TX',  650000.00, 'ML-1002', 2012, 1);

INSERT INTO dbo.PropertyImage (IdProperty, [File], Enabled)
VALUES
(1, 'https://cdn.example.com/p/1/main.jpg', 1),
(2, 'https://cdn.example.com/p/2/main.jpg', 1);

INSERT INTO dbo.PropertyTrace (DateSale, Name, Value, Tax, IdProperty)
VALUES
('2023-07-15', 'Initial Listing', 450000.00, 27000.00, 1),
('2024-03-10', 'Price Adjustment', 430000.00, 25800.00, 1),
('2022-11-20', 'Initial Listing', 650000.00, 39000.00, 2);
GO
