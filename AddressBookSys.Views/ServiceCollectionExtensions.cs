using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;

namespace AddressBookSys.Views;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAddressBookSysViews(this IServiceCollection services, RenderMode renderMode, bool prerender = true)
    {
        Settings.RenderMode = renderMode switch {
            RenderMode.Server => new InteractiveServerRenderMode(prerender),
            RenderMode.WebAssembly => new InteractiveWebAssemblyRenderMode(prerender),
            RenderMode.WebView => null,
            _ => throw new ArgumentOutOfRangeException(nameof(renderMode), $"Not expected renderMode value: {renderMode}"),
        };
        services.AddFluentUIComponents();
        return services;
    }
}