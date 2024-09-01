using AddressBookSys.Models.Entities;
using System.Collections.Immutable;
using AddressBookSys.Models.Repositories;

namespace AddressBookSys.Models.Services;

public class AddressBookService(IAddressBookRepository addressBookRepository) : IAddressBookService
{

    public Task<IImmutableList<AddressBook>> GetAddressBooks(string? nameFilter = null, string? mailFilter = null, int? skip = null, int? limit = null)
        => addressBookRepository.GetAddressBooks(nameFilter, mailFilter, skip, limit);

    public Task<int> CountAddressBooks(string? nameFilter = null, string? mailFilter = null)
        => addressBookRepository.CountAddressBooks(nameFilter, mailFilter);

    public Task<AddressBook> AddAddressBook(AddressBook addressBook)
        => addressBookRepository.AddAddressBook(addressBook);

    public Task<bool> UpdateAddressBook(AddressBook addressBook)
        => addressBookRepository.UpdateAddressBook(addressBook);

    public Task<bool> RemoveAddressBook(AddressBook addressBook)
        => addressBookRepository.RemoveAddressBook(addressBook);

    public Task<bool> RemoveAddressBook(int id)
        => addressBookRepository.RemoveAddressBook(id);
}