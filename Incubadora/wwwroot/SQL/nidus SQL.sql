CREATE DATABASE NIDUS


--Nidus
CREATE TABLE Empresas (
    id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    nomeEmpresa VARCHAR(150) NOT NULL,
    cnpj VARCHAR(18) NOT NULL,
    email VARCHAR(100),
    telefone VARCHAR(20)
)

-- 2. Tabela de Fazendas (Clientes)
CREATE TABLE Fazenda (
    fazendaId INT IDENTITY(1,1) PRIMARY KEY,
    nomeFazenda VARCHAR(150) NOT NULL,
    cep VARCHAR(9),
    ruaFazenda VARCHAR(200),
    bairroFazenda VARCHAR(100),
    cidadeFazenda VARCHAR(100),
    estadoFazenda VARCHAR(2)
)


-- 3. Tabela de Usuários (Controle de Acesso)
CREATE TABLE Usuarios (
    id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    nome VARCHAR(150) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    senha VARCHAR(255) NOT NULL, -- Armazenar como Hash
    tipoUsuario VARCHAR(30) NOT NULL, -- ADMIN_EMPRESA, ADMIN_FAZENDA, FUNCIONARIO
    fazendaId INT NULL, -- Nulo para Admin Empresa
    FOREIGN KEY (fazendaId) REFERENCES Fazendas(id)
)

-- 4. Tabela de Incubadoras (Hardware)
CREATE TABLE Incubadoras (
    id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    nomeIncubadora VARCHAR(100) NOT NULL,
    modelo VARCHAR(50),
    localizacao VARCHAR(150),
    status VARCHAR(30), -- ATIVA, INATIVA, MANUTENCAO
    capacidadeMaxima INT,
    dataInstalacao DATETIME,
    fazendaId INT NOT NULL,
    FOREIGN KEY (fazendaId) REFERENCES Fazendas(id)
)

-- 5. Tabela de Espécies (Parâmetros de Controle e Imagem)
CREATE TABLE Especies (
    id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    nomeEspecie VARCHAR(100) NOT NULL,
    temperaturaMin DECIMAL(4,2),
    temperaturaMax DECIMAL(4,2),
    umidadeMin DECIMAL(4,2),
    umidadeMax DECIMAL(4,2),
    luminosidadeIdeal DECIMAL(6,2),
    tempoIncubacao INT, -- Em dias
    imagemAnimal VARBINARY(MAX) -- Requisito de manipulação de imagem
)

-- 6. Tabela de Lotes (Produção)
CREATE TABLE Lotes (
    id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    codigoLote VARCHAR(50) NOT NULL,
    quantidadeOvos INT,
    dataEntrada DATETIME DEFAULT GETDATE(),
    statusLote VARCHAR(30), -- INCUBANDO, NASCIDO, DESCARTADO
    especieId INT NOT NULL,
    incubadoraId INT NOT NULL,
    FOREIGN KEY (especieId) REFERENCES Especies(id),
    FOREIGN KEY (incubadoraId) REFERENCES Incubadoras(id)
)
GO

-- Procedimento Genérico de Exclusão

create procedure spDelete 
( 
  @id int  , 
  @tabela varchar(max) 
) 
as 
begin 
   declare @sql varchar(max); 
   set @sql = ' delete ' + @tabela +  
       ' where id = ' + cast(@id as varchar(max)) 
   exec(@sql) 
end 
 
GO 
 
-- Procedimento Genérico de Consulta por ID
create procedure spConsulta 
( 
  @id int  , 
  @tabela varchar(max) 
) 
as 
begin 
   declare @sql varchar(max); 
   set @sql = 'select * from  ' + @tabela +  
        ' where id = ' + cast(@id as varchar(max)) 
   exec(@sql) 
end 
GO 

-- Procedimento Genérico de Listagem
create procedure spListagem 
( 
   @tabela varchar(max), 
   @ordem varchar(max)) 
as 
begin 
   exec('select * from ' + @tabela +  
        ' order by ' + @ordem) 
end 
GO 

---Procedimento Genérico de Próximo ID
create  procedure spProximoId 
 (@tabela  varchar(max)) 
as 
begin 
     exec('select isnull(max(id) +1, 1) as MAIOR from '  
          +@tabela) 
end 
GO

---

--------------------------------------------
--SP ESPECIE
------------------------------------------------
CREATE PROCEDURE spInsert_Especies (
    @nomeEspecie VARCHAR(100),
    @tempMin DECIMAL(4,2),
    @tempMax DECIMAL(4,2),
    @umidMin DECIMAL(4,2),
    @umidMax DECIMAL(4,2),
    @luzIdeal DECIMAL(6,2),
    @tempo INT,
    @imagem VARBINARY(MAX)
) AS 
BEGIN
    INSERT INTO Especies (nomeEspecie, temperaturaMin, temperaturaMax, umidadeMin, umidadeMax, luminosidadeIdeal, tempoIncubacao, imagemAnimal)
    VALUES (@nomeEspecie, @tempMin, @tempMax, @umidMin, @umidMax, @luzIdeal, @tempo, @imagem)
END;
GO

CREATE PROCEDURE spUpdate_Especies (
    @id INT,
    @nomeEspecie VARCHAR(100),
    @tempMin DECIMAL(4,2),
    @tempMax DECIMAL(4,2),
    @umidMin DECIMAL(4,2),
    @umidMax DECIMAL(4,2),
    @luzIdeal DECIMAL(6,2),
    @tempo INT,
    @imagem VARBINARY(MAX)
) AS 
BEGIN
    UPDATE Especies SET 
        nomeEspecie = @nomeEspecie,
        temperaturaMin = @tempMin,
        temperaturaMax = @tempMax,
        umidadeMin = @umidMin,
        umidadeMax = @umidMax,
        luminosidadeIdeal = @luzIdeal,
        tempoIncubacao = @tempo,
        imagemAnimal = @imagem
    WHERE id = @id
END;
GO
-------------------------------------
---spListagemFazendas
------------------------------------------

---------------------------------------------
--SP EMPRESAS
------------------------------------------------
CREATE PROCEDURE spInsert_Empresas (
    @nomeEmpresa VARCHAR(150),
    @cnpj VARCHAR(18),
    @email VARCHAR(100),
    @telefone VARCHAR(20)
) AS 
BEGIN
    INSERT INTO Empresas (nomeEmpresa, cnpj, email, telefone)
    VALUES (@nomeEmpresa, @cnpj, @email, @telefone)
END;
GO

CREATE PROCEDURE spUpdate_Empresas (
    @id INT,
    @nomeEmpresa VARCHAR(150),
    @cnpj VARCHAR(18),
    @email VARCHAR(100),
    @telefone VARCHAR(20)
) AS 
BEGIN
    UPDATE Empresas SET 
        nomeEmpresa = @nomeEmpresa,
        cnpj = @cnpj,
        email = @email,
        telefone = @telefone
    WHERE id = @id
END;
GO


---------------------
CREATE PROCEDURE spListagemFazendas AS
BEGIN
    SELECT F.*, E.nomeEmpresa AS NomeEmpresaJoin
    FROM Fazendas F
    INNER JOIN Empresas E ON F.empresaId = E.id
    ORDER BY F.nomeFazenda
END;
GO


CREATE PROCEDURE spInsert_Fazenda
    @nomeFazenda VARCHAR(150),
    @cep VARCHAR(9),
    @ruaFazenda VARCHAR(200),
    @bairroFazenda VARCHAR(100),
    @cidadeFazenda VARCHAR(100),
    @estadoFazenda VARCHAR(2)
AS
BEGIN
    INSERT INTO Fazenda
    (
        nomeFazenda,
        cep,
        ruaFazenda,
        bairroFazenda,
        cidadeFazenda,
        estadoFazenda
    )
    VALUES
    (
        @nomeFazenda,
        @cep,
        @ruaFazenda,
        @bairroFazenda,
        @cidadeFazenda,
        @estadoFazenda
    );
END;
GO



CREATE PROCEDURE spUpdate_Fazenda
    @fazendaId INT,
    @nomeFazenda VARCHAR(150),
    @cep VARCHAR(9),
    @ruaFazenda VARCHAR(200),
    @bairroFazenda VARCHAR(100),
    @cidadeFazenda VARCHAR(100),
    @estadoFazenda VARCHAR(2)
AS
BEGIN
    UPDATE Fazenda
    SET
        nomeFazenda = @nomeFazenda,
        cep = @cep,
        ruaFazenda = @ruaFazenda,
        bairroFazenda = @bairroFazenda,
        cidadeFazenda = @cidadeFazenda,
        estadoFazenda = @estadoFazenda
    WHERE fazendaId = @fazendaId;
END;
GO


-----------------------------------------
CREATE PROCEDURE spInsert_Usuarios (
    @nome VARCHAR(150),
    @email VARCHAR(100),
    @senha VARCHAR(255),
    @tipoUsuario VARCHAR(30),
    @fazendaId INT
) AS 
BEGIN
    INSERT INTO Usuarios (nome, email, senha, tipoUsuario, fazendaId)
    VALUES (@nome, @email, @senha, @tipoUsuario, @fazendaId)
END;
GO

CREATE PROCEDURE spUpdate_Usuarios (
    @id INT,
    @nome VARCHAR(150),
    @email VARCHAR(100),
    @senha VARCHAR(255),
    @tipoUsuario VARCHAR(30),
    @fazendaId INT
) AS 
BEGIN
    UPDATE Usuarios SET 
        nome = @nome,
        email = @email,
        senha = @senha,
        tipoUsuario = @tipoUsuario,
        fazendaId = @fazendaId
    WHERE id = @id
END;
GO
----------------------------------------------------------------

CREATE PROCEDURE spInsert_Incubadoras (
    @nomeIncubadora VARCHAR(100),
    @modelo VARCHAR(50),
    @localizacao VARCHAR(150),
    @status VARCHAR(30),
    @capacidadeMaxima INT,
    @dataInstalacao DATETIME,
    @fazendaId INT
) AS 
BEGIN
    INSERT INTO Incubadoras (nomeIncubadora, modelo, localizacao, status, capacidadeMaxima, dataInstalacao, fazendaId)
    VALUES (@nomeIncubadora, @modelo, @localizacao, @status, @capacidadeMaxima, @dataInstalacao, @fazendaId)
END;
GO

CREATE PROCEDURE spUpdate_Incubadoras (
    @id INT,
    @nomeIncubadora VARCHAR(100),
    @modelo VARCHAR(50),
    @localizacao VARCHAR(150),
    @status VARCHAR(30),
    @capacidadeMaxima INT,
    @dataInstalacao DATETIME,
    @fazendaId INT
) AS 
BEGIN
    UPDATE Incubadoras SET 
        nomeIncubadora = @nomeIncubadora,
        modelo = @modelo,
        localizacao = @localizacao,
        status = @status,
        capacidadeMaxima = @capacidadeMaxima,
        dataInstalacao = @dataInstalacao,
        fazendaId = @fazendaId
    WHERE id = @id
END;
GO

--------------------------------------------------
CREATE PROCEDURE spInsert_Lotes (
    @codigoLote VARCHAR(50),
    @quantidadeOvos INT,
    @dataEntrada DATETIME,
    @statusLote VARCHAR(30),
    @especieId INT,
    @incubadoraId INT
) AS 
BEGIN
    INSERT INTO Lotes (codigoLote, quantidadeOvos, dataEntrada, statusLote, especieId, incubadoraId)
    VALUES (@codigoLote, @quantidadeOvos, @dataEntrada, @statusLote, @especieId, @incubadoraId)
END;
GO

CREATE PROCEDURE spUpdate_Lotes (
    @id INT,
    @codigoLote VARCHAR(50),
    @quantidadeOvos INT,
    @dataEntrada DATETIME,
    @statusLote VARCHAR(30),
    @especieId INT,
    @incubadoraId INT
) AS 
BEGIN
    UPDATE Lotes SET 
        codigoLote = @codigoLote,
        quantidadeOvos = @quantidadeOvos,
        dataEntrada = @dataEntrada,
        statusLote = @statusLote,
        especieId = @especieId,
        incubadoraId = @incubadoraId
    WHERE id = @id
END;
GO