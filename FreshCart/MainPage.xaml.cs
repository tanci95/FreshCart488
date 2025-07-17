//using System.Collections.ObjectModel;
using System.Text;
//using AndroidX.Lifecycle;
using FreshCart.Models;
using FreshCart.Services;
using FreshCart.ViewModels;


namespace FreshCart;
    //public List<GroceryGroup> GroupedItem { get; set; }
    public partial class MainPage : ContentPage
    {
    private GroceryViewModel viewModel;

    public MainPage()
        {
            InitializeComponent();
            viewModel = new GroceryViewModel();
            BindingContext = viewModel;

    }
    private async void OnExportClicked(object sender, EventArgs e)
        {
            try
            {
                var items = await App.Database.GetItemsAsync(); // assumes async DB access
                var sortedItems = items
                    .OrderBy(i => i.Category)
                    .ThenBy(i => i.Name)
                    .ThenBy(i => i.Quantity)
                    .ToList();

                var lines = new List<string>();
                string currentCategory = null;

                foreach (var item in sortedItems)
                {
                    if (item.Category != currentCategory)
                    {
                        currentCategory = item.Category;
                        lines.Add($"\n== {currentCategory.ToUpper()} ==");
                    }

                    lines.Add($"{item.Name} - {item.Quantity}");
                }

                string fileName = $"GroceryList_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

                File.WriteAllLines(filePath, lines);

                await DisplayAlert("Export Successful", $"File saved to:\n{filePath}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Export Failed", ex.Message, "OK");
            }
        }
    }
    

