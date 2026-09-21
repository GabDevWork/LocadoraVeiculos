# Sistema de Locadora de Veículos - TP1 (Etapa 1)

Projeto desenvolvido para a disciplina de Backend e Banco de Dados (TADS).

## Escopo da Entrega: Etapa 1 - Modelagem do Banco de Dados
- Modelagem de entidades na camada `Model` (Veículo, Fabricante, Cliente, Aluguel, Categoria).
- Definição explícita de chaves primárias.
- Definição explícita de chaves estrangeiras e integridade referencial.
- Configuração da classe `ApplicationContext` para mapeamento no SQL Server via Entity Framework Core.
- Mínimo de 5 entidades modeladas.

*(Nota: Os controladores REST e filtros avançados pertencem à **Etapa 2**, e a integração/testes com Swagger pertencem à **Etapa 3**).*

## Estrutura do Projeto
- `Models/`: Classes de entidades do domínio (`Fabricante.cs`, `Categoria.cs`, `Veiculo.cs`, `Cliente.cs`, `Aluguel.cs`).
- `Data/`: Classe `ApplicationContext.cs` (DbSets, Fluent API, PKs, FKs, índices e seed data).
- `Migrations/`: Migração inicial do Entity Framework Core (`InitialCreate`).
- `docs/`: Documentação formal (`MODELO_CONCEITUAL_E_LOGICO.md`) e script DDL gerado (`Script_Criacao_Banco.sql`).
- `_backup_etapas_futuras/`: Códigos de suporte preparados para as próximas etapas (Controllers para a Etapa 2).

## Como Executar

### 1. Compilar o Projeto
```powershell
dotnet build
```

### 2. Aplicar as Migrações no Banco de Dados (SQL Server Express)
```powershell
dotnet ef database update
```
*(Ou execute o script `docs/Script_Criacao_Banco.sql` no SQL Server Management Studio).*

### 3. Executar o Projeto
```powershell
dotnet run
```
Acesse `http://localhost:5221/status-modelagem` para visualizar a confirmação do mapeamento das entidades do `ApplicationContext`.
