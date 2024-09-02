# dotnet_blazor_fluent_ui_address_book1

## 試行錯誤中...

![alt text](images/README/image.png)

```
dotnet new gitignore
dotnet new sln -n AddressBookSys

dotnet new classlib -n AddressBookSys.Models
dotnet sln add AddressBookSys.Models
dotnet add AddressBookSys.Models package Microsoft.EntityFrameworkCore
dotnet add AddressBookSys.Models package Microsoft.EntityFrameworkCore.Sqlite
dotnet add AddressBookSys.Models package System.Collections.Immutable

dotnet new nunit -n AddressBookSys.Models.Tests
dotnet sln add AddressBookSys.Models.Tests
dotnet add AddressBookSys.Models.Tests reference AddressBookSys.Models

dotnet new fluentblazor -n AddressBookSys.App.Web --no-https
dotnet sln add AddressBookSys.App.Web
dotnet add AddressBookSys.App.Web reference AddressBookSys.Models
dotnet add AddressBookSys.App.Web package Microsoft.EntityFrameworkCore


```

```
dotnet new console -n AddressBookSys.Console
dotnet sln add AddressBookSys.Console
dotnet add AddressBookSys.Console reference AddressBookSys.Models
dotnet add AddressBookSys.Console package Microsoft.Extensions.DependencyInjection
```

```
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate --project AddressBookSys.Console
```

```
dotnet run --project AddressBookSys.Console
dotnet run --project AddressBookSys.App.Web
```

View を分離
```
dotnet new razorclasslib -n AddressBookSys.Views
dotnet sln add AddressBookSys.Views
dotnet add AddressBookSys.Views reference AddressBookSys.Models
dotnet add AddressBookSys.Views package Microsoft.FluentUI.AspNetCore.Components
dotnet add AddressBookSys.Views package Microsoft.FluentUI.AspNetCore.Components.Icons
dotnet add AddressBookSys.Views package Microsoft.Extensions.DependencyInjection

dotnet add AddressBookSys.App.Web reference AddressBookSys.Views
dotnet remove AddressBookSys.App.Web package Microsoft.FluentUI.AspNetCore.Components
dotnet remove AddressBookSys.App.Web package Microsoft.FluentUI.AspNetCore.Components.Icons
```
* `AddressBookSys.Views.csproj` 以外削除
* `AddressBookSys.App.Serve` から `Components` と `wwwroot` を移動
* `AddressBookSys.Views\Components\_Imports.razor` で `App.Web` になっている箇所を `Views` に変更
* `AddressBookSys.App.Web\Program.cs` で `App.Web` になっている箇所を `Views` に変更
* `AddressBookSys.Views\Components\Dialogs\DialogData.cs` で `App.Web` になっている箇所を `Views` に変更
* `AddressBookSys.Views\Components\Routes.razor` `Program` を `App` に変更
* `AddressBookSys.Views\Components\App.razor` で `App.Web` になっている箇所を `Views` に変更
* `AddressBookSys.Views\ServiceCollectionExtensions.cs` を作成
  ```cs
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
  ```
* `AddressBookSys.App.Web\Program.cs` の `AddFluentUIComponents` を `AddAddressBookSysViews` に置き換え

TODO
```
AddressBookSys.Views\Components\Pages\Error.razor(29,13): error CS0246: 型または名前空間の名前 'HttpContext' が見つか
りませんでした (using ディレクテ
ィブまたはアセンブリ参照が指定されていることを確認してください)
```

AddressBookSys.Views\Components\App.razor  
* リンクを変更　→　`href="AddressBookSys.App.Web.styles.css"` だけ呼び出し元に依存してしまう　→　要対応
```
    <link rel="stylesheet" href="_content/AddressBookSys.Views/app.css" />
    <link rel="stylesheet" href="AddressBookSys.App.Web.styles.css" />
    <link rel="icon" type="image/x-icon" href="_content/AddressBookSys.Views/favicon.ico" />
```


```
dotnet new wpf -n AddressBookSys.App.WPF
dotnet sln add AddressBookSys.App.WPF
dotnet add AddressBookSys.App.WPF reference AddressBookSys.Models
dotnet add AddressBookSys.App.WPF reference AddressBookSys.Views
dotnet add AddressBookSys.App.WPF package Microsoft.AspNetCore.Components.WebView.Wpf
dotnet add AddressBookSys.App.WPF package Microsoft.EntityFrameworkCore
```
<Project Sdk="Microsoft.NET.Sdk.Razor">
<RootNamespace>AddressBookSys.App.WPF</RootNamespace>

```
dotnet run --project AddressBookSys.App.WPF
```
