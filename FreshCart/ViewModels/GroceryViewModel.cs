using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using FreshCart.Models;
using FreshCart.ViewModels;
using SQLite;

namespace FreshCart.ViewModels
{
    public class GroceryViewModel : BaseViewModel
    {
        public ObservableCollection<GroceryItemGroup> GroupedItems { get; set; } = new();

        public string NewItemName { get; set; }
        public int NewItemQuantity { get; set; }
        public string NewItemCategory { get; set; }

        public ICommand AddItemCommand { get; }
        public ICommand DeleteItemCommand { get; }
        public ICommand IncreaseQuantityCommand { get; }
        public ICommand DecreaseQuantityCommand { get; }


        public GroceryViewModel()
        {
            AddItemCommand = new Command(async () => await AddItemAsync());
            DeleteItemCommand = new Command<GroceryItem>(async (item) => await DeleteItemAsync(item));
            IncreaseQuantityCommand = new Command<GroceryItem>(async (item) => await ChangeQuantityAsync(item, 1));
            DecreaseQuantityCommand = new Command<GroceryItem>(async (item) => await ChangeQuantityAsync(item, -1));

            InitializeAsync(); // start loading
        }

        private async void InitializeAsync()
        {
            await LoadItems();
        }
        private readonly SQLiteAsyncConnection _database;


        public async Task LoadItems()
        {
            var allItems = await App.Database.GetItemsAsync();

            var validItems = allItems
                .Where(i => !string.IsNullOrWhiteSpace(i.Category))
                .ToList();

            var grouped = validItems
                .GroupBy(item => item.Category)
                .OrderBy(group => group.Key)
                .Select(g => new GroceryItemGroup(g.Key, g.OrderBy(i => i.Name)))
                .ToList();

            // Refresh the entire ObservableCollection instance
            GroupedItems = new ObservableCollection<GroceryItemGroup>(grouped);

            // Notify the UI that GroupedItems was replaced
            OnPropertyChanged(nameof(GroupedItems));
        }


        //public async Task LoadItems()
        //{
        //    if (GroupedItems == null)
        //        GroupedItems = new ObservableCollection<GroceryItemGroup>();
        //    else
        //        GroupedItems.Clear();

        //    var allitems = await App.Database.GetItemsAsync();

        //    var grouped = allitems
        //        .GroupBy(item => item.Category)
        //        .OrderBy(group => group.Key)
        //        .Select(g => new GroceryItemGroup(g.Key, g.OrderBy(i => i.Name)));

        //    foreach (var group in grouped)
        //    {
        //        GroupedItems.Add(group);
        //    }

        //    OnPropertyChanged(nameof(GroupedItems)); // Optional but helpful
        //}


        //public async Task LoadItems()
        //{
        //    GroupedItems.Clear(); 

        //    var allitems = await App.Database.GetItemsAsync(); 

        //    var grouped = allitems
        //        .GroupBy(item => item.Category)
        //        .OrderBy(group => group.Key)
        //        .Select(g => new GroceryItemGroup(g.Key, g.OrderBy(i => i.Name)));

        //    foreach (var group in grouped)
        //    {
        //        GroupedItems.Add(group);
        //    }
        //}

        private async Task AddItemAsync()
        {
            if (string.IsNullOrWhiteSpace(NewItemName) || NewItemQuantity <= 0 || string.IsNullOrWhiteSpace(NewItemCategory))
                return;

            var item = new GroceryItem
            {
                Name = NewItemName,
                Quantity = NewItemQuantity,
                Category = NewItemCategory
            };

            await App.Database.SaveItemAsync(item);
            await LoadItems(); // Refresh the grouped list

            NewItemName = string.Empty;
            NewItemQuantity = 0;
            NewItemCategory = string.Empty;
            OnPropertyChanged(nameof(NewItemName));
            OnPropertyChanged(nameof(NewItemQuantity));
            OnPropertyChanged(nameof(NewItemCategory));
        }

        private async Task DeleteItemAsync(GroceryItem item)
        {
            if (item == null) return;

            await App.Database.DeleteItemAsync(item);
            await LoadItems(); // Refresh the grouped list
        }

        private async Task ChangeQuantityAsync(GroceryItem item, int change)
        {
            if (item == null)
                return;

            item.Quantity += change;
            if (item.Quantity < 0)
                item.Quantity = 0;

            await App.Database.SaveItemAsync(item);
            await LoadItems(); // Refresh the grouped list
        }

    }
}

