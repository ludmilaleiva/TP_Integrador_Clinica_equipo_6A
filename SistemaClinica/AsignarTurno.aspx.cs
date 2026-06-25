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

        protected void txtFechaTurno_TextChanged(object sender, EventArgs e)
        {
            // 1. Validamos que tengamos un médico seleccionado y una fecha válida
            if (!string.IsNullOrEmpty(ddlMedico.SelectedValue) && !string.IsNullOrEmpty(txtFechaTurno.Text))
            {
                int idMedico = Convert.ToInt32(ddlMedico.SelectedValue);
                DateTime fechaSeleccionada = Convert.ToDateTime(txtFechaTurno.Text);

                //No se pueden dar de alta turnos vencidos (anteriores a hoy)
                if (fechaSeleccionada < DateTime.Today)
                {
                    // Podríamos limpiar el campo y avisar (luego armamos una alerta visual elegante)
                    txtFechaTurno.Text = "";
                    ddlHorario.Items.Clear();
                    ddlHorario.Items.Insert(0, new ListItem("¡No elija fechas pasadas!", ""));
                    return;
                }

                try
                {
                    // Cargamos los rangos de 1 hora disponibles
                    cargarHorariosDisponibles(idMedico, fechaSeleccionada);
                }
                catch (Exception ex)
                {
                    Session.Add("error", ex.ToString());
                }
            }
            else
            {
                ddlHorario.Items.Clear();
                ddlHorario.Items.Insert(0, new ListItem("Seleccione médico y fecha...", ""));
            }
        }

        private void cargarHorariosDisponibles(int idMedico, DateTime fecha)
        {
            ddlHorario.Items.Clear();

            // Simulamos una agenda: supongamos que el médico trabaja de 8:00 a 14:00 hs
            // En la siguiente fase, estos límites saldrán de la tabla 'Turnos_Trabajo' de tu SQL
            int horaEntrada = 8;
            int horaSalida = 14;

            for (int hora = horaEntrada; hora < horaSalida; hora++)
            {
                // Formateamos la cadena estética que pide la consigna: "de 10 a 11", "de 11 a 12"
                string textoHorario = $"de {hora}:00 a {hora + 1}:00 hs";
                string valorHorario = hora.ToString(); // Guardamos solo el número de hora de inicio (ej: "10")

                ddlHorario.Items.Add(new ListItem(textoHorario, valorHorario));
            }

            // Insertamos el ítem neutro al principio
            ddlHorario.Items.Insert(0, new ListItem("Seleccione un horario...", ""));
        }
    }
}