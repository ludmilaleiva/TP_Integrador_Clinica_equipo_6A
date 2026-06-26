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
            // Validamos que tengamos un médico seleccionado y una fecha válida antes de consultar
            if (!string.IsNullOrEmpty(ddlMedico.SelectedValue) && !string.IsNullOrEmpty(txtFechaTurno.Text))
            {
                int idMedico = Convert.ToInt32(ddlMedico.SelectedValue);
                DateTime fechaSeleccionada = Convert.ToDateTime(txtFechaTurno.Text);

                //Regla de negocio: Impedir la selección de fechas pasadas
                if (fechaSeleccionada < DateTime.Today)
                {
                    txtFechaTurno.Text = "";
                    ddlHorario.Items.Clear();
                    ddlHorario.Items.Insert(0, new ListItem("¡No elija fechas pasadas!", ""));
                    return;
                }

                try
                {
                    //Invocamos la carga real desde el Stored Procedure de SQL
                    cargarHorariosDisponibles(idMedico, fechaSeleccionada);
                }
                catch (Exception ex)
                {
                    Session.Add("error", ex.ToString());
                    // Si tenés una pantalla de error general (ej: Error.aspx), podés redirigir acá
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
            // Instanciamos la clase de acceso a datos (ajustá el namespace si es 'acceso_datos' o 'negocio')
            acceso_datos.AccesoDatos datos = new acceso_datos.AccesoDatos();

            try
            {
                // Consumimos el SP del script que evalúa el turno de trabajo y la ocupación real
                datos.setearProcedimiento("SP_ObtenerHorariosDisponibles");
                datos.setearParametro("@MedicoId", idMedico);
                datos.setearParametro("@Fecha", fecha);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    // En SQL los campos de tipo TIME se leen en C# como objetos 'TimeSpan'
                    TimeSpan horaInicio = (TimeSpan)datos.Lector["HoraInicio"];
                    TimeSpan horaFin = (TimeSpan)datos.Lector["HoraFin"];
                    int disponible = Convert.ToInt32(datos.Lector["Disponible"]);

                    // Formateamos estéticamente el texto que ve el usuario (ej: "de 08:00 a 09:00 hs")
                    string textoHorario = $"de {horaInicio.ToString(@"hh\:mm")} a {horaFin.ToString(@"hh\:mm")} hs";

                    // Guardamos el valor de inicio completo como string en el Value (ej: "08:00:00")
                    string valorHorario = horaInicio.ToString();

                    ListItem item = new ListItem(textoHorario, valorHorario);

                    // Regla de tu SP: si el slot está ocupado (Disponible = 0), lo deshabilitamos en la interfaz
                    if (disponible == 0)
                    {
                        item.Text += " [OCUPADO]";
                        item.Attributes.Add("disabled", "disabled"); // Bootstrap e HTML impiden que el usuario lo clickee
                    }

                    ddlHorario.Items.Add(item);
                }

                ddlHorario.Items.Insert(0, new ListItem("Seleccione un horario...", ""));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        //Confirmamos el turno y lo guardamos en la base de datos
        protected void btnConfirmarTurno_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlPaciente.SelectedValue) ||
                string.IsNullOrEmpty(ddlMedico.SelectedValue) ||
                string.IsNullOrEmpty(ddlHorario.SelectedValue) ||
                string.IsNullOrEmpty(txtFechaTurno.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertFaltanCampos", "alert('Por favor, complete todos los campos obligatorios.');", true);
                return;
            }

            try
            {
                Turno nuevoTurno = new Turno();

                // Inicializamos los objetos complejos asignando sus respectivos IDs
                nuevoTurno.Paciente = new Paciente { Id = Convert.ToInt32(ddlPaciente.SelectedValue) };
                nuevoTurno.Medico = new Medico { Id = Convert.ToInt32(ddlMedico.SelectedValue) };
                nuevoTurno.Especialidad = new Especialidad { Id = Convert.ToInt32(ddlEspecialidad.SelectedValue) };

                nuevoTurno.Fecha = Convert.ToDateTime(txtFechaTurno.Text);

                TimeSpan horaInicio = TimeSpan.Parse(ddlHorario.SelectedValue);
                nuevoTurno.HoraInicio = horaInicio;
                nuevoTurno.HoraFin = horaInicio.Add(new TimeSpan(1, 0, 0)); // Slot de 1 hora

                // Mapeamos al cuadro de texto de observaciones de tu pantalla
                nuevoTurno.ObservacionesPaciente = "Cita programada desde panel.";

                TurnoNegocio negocio = new TurnoNegocio();
                negocio.agregar(nuevoTurno);

                string scriptExito = "alert('¡Turno confirmado y guardado con éxito!'); window.location='Default.aspx';";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertRedirect", scriptExito, true);
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());

                string mensajeError = ex.Message.Replace("'", "\\'");
                string scriptError = $"alert('Error crítico al guardar: {mensajeError}');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorAlert", scriptError, true);
            }
        }

        // Limpia todo el formulario
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            ddlPaciente.SelectedIndex = 0;
            ddlEspecialidad.SelectedIndex = 0;

            ddlMedico.Items.Clear();
            ddlMedico.Items.Insert(0, new ListItem("Seleccione primero la especialidad...", ""));

            ddlHorario.Items.Clear();
            ddlHorario.Items.Insert(0, new ListItem("Seleccione médico y fecha...", ""));

            txtFechaTurno.Text = "";
        }
    }
}