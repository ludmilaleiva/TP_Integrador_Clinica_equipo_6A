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
    public partial class AsignarTurno : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
                cargarPacientes();
                cargarEspecialidades();
            }
        }

        private void cargarPacientes()
        {
            PacienteNegocio negocio = new PacienteNegocio();
            try
            {
                // Reutilizamos el método listar que ya creamos para la grilla
                ddlPaciente.DataSource = negocio.listar();

                // Propiedad que se guarda en el backend (ID)
                ddlPaciente.DataValueField = "Id";

                // Texto que ve el usuario en el desplegable (combinamos o mostramos Apellido)
                ddlPaciente.DataTextField = "DatosCompletosCombo";

                ddlPaciente.DataBind();

                // Agregamos un ítem neutro al principio de todo
                ddlPaciente.Items.Insert(0, new ListItem("Seleccione un paciente...", ""));
            }
            catch (Exception ex)
            {
                // Podés activar un panel de error aquí si lo deseas
                Session.Add("error", ex.ToString());
            }
        }

        private void cargarEspecialidades()
        {
            EspecialidadNegocio negocio = new EspecialidadNegocio();
            try
            {
                ddlEspecialidad.DataSource = negocio.listar();
                ddlEspecialidad.DataValueField = "Id";
                ddlEspecialidad.DataTextField = "Nombre";
                ddlEspecialidad.DataBind();

                ddlEspecialidad.Items.Insert(0, new ListItem("Seleccione una especialidad...", ""));
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
            }
        }

        protected void ddlEspecialidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Verificamos que realmente hayan elegido una especialidad válida y no el texto inicial
            if (!string.IsNullOrEmpty(ddlEspecialidad.SelectedValue))
            {
                int idEspecialidad = Convert.ToInt32(ddlEspecialidad.SelectedValue);

                MedicoNegocio negocio = new MedicoNegocio();
                try
                {
                    // Buscamos los médicos en la base de datos que tengan esa especialidad
                    ddlMedico.DataSource = negocio.listarPorEspecialidad(idEspecialidad);
                    ddlMedico.DataValueField = "Id";
                    ddlMedico.DataTextField = "NombreCompleto";
                    ddlMedico.DataBind();

                    // Habilitamos el combo de médicos que antes estaba vacío
                    ddlMedico.Items.Insert(0, new ListItem("Seleccione un médico...", ""));

                    // TODO: Acá más adelante vamos a hacer que la tarjeta derecha de "Sugerencias"
                    // parpadee y cargue los 3 horarios sugeridos automáticamente.
                }
                catch (Exception ex)
                {
                    Session.Add("error", ex.ToString());
                }
            }
            else
            {
                // Si vuelven a poner "Seleccione una especialidad...", limpiamos el combo de médicos
                ddlMedico.Items.Clear();
                ddlMedico.Items.Insert(0, new ListItem("Seleccione primero la especialidad...", ""));
            }
        }
    }
}