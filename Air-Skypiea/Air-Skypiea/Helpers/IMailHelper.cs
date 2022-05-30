using Air_Skypiea.Common;

namespace Air_Skypiea.Helpers
{
    public interface IMailHelper
    {
        Response SendMail(string toName, string toEmail, string subject, string body);

    }
}
