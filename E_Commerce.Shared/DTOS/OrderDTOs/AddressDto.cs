using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.DTOS.OrderDTOs
{
    public record AddressDto
    {
        public string FirstName { get; init; }//this Property Will Be immutable يعنى بيتعملها setting اول مرة فقط وبعد كدة خلاص مش هقدر اعمل setting عليها تانى لانها بقت immutable
        public string LastName { get; init; }
        public string Street { get; init; }
        public string City { get; init; }
        public string Country { get; init; }
        //يبقى فى طريقيتن انى اخلى DTO has Property Immutable

    }

    //Anthore Way To Make DTO Has Property Immutable
    //public class AddressDto(string FirstName, string LastName, string Street, string City, string Country);
    }
