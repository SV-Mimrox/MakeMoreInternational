using MakeMoreInternational.Models;
using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Text;

namespace MakeMoreInternational.Controllers
{
    public class InquiryController : BaseController
    {
        private readonly InquiryService _inqService;
        public InquiryController(WebSettingService service, 
            CategoryService catService, InquiryService inqService, PageSEOService seoService, LanguageService languageService) 
            : base(service, catService, seoService, languageService)
        {
            _inqService = inqService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("/inquiry/submit")]
        public IActionResult Index(InquiryMaster inq)
        {
            try
            {
                _inqService.Create(inq);
                StringBuilder messages = new StringBuilder();
                MailAddress from = new MailAddress("makemoreinternational@gmail.com", "Makemore International LLP");
                messages.Append("<h4>Inquiry for Makemore</h4>" + "\n");
                messages.Append("Name: " + inq.inqName + "<br/>\n");
                messages.Append("Email: " + inq.inqEmail + "<br/>\n");
                messages.Append("Mobile No: " + inq.inqContact + "<br/>\n\n");
                messages.Append("Subject: " + inq.inqSubject + "<br/>\n\n");
                messages.Append(inq.inqMessage);
                MailMessage mail = new MailMessage();

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                mail.IsBodyHtml = true;
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("makemoreinternational@gmail.com", "rcrihyjhnczmkoav");


                smtp.Credentials = credentials;
                smtp.EnableSsl = true;
                mail.From = from;

                mail.To.Add("info@makemoreinternational.com");
                mail.Subject = "Website Inquiry";
                mail.Body = messages.ToString();
                smtp.Send(mail);
                return Json("Success");
            }
            catch (Exception ex)
            {
                return Json("Error Occurred, Please try again");
            }
        }
    }
}
