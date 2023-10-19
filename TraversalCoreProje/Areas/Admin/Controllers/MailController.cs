using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using TraversalCoreProje.Models;

namespace TraversalCoreProje.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MailController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(MailRequest mailRequest)
        {
            MimeMessage mimeMessage = new MimeMessage();
            MailboxAddress mailboxAddressFrom = new MailboxAddress("Admin", "btgmrphantaso91@gmail.com"); //gönderen mail adresi
            mimeMessage.From.Add(mailboxAddressFrom);
            MailboxAddress mailboxAddressTo = new MailboxAddress("User", mailRequest.ReceiverMail); //Alıcı mail adresi
            mimeMessage.To.Add(mailboxAddressTo);
            mimeMessage.Subject = mailRequest.Subject;
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = mailRequest.Body;
            mimeMessage.Body = bodyBuilder.ToMessageBody();
            SmtpClient client = new SmtpClient();
            client.Connect("smtp.gmail.com", 587, false);
            client.Authenticate("btgmrphantaso91@gmail.com", "axya nzuu rsrn pgwf"); // 2 faktörlü doğrulamayı açtıktan sonra uygulama şifresi gibi bir kısımdan 16 harfli kod alman gerekecek. o kodu 2.kısma yazacaksın.  1. kısım zaten gönderen maili ile aynı olacak.
            client.Send(mimeMessage);
            client.Disconnect(true);
            
            return View();
        }
    }
}
