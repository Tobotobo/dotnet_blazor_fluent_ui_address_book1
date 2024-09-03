using AddressBookSys.Models.Entities;
using System.Collections.Immutable;

namespace AddressBookSys.Models.Repositories;

public class AddressBookRepositoryMoc : IAddressBookRepository
{
    private readonly object addressBooksLock = new();
    private readonly List<AddressBook> addressBooks = new();
    private int nextId = 1;

    public async Task DatabaseEnsureCreated()
    {
        // ダミー
        await Task.CompletedTask;
    }

    public Task<IImmutableList<AddressBook>> GetAddressBooks(
        string? nameFilter = null, string? mailFilter = null, int? skip = null, int? limit = null, bool sortByIdAscending = true)
    {
        return Task.Run(() => {
            lock(addressBooksLock) {
                var query = addressBooks.AsQueryable();
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
                return (IImmutableList<AddressBook>)query.ToImmutableList();
            }
        });
    }

    public Task<AddressBook?> GetAddressBook(int id) {
        return Task.Run(() => {
            lock(addressBooksLock) {
                var existingEntity = addressBooks.FirstOrDefault(x => x!.Id == id, null);
                return existingEntity;
            }
        });
    }

    public Task<int> CountAddressBooks(string? nameFilter = null, string? mailFilter = null)
    {
        return Task.Run(() => {
            lock(addressBooksLock) {
                var query = addressBooks.AsQueryable();
                if (!string.IsNullOrEmpty(nameFilter))
                {
                    query = query.Where(x => x.Name.Contains(nameFilter));
                }
                if (!string.IsNullOrEmpty(mailFilter))
                {
                    query = query.Where(x => x.Mail.Contains(mailFilter));
                }
                return query.Count();
            }
        });
    }

    public Task<AddressBook> AddAddressBook(AddressBook addressBook)
    {
        return Task.Run(() => {
            lock(addressBooksLock) {
                var newAddressBook = addressBook with { Id = nextId };
                nextId += 1;
                addressBooks.Add(newAddressBook);
                return newAddressBook;
            }
        });
    }

    public Task<bool> UpdateAddressBook(AddressBook addressBook)
    {
        return Task.Run(() => {
            lock(addressBooksLock) {
                var existingEntity = addressBooks.FirstOrDefault(x => x!.Id == addressBook.Id, null);
                if (existingEntity == null)
                {
                    return false;
                }
                var index = addressBooks.IndexOf(existingEntity);
                addressBooks[index] = addressBook;
                return true;
            }
        });
    }

    public Task<bool> RemoveAddressBook(AddressBook addressBook)
    {
        return RemoveAddressBook(addressBook.Id);
    }

    public Task<bool> RemoveAddressBook(int id)
    {
        return Task.Run(() => {
            lock(addressBooksLock) {
                var existingEntity = addressBooks.FirstOrDefault(x => x!.Id == id, null);
                if (existingEntity == null)
                {
                    return false;
                }
                addressBooks.Remove(existingEntity);
                return true;
            }
        });
    }
}