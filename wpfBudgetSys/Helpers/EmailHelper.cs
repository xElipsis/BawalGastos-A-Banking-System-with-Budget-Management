using MailKit.Security;
using MailKit.Net.Smtp; 
using MimeKit;
using Org.BouncyCastle.Tls;

namespace wpfBudgetSys.Helpers
{
    public class EmailHelper
    {
        private const string SenderEmail = "xianreel2006@gmail.com";
        private const string SenderPassword = "onqh qyep jepl momk"; // Gmail App Password
        private const string SenderName = "Bawal Gastos";

        public static void SendOtp(string recipientEmail, string otpCode)
        {
            MimeMessage email = new MimeMessage();

            email.From.Add(new MailboxAddress(SenderName, SenderEmail));
            email.To.Add(new MailboxAddress("", recipientEmail));
            email.Subject = "Your Bawal Gastos OTP Code";

            email.Body = new TextPart("html")
            {
                Text = $@"
                <h2>Your OTP Code</h2>
                <p>Use the code below to complete your registration:</p>
                <h1 style='letter-spacing: 8px;'>{otpCode}</h1>
                <p>This code expires in <strong>5 minutes</strong>.</p>
                <p>If you did not request this, please ignore this email.</p>
            "
            };

            using SmtpClient smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(SenderEmail, SenderPassword);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public static void SendEmail(string recipientEmail, string subject, string body)
        {
            MimeMessage email = new MimeMessage();

            email.From.Add(new MailboxAddress(SenderName, SenderEmail));
            email.To.Add(new MailboxAddress("", recipientEmail));
            email.Subject = subject;

            email.Body = new TextPart("html")
            {
                Text = body
            };

            using SmtpClient smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(SenderEmail, SenderPassword);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
