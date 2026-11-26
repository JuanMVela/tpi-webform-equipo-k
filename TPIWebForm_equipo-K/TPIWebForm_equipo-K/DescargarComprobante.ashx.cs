using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using dominio;
using negocio;

namespace TPIWebForm_equipo_K
{
    /// <summary>
    /// Descripción breve de DescargarComprobante
    /// </summary>
    public class DescargarComprobante : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.Cache.SetNoStore();

            Usuario usuario =
                context.Session["UsuarioLogueado"] as Usuario;

            if (usuario == null)
            {
                context.Response.StatusCode = 401;
                context.Response.Write("Debés iniciar sesión.");
                return;
            }

            int idComprobante;

            try
            {
                idComprobante = int.Parse(context.Request.QueryString["id"]);
            }
            catch (Exception ex) when (
                ex is FormatException ||
                ex is OverflowException ||
                ex is ArgumentNullException)
            {
                context.Response.StatusCode = 400;
                context.Response.Write("El comprobante indicado no es válido.");
                return;
            }

            try
            {
                UsuarioNegocio usuarios = new UsuarioNegocio();

                if (!usuarios.esAdministradorActivo(usuario.Id))
                {
                    context.Response.StatusCode = 403;
                    context.Response.Write("No tenés permisos.");
                    return;
                }

                ComprobantePagoNegocio negocio =
                    new ComprobantePagoNegocio();

                var comprobante = negocio.obtenerArchivoAdministrador(
                    idComprobante, usuario.Id);

                if (comprobante == null)
                {
                    context.Response.StatusCode = 404;
                    context.Response.Write("El comprobante no está disponible.");
                    return;
                }

                // Elegimos una extensión conocida para la descarga.
                string extension;

                switch (comprobante.TipoContenido)
                {
                    case "application/pdf":
                        extension = ".pdf";
                        break;

                    case "image/jpeg":
                        extension = ".jpg";
                        break;

                    case "image/png":
                        extension = ".png";
                        break;

                    default:
                        throw new Exception("Formato de archivo no permitido.");
                }

                context.Response.ContentType = comprobante.TipoContenido;
                context.Response.AddHeader(
                    "X-Content-Type-Options", "nosniff");

                context.Response.AddHeader(
                    "Content-Disposition",
                    "attachment; filename=\"Comprobante-" +
                    comprobante.Id + extension + "\"");

                context.Response.BinaryWrite(comprobante.Archivo);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());

                context.Response.Clear();
                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/plain";
                context.Response.Write(
                    "No pudimos descargar el comprobante.");
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}