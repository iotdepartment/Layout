using System.Net.Mail;

namespace Layout.Services
{
    public class EmailService : IEmailService
    {
        private const string SMTP_SERVER =
            "kysmtp.tggroup.local";

        private const int SMTP_PORT = 25;

        private const string REMITENTE =
            "No-replayLayoutSystem@TGRMX.com";

        public async Task EnviarCorreoAsync(
            string destinatario,
            string asunto,
            string mensajeHtml)
        {
            using var mensaje = new MailMessage();

            mensaje.From = new MailAddress(
                REMITENTE,
                "Sistema de Movimientos de Layout");

            mensaje.To.Add(destinatario);

            mensaje.Subject = asunto;

            mensaje.Body = mensajeHtml;

            mensaje.IsBodyHtml = true;

            using var smtp =
                new SmtpClient(
                    SMTP_SERVER,
                    SMTP_PORT);

            smtp.EnableSsl = false;

            await smtp.SendMailAsync(mensaje);
        }

        public async Task EnviarCorreoAsync(
            List<string> destinatarios,
            string asunto,
            string mensajeHtml)
        {
            using var mensaje = new MailMessage();

            mensaje.From = new MailAddress(
                REMITENTE,
                "Sistema de Movimientos de Layout");

            foreach (var correo in destinatarios)
            {
                mensaje.To.Add(correo);
            }

            mensaje.Subject = asunto;

            mensaje.Body = mensajeHtml;

            mensaje.IsBodyHtml = true;

            using var smtp =
                new SmtpClient(
                    SMTP_SERVER,
                    SMTP_PORT);

            smtp.EnableSsl = false;

            await smtp.SendMailAsync(mensaje);
        }

        public string ObtenerPlantilla(
            string nombrePlantilla)
        {
            var ruta = Path.Combine(
                Directory.GetCurrentDirectory(),
                "EmailTemplates",
                nombrePlantilla);

            if (!File.Exists(ruta))
            {
                throw new FileNotFoundException(
                    $"No se encontró la plantilla: {ruta}");
            }

            return File.ReadAllText(ruta);
        }

        public string ReemplazarVariables(
            string html,
            Dictionary<string, string> valores)
        {
            foreach (var item in valores)
            {
                html = html.Replace(
                    $"{{{{{item.Key}}}}}",
                    item.Value ?? string.Empty);
            }

            return html;
        }
    }
}