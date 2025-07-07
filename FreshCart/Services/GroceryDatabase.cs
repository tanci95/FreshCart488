using SQLite;
using FreshCart.Models;

namespace FreshCart.Services
{
    public class GroceryDatabase
    {
        private readonly SQLiteAsyncConnection _db;

        public GroceryDatabase(string dbPath)
        {
            _db = new SQLiteAsyncConnection(dbPath);
            _db.CreateTableAsync<GroceryItem>().Wait();
        }

        public Task<List<GroceryItem>> GetItemsAsync()
        {
            return _db.Table<GroceryItem>().ToListAsync();
        }

        public Task<int> SaveItemAsync(GroceryItem item)
        {
            if (item.Id != 0)
                return _db.UpdateAsync(item);
            else
                return _db.InsertAsync(item);
        }

        public Task<int> DeleteItemAsync(GroceryItem item)
        {
            return _db.DeleteAsync(item);
        }
    }
}

