using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Constracts
{
    public interface IDataSeeding
    {
        //Functions Check If Seeding Is Succes of Fail
       Task  IntializeAsync();
    }
}
