using acceso_datos;
using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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

                if (Request.QueryString["reprogramarId"] != null)
                {
                    int idTurnoOriginal = Convert.ToInt32(Request.QueryString["reprogramarId"]);

                    TurnoNegocio negocio = new TurnoNegocio();
                    MedicoNegocio medicoNegocio = new MedicoNegocio();
                    Turno turnoOriginal = negocio.buscarPorId(idTurnoOriginal); // Necesitás este método que devuelva un Turno
                    string idEspecialidad = turnoOriginal.Especialidad.Id.ToString();
                    if (turnoOriginal != null)
                    {
                        // 1. Pre-selecciona al Paciente
                        ddlPaciente.SelectedValue = turnoOriginal.Paciente.Id.ToString();
                        ddlPaciente.Enabled = false;

                        // Pre-selecciona la Especialidad original
                        ddlEspecialidad.SelectedValue = idEspecialidad;

                        //CargarMedicosPorEspecialidad(turnoOriginal.Especialidad.Id);
                        ddlMedico.DataSource = medicoNegocio.listarPorEspecialidad(Convert.ToInt32(idEspecialidad));
                        ddlMedico.DataValueField = "Id";
                        ddlMedico.DataTextField = "NombreCompleto";
                        ddlMedico.DataBind();

                        // Avisamos visualmente en las observaciones
                        txtObservaciones.Text = $"Reprogramación del turno número: {turnoOriginal.Numero}. ";
                    }
                }
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
            // Validamos que se hayan seleccionado opciones reales (con IDs mayores a 0)
            if (string.IsNullOrEmpty(ddlPaciente.SelectedValue) || ddlPaciente.SelectedValue == "0" ||
                string.IsNullOrEmpty(ddlMedico.SelectedValue) || ddlMedico.SelectedValue == "0" ||
                string.IsNullOrEmpty(ddlEspecialidad.SelectedValue) || ddlEspecialidad.SelectedValue == "0" ||
                string.IsNullOrEmpty(ddlHorario.SelectedValue) || ddlHorario.SelectedValue == "0" ||
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
                nuevoTurno.ObservacionesPaciente = txtObservaciones.Text;

                //AUTOGENERACIÓN DEL NÚMERO DE TURNO ÚNICO
                string añoActual = DateTime.Today.Year.ToString();
                string codigoCorto = Guid.NewGuid().ToString().Substring(0, 5).ToUpper();
                nuevoTurno.Numero = $"T{añoActual}-{codigoCorto}"; // Genera algo como: T2026-A5B2D

                // MANEJO DE ESTADO Y REPROGRAMACIÓN DILIGENTE
                if (Request.QueryString["reprogramarId"] != null)
                {
                    nuevoTurno.TurnoOriginalId = Convert.ToInt32(Request.QueryString["reprogramarId"]);
                    nuevoTurno.Estado = new EstadoTurno { Id = 2 };// EstadoId = 2 ('Reprogramado')
                }
                else
                {
                    nuevoTurno.Estado = new EstadoTurno { Id = 1 }; // EstadoId = 1 ('Nuevo')
                }

                // Guardamos en la base de datos (con su correspondiente TurnoHistorial adentro)
                TurnoNegocio negocio = new TurnoNegocio();
                if (Request.QueryString["reprogramarId"] != null)
                {
                    int idOriginal = Convert.ToInt32(Request.QueryString["reprogramarId"]);
                    nuevoTurno.TurnoOriginalId = idOriginal;
                    nuevoTurno.Estado = new EstadoTurno { Id = 2 }; // EstadoId = 2 ('Reprogramado')

                    // 1. Guardamos primero el nuevo turno
                    negocio.agregar(nuevoTurno);

                    //Se reprograma el turno 
                    negocio.reprogramarTurno(idOriginal);
                }
                else
                {
                    nuevoTurno.Estado = new EstadoTurno { Id = 1 }; // EstadoId = 1 ('Nuevo')
                    negocio.agregar(nuevoTurno);
                }

                //MÓDULO DE ENVÍO DE EMAIL AL PACIENTE
                /*try
                {
                    PacienteNegocio pacNegocio = new PacienteNegocio();
                    Paciente pac = pacNegocio.buscarPorId(nuevoTurno.Paciente.Id); // Necesitás este método en tu negocio para traer su Email

                    if (pac != null && !string.IsNullOrEmpty(pac.Email))
                    {
                        //EmailService email = new EmailService(); // Ajustá al nombre exacto de tu clase de correo
                        string asunto = $"Confirmación de Turno {nuevoTurno.Numero} - BioClinic";
                        string cuerpo = $@"<h3>¡Hola {pac.Nombre}!</h3>
                                   <p>Se ha reservado tu turno médico de manera exitosa.</p>
                                   <ul>
                                       <li><strong>Número de Turno:</strong> {nuevoTurno.Numero}</li>
                                       <li><strong>Fecha:</strong> {nuevoTurno.Fecha:dd/MM/yyyy}</li>
                                       <li><strong>Horario:</strong> de {nuevoTurno.HoraInicio.ToString(@"hh\:mm")} hs</li>
                                   </ul>
                                   <p>Gracias por confiar en BioClinic.</p>";

                        //email.armarCorreo(pac.Email, asunto, cuerpo);
                        //email.enviarCorreo();
                    }
                }
                catch (Exception)
                {
               
                }*/

                // Redirigimos al listado general para ver el nuevo registro impactado
                string scriptExito = "alert('¡Turno confirmado y guardado con éxito!'); window.location='ListadoTurnos.aspx';";
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

        public Turno buscarPorId(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT Id, Numero, PacienteId FROM Turnos WHERE Id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    Turno t = new Turno();
                    t.Id = Convert.ToInt32(datos.Lector["Id"]);
                    t.Numero = datos.Lector["Numero"].ToString();
                    t.Paciente = new Paciente { Id = Convert.ToInt32(datos.Lector["PacienteId"]) };
                    return t;
                }
                return null;
            }
            catch (Exception ex) { throw ex; }
            finally { datos.cerrarConexion(); }
        }
    }
}