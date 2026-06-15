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
    public partial class Pacientes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarGrilla();
            }
        }

        
        private void cargarGrilla()
        {
            try
            {
                PacienteNegocio negocio = new PacienteNegocio();
                dgvPacientes.DataSource = negocio.listar();
                dgvPacientes.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Se ejecuta al hacer clic en "Guardar Paciente"
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Paciente nuevo = new Paciente();
                PacienteNegocio negocio = new PacienteNegocio();

                // Mapeamos lo que el usuario escribió en los inputs al objeto de dominio
               
                nuevo.Nombre = txtNombre.Text;
                nuevo.Apellido = txtApellido.Text;
                nuevo.DNI = txtDNI.Text;
                nuevo.FechaNacimiento = DateTime.Parse(txtFechaNacimiento.Text);
                nuevo.Sexo = Convert.ToChar(ddlSexo.SelectedValue);
                nuevo.Email = txtEmail.Text;
                nuevo.Telefono = txtTelefono.Text;
                nuevo.Direccion = txtDireccion.Text;
                nuevo.ObraSocial = txtObraSocial.Text;
                nuevo.NroAfiliado = txtNroAfiliado.Text;
                nuevo.Activo = true;

                if (Session["IdPacienteSeleccionado"] != null)
                {
                    // Si la sesión tiene un ID, le asignamos ese ID al objeto y llamamos a MODIFICAR
                    nuevo.Id = Convert.ToInt32(Session["IdPacienteSeleccionado"]);
                    negocio.modificar(nuevo);

                    pnlAlertaExito.Visible = true;

                    // Una vez modificado, limpiamos la sesión para el próximo paciente
                    Session.Remove("IdPacienteSeleccionado");
                }
                else
                {
                    // Si la sesión es nula, significa que el formulario se usó para un alta nueva
                    negocio.agregar(nuevo);
                }

                // Volvemos a cargar la grilla para que el nuevo paciente aparezca arriba al instante
                cargarGrilla();
                limpiarFormulario();
                btnGuardar.Text = "Guardar Paciente";
            }
            catch (Exception ex)
            {
                // Dejamos el throw temporal por si alguna caja de texto tira error de formato
                throw ex;
            }
        }

        private void limpiarFormulario()
        {
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtDNI.Text = "";
            txtFechaNacimiento.Text = "";
            ddlSexo.SelectedIndex = 0;
            txtEmail.Text = "";
            txtTelefono.Text = "";
            txtDireccion.Text = "";
            txtObraSocial.Text = "";
            txtNroAfiliado.Text = "";
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                // Identificamos exactamente qué botón de la grilla se presionó
                Button btnEditar = (Button)sender;


                // Recuperamos el ID del paciente que guardamos en el CommandArgument
                int idPaciente = Convert.ToInt32(btnEditar.CommandArgument);

                PacienteNegocio negocio = new PacienteNegocio();
                Paciente seleccionado = negocio.buscarPorId(idPaciente);

                if (seleccionado != null)
                    // Guardamos el ID en la sesión temporal para saber a quién estamos editando al presionar Guardar
                    Session.Add("IdPacienteSeleccionado", seleccionado.Id);

                //RELLENAMOS EL FORMULARIO CON LOS DATOS REALES
                txtNombre.Text = seleccionado.Nombre;
                txtApellido.Text = seleccionado.Apellido;
                txtDNI.Text = seleccionado.DNI;

                // Formateamos la fecha para que el input de tipo date de HTML la entienda perfectamente (yyyy-MM-dd)
                txtFechaNacimiento.Text = seleccionado.FechaNacimiento.ToString("yyyy-MM-dd");

                ddlSexo.SelectedValue = seleccionado.Sexo.ToString();
                txtEmail.Text = seleccionado.Email;
                txtTelefono.Text = seleccionado.Telefono;
                txtDireccion.Text = seleccionado.Direccion;
                txtObraSocial.Text = seleccionado.ObraSocial;
                txtNroAfiliado.Text = seleccionado.NroAfiliado;

                
                btnGuardar.Text = "Modificar Paciente";

                string script = @"
                document.getElementById('seccionFormulario').scrollIntoView({ 
                    behavior: 'smooth', 
                    block: 'start' 
                });";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ScrollFormulario", script, true);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
               
    }
}