//using System.Collections.ObjectModel;
using FreshCart.Models;
using FreshCart.Services;

namespace FreshCart;

//public partial class MainPage : ContentPage
//{
//    private ObservableCollection<string> GroceryItems = new();

//    public MainPage()
//    {
//        InitializeComponent();
//        GroceryListView.ItemsSource = GroceryItems;
//    }

//    private void OnAddItemClicked(object sender, EventArgs e)
//    {
//        if (!string.IsNullOrWhiteSpace(ItemEntry.Text))
//        {
//            GroceryItems.Add(ItemEntry.Text);
//            ItemEntry.Text = string.Empty;
//        }

//    }
//}
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        LoadItems();
    }

    private async void LoadItems()
    {
        GroceryListView.ItemsSource = await App.Database.GetItemsAsync();
    }

    private async void OnAddItemClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ItemNameEntry.Text) || string.IsNullOrWhiteSpace(ItemQuantityEntry.Text))
            return;

        var item = new GroceryItem
        {
            Name = ItemNameEntry.Text,
            Quantity = int.Parse(ItemQuantityEntry.Text)
        };

        await App.Database.SaveItemAsync(item);
        ItemNameEntry.Text = "";
        ItemQuantityEntry.Text = "";
        LoadItems();
    }

    private async void OnIncreaseClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var item = (GroceryItem)button.CommandParameter;
        item.Quantity++;
        await App.Database.SaveItemAsync(item);
        LoadItems();
    }

    private async void OnDecreaseClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var item = (GroceryItem)button.CommandParameter;
        if (item.Quantity > 0)
        {
            item.Quantity--;
            await App.Database.SaveItemAsync(item);
            LoadItems();
        }
    }
}
