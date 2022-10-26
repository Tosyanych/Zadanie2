using System;
using System.ComponentModel.DataAnnotations;


namespace Zadanie2
{
    public partial class AddressBookItem
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
    }
}
