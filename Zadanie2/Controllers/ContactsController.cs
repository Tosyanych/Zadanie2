using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Zadanie2.Views.Contacts
{
    public class ContactsController : Controller
    {
        private readonly AddressBookContext _context;        

        public ContactsController(AddressBookContext context)
        {
            _context = context;           
        }

        // GET: Contacts
        public async Task<IActionResult> Index(string Search = "")
        {            
            if(Search != "" && Search != null)
            {                
                var Item = _context.AddressBookItems.Where(item => item.Name.Contains(Search) ? item.Name.Contains(Search) : item.Email.Contains(Search)).ToList();
                if (Item.Count == 0)
                {
                    DateTime DateSearch;
                    DateTime.TryParse(Search, out DateSearch);                    
                    return View(await _context.AddressBookItems.Where(p => Convert.ToString(p.Birthday).Contains(Search)).ToListAsync());                    
                }
                else                    
                    return View(await _context.AddressBookItems.Where(item => item.Name.Contains(Search) ? item.Name.Contains(Search) : item.Email.Contains(Search)).ToListAsync());

            }
            else
                return View(await _context.AddressBookItems.ToListAsync());
        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var addressBookItem = await _context.AddressBookItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (addressBookItem == null)
            {
                return NotFound();
            }

            return View(addressBookItem);
        }

        // GET: Contacts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Birthday")] AddressBookItem addressBookItem)
        {
            if (ModelState.IsValid)
            {                
                _context.Add(addressBookItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(addressBookItem);
        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var addressBookItem = await _context.AddressBookItems.FindAsync(id);
            if (addressBookItem == null)
            {
                return NotFound();
            }
            return View(addressBookItem);
        }

        // POST: Contacts/Edit/5       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Email,Birthday")] AddressBookItem addressBookItem)
        {
            if (id != addressBookItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(addressBookItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressBookItemExists(addressBookItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(addressBookItem);
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var addressBookItem = await _context.AddressBookItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (addressBookItem == null)
            {
                return NotFound();
            }

            return View(addressBookItem);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var addressBookItem = await _context.AddressBookItems.FindAsync(id);
            _context.AddressBookItems.Remove(addressBookItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AddressBookItemExists(long id)
        {
            return _context.AddressBookItems.Any(e => e.Id == id);
        }
    }
}
