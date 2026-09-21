using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocadoraVeiculos.Models
{
    /// Representa a locação de um veículo por um cliente em um período de tempo.
    [Table("Alugueis")]
    public class Aluguel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Chave estrangeira para Cliente
        [Required(ErrorMessage = "O cliente é obrigatório.")]
        public int ClienteId { get; set; }

        [ForeignKey(nameof(ClienteId))]
        public virtual Cliente? Cliente { get; set; }

        // Chave estrangeira para Veículo
        [Required(ErrorMessage = "O veículo é obrigatório.")]
        public int VeiculoId { get; set; }

        [ForeignKey(nameof(VeiculoId))]
        public virtual Veiculo? Veiculo { get; set; }

        [Required(ErrorMessage = "A data de início da locação é obrigatória.")]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "A previsão de devolução é obrigatória.")]
        public DateTime DataPrevisaoDevolucao { get; set; }

        // Data efetiva da devolução (preenchida quando o veículo é entregue)
        public DateTime? DataDevolucao { get; set; }

        [Required(ErrorMessage = "A quilometragem inicial é obrigatória.")]
        [Range(0, 2000000, ErrorMessage = "A quilometragem inicial não pode ser negativa.")]
        public int QuilometragemInicial { get; set; }

        // Quilometragem final (registrada na devolução)
        [Range(0, 2000000, ErrorMessage = "A quilometragem final não pode ser negativa.")]
        public int? QuilometragemFinal { get; set; }

        [Required(ErrorMessage = "O valor da diária é obrigatório.")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 100000.00, ErrorMessage = "O valor da diária deve ser positivo.")]
        public decimal ValorDiaria { get; set; }

        // Valor total da locação (calculado ou consolidado na devolução)
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ValorTotal { get; set; }

        [StringLength(20, ErrorMessage = "O status deve ter no máximo 20 caracteres.")]
        public string Status { get; set; } = "Ativo";
    }
}
