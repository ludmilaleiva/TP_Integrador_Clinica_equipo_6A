using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using acceso_datos;

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
               
                //datos.setearConsulta("SELECT Id, Nombre, Apellido, DNI, FechaNacimiento, Sexo, Email, Telefono, Direccion, ObraSocial, NroAfiliado, Estado FROM Pacientes");
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
                    // Si tu script usa "Estado", mapealo así:
                    //aux.Activo = datos.Lector["Estado"] is DBNull ? true : Convert.ToBoolean(datos.Lector["Estado"]);

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

        public Paciente buscarPorId(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT Id, Nombre, Apellido = Apellido, DNI, FechaNacimiento, Sexo, Email, Telefono, Direccion, ObraSocial, NroAfiliado FROM Pacientes WHERE Id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    Paciente aux = new Paciente();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Apellido = (string)datos.Lector["Apellido"];
                    aux.DNI = (string)datos.Lector["DNI"];
                    aux.FechaNacimiento = (DateTime)datos.Lector["FechaNacimiento"];
                    aux.Sexo = Convert.ToChar(datos.Lector["Sexo"].ToString());
                    aux.Email = datos.Lector["Email"] is DBNull ? "" : (string)datos.Lector["Email"];
                    aux.Telefono = datos.Lector["Telefono"] is DBNull ? "" : (string)datos.Lector["Telefono"];
                    aux.Direccion = datos.Lector["Direccion"] is DBNull ? "" : (string)datos.Lector["Direccion"];
                    aux.ObraSocial = datos.Lector["ObraSocial"] is DBNull ? "" : (string)datos.Lector["ObraSocial"];
                    aux.NroAfiliado = datos.Lector["NroAfiliado"] is DBNull ? "" : (string)datos.Lector["NroAfiliado"];

                    return aux;
                }
                return null;
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

        public void modificar(Paciente pac)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE Pacientes SET Nombre = @nombre, Apellido = @apellido, DNI = @dni, FechaNacimiento = @fechaNac, Sexo = @sexo, Email = @email, Telefono = @telefono, Direccion = @direccion, ObraSocial = @obraSocial, NroAfiliado = @nroAfiliado WHERE Id = @id");

                datos.setearParametro("@nombre", pac.Nombre);
                datos.setearParametro("@apellido", pac.Apellido);
                datos.setearParametro("@dni", pac.DNI);
                datos.setearParametro("@fechaNac", pac.FechaNacimiento);
                datos.setearParametro("@sexo", pac.Sexo);
                datos.setearParametro("@email", string.IsNullOrEmpty(pac.Email) ? (object)DBNull.Value : pac.Email);
                datos.setearParametro("@telefono", string.IsNullOrEmpty(pac.Telefono) ? (object)DBNull.Value : pac.Telefono);
                datos.setearParametro("@direccion", string.IsNullOrEmpty(pac.Direccion) ? (object)DBNull.Value : pac.Direccion);
                datos.setearParametro("@obraSocial", string.IsNullOrEmpty(pac.ObraSocial) ? (object)DBNull.Value : pac.ObraSocial);
                datos.setearParametro("@nroAfiliado", string.IsNullOrEmpty(pac.NroAfiliado) ? (object)DBNull.Value : pac.NroAfiliado);
                datos.setearParametro("@id", pac.Id);

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

        public void eliminarLogico(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                // Pasamos Activo a 0 (Falso) en lugar de hacer un DELETE físico
                datos.setearConsulta("UPDATE Pacientes SET Activo = 0 WHERE Id = @id");
                datos.setearParametro("@id", id);
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
