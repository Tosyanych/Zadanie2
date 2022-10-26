using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Zadanie2.Views.Contact
{

    public class ContactController : Controller
    {
        private readonly AddressBookContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _user;
        private readonly bool _admin = false;       
                
        public ContactController(AddressBookContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.User?.Identity?.Name;
            if (_user == @"Domain\Admin")
                _admin = true;              
        }

        // GET: Contact
        public async Task<IActionResult> Index()
        {
            if(_admin)
                return View(await _context.AddressBookItems.ToListAsync());
            return NotFound();
        }

        // GET: Contact/Details/5
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
            if (_admin)
                return View(addressBookItem);
            return NotFound();
        }

        // GET: Contact/Create
        public IActionResult Create()
        {
            if(_admin)
                return View();
            return NotFound();
        }

        // POST: Contact/Create        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Birthday")] AddressBookItem addressBookItem)
        {
            if (ModelState.IsValid)
            {
                if (Regex.IsMatch(addressBookItem.Name, @"^[a-zа-яёA-ZА-ЯЁ][-\sa-zа-яёA-ZА-ЯЁ]*$"))
                {
                    if (Regex.IsMatch(addressBookItem.Email, @"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$"))
                    {
                        _context.Add(addressBookItem);
                        if (_admin)
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "Пожалуйста, введите действительный адрес электронной почты.");
                    }
                }
                else
                {
                    ModelState.AddModelError("Name", "Пожалуйста, вводите только буквы.");
                }
            }
            if(_admin)
                return View(addressBookItem);
            return NotFound();
        }

        // GET: Contact/Edit/5
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
            if(_admin)
                return View(addressBookItem);
            return NotFound();
        }

        // POST: Contact/Edit/5       
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
                if (Regex.IsMatch(addressBookItem.Name, @"^[a-zа-яёA-ZА-ЯЁ][-\sa-zа-яёA-ZА-ЯЁ]*$"))
                {
                    if (Regex.IsMatch(addressBookItem.Email, @"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$"))
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
                    else
                    {
                        ModelState.AddModelError("Email", "Пожалуйста, введите действительный адрес электронной почты.");
                    }
                }
                else
                {
                    ModelState.AddModelError("Name", "Пожалуйста, вводите только буквы.");
                }
            }
            if(_admin)
                return View(addressBookItem);
            return NotFound();
        }

        // GET: Contact/Delete/5
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
            if(_admin)
                return View(addressBookItem);
            return NotFound();
        }

        // POST: Contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var addressBookItem = await _context.AddressBookItems.FindAsync(id);
            _context.AddressBookItems.Remove(addressBookItem);
            await _context.SaveChangesAsync();
            if(_admin)
                return RedirectToAction(nameof(Index));
            return NotFound();
        }

        private bool AddressBookItemExists(long id)
        {
            return _context.AddressBookItems.Any(e => e.Id == id);
        }
    }
}
