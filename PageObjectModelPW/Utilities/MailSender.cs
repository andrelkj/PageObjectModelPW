using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace PageObjectModelPW.Utilities
{
    class MailSender
    {

        public static void SendEmail(string fromEmail, string password, List<string> toEmails, string subject, string body, string attachmentPath)
        {


            // Mail message
            var message = new MailMessage();
            message.From = new MailAddress(fromEmail);

            // Add recipients
            foreach (string toEmail in toEmails)
            {
                message.To.Add(toEmail);
            }

            message.Subject = subject;
            message.Body = body;

            // Attach file if provided
            if (!string.IsNullOrEmpty(attachmentPath))
            {
                Attachment attachment = new Attachment(attachmentPath, MediaTypeNames.Application.Octet);
                message.Attachments.Add(attachment);
            }

            // SMTP client
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(fromEmail, password);

            try
            {
                // Send the email
                smtpClient.Send(message);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
            finally
            {
                // Dispose of attachments
                foreach (var attachment in message.Attachments)
                {
                    attachment.Dispose();
                }
            }
        }
        }
}
