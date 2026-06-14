using Demo.DAL.Models;
using System.Net.Mail;
using System.Net;

namespace Demp.PL.Helpers
{
	public static class EmailSettings
	{
		public static void SendEmail(Email email)
		{
			var Client = new SmtpClient("smtp.gmail.com", 587);
			Client.EnableSsl = true;
			Client.Credentials = new NetworkCredential("mostafayasseen33@gmail.com", "dcofxsgeejwidjgh");
			Client.Send("mostafayasseen33@gmail.com", email.To, email.Subject, email.Body);

		}
	}
}
