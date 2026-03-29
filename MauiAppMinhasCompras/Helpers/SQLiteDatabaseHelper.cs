using MauiAppMinhasCompras.Models;
using SQLite;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
        }

        // Inserir produto
        public Task<int> Insert(Produto p)
        {
            return _conn.InsertAsync(p);
        }

        // Atualizar produto (agora inclui Categoria)
        public Task<int> Update(Produto p)
        {
            return _conn.UpdateAsync(p);
        }

        // Remover produto
        public Task<int> Delete(int id)
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }

        // Listar todos os produtos
        public Task<List<Produto>> GetAll()
        {
            return _conn.Table<Produto>().ToListAsync();
        }

        // Buscar por descrição
        public Task<List<Produto>> Search(string q)
        {
            string sql = $"SELECT * FROM Produto WHERE Descricao LIKE '%{q}%'";
            return _conn.QueryAsync<Produto>(sql);
        }
    }
}