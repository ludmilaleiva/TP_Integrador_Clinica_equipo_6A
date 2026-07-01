using System;
using System.Collections.Generic;
using System.Web;

namespace SistemaClinica
{
    public static class Seguridad
    {
        public const int ADMINISTRADOR = 1;
        public const int RECEPCIONISTA = 2;
        public const int MEDICO = 3;

        private static readonly Dictionary<string, int[]> permisos = new Dictionary<string, int[]>
        {
            // Admin ve todo
            { "Default.aspx", new[] { ADMINISTRADOR, RECEPCIONISTA, MEDICO } },

            // Recepcionista
            { "Pacientes.aspx", new[] { ADMINISTRADOR, RECEPCIONISTA } },
            { "Medicos.aspx", new[] { ADMINISTRADOR, RECEPCIONISTA } },
            { "AsignarTurno.aspx", new[] { ADMINISTRADOR, RECEPCIONISTA } },
            { "ListadoTurnos.aspx", new[] { ADMINISTRADOR, RECEPCIONISTA } },
            { "TurnoTrabajo.aspx", new[] { ADMINISTRADOR, RECEPCIONISTA } },

            // Médico
            { "MisTurnos.aspx", new[] { ADMINISTRADOR, MEDICO } }
        };

        public static void ValidarPagina()
        {
            if (HttpContext.Current.Session["UsuarioId"] == null)
            {
                HttpContext.Current.Response.Redirect("~/Login.aspx");
                return;
            }

            int perfilId = Convert.ToInt32(HttpContext.Current.Session["PerfilId"]);
            string paginaActual = System.IO.Path.GetFileName(
                HttpContext.Current.Request.Url.AbsolutePath
            );

            if (perfilId == ADMINISTRADOR)
                return;

            if (!permisos.ContainsKey(paginaActual))
            {
                HttpContext.Current.Response.Redirect("~/SinPermiso.aspx");
                return;
            }

            if (Array.IndexOf(permisos[paginaActual], perfilId) == -1)
            {
                HttpContext.Current.Response.Redirect("~/SinPermiso.aspx");
            }
        }
    }
}