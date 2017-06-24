using EmpeekTest2.Infrastructure;
using EmpeekTest2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace EmpeekTest2.Controllers
{
    public class PetController : ApiController
    {
        private IDBService _DBservice;

        public PetController()
        {
            _DBservice = new DBService();
        }

        
        // GET api/pet/5
        public async Task<JsonResult<IEnumerable<Pet>>> Get(int id)
        {
            return Json(await _DBservice.GetPets(id));
        }
 
        // POST api/pet/5
        public async Task<JsonResult<IEnumerable<Pet>>> Post(int id, [FromBody]string petName)
        {
            await _DBservice.CreateNewPet(petName, id);
            return Json(await _DBservice.GetPets(id));
        }
          

        
        // DELETE api/pet/5
        [HttpDelete]
        public async void Delete(int id)
        {
            await _DBservice.DeletePet(id);
        }
    }
}