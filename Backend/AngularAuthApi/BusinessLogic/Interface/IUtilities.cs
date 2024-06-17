using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interface
{
    public interface IUtilities
    {
        public string EncryptEmail(string plainBytes, string Key);
        public string DecryptEmail(string encryptedEmail, string Key);
        public Task SendMail(string toEmail, string subject, string emailbody);
    }
}
