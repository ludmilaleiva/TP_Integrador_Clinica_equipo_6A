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
    public partial class ListadoTurnos : System.Web.UI.Page
    {
        // Guardamos la lista en la sesión para poder filtrarla sin volver a la base de datos cada vez
        private List<Turno> listaOriginal
        {
            get
            {
                if (Session["ListaTurnos"] == null)
                {
                    TurnoNegocio negocio = new TurnoNegocio();
                    Session["ListaTurnos"] = negocio.listar();
                }
                return (List<Turno>)Session["ListaTurnos"];
            }
            set { Session["ListaTurnos"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                cargarGrilla();
            }
        }

        private void cargarGrilla()
        {
            //la grilla con los datos que vienen de la capa de negocio
            dgvTurnos.DataSource = listaOriginal;
            dgvTurnos.DataBind();
        }

        // Accion del botón Buscar / Filtrar
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            filtrarGrilla();
        }

        // Filtro dinámico al escribir en el TextBox
        protected void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            filtrarGrilla();
        }

        private void filtrarGrilla()
        {
            string filtro = txtFiltro.Text.Trim().ToLower();

            if (!string.IsNullOrEmpty(filtro))
            {
                // Filtra por Apellido del Paciente, Apellido del Médico o Número de Turno
                var listaFiltrada = listaOriginal.Where(x =>
                    x.Paciente.Apellido.ToLower().Contains(filtro) ||
                    x.Medico.Apellido.ToLower().Contains(filtro) ||
                    x.Numero.ToLower().Contains(filtro)
                ).ToList();

                dgvTurnos.DataSource = listaFiltrada;
            }
            else
            {
                dgvTurnos.DataSource = listaOriginal;
            }
            dgvTurnos.DataBind();
        }

        // Botón para resetear la búsqueda
        protected void btnLimpiarFiltro_Click(object sender, EventArgs e)
        {
            txtFiltro.Text = "";
            // Forzamos a que vuelva a consultar la BD por si se agregaron turnos nuevos
            Session["ListaTurnos"] = null;
            cargarGrilla();
        }

        // Manejo de eventos de las filas (como el botón Cancelar)
        protected void dgvTurnos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // 1. Si es Reprogramar, redirigimos directamente antes de parsear nada más
            if (e.CommandName == "Reprogramar")
            {
                int idTurno = Convert.ToInt32(e.CommandArgument);
                Response.Redirect($"AsignarTurno.aspx?reprogramarId={idTurno}");
                return;
            }

            try
            {
                int idTurno = Convert.ToInt32(e.CommandArgument);
                TurnoNegocio negocio = new TurnoNegocio();

                // Sincronizado con "Cancelar" que es el CommandName de tu HTML
                if (e.CommandName == "Cancelar")
                {
                    negocio.cancelar(idTurno);

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('Turno N° {idTurno} cancelado con éxito.');", true);

                    Session["ListaTurnos"] = null;
                    cargarGrilla();
                }
                // Procesa "NoAsistio" e impacta el cambio
                else if (e.CommandName == "NoAsistio")
                {
                    // ALERT DE PRUEBA: Si ves este cartel, significa que el evento RowCommand está llegando correctamente acá
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertTest", $"alert('¡Llegó al bloque NoAsistio! ID del turno: {idTurno}');", true);

                    // Ejecuta el UPDATE a EstadoId = 4
                    negocio.registrarAusencia(idTurno);

                    // Limpia la sesión para actualizar el Badge a gris al instante
                    Session["ListaTurnos"] = null;
                    cargarGrilla();
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }
    }
}