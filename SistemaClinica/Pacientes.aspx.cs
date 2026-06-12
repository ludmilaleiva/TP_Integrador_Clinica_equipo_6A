using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using negocio;

namespace SistemaClinica
{
    public partial class Pacientes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Solo cargamos la grilla la primera vez que se entra a la página (evita recargas molestas)
            if (!IsPostBack)
            {
                try
                {
                    PacienteNegocio negocio = new PacienteNegocio();

                    // Vinculamos la lista de la BD directamente con el control visual GridView
                    dgvPacientes.DataSource = negocio.listar();
                    dgvPacientes.DataBind();
                }
                catch (Exception ex)
                {
                    // Si falla (porque todavía no reconfiguramos la BD local), atrapamos el error para que no explote
                    Session.Add("Error", ex.ToString());
                }
            }
        }
    }
}