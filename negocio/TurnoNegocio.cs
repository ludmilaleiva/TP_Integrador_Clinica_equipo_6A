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
    }
}