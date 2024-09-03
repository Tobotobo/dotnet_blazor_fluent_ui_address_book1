// using AddressBookSys.Models.Repositories;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using AddressBookSys.Models.Entities;
// using Microsoft.Data.Sqlite;
// using AddressBookSys.Models.Services;

// // var connectionString = "Data Source=addressbook.db";
// var connectionString = "DataSource=:memory:";
// using var connection = new SqliteConnection(connectionString);
// connection.Open();

// var services = new ServiceCollection()
//     // .AddSqlite<AddressBookContext>(connectionString)
//     // .AddDbContext<AddressBookContext>(x => x.UseSqlite(connectionString))
//     .AddDbContext<AddressBookContext>(x => x.UseSqlite(connection))
//     .AddTransient<IAddressBookRepository, AddressBookRepository>()
//     .AddTransient<IAddressBookService, AddressBookService>()
//     .BuildServiceProvider();

// Console.WriteLine("- DB作成 ------------");
// {
//     var context = services.GetRequiredService<AddressBookContext>();
//     var addressBookRepository = new AddressBookRepository(context);
//     await addressBookRepository.DatabaseEnsureCreated();
// }

// var addressBookService = services.GetRequiredService<IAddressBookService>();

// async Task dump()
// {
//     foreach (var addressBook in await addressBookService.GetAddressBooks())
//     {
//         Console.WriteLine(addressBook);
//     }
// }

// Console.WriteLine("- 追加１ ------------");
// var newAddressBook = await addressBookService.AddAddressBook(new AddressBook(0, "東京 太郎", "tokyo_taro@example.com"));
// Console.WriteLine(newAddressBook);

// Console.WriteLine("- 追加２ ------------");
// await addressBookService.AddAddressBook(new AddressBook(0, "東京 花子", "tokyo_hanako@example.com"));
// await dump();

// Console.WriteLine("- 更新 ------------");
// await addressBookService.UpdateAddressBook(newAddressBook with { Name = "東京 太郎!!!" });
// await dump();

// Console.WriteLine("- 削除 ------------");
// await addressBookService.RemoveAddressBook(2);
// await dump();

using System.Net;
using System.Web;

static string GetUriWithQueryParameters(string baseUrl, IDictionary<string, object?> parameters)
{
    var uriBuilder = new UriBuilder(baseUrl);
    var query = HttpUtility.ParseQueryString(uriBuilder.Query);

    foreach (var param in parameters)
    {
        if (param.Value != null)
        {
            query[param.Key] = param.Value.ToString();
        }
    }

    uriBuilder.Query = query.ToString();
    return uriBuilder.ToString();
}

var baseUrl = "https://api.example.com/data";
var parameters = new Dictionary<string, object?>
{
    { "skip", 10 },
    { "limit", 20 },
    { "search", "example" }
};

var fullUrl = GetUriWithQueryParameters(baseUrl, parameters);
        Console.WriteLine(fullUrl);