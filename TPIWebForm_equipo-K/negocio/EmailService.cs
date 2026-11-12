using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace negocio
{
    public class EmailService
    {
        private SmtpClient server;
        private MailMessage email;

        public EmailService()
        {
            // ACÁ VAN TUS CREDENCIALES REALES DE MAILTRAP
            server = new SmtpClient("sandbox.smtp.mailtrap.io", 2525);
            server.Credentials = new NetworkCredential("be6fa5df8d3e8d", "07d9e89d55ba86");
            server.EnableSsl = true;
        }

        public void armarCorreo(string emailDestino, string asunto, string cuerpo)
        {
            email = new MailMessage();
            email.From = new MailAddress("no-reply@ecommerce.com");
            email.To.Add(emailDestino);
            email.Subject = asunto;
            email.IsBodyHtml = true;
            email.Body = cuerpo;
        }

        public void enviarEmail()
        {
            try
            {
                server.Send(email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
