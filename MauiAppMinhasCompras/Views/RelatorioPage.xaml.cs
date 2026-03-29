using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class RelatorioPage : ContentPage
{
    public RelatorioPage()
    {
        InitializeComponent();
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await GerarRelatorio();
    }

    private async Task GerarRelatorio()
    {
        try
        {
            var produtos = await App.Db.GetAll();

            var relatorio = produtos
                .GroupBy(p => p.Categoria)
                .Select(g => new RelatorioCategoria
                {
                    Categoria = g.Key,
                    Total = g.Sum(p => p.Total),
                    QuantidadeItens = g.Count()
                })
                .ToList();

            lst_relatorio.ItemsSource = relatorio;

            if (!relatorio.Any())
                await DisplayAlert("Aviso", "Nenhum dado para relatório.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}