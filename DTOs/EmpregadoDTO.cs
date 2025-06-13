using CommunityToolkit.Mvvm.ComponentModel;

namespace crud_maui.DTOs
{
    public partial class EmpregadoDTO : ObservableObject
    {
        [ObservableProperty]
        public int idEmpregado;
        [ObservableProperty]
        public string nomeCompleto;
        [ObservableProperty]
        public string email;
        [ObservableProperty]
        public decimal salario;
        [ObservableProperty]
        public DateTime dataContratacao;
    }
}
