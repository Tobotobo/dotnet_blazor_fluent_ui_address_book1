using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;

namespace AddressBookSys.Views;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAddressBookSysViews(this IServiceCollection services, bool wasm = false, bool prerender = true)
    {
        if (wasm) {
            Settings.RenderMode = new InteractiveWebAssemblyRenderMode(prerender);
        } else {
            Settings.RenderMode = new InteractiveServerRenderMode(prerender);
        }
        services.AddFluentUIComponents();
        return services;
    }
}