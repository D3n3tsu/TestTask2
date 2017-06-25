using EmpeekTest2.Infrastructure;
using EmpeekTest2.Models;
using EmpeekTest2.Models.ViewModels;
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
        public async Task<JsonResult<PetsViewModel>> Get(int id)
        {
            IEnumerable<Pet> pets = await _DBservice.GetPets(id);
            return Json(new PetsViewModel
            {
                NumberOfPets = pets.Count(),
                Pets = pets
            });
        }
 
        // POST api/pet
        public async Task<JsonResult<PetsViewModel>> Post([FromBody]PetPostViewModel data)
        {
            await _DBservice.CreateNewPet(data.newPet, data.ownerId);
            IEnumerable<Pet> pets = await _DBservice.GetPets(data.ownerId);
            return Json(new PetsViewModel
            {
                NumberOfPets = pets.Count(),
                Pets = pets
            });
        }
          
        
        // DELETE api/pet/5
        [HttpDelete]
        public async void Delete(int id)
        {
            await _DBservice.DeletePet(id);
        }
    }
}