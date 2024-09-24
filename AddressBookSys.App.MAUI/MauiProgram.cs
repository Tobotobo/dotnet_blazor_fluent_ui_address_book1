using Microsoft.Extensions.Logging;
using AddressBookSys.Views;
using AddressBookSys.Views.Components;
using AddressBookSys.Models.Entities;
using AddressBookSys.Models.Repositories;
using AddressBookSys.Models.Services;

namespace AddressBookSys.App.MAUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif
		builder.Services.AddAddressBookSysViews(AppType.WPF, RenderMode.WebView);

        builder.Services
            .AddHttpClient()
            // .AddTransient<MainWindow>()
            // .AddSingleton(_ => new AddressBookContext(x => x.UseNpgsql(connectionString)))
            // .AddDbContext<AddressBookContext>(x => x.UseSqlite(connection), ServiceLifetime.Singleton)
            // .AddDbContext<AddressBookContext>(x => x.UseNpgsql(connectionString), ServiceLifetime.Singleton, ServiceLifetime.Singleton)
            // .AddTransient(_ => new AddressBookContext(x => x.UseNpgsql(connectionString)))
            // .AddTransient<AddressBookContextFactory>()
            // .AddTransient<IAddressBookRepository, AddressBookRepositoryWebAPI>(services => new AddressBookRepositoryWebAPI(
            //     httpClient: services.GetRequiredService<HttpClient>(),
            //     baseUrl: _config["AddressBookSys:WebApiBaseUrl"]!
            // ))
            .AddSingleton<IAddressBookRepository>(_ => new AddressBookRepositoryMoc(includeDummyData: true))
            .AddTransient<IAddressBookService, AddressBookService>();

		return builder.Build();
	}
}
