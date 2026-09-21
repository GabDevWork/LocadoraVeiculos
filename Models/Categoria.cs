using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LocadoraVeiculos.Models
{
    /// Representa o grupo/categoria de um veículo (ex: Econômico, Sedan Médio, SUV, Luxo).
    [Table("Categorias")]
    public class Categoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
        [StringLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "A descrição deve ter no máximo 250 caracteres.")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O valor padrão da diária é obrigatório.")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 100000.00, ErrorMessage = "O valor da diária padrão deve ser maior que zero.")]
        public decimal ValorDiariaPadrao { get; set; }

        // Propriedade de navegação: 1 Categoria possui N Veículos
        [JsonIgnore]
        public virtual ICollection<Veiculo> Veiculos { get; set; } = new List<Veiculo>();
    }
}
