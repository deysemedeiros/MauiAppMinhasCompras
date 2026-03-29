using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
    public EditarProduto()
    {
        InitializeComponent();
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Produto p = (Produto)BindingContext;

            if (pickerCategoria.SelectedItem == null)
            {
                await DisplayAlert("Aviso", "Por favor, selecione uma categoria", "OK");
                return;
            }

            p.Descricao = txt_descricao.Text;
            p.Quantidade = Convert.ToDouble(txt_quantidade.Text);
            p.Preco = Convert.ToDouble(txt_preco.Text);
            p.Categoria = pickerCategoria.SelectedItem.ToString();

            await App.Db.Update(p);
            await DisplayAlert("Sucesso!", "Registro Atualizado", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}