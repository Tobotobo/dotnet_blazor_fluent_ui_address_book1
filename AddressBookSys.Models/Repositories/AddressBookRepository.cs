using AddressBookSys.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace AddressBookSys.Models.Repositories;

public class AddressBookRepository(AddressBookContextFactory contextFactory) : IAddressBookRepository
{

    public async Task DatabaseEnsureCreated()
    {
        using var context = contextFactory.CreateDbContext();
        await context.Database.EnsureCreatedAsync();
    }

    public async Task<IImmutableList<AddressBook>> GetAddressBooks(
        string? nameFilter = null, string? mailFilter = null, int? skip = null, int? limit = null, bool sortByIdAscending = true)
    {
        using var context = contextFactory.CreateDbContext();
        var query = context.AddressBooks.AsQueryable();
        if (!string.IsNullOrEmpty(nameFilter))
        {
            query = query.Where(x => x.Name.Contains(nameFilter));
        }
        if (!string.IsNullOrEmpty(mailFilter))
        {
            query = query.Where(x => x.Mail.Contains(mailFilter));
        }
        if (sortByIdAscending)
        {
            query = query.OrderBy(x => x.Id);
        }
        else
        {
            query = query.OrderByDescending(x => x.Id);
        }
        if (skip != null)
        {
            query = query.Skip((int)skip);
        }
        if (limit != null)
        {
            query = query.Take((int)limit);
        }
        return (await query.ToListAsync()).ToImmutableList();
    }

    public async Task<AddressBook?> GetAddressBook(int id) {
        using var context = contextFactory.CreateDbContext();
        var existingEntity = await context.AddressBooks.FindAsync(id);
        return existingEntity;
    }

    public async Task<int> CountAddressBooks(string? nameFilter = null, string? mailFilter = null)
    {
        using var context = contextFactory.CreateDbContext();
        var query = context.AddressBooks.AsQueryable();
        if (!string.IsNullOrEmpty(nameFilter))
        {
            query = query.Where(x => x.Name.Contains(nameFilter));
        }
        if (!string.IsNullOrEmpty(mailFilter))
        {
            query = query.Where(x => x.Mail.Contains(mailFilter));
        }
        return await query.CountAsync();
    }

    public async Task<AddressBook> AddAddressBook(AddressBook addressBook)
    {
        using var context = contextFactory.CreateDbContext();
        var result = await context.AddressBooks.AddAsync(addressBook);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<bool> UpdateAddressBook(AddressBook addressBook)
    {
        using var context = contextFactory.CreateDbContext();
        var existingEntity = await context.AddressBooks.FindAsync(addressBook.Id);
        if (existingEntity == null)
        {
            return false;
        }
        context.Entry(existingEntity).CurrentValues.SetValues(addressBook);
        await context.SaveChangesAsync();
        return true;
    }

    public Task<bool> RemoveAddressBook(AddressBook addressBook)
    {
        return RemoveAddressBook(addressBook.Id);
    }

    public async Task<bool> RemoveAddressBook(int id)
    {
        using var context = contextFactory.CreateDbContext();
        var existingEntity = await context.AddressBooks.FindAsync(id);
        if (existingEntity == null)
        {
            return false;
        }
        context.AddressBooks.Remove(existingEntity);
        await context.SaveChangesAsync();
        return true;
    }
}