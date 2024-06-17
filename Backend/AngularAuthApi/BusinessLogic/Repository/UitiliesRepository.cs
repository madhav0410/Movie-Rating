using BusinessLogic.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class UitiliesRepository : IUtilities
    {
        public string EncryptEmail(string plainBytes, string Key)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(plainBytes);

            using (Aes aesAlgo = Aes.Create())
            {
                aesAlgo.Key = Encoding.UTF8.GetBytes(Key);


                aesAlgo.IV = new byte[16];

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, aesAlgo.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(bytes, 0, bytes.Length);
                        csEncrypt.FlushFinalBlock();
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray()).Replace("+", "-").Replace("/", "_").Replace("=", "");

                }
            }
        }



        public string DecryptEmail(string encryptedEmail, string Key)
        {
            string paddedData = encryptedEmail;

            // Check if padding is needed
            if (paddedData.Length % 4 != 0)
            {
                paddedData += new string('=', 4 - (paddedData.Length % 4));
            }

            byte[] encryptedBytes = Convert.FromBase64String(paddedData.Replace("-", "+").Replace("_", "/"));

            using (Aes aesAlgo = Aes.Create())
            {
                aesAlgo.Key = Encoding.UTF8.GetBytes(Key)
;
                aesAlgo.IV = new byte[16];

                using (MemoryStream msDecrypt = new MemoryStream())
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, aesAlgo.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        csDecrypt.Write(encryptedBytes, 0, encryptedBytes.Length);
                        csDecrypt.FlushFinalBlock();
                    }
                    return Encoding.UTF8.GetString(msDecrypt.ToArray());
                }
            }

        }


        public async Task SendMail(string toEmail, string subject, string emailbody)
        {
            try
            {

                //mail.etatvasoft.com
                using (SmtpClient smtpClient = new SmtpClient("sandbox.smtp.mailtrap.io"))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential("25e728a38d10e4", "6695ae083d46e0");
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;

                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress("madhav.parmar@etatvasoft.com");
                        mailMessage.To.Add(new MailAddress(toEmail));
                        mailMessage.Subject = subject;
                        mailMessage.IsBodyHtml = true;
                        mailMessage.Body = emailbody;
                        await Task.Run(() => smtpClient.Send(mailMessage));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to send email", ex);
            }
        }
    }
}
