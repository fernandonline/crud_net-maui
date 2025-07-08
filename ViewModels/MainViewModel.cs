using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using crud_maui.DataAcess;
using crud_maui.Models;
using crud_maui.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace crud_maui.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ColaboradorDbContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<Colaborador> listaColaborador = new();

        public MainViewModel(ColaboradorDbContext context)
        {
            _dbContext = context;
        }

        public async Task Obter()
        {
            var lista = await _dbContext.Colaboradores.AsNoTracking().ToListAsync();
            ListaColaborador.Clear();
            foreach (var item in lista)
                ListaColaborador.Add(item);
        }

        [RelayCommand]
        private async Task Criar()
        {
            await Shell.Current.GoToAsync($"{nameof(ColaboradorPage)}?id=0");
        }

        [RelayCommand]
        private async Task Editar(Colaborador colaborador)
        {
            await Shell.Current.GoToAsync($"{nameof(ColaboradorPage)}?id={colaborador.IdColaborador}");
        }

        [RelayCommand]
        private async Task Deletar(Colaborador colaborador)
        {
            bool confirmar = await Shell.Current.DisplayAlert("Confirmação", "Deseja realmente excluir?", "Sim", "Não");
            if (!confirmar) return;

            var encontrado = await _dbContext.Colaboradores.FirstAsync(e => e.IdColaborador == colaborador.IdColaborador);
            _dbContext.Colaboradores.Remove(encontrado);
            await _dbContext.SaveChangesAsync();
            ListaColaborador.Remove(colaborador);
        }
    }
}
