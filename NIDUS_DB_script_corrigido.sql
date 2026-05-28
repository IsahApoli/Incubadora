-- =====================================================
-- NIDUS - SCRIPT CORRIGIDO PARA O FRONTEND (NidusFront)
-- =====================================================
-- Rode este script no SSMS conectado em .\SQLEXPRESS
-- com Autenticacao do Windows
-- =====================================================

-- 1. CRIACAO DO BANCO DE DADOS
CREATE DATABASE NIDUS_DB;
GO

USE NIDUS_DB;
GO

-- 2. TABELA: USUARIOS
CREATE TABLE Usuarios (
    IdUsuario INT IDENTITY(1,1) NOT NULL,
    Nome VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Senha VARCHAR(255) NOT NULL,
    Celular VARCHAR(11) NOT NULL,
    Perfil VARCHAR(20) NOT NULL,       -- 'Admin' ou 'Cliente'
    Foto VARBINARY(MAX) NULL,          -- Foto do perfil
    CONSTRAINT PK_Usuarios PRIMARY KEY (IdUsuario),
    CONSTRAINT UQ_Usuarios_Email UNIQUE (Email)
);
GO

-- 3. TABELA: CLIENTES_DETALHES (heranca)
CREATE TABLE Clientes_Detalhes (
    IdUsuario INT NOT NULL,
    CnpjVinculado VARCHAR(14) NOT NULL,
    CONSTRAINT PK_Clientes_Detalhes PRIMARY KEY (IdUsuario),
    CONSTRAINT UQ_Clientes_Cnpj UNIQUE (CnpjVinculado),
    CONSTRAINT FK_Clientes_Usuarios FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario) ON DELETE CASCADE
);
GO

-- 4. TABELA: FAZENDAS
CREATE TABLE Fazendas (
    IdFazenda INT IDENTITY(1,1) NOT NULL,
    NomeFantasia VARCHAR(100) NOT NULL,
    Cnpj VARCHAR(14) NOT NULL,
    CONSTRAINT PK_Fazendas PRIMARY KEY (IdFazenda),
    CONSTRAINT UQ_Fazendas_Cnpj UNIQUE (Cnpj)
);
GO

-- 5. TABELA: ANIMAIS
CREATE TABLE Animais (
    IdAnimal INT IDENTITY(1,1) NOT NULL,
    NomeEspecie VARCHAR(50) NOT NULL,
    TempMin DECIMAL(4,1) NOT NULL,
    TempMax DECIMAL(4,1) NOT NULL,
    UmidMin INT NOT NULL,
    UmidMax INT NOT NULL,
    LuzMin INT NOT NULL,
    LuzMax INT NOT NULL,
    Tipo VARCHAR(20) NOT NULL,
    Foto VARBINARY(MAX) NULL,
    IdUsuario INT NULL,                -- Dono do animal (NULL = padrao do sistema)
    CONSTRAINT PK_Animais PRIMARY KEY (IdAnimal),
    CONSTRAINT FK_Animais_Usuarios FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario)
);
GO

-- 6. TABELA: INCUBADORAS
CREATE TABLE Incubadoras (
    IdIncubadora INT IDENTITY(1,1) NOT NULL,
    NomeIncubadora VARCHAR(50) NOT NULL,
    QuantidadeOvos INT NOT NULL,
    Status VARCHAR(20) NOT NULL,
    IdAnimal INT NOT NULL,
    IdUsuario INT NOT NULL,
    IdFazenda INT NULL,                -- Vinculo com a fazenda
    CONSTRAINT PK_Incubadoras PRIMARY KEY (IdIncubadora),
    CONSTRAINT CHK_QuantidadeOvos CHECK (QuantidadeOvos >= 0 AND QuantidadeOvos <= 5),
    CONSTRAINT CHK_StatusIncubadora CHECK (Status IN ('Ativa', 'Inativa')),
    CONSTRAINT FK_Incubadoras_Animais FOREIGN KEY (IdAnimal) REFERENCES Animais(IdAnimal),
    CONSTRAINT FK_Incubadoras_Usuarios FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario),
    CONSTRAINT FK_Incubadoras_Fazendas FOREIGN KEY (IdFazenda) REFERENCES Fazendas(IdFazenda)
);
GO

-- 7. TABELA: TELEMETRIA (Historico IoT)
CREATE TABLE Telemetria (
    IdTelemetria BIGINT IDENTITY(1,1) NOT NULL,
    IdIncubadora INT NOT NULL,
    DataHora DATETIME DEFAULT GETDATE() NOT NULL,
    TemperaturaAtual DECIMAL(4,1) NOT NULL,
    UmidadeAtual INT NOT NULL,
    StatusGeral VARCHAR(20) DEFAULT 'OK',
    CONSTRAINT PK_Telemetria PRIMARY KEY (IdTelemetria),
    CONSTRAINT FK_Telemetria_Incubadoras FOREIGN KEY (IdIncubadora) REFERENCES Incubadoras(IdIncubadora) ON DELETE CASCADE
);
GO

-- =====================================================
-- VIEW DE RESUMO (usado na listagem de incubadoras)
-- =====================================================
CREATE VIEW Vw_ResumoIncubadoras AS
SELECT
    I.IdIncubadora,
    I.NomeIncubadora,
    I.QuantidadeOvos,
    I.Status AS StatusIncubadora,
    A.NomeEspecie AS AnimalVinculado,
    A.TempMin AS MetaTempMin,
    A.TempMax AS MetaTempMax,
    U.Nome AS Responsavel
FROM Incubadoras I
INNER JOIN Animais A ON I.IdAnimal = A.IdAnimal
INNER JOIN Usuarios U ON I.IdUsuario = U.IdUsuario;
GO

-- =====================================================
-- CARGA INICIAL DE DADOS
-- =====================================================

-- Usuario Admin padrao
INSERT INTO Usuarios (Nome, Email, Senha, Celular, Perfil)
VALUES ('Administrador Nidus', 'admin@nidus.com', '123456', '11999999999', 'Admin');

-- Animais padrao
INSERT INTO Animais (NomeEspecie, TempMin, TempMax, UmidMin, UmidMax, LuzMin, LuzMax, Tipo, Foto, IdUsuario)
VALUES
('Galinha', 37.2, 37.8, 50, 60, 0, 10, 'Ave', NULL, NULL),
('Jacare', 30.0, 33.0, 85, 95, 20, 50, 'Reptil', NULL, NULL);

-- Fazenda de exemplo
INSERT INTO Fazendas (NomeFantasia, Cnpj)
VALUES ('Vale Verde', '12345678000100');
GO

PRINT '=============================================';
PRINT 'BANCO NIDUS_DB CRIADO COM SUCESSO!';
PRINT '';
PRINT 'Login: admin@nidus.com';
PRINT 'Senha: 123456';
PRINT '=============================================';
GO
