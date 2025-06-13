using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using crud_maui.Models;
using crud_maui.Ultilidade;
using crud_maui.DTOs;
using crud_maui.DataAcess;

namespace crud_maui.ViewModels
{
    public partial class EmpregadoViewModel : ObservableObject, IQueryAttributable
    {
        private readonly EmpregadoDbContext _dbContext;

        [ObservableProperty]
        private EmpregadoDTO empregadoDto = new EmpregadoDTO();

        [ObservableProperty]
        private string tituloPagina;
        
        private int IdEmpregado;

        [ObservableProperty]
        private bool loadingVisivel = false;

        public EmpregadoViewModel(EmpregadoDbContext context)
        {
            _dbContext = context;
            EmpregadoDto.DataContratacao = DateTime.Now;

        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var id = int.Parse(query["id"].ToString());
            IdEmpregado = id;

            if (IdEmpregado == 0)
            {
                TituloPagina = "Novo Empregado";
            }
            else
            {
                TituloPagina = "Editar Empregado";
                loadingVisivel = true;

                await Task.Run(async () =>
                {
                    var encontrado = await _dbContext.Empregados.FirstAsync(e => e.IdEmpregado == IdEmpregado);
                    EmpregadoDto.IdEmpregado = encontrado.IdEmpregado;
                    EmpregadoDto.NomeCompleto = encontrado.NomeCompleto;
                    EmpregadoDto.Email = encontrado.Email;
                    EmpregadoDto.Salario = encontrado.Salario;
                    EmpregadoDto.DataContratacao = encontrado.DataContratacao;

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        loadingVisivel = false;
                    });
                });
            }
        }

        [RelayCommand]
        private async Task Salvar()
        {
            LoadingVisivel = true;
            EmpregadoMensagem mensagem = new EmpregadoMensagem();

            await Task.Run(async () =>
            {
                if (IdEmpregado == 0)
                {
                    var novoEmpregado = new Empregado
                    {
                        NomeCompleto = EmpregadoDto.NomeCompleto,
                        Email = EmpregadoDto.Email,
                        Salario = EmpregadoDto.Salario,
                        DataContratacao = EmpregadoDto.DataContratacao
                    };

                    _dbContext.Empregados.Add(novoEmpregado);
                    await _dbContext.SaveChangesAsync();
                    empregadoDto.IdEmpregado = novoEmpregado.IdEmpregado;

                    mensagem = new EmpregadoMensagem()
                    {
                        Criando = true,
                        EmpregadoDto = EmpregadoDto
                    };
                }
                else
                {
                    var empregadoExistente = await _dbContext.Empregados.FirstAsync(e => e.IdEmpregado == IdEmpregado);
                    empregadoExistente.NomeCompleto = EmpregadoDto.NomeCompleto;
                    empregadoExistente.Email = EmpregadoDto.Email;
                    empregadoExistente.Salario = EmpregadoDto.Salario;
                    empregadoExistente.DataContratacao = EmpregadoDto.DataContratacao;

                    await _dbContext.SaveChangesAsync();
                    mensagem = new EmpregadoMensagem()
                    {
                        Criando = false,
                        EmpregadoDto = EmpregadoDto
                    };
                }

                MainThread.BeginInvokeOnMainThread(async() =>
                {
                    LoadingVisivel = false;
                    WeakReferenceMessenger.Default.Send(new EmpregadoMensageria(mensagem));
                    await Shell.Current.Navigation.PopAsync();
                });

            });
        }
    }
}
