using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;
using AddressBookSys.Views;
using AddressBookSys.Views.Components;
using AddressBookSys.Models.Entities;
using AddressBookSys.Models.Repositories;
using AddressBookSys.Models.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AddressBookSys.WPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var connectionString = "DataSource=:memory:";
        var connection = new SqliteConnection(connectionString);
        connection.Open();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddWpfBlazorWebView();
#if DEBUG
    		serviceCollection.AddBlazorWebViewDeveloperTools();
#endif

        serviceCollection.AddAddressBookSysViews();

        serviceCollection
            .AddDbContext<AddressBookContext>(x => x.UseSqlite(connection))
            .AddTransient<IAddressBookRepository, AddressBookRepository>()
            .AddTransient<IAddressBookService, AddressBookService>();



        Resources.Add("services", serviceCollection.BuildServiceProvider());
    }
}