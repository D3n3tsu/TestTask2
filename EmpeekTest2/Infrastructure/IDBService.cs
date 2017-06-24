using EmpeekTest2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpeekTest2.Infrastructure
{
    public interface IDBService
    {
        Task CreateNewOwner(string newOwner);

        Task CreateNewPet(string petName, int ownerId);

        Task DeleteOwner(int ownerId);

        Task DeletePet(int petId);

        Task<IEnumerable<Owner>> GetOwners();

        Task<IEnumerable<Pet>> GetPets(int ownerId);
    }
}
