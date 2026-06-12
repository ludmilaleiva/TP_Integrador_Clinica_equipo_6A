using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class PacienteNegocio
    {
        // Método para listar todos los pacientes activos
        public List<Paciente> listar()
        {
            List<Paciente> lista = new List<Paciente>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Id, Nombre, Apellido, DNI, FechaNacimiento, Sexo, Email, Telefono, Direccion, ObraSocial, NroAfiliado, Activo FROM Pacientes WHERE Activo = 1");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Paciente aux = new Paciente();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Apellido = (string)datos.Lector["Apellido"];
                    aux.DNI = (string)datos.Lector["DNI"];
                    aux.FechaNacimiento = (DateTime)datos.Lector["FechaNacimiento"];
                    aux.Sexo = Convert.ToChar(datos.Lector["Sexo"].ToString());

                    // Manejo de nulos para los campos opcionales en la BD
                    aux.Email = datos.Lector["Email"] is DBNull ? "" : (string)datos.Lector["Email"];
                    aux.Telefono = datos.Lector["Telefono"] is DBNull ? "" : (string)datos.Lector["Telefono"];
                    aux.Direccion = datos.Lector["Direccion"] is DBNull ? "" : (string)datos.Lector["Direccion"];
                    aux.ObraSocial = datos.Lector["ObraSocial"] is DBNull ? "" : (string)datos.Lector["ObraSocial"];
                    aux.NroAfiliado = datos.Lector["NroAfiliado"] is DBNull ? "" : (string)datos.Lector["NroAfiliado"];
                    aux.Activo = (bool)datos.Lector["Activo"];

                    lista.Add(aux);
                }

                return lista;
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

        // Método para agregar un nuevo paciente (Alta)
        public void agregar(Paciente nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("INSERT INTO Pacientes (Nombre, Apellido, DNI, FechaNacimiento, Sexo, Email, Telefono, Direccion, ObraSocial, NroAfiliado) " +
                    "VALUES (@nombre, @apellido, @dni, @fechaNac, @sexo, @email, @telefono, @direccion, @obraSocial, @nroAfiliado)");

                datos.setearParametro("@nombre", nuevo.Nombre);
                datos.setearParametro("@apellido", nuevo.Apellido);
                datos.setearParametro("@dni", nuevo.DNI);
                datos.setearParametro("@fechaNac", nuevo.FechaNacimiento);
                datos.setearParametro("@sexo", nuevo.Sexo);
                datos.setearParametro("@email", string.IsNullOrEmpty(nuevo.Email) ? (object)DBNull.Value : nuevo.Email);
                datos.setearParametro("@telefono", string.IsNullOrEmpty(nuevo.Telefono) ? (object)DBNull.Value : nuevo.Telefono);
                datos.setearParametro("@direccion", string.IsNullOrEmpty(nuevo.Direccion) ? (object)DBNull.Value : nuevo.Direccion);
                datos.setearParametro("@obraSocial", string.IsNullOrEmpty(nuevo.ObraSocial) ? (object)DBNull.Value : nuevo.ObraSocial);
                datos.setearParametro("@nroAfiliado", string.IsNullOrEmpty(nuevo.NroAfiliado) ? (object)DBNull.Value : nuevo.NroAfiliado);

                datos.ejecutarAccion();
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
    }
}
