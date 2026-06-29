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
            TurnoTrabajoNegocio turnoNegocio = new TurnoTrabajoNegocio();
            ddlTurnoTrabajo.DataSource = turnoNegocio.listar();
            ddlTurnoTrabajo.DataValueField = "Id";
            ddlTurnoTrabajo.DataTextField = "Nombre";
            ddlTurnoTrabajo.DataBind();
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
                string dniIngresado = txtDni.Text.Trim();

                //Validar DNI único antes de registrar un alta nueva
                if (string.IsNullOrEmpty(hfIdMedico.Value))
                {
                    if (negocio.existeDni(dniIngresado))
                    {
                        string script = "alert('Error: Ya existe un médico registrado con el DNI " + dniIngresado + ".');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertDuplicado", script, true);
                        return; 
                    }
                }


                Medico med = new Medico();

                med.Nombre = txtNombre.Text;
                med.Apellido = txtApellido.Text;
                med.DNI = dniIngresado;
                med.Matricula = txtMatricula.Text;
                med.Telefono = txtTelefono.Text; 
                med.Email = txtEmail.Text;
                med.Especialidades = new List<Especialidad>();
                Especialidad espSeleccionada = new Especialidad { Id = Convert.ToInt32(ddlEspecialidad.SelectedValue) };
                med.Especialidades.Add(espSeleccionada);
                med.TurnoTrabajo = new TurnoTrabajo { Id = Convert.ToInt32(ddlTurnoTrabajo.SelectedValue) };


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
                throw ex;
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
                    txtDni.Text = seleccionado.DNI;
                    txtTelefono.Text = seleccionado.Telefono;
                    txtEmail.Text = seleccionado.Email;
                    ddlEspecialidad.SelectedValue = seleccionado.Especialidades[0].Id.ToString();


                    if (seleccionado.Especialidades != null && seleccionado.Especialidades.Count > 0)
                    {
                        ddlEspecialidad.SelectedValue = seleccionado.Especialidades[0].Id.ToString();
                    }
                    else
                    {
                        if (ddlEspecialidad.Items.Count > 0) ddlEspecialidad.SelectedIndex = 0;
                    }

                  
                    if (seleccionado.TurnoTrabajo != null && seleccionado.TurnoTrabajo.Id > 0)
                    {
                        ddlTurnoTrabajo.SelectedValue = seleccionado.TurnoTrabajo.Id.ToString();
                    }
                    else
                    {
                        if (ddlTurnoTrabajo.Items.Count > 0) ddlTurnoTrabajo.SelectedIndex = 0;
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