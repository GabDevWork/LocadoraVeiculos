using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LocadoraVeiculos.Models
{
    /// Representa um veículo disponível para locação.
    [Table("Veiculos")]
    public class Veiculo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O modelo do veículo é obrigatório.")]
        [StringLength(100, ErrorMessage = "O modelo deve ter no máximo 100 caracteres.")]
        public string Modelo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O ano de fabricação é obrigatório.")]
        [Range(1950, 2100, ErrorMessage = "Ano de fabricação inválido.")]
        public int AnoFabricacao { get; set; }

        [Required(ErrorMessage = "A quilometragem é obrigatória.")]
        [Range(0, 2000000, ErrorMessage = "A quilometragem não pode ser negativa.")]
        public int Quilometragem { get; set; }

        [Required(ErrorMessage = "A placa é obrigatória.")]
        [StringLength(10, ErrorMessage = "A placa deve ter no máximo 10 caracteres.")]
        public string Placa { get; set; } = string.Empty;

        [StringLength(30, ErrorMessage = "A cor deve ter no máximo 30 caracteres.")]
        public string Cor { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "O status deve ter no máximo 20 caracteres.")]
        public string Status { get; set; } = "Disponivel";

        // Chave estrangeira para Fabricante (1 Fabricante -> N Veiculos)
        [Required(ErrorMessage = "O fabricante é obrigatório.")]
        public int FabricanteId { get; set; }

        [ForeignKey(nameof(FabricanteId))]
        public virtual Fabricante? Fabricante { get; set; }

        // Chave estrangeira para Categoria (1 Categoria -> N Veiculos)
        [Required(ErrorMessage = "A categoria é obrigatória.")]
        public int CategoriaId { get; set; }

        [ForeignKey(nameof(CategoriaId))]
        public virtual Categoria? Categoria { get; set; }

        // Propriedade de navegação: 1 Veículo pode estar associado a N Aluguéis
        [JsonIgnore]
        public virtual ICollection<Aluguel> Alugueis { get; set; } = new List<Aluguel>();
    }
}
