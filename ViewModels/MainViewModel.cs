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
        private readonly EmpregadoDbContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<Empregado> listaEmpregado = new();

        public MainViewModel(EmpregadoDbContext context)
        {
            _dbContext = context;
        }

        public async Task Obter()
        {
            var lista = await _dbContext.Empregados.ToListAsync();
            ListaEmpregado.Clear();
            foreach (var item in lista)
                ListaEmpregado.Add(item);
        }

        [RelayCommand]
        private async Task Criar()
        {
            await Shell.Current.GoToAsync($"{nameof(ColaboradorPage)}?id=0");
        }

        [RelayCommand]
        private async Task Editar(Empregado empregado)
        {
            await Shell.Current.GoToAsync($"{nameof(ColaboradorPage)}?id={empregado.IdEmpregado}");
        }

        [RelayCommand]
        private async Task Deletar(Empregado empregado)
        {
            bool confirmar = await Shell.Current.DisplayAlert("Confirmação", "Deseja realmente excluir?", "Sim", "Não");
            if (!confirmar) return;

            var encontrado = await _dbContext.Empregados.FirstAsync(e => e.IdEmpregado == empregado.IdEmpregado);
            _dbContext.Empregados.Remove(encontrado);
            await _dbContext.SaveChangesAsync();
            ListaEmpregado.Remove(empregado);
        }
    }

}
