using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Providers.Interfaces
{
    public interface IRandomPasswordGeneratorProvider
    {
        public string GenerateRandomPassword ();
    }
}
