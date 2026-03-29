using SQLite;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Descricao { get; set; }
        public double Quantidade { get; set; }
        public double Preco { get; set; }

        // Campo calculado
        public double Total => Quantidade * Preco;

        // Campo Categoria
        public string Categoria { get; set; }
    }
}