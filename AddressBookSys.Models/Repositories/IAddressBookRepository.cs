using AddressBookSys.Models.Entities;
using System.Collections.Immutable;

namespace AddressBookSys.Models.Repositories;

public interface IAddressBookRepository
{
    Task<IImmutableList<AddressBook>> GetAddressBooks(string? nameFilter = null, string? mailFilter = null, int? skip = null, int? limit = null, bool sortByIdAscending = true);

    Task<AddressBook?> GetAddressBook(int id);

    Task<AddressBook> AddAddressBook(AddressBook addressBook);

    Task<bool> UpdateAddressBook(AddressBook addressBook);

    Task<bool> RemoveAddressBook(AddressBook addressBook);

    Task<bool> RemoveAddressBook(int id);

    Task<int> CountAddressBooks(string? nameFilter = null, string? mailFilter = null);
}