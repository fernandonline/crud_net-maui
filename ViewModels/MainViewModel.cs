using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using crud_maui.Views;
using crud_maui.Models;
using crud_maui.Ultilidade;
using crud_maui.DTOs;
using crud_maui.DataAcess;
using System.Collections.ObjectModel;

namespace crud_maui.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly EmpregadoDbContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<EmpregadoDTO> listaEmpregado = new ObservableCollection<EmpregadoDTO>();
    
        public MainViewModel(EmpregadoDbContext context)
        {
            _dbContext = context;

            MainThread.BeginInvokeOnMainThread(new Action(async () => await Obter()));

            WeakReferenceMessenger.Default.Register<EmpregadoMensageria>(this, (r, m) => EmpregadoMensagemRecebida(m.Value));
        }

        public async Task Obter()
        {
            var lista = await _dbContext.Empregados.ToListAsync();
            if(lista.Count != 0)
            {
                ListaEmpregado.Clear();
                foreach (var item in lista)
                {
                    ListaEmpregado.Add(new EmpregadoDTO
                    {
                        IdEmpregado = item.IdEmpregado,
                        NomeCompleto = item.NomeCompleto,
                        Email = item.Email,
                        Salario = item.Salario,
                        DataContratacao = item.DataContratacao
                    });
                }
            }
        }

        private void EmpregadoMensagemRecebida(EmpregadoMensagem empregadoMensagem)
        {
            var empregadoDto = empregadoMensagem.EmpregadoDto;
            if( empregadoMensagem.Criando)
            {
                ListaEmpregado.Add(empregadoDto);
            }
            else
            {
                var encontrado = ListaEmpregado
                    .First(e => e.IdEmpregado == empregadoDto.IdEmpregado);

                encontrado.NomeCompleto = empregadoDto.NomeCompleto;
                encontrado.Email = empregadoDto.Email;
                encontrado.Salario = empregadoDto.Salario;
                encontrado.DataContratacao = empregadoDto.DataContratacao;
            }
        }

        [RelayCommand]
        private async Task Criar()
        {
            var uri = $"{nameof(ColaboradorPage)}?id=0";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task Editar(EmpregadoDTO empregadoDto)
        {
            var uri = $"{nameof(ColaboradorPage)}?id={empregadoDto.IdEmpregado}";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task Deletar(EmpregadoDTO empregadoDto)
        {
            bool confirmacao = await Shell.Current.DisplayAlert("Confirmação", "Deseja realmente excluir?", "Sim", "Não");
            if (confirmacao)
            {
                var encontrado = await _dbContext.Empregados.FirstAsync(e => e.IdEmpregado == empregadoDto.IdEmpregado);
                _dbContext.Empregados.Remove(encontrado);
                await _dbContext.SaveChangesAsync();
                ListaEmpregado.Remove(empregadoDto);

            }
        }

    }
}
