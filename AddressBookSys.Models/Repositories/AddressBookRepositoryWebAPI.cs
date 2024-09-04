using AddressBookSys.Models.Entities;
using Microsoft.Extensions.Options;
using System.Collections.Immutable;
using System.Net.Http.Json;
using System.Web;

namespace AddressBookSys.Models.Repositories;

public class AddressBookRepositoryWebAPI(HttpClient httpClient, string baseUrl) : IAddressBookRepository
{
    private readonly Uri _baseUri = GetUri(new(baseUrl), "AddressBooks");

    static string GetUriWithQueryParameters(Uri baseUri, Dictionary<string, object?> parameters)
        => GetUriWithQueryParameters(baseUri.ToString(), parameters);

    static string GetUriWithQueryParameters(string baseUrl, Dictionary<string, object?> parameters)
    {
        var uriBuilder = new UriBuilder(baseUrl);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        foreach (var param in parameters)
        {
            if (param.Value != null)
            {
                query[param.Key] = param.Value.ToString();
            }
        }

        uriBuilder.Query = query.ToString();
        return uriBuilder.ToString();
    }

    static Uri GetUri(Uri baseUri, params object[] relativeUris) => new(GetUrl(baseUri, relativeUris));

    static string GetUrl(Uri baseUri, params object[] relativeUris)
    {
        var uri = baseUri.ToString();

        foreach (var relativeUri in relativeUris)
        {
            var relativeUriStr = relativeUri.ToString();
            if (relativeUriStr is null) continue;

            if (!uri.EndsWith('/'))
            {
                uri += "/";
            }

            if (relativeUriStr.StartsWith('/'))
            {
                relativeUriStr = relativeUriStr.TrimStart('/');
            }

            uri = new Uri(new Uri(uri), relativeUriStr).ToString();
        }

        return uri;
    }

    public async Task DatabaseEnsureCreated()
    {
        // ダミー
        await Task.CompletedTask;
    }

    public async Task<IImmutableList<AddressBook>> GetAddressBooks(
        string? nameFilter = null, string? mailFilter = null, int? skip = null, int? limit = null, bool sortByIdAscending = true)
    {
        var url = GetUriWithQueryParameters(_baseUri, new() {
            { "nameFilter", nameFilter },
            { "mailFilter", mailFilter },
            { "skip", skip },
            { "limit", limit },
            { "sortByIdAscending", sortByIdAscending },
        });
        var addressBooks = await httpClient.GetFromJsonAsync<AddressBook[]>(url);
        return addressBooks!.ToImmutableList();
    }

    public async Task<AddressBook?> GetAddressBook(int id) {
        var url = GetUrl(_baseUri, id);
        try
        {
            var addressBook = await httpClient.GetFromJsonAsync<AddressBook>(url);
            return addressBook;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            // 404 Not Found の場合は null を返す
            return null;
        }
    }

    public async Task<int> CountAddressBooks(string? nameFilter = null, string? mailFilter = null)
    {
        var url = GetUriWithQueryParameters(GetUrl(_baseUri, "Count"), new() {
            { "nameFilter", nameFilter },
            { "mailFilter", mailFilter },
        });
        var count = await httpClient.GetFromJsonAsync<int>(url);
        return count;
    }

    public async Task<AddressBook> AddAddressBook(AddressBook addressBook)
    {
        var url = GetUrl(_baseUri);
        var response = await httpClient.PostAsJsonAsync(url, addressBook);
        var newAddressBook = await response.Content.ReadFromJsonAsync<AddressBook>();
        return newAddressBook!;
    }

    public async Task<bool> UpdateAddressBook(AddressBook addressBook)
    {
        var url = GetUrl(_baseUri, addressBook.Id);
        var response = await httpClient.PutAsJsonAsync(url, addressBook);
        return response.IsSuccessStatusCode;
    }

    public Task<bool> RemoveAddressBook(AddressBook addressBook)
    {
        return RemoveAddressBook(addressBook.Id);
    }

    public async Task<bool> RemoveAddressBook(int id)
    {
        var url = GetUrl(_baseUri, id);
        var response = await httpClient.DeleteAsync(url);
        return response.IsSuccessStatusCode;
    }
}