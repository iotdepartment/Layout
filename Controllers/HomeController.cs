using Layout.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Mail;

namespace Layout.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        

public async Task<IActionResult> ProbarCorreo()
    {
        try
        {
            var mail = new MailMessage();

            mail.From = new MailAddress(
                "No-replayIoTDepartment@TGRMX.com");

            mail.To.Add(
                "luis.villarreal@toyodagosei.com");

            mail.Subject = "Prueba LAYOUT";

            mail.Body =
                "Si recibes este correo, el SMTP funciona correctamente.";

            using var smtp =
                new SmtpClient(
                    "kysmtp.tggroup.local",
                    25);

            smtp.EnableSsl = false;

            await smtp.SendMailAsync(mail);

            return Content(
                "Correo enviado correctamente");
        }
        catch (Exception ex)
        {
            return Content(
                $"Error: {ex.Message}");
        }
    }

    public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
