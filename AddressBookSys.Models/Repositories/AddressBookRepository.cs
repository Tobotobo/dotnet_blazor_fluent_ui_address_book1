using System.Collections.ObjectModel;
using AddressBookSys.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace AddressBookSys.Models.Repositories;

public class AddressBookRepository(AddressBookContext context) : IAddressBookRepository
{

    public async Task DatabaseEnsureCreated()
    {
        await context.Database.EnsureCreatedAsync();
    }

    public async Task<IImmutableList<AddressBook>> GetAddressBooks(string? nameFilter = null, string? mailFilter = null, int? skip = null, int? limit = null)
    {
        var query = context.AddressBooks.AsQueryable();
        if (!string.IsNullOrEmpty(nameFilter)) {
            query = query.Where(x => x.Name.Contains(nameFilter));
        }
        if (!string.IsNullOrEmpty(mailFilter)) {
            query = query.Where(x => x.Mail.Contains(mailFilter));
        }
        query = query.OrderBy(x => x.Id);
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

    public async Task<int> CountAddressBooks(string? nameFilter = null, string? mailFilter = null)
    {
        var query = context.AddressBooks.AsQueryable();
        if (!string.IsNullOrEmpty(nameFilter)) {
            query = query.Where(x => x.Name.Contains(nameFilter));
        }
        if (!string.IsNullOrEmpty(mailFilter)) {
            query = query.Where(x => x.Mail.Contains(mailFilter));
        }
        return await query.CountAsync();
    }

    public async Task<AddressBook> AddAddressBook(AddressBook addressBook)
    {
        var result = await context.AddressBooks.AddAsync(addressBook);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<bool> UpdateAddressBook(AddressBook addressBook)
    {
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