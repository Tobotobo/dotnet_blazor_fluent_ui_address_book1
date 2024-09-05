using AddressBookSys.Models.Entities;
using System.Collections.Immutable;

namespace AddressBookSys.Models.Repositories;

public class AddressBookRepositoryMoc : IAddressBookRepository
{
    private readonly object addressBooksLock = new();
    private readonly List<AddressBook> addressBooks = new();
    private int nextId = 1;

    public AddressBookRepositoryMoc(bool includeDummyData = false) {
        // ダミーデータを追加
        if (includeDummyData) {
            foreach(var x in new[]{
                ("東京 太郎", "tokyo_taro@example.com"),
                ("大阪 花子", "osaka_hanako@example.com"),
                ("京都 次郎", "kyoto_jiro@example.com"),
                ("名古屋 美咲", "nagoya_misaki@example.com"),
                ("札幌 健", "sapporo_ken@example.com"),
                ("福岡 由美", "fukuoka_yumi@example.com"),
                ("横浜 大輔", "yokohama_daisuke@example.com"),
                ("神戸 優子", "kobe_yuko@example.com"),
                ("仙台 忠", "sendai_tadashi@example.com"),
                ("広島 美紀", "hiroshima_miki@example.com"),
                ("東京 花子", "tokyo_hanako@example.com"),
                ("大阪 太郎", "osaka_taro@example.com"),
                ("京都 美咲", "kyoto_misaki@example.com"),
                ("名古屋 健", "nagoya_ken@example.com"),
                ("札幌 由美", "sapporo_yumi@example.com"),
                ("福岡 次郎", "fukuoka_jiro@example.com"),
                ("横浜 優子", "yokohama_yuko@example.com"),
                ("神戸 大輔", "kobe_daisuke@example.com"),
                ("仙台 美紀", "sendai_miki@example.com"),
                ("広島 忠", "hiroshima_tadashi@example.com"),
                ("東京 次郎", "tokyo_jiro@example.com"),
                ("大阪 美咲", "osaka_misaki@example.com"),
                ("京都 健", "kyoto_ken@example.com"),
                ("名古屋 由美", "nagoya_yumi@example.com"),
                ("札幌 太郎", "sapporo_taro@example.com"),
                ("福岡 美紀", "fukuoka_miki@example.com"),
                ("横浜 忠", "yokohama_tadashi@example.com"),
                ("神戸 花子", "kobe_hanako@example.com"),
                ("仙台 大輔", "sendai_daisuke@example.com"),
                ("広島 優子", "hiroshima_yuko@example.com"),
            }) {
                InnerAddAddressBook(new AddressBook(0, x.Item1, x.Item2));
            }
        }
    }

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
                return InnerAddAddressBook(addressBook);
            }
        });
    }

    private AddressBook InnerAddAddressBook(AddressBook addressBook) {
        var newAddressBook = addressBook with { Id = nextId };
        nextId += 1;
        addressBooks.Add(newAddressBook);
        return newAddressBook;
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