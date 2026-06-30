using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;
using acceso_datos;
using System.Data.SqlClient;
using System.Data;

namespace negocio
{
    public class TurnoNegocio
    {
        public void agregar(Turno nuevo)
        {
            string numeroTurnoAutogenerado = "";

            try
            {
                // 1. Usamos la conexión nativa transitoria para leer el parámetro OUTPUT del SP
                using (SqlConnection conexion = new SqlConnection("server=.\\SQLEXPRESS; database=ClinicaTurnos; integrated security=true"))
                {
                    using (SqlCommand comando = new SqlCommand("SP_GenerarNumeroTurno", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;

                        SqlParameter paramOut = new SqlParameter("@NroTurno", SqlDbType.VarChar, 20);
                        paramOut.Direction = ParameterDirection.Output;
                        comando.Parameters.Add(paramOut);

                        conexion.Open();
                        comando.ExecuteNonQuery();
                        numeroTurnoAutogenerado = paramOut.Value.ToString();
                    }
                }

                // 2. Insertamos el registro definitivo usando tu clase AccesoDatos estándar
                AccesoDatos datosInsert = new AccesoDatos();
                try
                {
                    datosInsert.setearConsulta(@"
                        INSERT INTO Turnos (Numero, PacienteId, MedicoId, EspecialidadId, Fecha, HoraInicio, HoraFin, EstadoId, ObservacionesPaciente, FechaAlta) 
                        VALUES (@Numero, @PacienteId, @MedicoId, @EspecialidadId, @Fecha, @HoraInicio, @HoraFin, @EstadoId, @ObservacionesPaciente, GETDATE())");

                    datosInsert.setearParametro("@Numero", numeroTurnoAutogenerado);
                    datosInsert.setearParametro("@PacienteId", nuevo.Paciente.Id);
                    datosInsert.setearParametro("@MedicoId", nuevo.Medico.Id);
                    datosInsert.setearParametro("@EspecialidadId", nuevo.Especialidad.Id);
                    datosInsert.setearParametro("@Fecha", nuevo.Fecha);
                    datosInsert.setearParametro("@HoraInicio", nuevo.HoraInicio);
                    datosInsert.setearParametro("@HoraFin", nuevo.HoraFin);
                    datosInsert.setearParametro("@EstadoId", 1); // 1 = Estado 'Nuevo'
                    datosInsert.setearParametro("@ObservacionesPaciente", (object)nuevo.ObservacionesPaciente ?? DBNull.Value);

                    datosInsert.ejecutarAccion();
                }
                finally
                {
                    datosInsert.cerrarConexion();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Turno> listar()
        {
            List<Turno> lista = new List<Turno>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Trae los datos clave uniendo las tablas para armar los objetos complejos
                datos.setearConsulta(@"
            SELECT T.Id, T.Numero, T.Fecha, T.HoraInicio, T.HoraFin, T.ObservacionesPaciente,
                   P.Id AS PacienteId, P.Nombre AS PacienteNombre, P.Apellido AS PacienteApellido,
                   M.Id AS MedicoId, M.Nombre AS MedicoNombre, M.Apellido AS MedicoApellido,
                   E.Id AS EspecialidadId, E.Nombre AS EspecialidadNombre,
                   T.EstadoId, Est.Nombre AS EstadoNombre
            FROM Turnos T
            INNER JOIN Pacientes P ON T.PacienteId = P.Id
            INNER JOIN Medicos M ON T.MedicoId = M.Id
            INNER JOIN Especialidades E ON T.EspecialidadId = E.Id
            INNER JOIN EstadosTurno Est ON T.EstadoId = Est.Id
      
            ORDER BY T.Fecha DESC, T.HoraInicio DESC");

                
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Turno aux = new Turno();
                    aux.Id = Convert.ToInt32(datos.Lector["Id"]);
                    aux.Numero = datos.Lector["Numero"].ToString();
                    aux.Fecha = Convert.ToDateTime(datos.Lector["Fecha"]);
                    aux.HoraInicio = (TimeSpan)datos.Lector["HoraInicio"];
                    aux.HoraFin = (TimeSpan)datos.Lector["HoraFin"];
                    aux.ObservacionesPaciente = datos.Lector["ObservacionesPaciente"] != DBNull.Value ? datos.Lector["ObservacionesPaciente"].ToString() : "";

                    // Instancia y mapeo de los objetos compuestos en dominio
                    aux.Paciente = new Paciente
                    {
                        Id = Convert.ToInt32(datos.Lector["PacienteId"]),
                        Nombre = datos.Lector["PacienteNombre"].ToString(),
                        Apellido = datos.Lector["PacienteApellido"].ToString()
                    };

                    aux.Medico = new Medico
                    {
                        Id = Convert.ToInt32(datos.Lector["MedicoId"]),
                        Nombre = datos.Lector["MedicoNombre"].ToString(),
                        Apellido = datos.Lector["MedicoApellido"].ToString()
                    };

                    aux.Especialidad = new Especialidad
                    {
                        Id = Convert.ToInt32(datos.Lector["EspecialidadId"]),
                        Nombre = datos.Lector["EspecialidadNombre"].ToString()
                    };

                    aux.Estado = new EstadoTurno
                    {
                        Id = Convert.ToInt32(datos.Lector["EstadoId"]),
                        Nombre = datos.Lector["EstadoNombre"].ToString()
                    };

                   
                
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

        public void cancelar(int idTurno)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                // estado a 3 (Cancelado) para el turno correspondiente
                datos.setearConsulta("UPDATE Turnos SET EstadoId = 3 WHERE Id = @id");
                datos.setearParametro("@id", idTurno);
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

        public void reprogramarTurno(int idTurno)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                //Cambiamos el EstadoId a 2 (Reprogramado) para el turno que se está reemplazando
                datos.setearConsulta("UPDATE Turnos SET EstadoId = 2 WHERE Id = @id");
                datos.setearParametro("@id", idTurno);
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

        public Turno buscarPorId(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                // Traemos las claves del turno viejo para saber a qué paciente corresponde
                datos.setearConsulta("SELECT Id, Numero, PacienteId, EspecialidadId FROM Turnos WHERE Id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    Turno t = new Turno();
                    t.Id = Convert.ToInt32(datos.Lector["Id"]);
                    t.Numero = datos.Lector["Numero"].ToString();

                    // Inicializamos el objeto complejo Paciente con su ID para poder pre-seleccionarlo
                    t.Paciente = new Paciente { Id = Convert.ToInt32(datos.Lector["PacienteId"]) };
                    t.Especialidad = new Especialidad { Id = Convert.ToInt32(datos.Lector["EspecialidadId"]) };

                    return t;
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

        public void registrarAusencia(int idTurno)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                //Cambiamos el EstadoId a 4 (No Asistió)
                datos.setearConsulta("UPDATE Turnos SET EstadoId = 4 WHERE Id = @id");
                datos.setearParametro("@id", idTurno);
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