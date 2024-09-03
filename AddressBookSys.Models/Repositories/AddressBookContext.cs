using AddressBookSys.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AddressBookSys.Models.Repositories;

public class AddressBookContext : DbContext {
    public DbSet<AddressBook> AddressBooks { get; set; } = null!;

    private readonly Action<DbContextOptionsBuilder>? _config;

    // public AddressBookContext(DbContextOptions<AddressBookContext> options)
    //     : base(options)
    // {
    // }

    public AddressBookContext(DbContextOptions options) : base(options) { }

    public AddressBookContext(Action<DbContextOptionsBuilder> config) { 
        _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _config?.Invoke(optionsBuilder);
    }
}