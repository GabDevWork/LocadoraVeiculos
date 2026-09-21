IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Categorias] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(50) NOT NULL,
    [Descricao] nvarchar(250) NULL,
    [ValorDiariaPadrao] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Categorias] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Clientes] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(150) NOT NULL,
    [CPF] nvarchar(14) NOT NULL,
    [Email] nvarchar(150) NOT NULL,
    [Telefone] nvarchar(20) NULL,
    [CNH] nvarchar(20) NULL,
    CONSTRAINT [PK_Clientes] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Fabricantes] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(100) NOT NULL,
    [PaisOrigem] nvarchar(50) NULL,
    CONSTRAINT [PK_Fabricantes] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Veiculos] (
    [Id] int NOT NULL IDENTITY,
    [Modelo] nvarchar(100) NOT NULL,
    [AnoFabricacao] int NOT NULL,
    [Quilometragem] int NOT NULL,
    [Placa] nvarchar(10) NOT NULL,
    [Cor] nvarchar(30) NOT NULL,
    [Status] nvarchar(20) NOT NULL DEFAULT N'Disponivel',
    [FabricanteId] int NOT NULL,
    [CategoriaId] int NOT NULL,
    CONSTRAINT [PK_Veiculos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Veiculos_Categorias_CategoriaId] FOREIGN KEY ([CategoriaId]) REFERENCES [Categorias] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Veiculos_Fabricantes_FabricanteId] FOREIGN KEY ([FabricanteId]) REFERENCES [Fabricantes] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Alugueis] (
    [Id] int NOT NULL IDENTITY,
    [ClienteId] int NOT NULL,
    [VeiculoId] int NOT NULL,
    [DataInicio] datetime2 NOT NULL,
    [DataPrevisaoDevolucao] datetime2 NOT NULL,
    [DataDevolucao] datetime2 NULL,
    [QuilometragemInicial] int NOT NULL,
    [QuilometragemFinal] int NULL,
    [ValorDiaria] decimal(18,2) NOT NULL,
    [ValorTotal] decimal(18,2) NULL,
    [Status] nvarchar(20) NOT NULL DEFAULT N'Ativo',
    CONSTRAINT [PK_Alugueis] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Alugueis_Clientes_ClienteId] FOREIGN KEY ([ClienteId]) REFERENCES [Clientes] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Alugueis_Veiculos_VeiculoId] FOREIGN KEY ([VeiculoId]) REFERENCES [Veiculos] ([Id]) ON DELETE NO ACTION
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Descricao', N'Nome', N'ValorDiariaPadrao') AND [object_id] = OBJECT_ID(N'[Categorias]'))
    SET IDENTITY_INSERT [Categorias] ON;
INSERT INTO [Categorias] ([Id], [Descricao], [Nome], [ValorDiariaPadrao])
VALUES (1, N'Veículos compactos ideais para cidade', N'Econômico / Hatch', 99.9),
(2, N'Conforto e amplo porta-malas para viagens', N'Sedan Médio', 149.9),
(3, N'Maior altura do solo, espaço e versatilidade', N'SUV', 219.9),
(4, N'Veículos premium com alta tecnologia', N'Executivo / Luxo', 399.9);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Descricao', N'Nome', N'ValorDiariaPadrao') AND [object_id] = OBJECT_ID(N'[Categorias]'))
    SET IDENTITY_INSERT [Categorias] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CNH', N'CPF', N'Email', N'Nome', N'Telefone') AND [object_id] = OBJECT_ID(N'[Clientes]'))
    SET IDENTITY_INSERT [Clientes] ON;
INSERT INTO [Clientes] ([Id], [CNH], [CPF], [Email], [Nome], [Telefone])
VALUES (1, N'01234567890', N'123.456.789-00', N'gabriel.souza@email.com', N'Gabriel Souza', N'(11) 98765-4321'),
(2, N'09876543210', N'987.654.321-99', N'mariana.oliveira@email.com', N'Mariana Oliveira', N'(11) 91234-5678');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CNH', N'CPF', N'Email', N'Nome', N'Telefone') AND [object_id] = OBJECT_ID(N'[Clientes]'))
    SET IDENTITY_INSERT [Clientes] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nome', N'PaisOrigem') AND [object_id] = OBJECT_ID(N'[Fabricantes]'))
    SET IDENTITY_INSERT [Fabricantes] ON;
INSERT INTO [Fabricantes] ([Id], [Nome], [PaisOrigem])
VALUES (1, N'Toyota', N'Japão'),
(2, N'Volkswagen', N'Alemanha'),
(3, N'Fiat', N'Itália'),
(4, N'Hyundai', N'Coreia do Sul');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nome', N'PaisOrigem') AND [object_id] = OBJECT_ID(N'[Fabricantes]'))
    SET IDENTITY_INSERT [Fabricantes] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AnoFabricacao', N'CategoriaId', N'Cor', N'FabricanteId', N'Modelo', N'Placa', N'Quilometragem', N'Status') AND [object_id] = OBJECT_ID(N'[Veiculos]'))
    SET IDENTITY_INSERT [Veiculos] ON;
INSERT INTO [Veiculos] ([Id], [AnoFabricacao], [CategoriaId], [Cor], [FabricanteId], [Modelo], [Placa], [Quilometragem], [Status])
VALUES (1, 2024, 1, N'Branco', 2, N'Polo Track 1.0', N'BRA2E19', 12500, N'Disponivel'),
(2, 2023, 2, N'Prata', 1, N'Corolla XEi 2.0', N'ABC1D23', 35000, N'Disponivel'),
(3, 2024, 3, N'Cinza', 4, N'Creta Ultimate 2.0', N'XYZ9W87', 8900, N'Disponivel');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AnoFabricacao', N'CategoriaId', N'Cor', N'FabricanteId', N'Modelo', N'Placa', N'Quilometragem', N'Status') AND [object_id] = OBJECT_ID(N'[Veiculos]'))
    SET IDENTITY_INSERT [Veiculos] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ClienteId', N'DataDevolucao', N'DataInicio', N'DataPrevisaoDevolucao', N'QuilometragemFinal', N'QuilometragemInicial', N'Status', N'ValorDiaria', N'ValorTotal', N'VeiculoId') AND [object_id] = OBJECT_ID(N'[Alugueis]'))
    SET IDENTITY_INSERT [Alugueis] ON;
INSERT INTO [Alugueis] ([Id], [ClienteId], [DataDevolucao], [DataInicio], [DataPrevisaoDevolucao], [QuilometragemFinal], [QuilometragemInicial], [Status], [ValorDiaria], [ValorTotal], [VeiculoId])
VALUES (1, 1, '2024-08-05T08:30:00.0000000', '2024-08-01T09:00:00.0000000', '2024-08-05T09:00:00.0000000', 12500, 12000, N'Finalizado', 99.9, 399.6, 1);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ClienteId', N'DataDevolucao', N'DataInicio', N'DataPrevisaoDevolucao', N'QuilometragemFinal', N'QuilometragemInicial', N'Status', N'ValorDiaria', N'ValorTotal', N'VeiculoId') AND [object_id] = OBJECT_ID(N'[Alugueis]'))
    SET IDENTITY_INSERT [Alugueis] OFF;
GO

CREATE INDEX [IX_Alugueis_ClienteId] ON [Alugueis] ([ClienteId]);
GO

CREATE INDEX [IX_Alugueis_VeiculoId] ON [Alugueis] ([VeiculoId]);
GO

CREATE UNIQUE INDEX [IX_Clientes_CNH] ON [Clientes] ([CNH]) WHERE [CNH] IS NOT NULL;
GO

CREATE UNIQUE INDEX [IX_Clientes_CPF] ON [Clientes] ([CPF]);
GO

CREATE INDEX [IX_Veiculos_CategoriaId] ON [Veiculos] ([CategoriaId]);
GO

CREATE INDEX [IX_Veiculos_FabricanteId] ON [Veiculos] ([FabricanteId]);
GO

CREATE UNIQUE INDEX [IX_Veiculos_Placa] ON [Veiculos] ([Placa]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260921015141_InitialCreate', N'8.0.13');
GO

COMMIT;
GO

