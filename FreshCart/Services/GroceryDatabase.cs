using SQLite;
using FreshCart.Models;

namespace FreshCart.Services
{
    public class GroceryDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public GroceryDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<GroceryItem>().Wait();
        }

        public Task<List<GroceryItem>> GetItemsAsync()
        {
            return _database.Table<GroceryItem>().ToListAsync();
        }

        public Task<GroceryItem> GetItemAsync(int id)
        {
            return _database.Table<GroceryItem>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(GroceryItem item)
        {
            if (item.Id != 0)
            {
                return _database.UpdateAsync(item);
            }
            else
            {
                return _database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(GroceryItem item)
        {
            return _database.DeleteAsync(item);
        }
        //    private readonly SQLiteAsyncConnection _database;

        //    public GroceryDatabase(string dbPath)
        //    {
        //        _database = new SQLiteAsyncConnection(dbPath);
        //        _database.CreateTableAsync<GroceryItem>().Wait();
        //    }

        //    public Task<List<GroceryItem>> GetItemsAsync() =>
        //        _database.Table<GroceryItem>().ToListAsync();

        //    public Task<List<GroceryItem>> GetItemsByCategoryAsync(string category) =>
        //        _database.Table<GroceryItem>().Where(i => i.Category == category).ToListAsync();

        //    public Task<int> SaveItemAsync(GroceryItem item) =>
        //        _database.InsertOrReplaceAsync(item);

        //    public Task<int> DeleteItemAsync(GroceryItem item) =>
        //        _database.DeleteAsync(item);
        //}
    }
}

