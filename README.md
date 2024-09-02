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

dotnet new fluentblazor -n AddressBookSys.App.Server --no-https
dotnet sln add AddressBookSys.App.Server
dotnet add AddressBookSys.App.Server reference AddressBookSys.Models
dotnet add AddressBookSys.App.Server package Microsoft.EntityFrameworkCore


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
dotnet run --project AddressBookSys.App.Server
```
