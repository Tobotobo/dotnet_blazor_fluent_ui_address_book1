using AddressBookSys.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AddressBookSys.Models.Repositories;

public class AddressBookContext : DbContext {
    public DbSet<AddressBook> AddressBooks { get; set; } = null!;

    // public AddressBookContext(DbContextOptions<AddressBookContext> options)
    //     : base(options)
    // {
    // }

    public AddressBookContext(DbContextOptions options) : base(options) { }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.
    // }
}