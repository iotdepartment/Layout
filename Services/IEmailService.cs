namespace Layout.Services
{
    public interface IEmailService
    {
        Task EnviarCorreoAsync(
            string destinatario,
            string asunto,
            string mensajeHtml);

        Task EnviarCorreoAsync(
            List<string> destinatarios,
            string asunto,
            string mensajeHtml);

        string ObtenerPlantilla(
            string nombrePlantilla);

        string ReemplazarVariables(
            string html,
            Dictionary<string, string> valores);
    }
}