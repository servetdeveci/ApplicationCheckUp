using ApplicationHealth.Domain.ViewModels;
using ApplicationHealth.Services.Services;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
namespace ApplicationHealth.Services.Managers
{

    public class MailManager : IMailService
    {
        public async Task<string> SendMailViaSystemNetAsync(Email emailModel)
        {
            string _retval = "+Ok";

            if (string.IsNullOrEmpty(emailModel.toList))
            {
                emailModel.toList = "";
            }

            if (string.IsNullOrEmpty(emailModel.cc))
            {
                emailModel.cc = "";
            }

            if (string.IsNullOrEmpty(emailModel.bcc))
            {
                emailModel.bcc = "";
            }

            try
            {
                SmtpClient _smtpClient = new SmtpClient
                {
                    Port = 587,
                    Host = "smtp.gmail.com",
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    EnableSsl = true,
                    Credentials = new NetworkCredential("checkhealth263@gmail.com", "hhPK3MCz-z-RCD3LvyPGjX$VDH62VQmr"),
                };

                MailMessage _Message = new MailMessage
                {
                    BodyEncoding = Encoding.GetEncoding("UTF-8"),
                    Body = emailModel.content,
                    IsBodyHtml = true,
                    From = new MailAddress("checkhealth263@gmail.com", "Application Healt Check"),
                };


                if (emailModel.toList.Trim().Length > 0)
                {
                    foreach (string _str in emailModel.toList.Split(';'))
                    {
                        if (!string.IsNullOrEmpty(_str))
                        {
                            _Message.To.Add(_str);

                        }
                    }
                }
                if (emailModel.cc.Trim().Length > 0)
                {
                    foreach (string _str in emailModel.cc.Split(';'))
                    {
                        if (!string.IsNullOrEmpty(_str))
                        {
                            _Message.CC.Add(_str);
                        }
                    }
                }
                if (emailModel.bcc.Trim().Length > 0)
                {
                    foreach (string _str in emailModel.bcc.Split(';'))
                    {
                        if (_str.Trim().Length > 0)
                        {
                            if (!string.IsNullOrEmpty(_str))
                            {
                                _Message.Bcc.Add(_str);
                            }
                        }
                    }
                }
                _Message.Subject = emailModel.subject;

                await _smtpClient.SendMailAsync(_Message);
            }
            catch (Exception ex)
            {
                _retval = "-Ok " + ex.Message;
            }
            return _retval;
        }
    }

}
