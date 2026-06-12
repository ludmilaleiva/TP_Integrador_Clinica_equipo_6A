using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class MedicoNegocio
    {
        // Método para listar médicos filtrados por Especialidad (Relación N:N)
        public List<Medico> listarPorEspecialidad(int idEspecialidad)
        {
            List<Medico> lista = new List<Medico>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Hacemos JOINs para cruzar los Médicos con la tabla intermedia de Especialidades
                datos.setearConsulta("SELECT m.Id, m.Nombre, m.Apellido, m.Matricula, m.Email " +
                    "FROM Medicos m " +
                    "INNER JOIN Medico_Especialidades me ON m.Id = me.MedicoId " +
                    "WHERE me.EspecialidadId = @idEsp AND m.Activo = 1");

                datos.setearParametro("@idEsp", idEspecialidad);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Medico aux = new Medico();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Apellido = (string)datos.Lector["Apellido"];
                    aux.Matricula = (string)datos.Lector["Matricula"];
                    aux.Email = (string)datos.Lector["Email"];

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
    }
}
