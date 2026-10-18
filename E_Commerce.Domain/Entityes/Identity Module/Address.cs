using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entityes.Identity_Module
{
    public class Address
    {
        public int Id { get; set; }
        public string City { get; set; }=default!;
        public string Street { get; set; }=default!;
        public string Country { get; set; }=default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        //Each Address has Application User  1-1=> Take Pk of Partial as Fk in Total
        public ApplicationUser User { get; set; } = default!;
        public string UserId { get; set; } = default!; 
    }
}
