using dominio;
using System;
using System.Collections.Generic;
using acceso_datos;

namespace negocio
{
    public class TurnoTrabajoNegocio
    {
        // Traer todos los turnos activos de la base de datos
        public List<TurnoTrabajo> listar()
        {
            List<TurnoTrabajo> lista = new List<TurnoTrabajo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                
                datos.setearConsulta("SELECT Id, Nombre, HoraEntrada, HoraSalida, Descripcion FROM TurnosTrabajo WHERE Activo = 1");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    TurnoTrabajo aux = new TurnoTrabajo();
                    aux.Id = Convert.ToInt32(datos.Lector["Id"]);
                    aux.Nombre = datos.Lector["Nombre"].ToString();
                    aux.HoraEntrada = (TimeSpan)datos.Lector["HoraEntrada"];
                    aux.HoraSalida = (TimeSpan)datos.Lector["HoraSalida"];
                    aux.Descripcion = datos.Lector["Descripcion"] != DBNull.Value ? datos.Lector["Descripcion"].ToString() : "";
                    aux.Activo = true;

                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex) { throw ex; }
            finally { datos.cerrarConexion(); }
        }

        // Registrar un nuevo turno
        public void agregar(TurnoTrabajo nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO TurnosTrabajo (Nombre, HoraEntrada, HoraSalida, Descripcion, Activo) VALUES (@nombre, @entrada, @salida, @descripcion, 1)");
                datos.setearParametro("@nombre", nuevo.Nombre);
                datos.setearParametro("@entrada", nuevo.HoraEntrada);
                datos.setearParametro("@salida", nuevo.HoraSalida);
                datos.setearParametro("@descripcion", (object)nuevo.Descripcion ?? DBNull.Value);
                datos.ejecutarAccion();
            }
            catch (Exception ex) { throw ex; }
            finally { datos.cerrarConexion(); }
        }

        // Modificar un registro existente
        public void modificar(TurnoTrabajo turno)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE TurnosTrabajo SET Nombre = @nombre, HoraEntrada = @entrada, HoraSalida = @salida, Descripcion = @descripcion WHERE Id = @id");
                datos.setearParametro("@nombre", turno.Nombre);
                datos.setearParametro("@entrada", turno.HoraEntrada);
                datos.setearParametro("@salida", turno.HoraSalida);
                datos.setearParametro("@descripcion", (object)turno.Descripcion ?? DBNull.Value);
                datos.setearParametro("@id", turno.Id);
                datos.ejecutarAccion();
            }
            catch (Exception ex) { throw ex; }
            finally { datos.cerrarConexion(); }
        }
    }
}