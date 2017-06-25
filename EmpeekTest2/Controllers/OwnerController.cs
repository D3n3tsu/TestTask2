using EmpeekTest2.Infrastructure;
using EmpeekTest2.Models;
using EmpeekTest2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Results;

namespace EmpeekTest2.Controllers
{
    public class OwnerController : ApiController
    {
        private IDBService _DBservice;

        public OwnerController()
        {
            _DBservice = new DBService();
        }

        // GET api/owner
        public async Task<JsonResult<OwnersViewModel>> Get()
        {
            IEnumerable<Owner> owners = await _DBservice.GetOwners();
            return Json(new OwnersViewModel
            {
                NumberOfOwners = owners.Count(),
                Owners = owners
            });
        }
        

        // POST api/owner
        [HttpPost]
        public async Task<JsonResult<OwnersViewModel>> Post([FromBody]OwnerPostViewModel data)
        {
            await _DBservice.CreateNewOwner(data.newOwner);
            IEnumerable<Owner> owners = await _DBservice.GetOwners();
            return Json(new OwnersViewModel
            {
                NumberOfOwners = owners.Count(),
                Owners = owners
            });
        }


        // DELETE api/owner/5
        [HttpDelete]
        public async void Delete(int id)
        {
            await _DBservice.DeleteOwner(id);
        }
        
    }
}
