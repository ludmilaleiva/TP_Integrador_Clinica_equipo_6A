using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using dominio;
using negocio;

namespace SistemaClinica
{
    public partial class Medicos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarDesplegable();
                cargarGrilla();
            }
        }

        private void cargarDesplegable()
        {
            EspecialidadNegocio negocioEsp = new EspecialidadNegocio();
            ddlEspecialidad.DataSource = negocioEsp.listar(); // Asegurate de que tu EspecialidadNegocio tenga un método listar()
            ddlEspecialidad.DataTextField = "Nombre";
            ddlEspecialidad.DataValueField = "Id";
            ddlEspecialidad.DataBind();
        }

        private void cargarGrilla()
        {
            MedicoNegocio negocio = new MedicoNegocio();
            dgvMedicos.DataSource = negocio.listar();
            dgvMedicos.DataBind();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                MedicoNegocio negocio = new MedicoNegocio();
                Medico med = new Medico();

                med.Nombre = txtNombre.Text;
                med.Apellido = txtApellido.Text;
                med.Matricula = txtMatricula.Text;
                
                med.Email = "";
                
                int IdEsp = Convert.ToInt32(ddlEspecialidad.SelectedValue);
                

                if (!string.IsNullOrEmpty(hfIdMedico.Value))
                {
                    med.Id = Convert.ToInt32(hfIdMedico.Value);
                    negocio.modificar(med);
                }
                else
                {
                    negocio.agregar(med, IdEsp);
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

        protected void dgvMedicos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditarMedico")
            {
                int idSeleccionado = Convert.ToInt32(e.CommandArgument);
                hfIdMedico.Value = idSeleccionado.ToString();

                MedicoNegocio negocio = new MedicoNegocio();
                Medico seleccionado = negocio.listar().Find(x => x.Id == idSeleccionado);

                if (seleccionado != null)
                {
                    txtNombre.Text = seleccionado.Nombre;
                    txtApellido.Text = seleccionado.Apellido;
                    txtMatricula.Text = seleccionado.Matricula;
                    if (seleccionado.Especialidades != null && seleccionado.Especialidades.Count > 0)
                    {
                        ddlEspecialidad.SelectedValue = seleccionado.Especialidades[0].Id.ToString();
                    }
                    else
                    {
                        if (ddlEspecialidad.Items.Count > 0) ddlEspecialidad.SelectedIndex = 0;
                    }

                    litTituloForm.Text = "Editar Datos Médico N° " + idSeleccionado;
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiarFormulario();
        }

        private void limpiarFormulario()
        {
            hfIdMedico.Value = "";
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtMatricula.Text = "";
            if (ddlEspecialidad.Items.Count > 0) ddlEspecialidad.SelectedIndex = 0;
            litTituloForm.Text = "Registrar / Modificar Médico";
        }
    }
}