using dominio;
using System;
using System.Collections.Generic;
using acceso_datos;

namespace negocio
{
    public class MedicoNegocio
    {
        // LISTAR TODOS LOS MÉDICOS ACTIVOS
        public List<Medico> listar()
        {
            List<Medico> lista = new List<Medico>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
              
                datos.setearConsulta(@"
                    SELECT m.Id, m.Nombre, m.Apellido, m.Matricula, m.Email,
                           e.Id AS EspecialidadId, e.Nombre AS EspecialidadNombre
                    FROM Medicos m
                    LEFT JOIN Medico_Especialidades me ON m.Id = me.MedicoId
                    LEFT JOIN Especialidades e ON me.EspecialidadId = e.Id
                    WHERE m.Activo = 1 
                    ORDER BY m.Apellido ASC");

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Medico aux = new Medico();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Apellido = (string)datos.Lector["Apellido"];
                    aux.Matricula = (string)datos.Lector["Matricula"];
                    aux.Email = datos.Lector["Email"] != DBNull.Value ? (string)datos.Lector["Email"] : "";
                    aux.Especialidades = new List<Especialidad>(); 

                    Especialidad esp = new Especialidad();
                    esp.Id = datos.Lector["EspecialidadId"] != DBNull.Value ? Convert.ToInt32(datos.Lector["EspecialidadId"]) : 0;
                    esp.Nombre = datos.Lector["EspecialidadNombre"] != DBNull.Value ? datos.Lector["EspecialidadNombre"].ToString() : "Sin Asignar";
                    aux.Especialidades.Add(esp);
                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex) { throw ex; }
            finally { datos.cerrarConexion(); }
        }

        // LISTAR POR ESPECIALIDAD (Útil para filtrar médicos al asignar un turno)
        public List<Medico> listarPorEspecialidad(int idEspecialidad)
        {
            List<Medico> lista = new List<Medico>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
                    SELECT m.Id, m.Nombre, m.Apellido, m.Matricula, m.Email 
                    FROM Medicos m 
                    INNER JOIN Medico_Especialidades me ON m.Id = me.MedicoId 
                    WHERE me.EspecialidadId = @EspecialidadId AND m.Activo = 1");

                datos.setearParametro("@EspecialidadId", idEspecialidad);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Medico aux = new Medico();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Apellido = (string)datos.Lector["Apellido"];
                    aux.Matricula = (string)datos.Lector["Matricula"];
                    aux.Email = datos.Lector["Email"] != DBNull.Value ? (string)datos.Lector["Email"] : "";
                    aux.Especialidades = new List<Especialidad>();

                    Especialidad esp = new Especialidad();
                    esp.Id = datos.Lector["EspecialidadId"] != DBNull.Value ? Convert.ToInt32(datos.Lector["EspecialidadId"]) : 0;
                    esp.Nombre = datos.Lector["EspecialidadNombre"] != DBNull.Value ? datos.Lector["EspecialidadNombre"].ToString() : "Sin Asignar";

                    //guardamos adentro de lista de Especialidades
                    aux.Especialidades.Add(esp);

                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex) { throw ex; }
            finally { datos.cerrarConexion(); }
        }

        //  AGREGAR MÉDICO Y SU ESPECIALIDAD 
        public void agregar(Medico nuevo, int idEspecialidad)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
                    INSERT INTO Medicos (Nombre, Apellido, Matricula, Email, Activo) 
                    VALUES (@nombre, @apellido, @matricula, @email, 1);
                    SELECT SCOPE_IDENTITY();");

                datos.setearParametro("@nombre", nuevo.Nombre);
                datos.setearParametro("@apellido", nuevo.Apellido);
                datos.setearParametro("@matricula", nuevo.Matricula);
                datos.setearParametro("@email", nuevo.Email);

                int idMedicoInsertado = datos.ejecutarAccionScalar();
                datos.cerrarConexion();

                // relación en la tabla intermedia Medico_Especialidades
                datos = new AccesoDatos();
                datos.setearConsulta("INSERT INTO Medico_Especialidades (MedicoId, EspecialidadId) VALUES (@medicoId, @especialidadId)");
                datos.setearParametro("@medicoId", idMedicoInsertado);
                datos.setearParametro("@especialidadId", idEspecialidad);
                datos.ejecutarAccion();
            }
            catch (Exception ex) { throw ex; }
            finally { datos.cerrarConexion(); }
        }

        // MODIFICAR DATOS BÁSICOS DEL MÉDICO
        public void modificar(Medico med)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE Medicos SET Nombre = @nombre, Apellido = @apellido, Matricula = @matricula, Email = @email WHERE Id = @id");
                datos.setearParametro("@nombre", med.Nombre);
                datos.setearParametro("@apellido", med.Apellido);
                datos.setearParametro("@matricula", med.Matricula);
                datos.setearParametro("@email", med.Email);
                datos.setearParametro("@id", med.Id);
                datos.ejecutarAccion();
            }
            catch (Exception ex) { throw ex; }
            finally { datos.cerrarConexion(); }
        }
    }
}