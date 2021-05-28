using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises.Interfaces
{
    public interface IEmailProvider
    {
        public Task SendEmailAsync(string email, string subject, string message);   
    }
}
