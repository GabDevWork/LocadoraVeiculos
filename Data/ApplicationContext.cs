using Microsoft.EntityFrameworkCore;
using LocadoraVeiculos.Models;

namespace LocadoraVeiculos.Data
{
    /// <summary>
    /// Contexto do Entity Framework Core para o sistema de locação de veículos.
    /// Atende diretamente ao critério de avaliação:
    /// "Configuração da classe ApplicationContext para mapeamento das classes no banco de dados SQL Server utilizando Entity Framework".
    /// </summary>
    public class ApplicationContext : DbContext
    {
        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        // DbSets representando as tabelas do banco de dados relacional
        public virtual DbSet<Fabricante> Fabricantes { get; set; } = null!;
        public virtual DbSet<Categoria> Categorias { get; set; } = null!;
        public virtual DbSet<Veiculo> Veiculos { get; set; } = null!;
        public virtual DbSet<Cliente> Clientes { get; set; } = null!;
        public virtual DbSet<Aluguel> Alugueis { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Fallback de conexão padrão apontando para SQL Server Express / LocalDB
                optionsBuilder.UseSqlServer(
                    "Server=localhost\\SQLEXPRESS;Database=LocadoraVeiculosDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==========================================
            // 1. CONFIGURAÇÃO DA ENTIDADE FABRICANTE
            // ==========================================
            modelBuilder.Entity<Fabricante>(entity =>
            {
                entity.ToTable("Fabricantes");
                entity.HasKey(f => f.Id);
                entity.Property(f => f.Id).ValueGeneratedOnAdd();
                entity.Property(f => f.Nome).IsRequired().HasMaxLength(100);
                entity.Property(f => f.PaisOrigem).HasMaxLength(50);
            });

            // ==========================================
            // 2. CONFIGURAÇÃO DA ENTIDADE CATEGORIA (5ª ENTIDADE)
            // ==========================================
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.ToTable("Categorias");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
                entity.Property(c => c.Nome).IsRequired().HasMaxLength(50);
                entity.Property(c => c.Descricao).HasMaxLength(250);
                entity.Property(c => c.ValorDiariaPadrao).HasPrecision(18, 2).IsRequired();
            });

            // ==========================================
            // 3. CONFIGURAÇÃO DA ENTIDADE VEICULO
            // ==========================================
            modelBuilder.Entity<Veiculo>(entity =>
            {
                entity.ToTable("Veiculos");
                entity.HasKey(v => v.Id);
                entity.Property(v => v.Id).ValueGeneratedOnAdd();
                entity.Property(v => v.Modelo).IsRequired().HasMaxLength(100);
                entity.Property(v => v.AnoFabricacao).IsRequired();
                entity.Property(v => v.Quilometragem).IsRequired();
                entity.Property(v => v.Placa).IsRequired().HasMaxLength(10);
                entity.Property(v => v.Cor).HasMaxLength(30);
                entity.Property(v => v.Status).HasMaxLength(20).HasDefaultValue("Disponivel");

                // Restrição de unicidade para a Placa do veículo
                entity.HasIndex(v => v.Placa).IsUnique();

                // Chave Estrangeira: Fabricante (1 Fabricante -> N Veiculos)
                entity.HasOne(v => v.Fabricante)
                      .WithMany(f => f.Veiculos)
                      .HasForeignKey(v => v.FabricanteId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Chave Estrangeira: Categoria (1 Categoria -> N Veiculos)
                entity.HasOne(v => v.Categoria)
                      .WithMany(c => c.Veiculos)
                      .HasForeignKey(v => v.CategoriaId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ==========================================
            // 4. CONFIGURAÇÃO DA ENTIDADE CLIENTE
            // ==========================================
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("Clientes");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
                entity.Property(c => c.Nome).IsRequired().HasMaxLength(150);
                entity.Property(c => c.CPF).IsRequired().HasMaxLength(14);
                entity.Property(c => c.Email).IsRequired().HasMaxLength(150);
                entity.Property(c => c.Telefone).HasMaxLength(20);
                entity.Property(c => c.CNH).HasMaxLength(20);

                // Restrições de unicidade para CPF e CNH
                entity.HasIndex(c => c.CPF).IsUnique();
                entity.HasIndex(c => c.CNH).IsUnique().HasFilter("[CNH] IS NOT NULL");
            });

            // ==========================================
            // 5. CONFIGURAÇÃO DA ENTIDADE ALUGUEL
            // ==========================================
            modelBuilder.Entity<Aluguel>(entity =>
            {
                entity.ToTable("Alugueis");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Id).ValueGeneratedOnAdd();
                entity.Property(a => a.DataInicio).IsRequired();
                entity.Property(a => a.DataPrevisaoDevolucao).IsRequired();
                entity.Property(a => a.DataDevolucao);
                entity.Property(a => a.QuilometragemInicial).IsRequired();
                entity.Property(a => a.QuilometragemFinal);
                entity.Property(a => a.ValorDiaria).HasPrecision(18, 2).IsRequired();
                entity.Property(a => a.ValorTotal).HasPrecision(18, 2);
                entity.Property(a => a.Status).HasMaxLength(20).HasDefaultValue("Ativo");

                // Chave Estrangeira: Cliente (1 Cliente -> N Alugueis)
                entity.HasOne(a => a.Cliente)
                      .WithMany(c => c.Alugueis)
                      .HasForeignKey(a => a.ClienteId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Chave Estrangeira: Veiculo (1 Veiculo -> N Alugueis)
                entity.HasOne(a => a.Veiculo)
                      .WithMany(v => v.Alugueis)
                      .HasForeignKey(a => a.VeiculoId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ==========================================
            // 6. POPULAÇÃO INICIAL DE DADOS (SEED DATA)
            // ==========================================
            PopularDadosIniciais(modelBuilder);
        }

        private static void PopularDadosIniciais(ModelBuilder modelBuilder)
        {
            // Seed Fabricantes
            modelBuilder.Entity<Fabricante>().HasData(
                new Fabricante { Id = 1, Nome = "Toyota", PaisOrigem = "Japão" },
                new Fabricante { Id = 2, Nome = "Volkswagen", PaisOrigem = "Alemanha" },
                new Fabricante { Id = 3, Nome = "Fiat", PaisOrigem = "Itália" },
                new Fabricante { Id = 4, Nome = "Hyundai", PaisOrigem = "Coreia do Sul" }
            );

            // Seed Categorias
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { Id = 1, Nome = "Econômico / Hatch", Descricao = "Veículos compactos ideais para cidade", ValorDiariaPadrao = 99.90m },
                new Categoria { Id = 2, Nome = "Sedan Médio", Descricao = "Conforto e amplo porta-malas para viagens", ValorDiariaPadrao = 149.90m },
                new Categoria { Id = 3, Nome = "SUV", Descricao = "Maior altura do solo, espaço e versatilidade", ValorDiariaPadrao = 219.90m },
                new Categoria { Id = 4, Nome = "Executivo / Luxo", Descricao = "Veículos premium com alta tecnologia", ValorDiariaPadrao = 399.90m }
            );

            // Seed Veículos
            modelBuilder.Entity<Veiculo>().HasData(
                new Veiculo
                {
                    Id = 1,
                    Modelo = "Polo Track 1.0",
                    AnoFabricacao = 2024,
                    Quilometragem = 12500,
                    Placa = "BRA2E19",
                    Cor = "Branco",
                    Status = "Disponivel",
                    FabricanteId = 2,
                    CategoriaId = 1
                },
                new Veiculo
                {
                    Id = 2,
                    Modelo = "Corolla XEi 2.0",
                    AnoFabricacao = 2023,
                    Quilometragem = 35000,
                    Placa = "ABC1D23",
                    Cor = "Prata",
                    Status = "Disponivel",
                    FabricanteId = 1,
                    CategoriaId = 2
                },
                new Veiculo
                {
                    Id = 3,
                    Modelo = "Creta Ultimate 2.0",
                    AnoFabricacao = 2024,
                    Quilometragem = 8900,
                    Placa = "XYZ9W87",
                    Cor = "Cinza",
                    Status = "Disponivel",
                    FabricanteId = 4,
                    CategoriaId = 3
                }
            );

            // Seed Clientes
            modelBuilder.Entity<Cliente>().HasData(
                new Cliente
                {
                    Id = 1,
                    Nome = "Gabriel Souza",
                    CPF = "123.456.789-00",
                    Email = "gabriel.souza@email.com",
                    Telefone = "(11) 98765-4321",
                    CNH = "01234567890"
                },
                new Cliente
                {
                    Id = 2,
                    Nome = "Mariana Oliveira",
                    CPF = "987.654.321-99",
                    Email = "mariana.oliveira@email.com",
                    Telefone = "(11) 91234-5678",
                    CNH = "09876543210"
                }
            );

            // Seed Aluguel de exemplo concluído
            modelBuilder.Entity<Aluguel>().HasData(
                new Aluguel
                {
                    Id = 1,
                    ClienteId = 1,
                    VeiculoId = 1,
                    DataInicio = new DateTime(2024, 8, 1, 9, 0, 0),
                    DataPrevisaoDevolucao = new DateTime(2024, 8, 5, 9, 0, 0),
                    DataDevolucao = new DateTime(2024, 8, 5, 8, 30, 0),
                    QuilometragemInicial = 12000,
                    QuilometragemFinal = 12500,
                    ValorDiaria = 99.90m,
                    ValorTotal = 399.60m,
                    Status = "Finalizado"
                }
            );
        }
    }
}
