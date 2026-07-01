using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SistemaClinica
{
    public partial class Maestra : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

            Response.Redirect("Login.aspx");
        }

        private void configurarMenuPorPerfil()
        {
            if (Session["PerfilId"] == null)
                return;

            int perfilId = Convert.ToInt32(Session["PerfilId"]);

            liPacientes.Visible = false;
            liMedicos.Visible = false;
            liAsignarTurno.Visible = false;
            liListadoTurnos.Visible = false;
            liTurnoTrabajo.Visible = false;
            liMisTurnos.Visible = false;

            if (perfilId == 1) // Administrador
            {
                liPacientes.Visible = true;
                liMedicos.Visible = true;
                liAsignarTurno.Visible = true;
                liListadoTurnos.Visible = true;
                liTurnoTrabajo.Visible = true;
                liMisTurnos.Visible = true;
            }
            else if (perfilId == 2) // Recepcionista
            {
                liPacientes.Visible = true;
                liMedicos.Visible = true;
                liAsignarTurno.Visible = true;
                liListadoTurnos.Visible = true;
                liTurnoTrabajo.Visible = true;
            }
            else if (perfilId == 3) // Médico
            {
                liMisTurnos.Visible = true;
            }
        }
    }

    
}