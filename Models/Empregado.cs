using System.ComponentModel.DataAnnotations;

namespace crud_maui.Models
{
    public class Empregado
    {
        [Key]
        public int IdEmpregado { get; set; }
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public decimal Salario { get; set; }
        public DateTime DataContratacao { get; set; }
    }
}
