using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmpeekTest2.Models.ViewModels
{
    public class OwnersViewModel
    {
        public int NumberOfOwners { get; set; }

        public IEnumerable<Owner> Owners { get; set; }
    }
}