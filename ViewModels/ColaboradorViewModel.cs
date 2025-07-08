using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using crud_maui.DataAcess;
using crud_maui.Models;
using Microsoft.EntityFrameworkCore;

namespace crud_maui.ViewModels
{
    public partial class ColaboradorViewModel : ObservableObject, IQueryAttributable
    {
        private readonly ColaboradorDbContext _dbContext;

        [ObservableProperty]
        private Colaborador colaborador = new();

        [ObservableProperty]
        private string tituloPagina;

        [ObservableProperty]
        private bool loadingVisivel = false;

        private int IdColaborador;

        public ColaboradorViewModel(ColaboradorDbContext context)
        {
            _dbContext = context;
            Colaborador.DataContratacao = DateTime.Now;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            IdColaborador = int.Parse(query["id"].ToString());

            if (IdColaborador == 0)
            {
                TituloPagina = "Adicionar Colaborador";
            }
            else
            {
                TituloPagina = "Editar Colaborador";
                LoadingVisivel = true;

                var encontrado = await _dbContext.Colaboradores.FirstAsync(e => e.IdColaborador == IdColaborador);
                Colaborador = encontrado;

                LoadingVisivel = false;
            }
        }

        [RelayCommand]
        private async Task Salvar()
        {
            LoadingVisivel = true;

            if (IdColaborador == 0)
            {
                _dbContext.Colaboradores.Add(Colaborador);
            }
            else
            {
                var encontrado = await _dbContext.Colaboradores.FirstAsync(e => e.IdColaborador == IdColaborador);

                encontrado.NomeCompleto = Colaborador.NomeCompleto;
                encontrado.Email = Colaborador.Email;
                encontrado.Salario = Colaborador.Salario;
                encontrado.DataContratacao = Colaborador.DataContratacao;
            }

            await _dbContext.SaveChangesAsync();
            LoadingVisivel = false;
            await Shell.Current.GoToAsync("..");
        }
    }
}
