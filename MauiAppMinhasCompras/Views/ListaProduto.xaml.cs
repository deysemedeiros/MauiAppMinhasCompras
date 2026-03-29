using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();
        lst_produtos.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        try
        {
            await CarregarProdutos();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async Task CarregarProdutos(string? filtroCategoria = null, string? busca = null)
    {
        lista.Clear();
        var produtos = await App.Db.GetAll();

        if (!string.IsNullOrEmpty(busca))
            produtos = produtos.Where(p => p.Descricao.Contains(busca, StringComparison.OrdinalIgnoreCase)).ToList();

        if (!string.IsNullOrEmpty(filtroCategoria) && filtroCategoria != "Todas")
            produtos = produtos.Where(p =>
                !string.IsNullOrEmpty(p.Categoria) &&
                p.Categoria.Equals(filtroCategoria, StringComparison.OrdinalIgnoreCase)
            ).ToList();

        produtos.ForEach(p => lista.Add(p));

        if (!lista.Any())
            await DisplayAlert("Aviso", "Nenhum produto encontrado.", "OK");
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.NovoProduto());
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        lst_produtos.IsRefreshing = true;
        await CarregarProdutos(busca: e.NewTextValue);
        lst_produtos.IsRefreshing = false;
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        double soma = lista.Sum(i => i.Total);
        string msg = $"O total é {soma:C}";
        DisplayAlert("Total dos Produtos", msg, "OK");
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        MenuItem selecinado = sender as MenuItem;
        Produto p = selecinado.BindingContext as Produto;

        bool confirm = await DisplayAlert("Tem Certeza?", $"Remover {p.Descricao}?", "Sim", "Não");

        if (confirm)
        {
            await App.Db.Delete(p.Id);
            lista.Remove(p);
        }
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        Produto p = e.SelectedItem as Produto;
        Navigation.PushAsync(new Views.EditarProduto
        {
            BindingContext = p,
        });
    }

    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        await CarregarProdutos();
        lst_produtos.IsRefreshing = false;
    }

    private async void pickerCategoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        var categoriaSelecionada = pickerCategoria.SelectedItem?.ToString();
        await CarregarProdutos(filtroCategoria: categoriaSelecionada);
    }

    private void ToolbarItem_Clicked_Relatorio(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.RelatorioPage());
    }
}