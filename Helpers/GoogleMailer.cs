using System;
using System.Net;
using System.Net.Mail;

namespace HienlthOnline.Helpers
{
    public class GmailConfig
    {
        public string Email { get; set; }
        public string EmailName { get; set; }
        public string Password { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
    }

    public class GoogleMailer
    {
        private readonly GmailConfig _gmailConfig;
        public GoogleMailer(IOptions<GmailConfig> config)
        {
            _gmailConfig = config.Value;
        }

        public void Send(String to, String subject, String body)
        {
            String from = $"{_gmailConfig.EmailName} <{_gmailConfig.Email}>";
            Send(from, to, "", "", subject, body, "");
        }

        public void Send(String from, String to, String subject, String body)
        {
            Send(from, to, "", "", subject, body, "");
        }

        public void Send(String from, String to, String cc, String bcc, String subject, String body, String attachments)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            if (!String.IsNullOrEmpty(cc))
            {
                mail.CC.Add(cc);
            }

            if (!String.IsNullOrEmpty(bcc))
            {
                mail.Bcc.Add(bcc);
            }

            if (!String.IsNullOrEmpty(attachments))
            {
                String[] fileNames = attachments.Split(";,".ToCharArray());
                foreach (String fileName in fileNames)
                {
                    if (fileName.Trim().Length > 0)
                    {
                        mail.Attachments.Add(new Attachment(fileName.Trim()));
                    }
                }
            }

            SmtpClient client = new SmtpClient(_gmailConfig.SmtpHost, _gmailConfig.SmtpPort);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_gmailConfig.Email, _gmailConfig.Password);
            client.Send(mail);
        }
    }
}