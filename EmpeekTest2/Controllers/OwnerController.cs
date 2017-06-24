using EmpeekTest2.Infrastructure;
using EmpeekTest2.Models;
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
        public async Task<JsonResult<IEnumerable<Owner>>> Get()
        {
            return Json(await _DBservice.GetOwners());
        }
        

        // POST api/owner
        public async Task<JsonResult<IEnumerable<Owner>>> Post([FromBody]string ownerName)
        {
            await _DBservice.CreateNewOwner(ownerName);
            return Json(await _DBservice.GetOwners());
        }


        // DELETE api/owner/5
        [HttpDelete]
        public async void Delete(int id)
        {
            await _DBservice.DeleteOwner(id);
        }
        
    }
}
