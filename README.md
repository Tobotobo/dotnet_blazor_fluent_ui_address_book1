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
dotnet add AddressBookSys.App.WPF package Microsoft.Extensions.Logging.Debug
```
<Project Sdk="Microsoft.NET.Sdk.Razor">
<RootNamespace>AddressBookSys.App.WPF</RootNamespace>

```
dotnet run --project AddressBookSys.App.WPF
```

```
dotnet new blazorwasm --pwa -n AddressBookSys.App.PWA
dotnet sln add AddressBookSys.App.PWA
dotnet add AddressBookSys.App.PWA reference AddressBookSys.Models
dotnet add AddressBookSys.App.PWA reference AddressBookSys.Views
dotnet add AddressBookSys.App.PWA package Microsoft.EntityFrameworkCore
```

```
dotnet run --project AddressBookSys.App.Web
dotnet run --project AddressBookSys.App.PWA
dotnet run --project AddressBookSys.App.WPF
```

```
dotnet publish -r win-x64 --self-contained AddressBookSys.App.WPF -o publish/AddressBookSys.App.WPF
dotnet publish AddressBookSys.App.PWA -o publish/AddressBookSys.App.PWA
```

## TODO
* BlazorWebView の内部エラーの確認方法が不明（無言で落ちたりする）
* WPF で AddDbContext で追加すると当該コンテキストにアクセスが発生する操作のタイミングでアプリが無言で落ちる
* PWA の実行時に Models の sqlite のネイティブファイルに対する警告が表示される。また動作もしない。

## EFCore + PostgreSQL

```
dotnet add AddressBookSys.App.Web package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add AddressBookSys.App.Web package DotNetEnv
```

```
dotnet add AddressBookSys.App.PWA package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add AddressBookSys.App.PWA package DotNetEnv
```

PWA で Npgsql が使えず　→　WebAPI化を検討
```
Microsoft.AspNetCore.Components.WebAssembly.Rendering.WebAssemblyRenderer[100]
      Unhandled exception rendering component: System.Net.NameResolution is not supported on this platform.
System.PlatformNotSupportedException: System.Net.NameResolution is not supported on this platform.
```

```
dotnet add AddressBookSys.App.WPF package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add AddressBookSys.App.WPF package DotNetEnv
```


## WebAPI

チュートリアル: ASP.NET Core で Web API を作成する  
https://learn.microsoft.com/ja-jp/aspnet/core/tutorials/first-web-api?view=aspnetcore-8.0&tabs=visual-studio-code  

Dapr を使ってみる - 入門編  
https://qiita.com/takashiuesaka/items/bdae30de41b0daab881f  

```
dotnet new webapi --use-controllers -n AddressBookSys.App.WebAPI
dotnet sln add AddressBookSys.App.WebAPI
dotnet add AddressBookSys.App.WebAPI reference AddressBookSys.Models
dotnet add AddressBookSys.App.WebAPI package Microsoft.EntityFrameworkCore
dotnet add AddressBookSys.App.WebAPI package Npgsql.EntityFrameworkCore.PostgreSQL
```

```
dotnet run --project AddressBookSys.App.WebAPI
```
http://localhost:5285/swagger/index.html
http://localhost:5285/weatherforecast
http://localhost:5285/addressbooks
http://localhost:5285/addressbooks/1

ASP.NET Core Blazor configuration  
https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/configuration?view=aspnetcore-8.0  

```
dotnet remove AddressBookSys.App.Web package DotNetEnv
dotnet remove AddressBookSys.App.WPF package DotNetEnv
dotnet remove AddressBookSys.App.PWA package DotNetEnv
```

WPF デスクトップ アプリを .NET 8 にアップグレードする方法  
https://learn.microsoft.com/ja-jp/dotnet/desktop/wpf/migration/?view=netdesktop-8.0  

WPF でも appsettings.json を読み込むようにする

※ WPF のプロジェクトの SDK を Microsoft.NET.Sdk.Razor にしてるため以下は不要

```
dotnet add AddressBookSys.App.WPF package Microsoft.Extensions.Configuration.Json
```
```
  <ItemGroup>
    <Content Include="appsettings.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </Content>
  </ItemGroup>
```

## PWA 更新

Blazor WebAssembly オフライン対応 PWA をリロードしても更新できないのはなぜか?  
https://qiita.com/jsakamoto/items/39c434aecab5b771f824  