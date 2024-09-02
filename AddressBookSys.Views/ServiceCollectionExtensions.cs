using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;

namespace AddressBookSys.Views;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAddressBookSysViews(this IServiceCollection services)
    {
        services.AddFluentUIComponents();
        return services;
    }
}