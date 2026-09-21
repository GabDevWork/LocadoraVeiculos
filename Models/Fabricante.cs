using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LocadoraVeiculos.Models
{
    /// Representa a marca/fabricante de um veículo (ex: Toyota, Ford, Fiat, Volkswagen).
    [Table("Fabricantes")]
    public class Fabricante
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do fabricante é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "O país de origem deve ter no máximo 50 caracteres.")]
        public string? PaisOrigem { get; set; }

        // Propriedade de navegação: 1 Fabricante possui N Veículos
        [JsonIgnore]
        public virtual ICollection<Veiculo> Veiculos { get; set; } = new List<Veiculo>();
    }
}
