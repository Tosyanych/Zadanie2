using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zadanie2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressBookItemsController : ControllerBase
    {
        private readonly AddressBookContext _context;

        public AddressBookItemsController(AddressBookContext context)
        {
            _context = context;
        }

        // GET: api/AddressBookItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddressBookItem>>> GetAddressBookItems()
        {
            return await _context.AddressBookItems.ToListAsync();
        }

        // GET: api/AddressBookItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AddressBookItem>> GetAddressBookItem(long id)
        {
            var addressBookItem = await _context.AddressBookItems.FindAsync(id);

            if (addressBookItem == null)
            {
                return NotFound();
            }

            return addressBookItem;
        }

        // PUT: api/AddressBookItems/5       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAddressBookItem(long id, AddressBookItem addressBookItem)
        {
            if (id != addressBookItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(addressBookItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressBookItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/AddressBookItems        
        [HttpPost]
        public async Task<ActionResult<AddressBookItem>> PostAddressBookItem(AddressBookItem addressBookItem)
        {
            _context.AddressBookItems.Add(addressBookItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAddressBookItem", new { id = addressBookItem.Id }, addressBookItem);
        }

        // DELETE: api/AddressBookItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AddressBookItem>> DeleteAddressBookItem(long id)
        {
            var addressBookItem = await _context.AddressBookItems.FindAsync(id);
            if (addressBookItem == null)
            {
                return NotFound();
            }

            _context.AddressBookItems.Remove(addressBookItem);
            await _context.SaveChangesAsync();

            return addressBookItem;
        }

        private bool AddressBookItemExists(long id)
        {
            return _context.AddressBookItems.Any(e => e.Id == id);
        }
    }
}
