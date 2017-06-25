using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmpeekTest2.Models.ViewModels
{
    public class PetsViewModel
    {
        public int NumberOfPets { get; set; }

        public IEnumerable<Pet> Pets { get; set; }
    }
}