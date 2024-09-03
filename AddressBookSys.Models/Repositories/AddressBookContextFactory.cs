using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AddressBookSys.Models.Repositories;

public class AddressBookContextFactory : IDbContextFactory<AddressBookContext>
{
    private readonly IServiceProvider _services;

    public AddressBookContextFactory(IServiceProvider services)
    {
        _services = services;
    }

    public AddressBookContext CreateDbContext()
    {
        var context = _services.GetRequiredService<AddressBookContext>();
        return context;
    }
}