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
                    SELECT m.Id, m.Nombre, m.Apellido,m.DNI, m.Matricula, m.Telefono,m.Email,
                           e.Id AS EspecialidadId, e.Nombre AS EspecialidadNombre,
                            tt.Id AS TurnoTrabajoId, tt.Nombre AS TurnoTrabajoNombre            
                    FROM Medicos m
                    LEFT JOIN Medico_Especialidades me ON m.Id = me.MedicoId
                    LEFT JOIN Especialidades e ON me.EspecialidadId = e.Id
                    LEFT JOIN TurnosTrabajo tt ON m.TurnoTrabajoId = tt.Id
                    WHERE m.Activo = 1 
                    ORDER BY m.Apellido ASC");

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Medico aux = new Medico();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Apellido = (string)datos.Lector["Apellido"];
                    aux.DNI = datos.Lector["DNI"] != DBNull.Value ? datos.Lector["DNI"].ToString() : "";
                    aux.Matricula = (string)datos.Lector["Matricula"];
                    aux.Telefono = datos.Lector["Telefono"] != DBNull.Value ? datos.Lector["Telefono"].ToString() : "";
                    aux.Email = datos.Lector["Email"] != DBNull.Value ? (string)datos.Lector["Email"] : "";
                    aux.Especialidades = new List<Especialidad>(); 

                    Especialidad esp = new Especialidad();
                    esp.Id = datos.Lector["EspecialidadId"] != DBNull.Value ? Convert.ToInt32(datos.Lector["EspecialidadId"]) : 0;
                    esp.Nombre = datos.Lector["EspecialidadNombre"] != DBNull.Value ? datos.Lector["EspecialidadNombre"].ToString() : "Sin Asignar";
                    aux.Especialidades.Add(esp);
                    

                    aux.TurnoTrabajo = new TurnoTrabajo();
                    aux.TurnoTrabajo.Id = datos.Lector["TurnoTrabajoId"] != DBNull.Value ? Convert.ToInt32(datos.Lector["TurnoTrabajoId"]) : 0;
                    aux.TurnoTrabajo.Nombre = datos.Lector["TurnoTrabajoNombre"] != DBNull.Value ? datos.Lector["TurnoTrabajoNombre"].ToString() : "Sin Asignar";
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
                                        SELECT 
                                            m.Id, 
                                            m.Nombre, 
                                            m.Apellido, 
                                            m.Matricula, 
                                            m.Email,
                                            e.Id AS EspecialidadId,
                                            e.Nombre AS EspecialidadNombre
                                        FROM Medicos m 
                                        INNER JOIN Medico_Especialidades me ON m.Id = me.MedicoId
                                        INNER JOIN Especialidades e ON me.EspecialidadId = e.Id
                                        WHERE me.EspecialidadId = @EspecialidadId 
                                            AND m.Activo = 1");

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

        //  AGREGAR MÉDICO, USUARIO ID Y SU ESPECIALIDAD 
        public void agregar(Medico nuevo, int idEspecialidad)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // 1) Crear usuario para el médico
                datos.setearConsulta(@"
            INSERT INTO Usuarios (Nombre, Email, PasswordHash, PerfilId, Activo, FechaAlta)
            VALUES (@nombreUsuario, @emailUsuario, @password, 3, 1, GETDATE());

            SELECT SCOPE_IDENTITY();
        ");

                datos.setearParametro("@nombreUsuario", nuevo.Nombre + " " + nuevo.Apellido);
                datos.setearParametro("@emailUsuario", nuevo.Email);
                datos.setearParametro("@password", "123456");

                int idUsuarioInsertado = datos.ejecutarAccionScalar();
                datos.cerrarConexion();

                // 2) Crear médico asociado al usuario
                datos = new AccesoDatos();

                datos.setearConsulta(@"
            INSERT INTO Medicos 
            (
                Nombre, 
                Apellido, 
                DNI, 
                Matricula, 
                Telefono,
                Email, 
                TurnoTrabajoId,
                UsuarioId,
                Activo
            ) 
            VALUES 
            (
                @nombre, 
                @apellido, 
                @dni, 
                @matricula, 
                @telefono, 
                @email, 
                @turnoId,
                @usuarioId,
                1
            );

            SELECT SCOPE_IDENTITY();
        ");

                datos.setearParametro("@nombre", nuevo.Nombre);
                datos.setearParametro("@apellido", nuevo.Apellido);
                datos.setearParametro("@dni", nuevo.DNI);
                datos.setearParametro("@matricula", nuevo.Matricula);
                datos.setearParametro("@telefono", nuevo.Telefono);
                datos.setearParametro("@email", nuevo.Email);
                datos.setearParametro("@turnoId", nuevo.TurnoTrabajo.Id);
                datos.setearParametro("@usuarioId", idUsuarioInsertado);

                int idMedicoInsertado = datos.ejecutarAccionScalar();
                datos.cerrarConexion();

                // 3) Relación médico-especialidad
                datos = new AccesoDatos();

                datos.setearConsulta(@"
            INSERT INTO Medico_Especialidades 
            (
                MedicoId, 
                EspecialidadId
            ) 
            VALUES 
            (
                @medicoId, 
                @especialidadId
            )
        ");

                datos.setearParametro("@medicoId", idMedicoInsertado);
                datos.setearParametro("@especialidadId", idEspecialidad);

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

        // MODIFICAR DATOS BÁSICOS DEL MÉDICO
        public void modificar(Medico med)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE Medicos SET Nombre = @nombre, Apellido = @apellido,DNI = @dni, Matricula = @matricula,Telefono = @telefono, Email = @email,TurnoTrabajoId = @turnoId WHERE Id = @id");
                datos.setearParametro("@nombre", med.Nombre);
                datos.setearParametro("@apellido", med.Apellido);
                datos.setearParametro("@dni", med.DNI);
                datos.setearParametro("@matricula", med.Matricula);
                datos.setearParametro("@telefono", med.Telefono);
                datos.setearParametro("@email", med.Email);
                datos.setearParametro("@turnoId", med.TurnoTrabajo.Id);
                datos.setearParametro("@id", med.Id);
                datos.ejecutarAccion();

                datos = new AccesoDatos();
                datos.setearConsulta(@"
                    UPDATE Medico_Especialidades 
                    SET EspecialidadId = @especialidadId 
                    WHERE MedicoId = @medicoId");

                datos.setearParametro("@medicoId", med.Id);
                datos.setearParametro("@especialidadId", med.Especialidades[0].Id); // Tu lista del Dominio

                datos.ejecutarAccion();
            }
            catch (Exception ex) { throw ex; }
            finally { datos.cerrarConexion(); }
        }

        public bool existeDni(string dni)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT COUNT(*) FROM Medicos WHERE Dni = @dni AND Activo = 1");
                datos.setearParametro("@dni", dni);
                return datos.ejecutarAccionScalar() > 0;
            }
            catch (Exception ex) { throw ex; }
            finally { datos.cerrarConexion(); }
        }

        public Medico buscarPorUsuarioId(int usuarioId)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
            SELECT Id, Nombre, Apellido, DNI, Matricula, Email, Telefono, UsuarioId
            FROM Medicos
            WHERE UsuarioId = @UsuarioId
              AND Activo = 1
        ");

                datos.setearParametro("@UsuarioId", usuarioId);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    Medico medico = new Medico();

                    medico.Id = Convert.ToInt32(datos.Lector["Id"]);
                    medico.Nombre = datos.Lector["Nombre"].ToString();
                    medico.Apellido = datos.Lector["Apellido"].ToString();
                    medico.DNI = datos.Lector["DNI"].ToString();
                    medico.Matricula = datos.Lector["Matricula"].ToString();
                    medico.Email = datos.Lector["Email"].ToString();
                    medico.Telefono = datos.Lector["Telefono"].ToString();

                    return medico;
                }

                return null;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}