using Microsoft.AspNetCore.Mvc;
using AddressBookSys.Models.Services;
using AddressBookSys.Models.Entities;

namespace AddressBookSys.App.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AddressBooksController(ILogger<AddressBooksController> logger, IAddressBookService addressBookService) : ControllerBase
{
    // 複数件取得
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AddressBook>>> Get(
        [FromQuery] string? nameFilter = null, 
        [FromQuery] string? mailFilter = null, 
        [FromQuery] int? skip = null, 
        [FromQuery] int? limit = null, 
        [FromQuery] bool sortByIdAscending = true
    )
    {
        return Ok(await addressBookService.GetAddressBooks(
            nameFilter: nameFilter,
            mailFilter: mailFilter,
            skip: skip,
            limit: limit,
            sortByIdAscending: sortByIdAscending
        ));
    }

    [HttpGet("Count")]
    public async Task<ActionResult<int>> GetCount(
        [FromQuery] string? nameFilter = null, 
        [FromQuery] string? mailFilter = null
    )
    {
        var count = await addressBookService.CountAddressBooks(nameFilter, mailFilter);
        return Ok(count);
    }

    // 1件取得
    [HttpGet("{id:int}")]
    public async Task<ActionResult<IEnumerable<AddressBook>>> Get(int id)
    {
        var addressBook = await addressBookService.GetAddressBook(id);
        if (addressBook == null)
        {
            return NotFound();
        }
        return Ok(addressBook);
    }

    // 追加
    [HttpPost]
    public async Task<ActionResult<AddressBook>> Post(AddressBook addressBook)
    {
        var newAddressBook = await addressBookService.AddAddressBook(addressBook);
        return CreatedAtAction(nameof(Get), new { id = newAddressBook.Id }, newAddressBook);
    }

    // 更新
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, AddressBook addressBook)
    {
        if (id != addressBook.Id)
        {
            return BadRequest();
        }
        var updated = await addressBookService.UpdateAddressBook(addressBook);
        if (updated) {
            return NoContent();
        } else {
            return NotFound();
        }
    }

    // 削除
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var removed = await addressBookService.RemoveAddressBook(id);
        if (removed) {
            return NoContent();
        } else {
            return NotFound();
        }
    }
}
