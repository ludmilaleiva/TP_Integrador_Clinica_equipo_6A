using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using dominio;
using negocio;

namespace SistemaClinica
{
    public partial class TurnosTrabajo : System.Web.UI.Page
    {
        private TurnoTrabajoNegocio negocio = new TurnoTrabajoNegocio();

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
            try
            {
                dgvTurnosTrabajo.DataSource = negocio.listar();
                dgvTurnosTrabajo.DataBind();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                TurnoTrabajo nuevo = new TurnoTrabajo();
                nuevo.Nombre = txtNombre.Text;
                nuevo.HoraEntrada = TimeSpan.Parse(txtHoraEntrada.Text);
                nuevo.HoraSalida = TimeSpan.Parse(txtHoraSalida.Text);
                nuevo.Descripcion = txtDescripcion.Text;

                if (!string.IsNullOrEmpty(hfIdTurnoTrabajo.Value))
                {
                    nuevo.Id = Convert.ToInt32(hfIdTurnoTrabajo.Value);
                    negocio.modificar(nuevo);
                }
                else
                {
                    negocio.agregar(nuevo);
                }

                limpiarFormulario();
                cargarGrilla();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void dgvTurnosTrabajo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditarTurno")
            {
                int idSeleccionado = Convert.ToInt32(e.CommandArgument);
                hfIdTurnoTrabajo.Value = idSeleccionado.ToString();

                TurnoTrabajo seleccionado = negocio.listar().Find(x => x.Id == idSeleccionado);

                if (seleccionado != null)
                {
                    txtNombre.Text = seleccionado.Nombre;
                    txtHoraEntrada.Text = seleccionado.HoraEntrada.ToString(@"hh\:mm");
                    txtHoraSalida.Text = seleccionado.HoraSalida.ToString(@"hh\:mm");
                    txtDescripcion.Text = seleccionado.Descripcion;
                    litTituloForm.Text = "Editar Turno N° " + idSeleccionado;
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiarFormulario();
        }

        private void limpiarFormulario()
        {
            hfIdTurnoTrabajo.Value = "";
            txtNombre.Text = "";
            txtHoraEntrada.Text = "";
            txtHoraSalida.Text = "";
            txtDescripcion.Text = "";
            litTituloForm.Text = "Nuevo Turno de Trabajo";
        }
    }
}