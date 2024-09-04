using AddressBookSys.Models.Entities;
using System.Collections.Immutable;
using AddressBookSys.Models.Repositories;

namespace AddressBookSys.Models.Services;

// TODO: WebAPI と App 用でクラスを分ける ※今は処理が無いからよいが処理が追加されたら二重で実行される
public class AddressBookService(IAddressBookRepository addressBookRepository) : IAddressBookService
{

    public Task<IImmutableList<AddressBook>> GetAddressBooks(
        string? nameFilter = null, string? mailFilter = null, int? skip = null, int? limit = null, bool sortByIdAscending = true)
        => addressBookRepository.GetAddressBooks(nameFilter, mailFilter, skip, limit, sortByIdAscending);

    public Task<AddressBook?> GetAddressBook(int id)
        => addressBookRepository.GetAddressBook(id);

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