using FreshCart.Services;
using System.IO;
namespace FreshCart
{
    //    public partial class App : Application
    //    {
    //        public App()
    //        {
    //            InitializeComponent();
    //        }

    //        protected override Window CreateWindow(IActivationState? activationState)
    //        {
    //            return new Window(new AppShell());
    //        }
    //    }
    //}

    public partial class App : Application
    {
        private static GroceryDatabase _database;
        public static GroceryDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "grocery.db3");
                    _database = new GroceryDatabase(dbPath);
                }
                return _database;
            }
        }

        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
        }
    }
}