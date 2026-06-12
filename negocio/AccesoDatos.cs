using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace negocio
{
    public class AccesoDatos
    {
        // Objetos de ADO.NET necesarios para la conexión
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;

        // Propiedad pública para leer el Lector desde las clases de negocio (ej: PacienteNegocio)
        public SqlDataReader Lector
        {
            get { return lector; }
        }

        // Constructor: Configura la conexión a tu base de datos ClinicaTurnos
        public AccesoDatos()
        {
            // Cambiá "Server=." por el nombre de tu instancia de SQL si usas una específica (ej: .\SQLEXPRESS)
            conexion = new SqlConnection("server=.; database=ClinicaTurnos; integrated security=true");
            comando = new SqlCommand();
        }

        // Método para consultas puras en texto plano (ej: "SELECT * FROM Pacientes")
        public void setearConsulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }

        // Método para cuando usemos tus Stored Procedures (ej: "SP_ObtenerHorariosDisponibles")
        public void setearProcedimiento(string sp)
        {
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            comando.CommandText = sp;
        }

        // Método para agregar parámetros y evitar inyección de código (ej: @Id, @Nombre)
        public void setearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);
        }

        // Ejecuta consultas de lectura (SELECT)
        public void ejecutarLectura()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Ejecuta acciones de escritura (INSERT, UPDATE, DELETE lógico)
        public void ejecutarAccion()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Ejecuta consultas que devuelven un solo valor (ej: SELECT COUNT(*)...)
        public int ejecutarAccionScalar()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                return Convert.ToInt32(comando.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Cierra la conexión de manera segura para no saturar la base de datos
        public void cerrarConexion()
        {
            if (lector != null)
                lector.Close();

            if (conexion.State == ConnectionState.Open)
                conexion.Close();
        }
    }
}