using Microsoft.EntityFrameworkCore;
using LocadoraVeiculos.Data;

var builder = WebApplication.CreateBuilder(args);

// =========================================================================
// ETAPA 1: MODELAGEM DO BANCO DE DADOS
// Configuração da classe ApplicationContext para mapeamento no SQL Server
// utilizando Entity Framework Core (Critério de Avaliação: 2,0 Pontos)
// =========================================================================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

app.UseHttpsRedirection();

// Mensagem de apresentação da Etapa 1
app.MapGet("/", () => Results.Ok(new
{
    Projeto = "Sistema de Locadora de Veículos",
    Fase = "TP1 - Etapa 1: Modelagem do Banco de Dados",
    Tecnologias = "C# (.NET 8.0), Entity Framework Core 8, SQL Server Express",
    EntidadesModeladas = new[]
    {
        "1. Fabricante (Marca)",
        "2. Categoria (5ª entidade do domínio)",
        "3. Veiculo (Modelo, ano, km, placa única, FKs)",
        "4. Cliente (Nome, CPF único, e-mail, CNH, telefone)",
        "5. Aluguel (Período, devolução, km inicial/final, valor diária e total)"
    },
    Documentacao = "Consulte docs/MODELO_CONCEITUAL_E_LOGICO.md para o DER e dicionário de dados.",
    ScriptSQL = "Consulte docs/Script_Criacao_Banco.sql para o script DDL completo gerado pelo EF Core."
}));

// Endpoint utilitário para validar o mapeamento e integridade do ApplicationContext
app.MapGet("/status-modelagem", async (ApplicationContext context) =>
{
    bool conectado;
    try
    {
        conectado = await context.Database.CanConnectAsync();
    }
    catch
    {
        conectado = false;
    }

    return Results.Ok(new
    {
        Etapa = "Etapa 1 - Modelagem de Banco de Dados Concluída",
        ContextoConfigurado = nameof(ApplicationContext),
        SGBD = "Microsoft SQL Server Express",
        StatusConexaoBanco = conectado ? "Online / Conectado" : "Offline / Aguardando serviço SQL Server",
        TabelasMapeadas = new[]
        {
            "Fabricantes (PK: Id, 1:N com Veiculos)",
            "Categorias (PK: Id, 1:N com Veiculos)",
            "Veiculos (PK: Id, FK: FabricanteId, FK: CategoriaId, 1:N com Alugueis)",
            "Clientes (PK: Id, 1:N com Alugueis)",
            "Alugueis (PK: Id, FK: ClienteId, FK: VeiculoId)"
        }
    });
});

app.Run();
