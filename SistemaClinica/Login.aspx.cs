using acceso_datos;
using System;
using System.Web.UI;

namespace SistemaClinica
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Session["UsuarioId"] != null)
            {
                Response.Redirect("Default.aspx");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                mostrarError("Ingrese email y contraseña.");
                return;
            }

            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                    SELECT Id, Nombre, Email, PerfilId
                    FROM Usuarios
                    WHERE Email = @Email
                      AND PasswordHash = @Password
                      AND Activo = 1
                ");

                datos.setearParametro("@Email", email);
                datos.setearParametro("@Password", password);

                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    Session["UsuarioId"] = Convert.ToInt32(datos.Lector["Id"]);
                    Session["UsuarioNombre"] = datos.Lector["Nombre"].ToString();
                    Session["UsuarioEmail"] = datos.Lector["Email"].ToString();
                    Session["PerfilId"] = Convert.ToInt32(datos.Lector["PerfilId"]);

                    Response.Redirect("Default.aspx");
                }
                else
                {
                    mostrarError("Email o contraseña incorrectos.");
                }
            }
            catch (Exception ex)
            {
                mostrarError("Error al iniciar sesión.");
                Session.Add("error", ex.ToString());
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        private void mostrarError(string mensaje)
        {
            lblError.Text = mensaje;
            lblError.Visible = true;
        }
    }
}