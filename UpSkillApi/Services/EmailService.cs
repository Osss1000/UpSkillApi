using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

public static class EmailService
{
    public static async Task SendAsync(string toEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("UpSkill Team", "your_email@gmail.com"));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;

        var builder = new BodyBuilder
        {
            HtmlBody = $"<p>{body}</p>"
        };
        message.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync("upskillteam01@gmail.com", "bmtm zymv dhhu abug");
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}