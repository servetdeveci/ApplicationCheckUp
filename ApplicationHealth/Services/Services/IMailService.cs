using ApplicationHealth.Domain.ViewModels;
using System.Threading.Tasks;

namespace ApplicationHealth.Services.Services
{
    public interface IMailService
    {
        Task<string> SendMailViaSystemNetAsync(Email emailParameter);
       
    }
}
