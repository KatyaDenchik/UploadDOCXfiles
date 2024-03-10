using ServiceLayer.SubModels;

namespace ServiceLayer.Services.Abstract
{
    public interface IEmailServices
    {
        public void SendEmail(EmailInformation emailInformation);
    }
}
