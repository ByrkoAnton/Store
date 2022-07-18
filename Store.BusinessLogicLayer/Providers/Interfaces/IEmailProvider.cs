using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises.Interfaces//TODO wrong namespace
{
    public interface IEmailProvider
    {
        public Task SendEmailAsync(string email, string subject, string message);   
    }
}
