using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using crud_maui.DataAcess;
using crud_maui.Models;
using Microsoft.EntityFrameworkCore;

namespace crud_maui.ViewModels
{
    public partial class EmpregadoViewModel : ObservableObject, IQueryAttributable
    {
        private readonly EmpregadoDbContext _dbContext;

        [ObservableProperty]
        private Empregado empregado = new();

        [ObservableProperty]
        private string tituloPagina;

        [ObservableProperty]
        private bool loadingVisivel = false;

        private int IdEmpregado;

        public EmpregadoViewModel(EmpregadoDbContext context)
        {
            _dbContext = context;
            Empregado.DataContratacao = DateTime.Now;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            IdEmpregado = int.Parse(query["id"].ToString());

            if (IdEmpregado == 0)
            {
                TituloPagina = "Novo Empregado";
            }
            else
            {
                TituloPagina = "Editar Empregado";
                LoadingVisivel = true;

                var encontrado = await _dbContext.Empregados.FirstAsync(e => e.IdEmpregado == IdEmpregado);
                Empregado = encontrado;

                LoadingVisivel = false;
            }
        }

        [RelayCommand]
        private async Task Salvar()
        {
            LoadingVisivel = true;

            if (IdEmpregado == 0)
            {
                _dbContext.Empregados.Add(Empregado);
            }
            else
            {
                var encontrado = await _dbContext.Empregados.FirstAsync(e => e.IdEmpregado == IdEmpregado);

                encontrado.NomeCompleto = Empregado.NomeCompleto;
                encontrado.Email = Empregado.Email;
                encontrado.Salario = Empregado.Salario;
                encontrado.DataContratacao = Empregado.DataContratacao;
            }

            await _dbContext.SaveChangesAsync();

            LoadingVisivel = false;
            await Shell.Current.Navigation.PopAsync();
        }
    }

}
