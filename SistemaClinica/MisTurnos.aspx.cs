using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using negocio;
using dominio;

namespace SistemaClinica
{
    public partial class MisTurnos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Seguridad.ValidarPagina();

            if (!IsPostBack)
            {
                cargarGrillaMedico();
            }
        }

        private void cargarGrillaMedico()
        {
            if (Session["UsuarioId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            int usuarioId = Convert.ToInt32(Session["UsuarioId"]);

            MedicoNegocio medicoNegocio = new MedicoNegocio();
            Medico medicoLogueado = medicoNegocio.buscarPorUsuarioId(usuarioId);

            if (medicoLogueado == null)
            {
                dgvTurnosMedico.DataSource = null;
                dgvTurnosMedico.DataBind();
                return;
            }

            TurnoNegocio turnoNegocio = new TurnoNegocio();
            List<Turno> listaCompleta = turnoNegocio.listar();

            List<Turno> listaFiltrada = listaCompleta
                .Where(x => x.Medico != null && x.Medico.Id == medicoLogueado.Id)
                .ToList();

            dgvTurnosMedico.DataSource = listaFiltrada;
            dgvTurnosMedico.DataBind();
        }

        protected void dgvTurnosMedico_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "NoAsistio")
            {
                int idTurno = Convert.ToInt32(e.CommandArgument);
                TurnoNegocio negocio = new TurnoNegocio();

                negocio.registrarAusencia(idTurno);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertAusencia", "alert('Se registró la inasistencia del paciente de forma exitosa.');", true);
                cargarGrillaMedico();
            }
        }

        // Acción del botón interno del Modal de Bootstrap
        protected void btnGuardarNota_Click(object sender, EventArgs e)
        {
            try
            {
                int idTurno = Convert.ToInt32(hfTurnoId.Value);
                string observacion = txtModalObservacion.Text.Trim();

                TurnoNegocio negocio = new TurnoNegocio();
                negocio.agregarObservacionMedico(idTurno, observacion);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertNota", "alert('Evolución médica guardada correctamente.');", true);
                cargarGrillaMedico();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx");
            }
        }
    }
}