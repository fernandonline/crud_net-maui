using crud_maui.ViewModels;

namespace crud_maui.Views;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Task.Delay(100);
        await ((MainViewModel)BindingContext).Obter();
    }
}