using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AddressBookSys.Views;

public static class Settings {
    public static AppType AppType { get; set; }
    public static IComponentRenderMode? RenderMode { get; set; } = null; 
}