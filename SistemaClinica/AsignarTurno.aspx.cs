using acceso_datos;
using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SistemaClinica
{
    public partial class AsignarTurno : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)

        {
            Seguridad.ValidarPagina();

            if (!IsPostBack)
            {
                cargarPacientes();
                cargarEspecialidades();

                if (Request.QueryString["reprogramarId"] != null)
                {
                    cargarDatosReprogramacion();
                }
            }
        }

        private void cargarDatosReprogramacion()
        {
            int idTurnoOriginal = Convert.ToInt32(Request.QueryString["reprogramarId"]);

            TurnoNegocio turnoNegocio = new TurnoNegocio();
            MedicoNegocio medicoNegocio = new MedicoNegocio();

            Turno turnoOriginal = turnoNegocio.buscarPorId(idTurnoOriginal);

            if (turnoOriginal == null)
                return;

            string idEspecialidad = turnoOriginal.Especialidad.Id.ToString();

            ddlPaciente.SelectedValue = turnoOriginal.Paciente.Id.ToString();
            ddlPaciente.Enabled = false;

            ddlEspecialidad.SelectedValue = idEspecialidad;

            ddlMedico.DataSource = medicoNegocio.listarPorEspecialidad(Convert.ToInt32(idEspecialidad));
            ddlMedico.DataValueField = "Id";
            ddlMedico.DataTextField = "NombreCompleto";
            ddlMedico.DataBind();
            ddlMedico.Items.Insert(0, new ListItem("Seleccione un médico...", ""));

            ddlMedico.Enabled = true;
            txtFechaTurno.Enabled = true;
            ddlHorario.Enabled = false;

            txtObservaciones.Text = $"Reprogramación del turno número: {turnoOriginal.Numero}. ";
        }

        private void cargarPacientes()
        {
            PacienteNegocio negocio = new PacienteNegocio();

            try
            {
                ddlPaciente.DataSource = negocio.listar();
                ddlPaciente.DataValueField = "Id";
                ddlPaciente.DataTextField = "DatosCompletosCombo";
                ddlPaciente.DataBind();

                ddlPaciente.Items.Insert(0, new ListItem("Seleccione un paciente...", ""));
            }
            catch (Exception ex)
            {
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
            if (string.IsNullOrEmpty(ddlEspecialidad.SelectedValue))
            {
                limpiarSeleccionEspecialidad();
                return;
            }

            int idEspecialidad = Convert.ToInt32(ddlEspecialidad.SelectedValue);
            MedicoNegocio medicoNegocio = new MedicoNegocio();

            try
            {
                var listaMedicos = medicoNegocio.listarPorEspecialidad(idEspecialidad);

                ddlMedico.DataSource = listaMedicos;
                ddlMedico.DataValueField = "Id";
                ddlMedico.DataTextField = "NombreCompleto";
                ddlMedico.DataBind();
                ddlMedico.Items.Insert(0, new ListItem("Seleccione un médico...", ""));

                ddlMedico.Enabled = true;
                txtFechaTurno.Enabled = true;

                ddlHorario.Items.Clear();
                ddlHorario.Items.Insert(0, new ListItem("Seleccione médico y fecha...", ""));
                ddlHorario.Enabled = false;

                generarSugerencias(listaMedicos);
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
            }
        }

        private void limpiarSeleccionEspecialidad()
        {
            ddlMedico.Items.Clear();
            ddlMedico.Items.Insert(0, new ListItem("Seleccione primero la especialidad...", ""));
            ddlMedico.Enabled = false;

            txtFechaTurno.Text = "";
            txtFechaTurno.Enabled = false;

            ddlHorario.Items.Clear();
            ddlHorario.Items.Insert(0, new ListItem("Seleccione médico y fecha...", ""));
            ddlHorario.Enabled = false;

            repHorariosSugeridos.DataSource = null;
            repHorariosSugeridos.DataBind();
        }

        private void generarSugerencias(List<Medico> listaMedicos)
        {
            var sugerencias = new List<object>();

            if (listaMedicos == null || listaMedicos.Count == 0)
            {
                repHorariosSugeridos.DataSource = null;
                repHorariosSugeridos.DataBind();
                return;
            }

            int diasMaximosABuscar = 15;

            for (int i = 1; i <= diasMaximosABuscar && sugerencias.Count < 3; i++)
            {
                DateTime fechaEvaluar = DateTime.Today.AddDays(i);

                foreach (Medico medico in listaMedicos)
                {
                    if (sugerencias.Count >= 3)
                        break;

                    List<HorarioDisponible> horarios = obtenerHorariosDisponibles(medico.Id, fechaEvaluar);

                    HorarioDisponible primerHorarioLibre = horarios.FirstOrDefault(x => x.Disponible);

                    if (primerHorarioLibre != null)
                    {
                        sugerencias.Add(new
                        {
                            IdMedico = medico.Id,
                            NombreMedico = medico.NombreCompleto,
                            FechaTexto = fechaEvaluar.ToString("dd 'de' MMMM"),
                            FechaSql = fechaEvaluar.ToString("yyyy-MM-dd"),
                            HoraTexto = primerHorarioLibre.HoraInicio.ToString(@"hh\:mm"),
                            HoraInicio = primerHorarioLibre.HoraInicio.ToString(@"hh\:mm\:ss")
                        });
                    }
                }
            }

            repHorariosSugeridos.DataSource = sugerencias;
            repHorariosSugeridos.DataBind();
        }

        private List<HorarioDisponible> obtenerHorariosDisponibles(int idMedico, DateTime fecha)
        {
            List<HorarioDisponible> lista = new List<HorarioDisponible>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearProcedimiento("SP_ObtenerHorariosDisponibles");
                datos.setearParametro("@MedicoId", idMedico);
                datos.setearParametro("@Fecha", fecha);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    TimeSpan horaInicio = (TimeSpan)datos.Lector["HoraInicio"];
                    TimeSpan horaFin = (TimeSpan)datos.Lector["HoraFin"];
                    int disponible = Convert.ToInt32(datos.Lector["Disponible"]);

                    lista.Add(new HorarioDisponible
                    {
                        HoraInicio = horaInicio,
                        HoraFin = horaFin,
                        Disponible = disponible == 1
                    });
                }

                return lista;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        protected void repHorariosSugeridos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "SeleccionarTurno")
                return;

            string[] datos = e.CommandArgument.ToString().Split('|');

            int idMedico = Convert.ToInt32(datos[0]);
            DateTime fecha = Convert.ToDateTime(datos[1]);
            string hora = datos[2];

            ddlMedico.SelectedValue = idMedico.ToString();

            txtFechaTurno.Enabled = true;
            txtFechaTurno.Text = fecha.ToString("yyyy-MM-dd");

            cargarHorariosDisponibles(idMedico, fecha);

            ddlHorario.Enabled = true;

            ListItem itemHorario = ddlHorario.Items.FindByValue(hora);

            if (itemHorario != null)
            {
                ddlHorario.SelectedValue = hora;
            }
        }

        protected void txtFechaTurno_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlMedico.SelectedValue) && !string.IsNullOrEmpty(txtFechaTurno.Text))
            {
                int idMedico = Convert.ToInt32(ddlMedico.SelectedValue);
                DateTime fechaSeleccionada = Convert.ToDateTime(txtFechaTurno.Text);

                if (fechaSeleccionada < DateTime.Today)
                {
                    txtFechaTurno.Text = "";
                    ddlHorario.Items.Clear();
                    ddlHorario.Items.Insert(0, new ListItem("¡No elija fechas pasadas!", ""));
                    ddlHorario.Enabled = false;
                    return;
                }

                try
                {
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
                ddlHorario.Enabled = false;
            }
        }

        private void cargarHorariosDisponibles(int idMedico, DateTime fecha)
        {
            ddlHorario.Items.Clear();

            try
            {
                List<HorarioDisponible> horarios = obtenerHorariosDisponibles(idMedico, fecha);

                foreach (HorarioDisponible horario in horarios)
                {
                    string textoHorario = $"de {horario.HoraInicio.ToString(@"hh\:mm")} a {horario.HoraFin.ToString(@"hh\:mm")} hs";
                    string valorHorario = horario.HoraInicio.ToString(@"hh\:mm\:ss");

                    ListItem item = new ListItem(textoHorario, valorHorario);

                    if (!horario.Disponible)
                    {
                        item.Text += " [OCUPADO]";
                        item.Attributes.Add("disabled", "disabled");
                    }

                    ddlHorario.Items.Add(item);
                }

                ddlHorario.Items.Insert(0, new ListItem("Seleccione un horario...", ""));
                ddlHorario.Enabled = horarios.Count > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnConfirmarTurno_Click(object sender, EventArgs e)
        {
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

                nuevoTurno.Paciente = new Paciente { Id = Convert.ToInt32(ddlPaciente.SelectedValue) };
                nuevoTurno.Medico = new Medico { Id = Convert.ToInt32(ddlMedico.SelectedValue) };
                nuevoTurno.Especialidad = new Especialidad { Id = Convert.ToInt32(ddlEspecialidad.SelectedValue) };
                nuevoTurno.Fecha = Convert.ToDateTime(txtFechaTurno.Text);

                TimeSpan horaInicio = TimeSpan.Parse(ddlHorario.SelectedValue);
                nuevoTurno.HoraInicio = horaInicio;
                nuevoTurno.HoraFin = horaInicio.Add(new TimeSpan(1, 0, 0));
                nuevoTurno.ObservacionesPaciente = txtObservaciones.Text;

                string añoActual = DateTime.Today.Year.ToString();
                string codigoCorto = Guid.NewGuid().ToString().Substring(0, 5).ToUpper();
                nuevoTurno.Numero = $"T{añoActual}-{codigoCorto}";

                TurnoNegocio negocio = new TurnoNegocio();

                if (Request.QueryString["reprogramarId"] != null)
                {
                    int idOriginal = Convert.ToInt32(Request.QueryString["reprogramarId"]);

                    nuevoTurno.TurnoOriginalId = idOriginal;
                    nuevoTurno.Estado = new EstadoTurno { Id = 2 };

                    negocio.agregar(nuevoTurno);
                    negocio.reprogramarTurno(idOriginal);
                }
                else
                {
                    nuevoTurno.Estado = new EstadoTurno { Id = 1 };
                    negocio.agregar(nuevoTurno);
                }

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

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            ddlPaciente.SelectedIndex = 0;
            ddlEspecialidad.SelectedIndex = 0;

            limpiarSeleccionEspecialidad();

            txtObservaciones.Text = "";
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
            finally
            {
                datos.cerrarConexion();
            }
        }

        private class HorarioDisponible
        {
            public TimeSpan HoraInicio { get; set; }
            public TimeSpan HoraFin { get; set; }
            public bool Disponible { get; set; }
        }
    }
}