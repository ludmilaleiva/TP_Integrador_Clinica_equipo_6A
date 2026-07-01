USE [master]
GO
/****** Objeto: Database [ClinicaTurnos] Fecha de script: 1/7/2026 16:58:08 ******/
CREATE DATABASE [ClinicaTurnos]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ClinicaTurnos', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL17.SQLEXPRESS\MSSQL\DATA\ClinicaTurnos.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ClinicaTurnos_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL17.SQLEXPRESS\MSSQL\DATA\ClinicaTurnos_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [ClinicaTurnos] SET COMPATIBILITY_LEVEL = 170
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ClinicaTurnos].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ClinicaTurnos] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ClinicaTurnos] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ClinicaTurnos] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ClinicaTurnos] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ClinicaTurnos] SET ARITHABORT OFF 
GO
ALTER DATABASE [ClinicaTurnos] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [ClinicaTurnos] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ClinicaTurnos] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ClinicaTurnos] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ClinicaTurnos] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ClinicaTurnos] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ClinicaTurnos] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ClinicaTurnos] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ClinicaTurnos] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ClinicaTurnos] SET  ENABLE_BROKER 
GO
ALTER DATABASE [ClinicaTurnos] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ClinicaTurnos] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ClinicaTurnos] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ClinicaTurnos] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ClinicaTurnos] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ClinicaTurnos] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ClinicaTurnos] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ClinicaTurnos] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ClinicaTurnos] SET  MULTI_USER 
GO
ALTER DATABASE [ClinicaTurnos] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ClinicaTurnos] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ClinicaTurnos] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ClinicaTurnos] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ClinicaTurnos] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ClinicaTurnos] SET OPTIMIZED_LOCKING = OFF 
GO
ALTER DATABASE [ClinicaTurnos] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [ClinicaTurnos] SET QUERY_STORE = ON
GO
ALTER DATABASE [ClinicaTurnos] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [ClinicaTurnos]
GO
/****** Objeto: Table [dbo].[Especialidades] Fecha de script: 1/7/2026 16:58:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Especialidades](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Activo] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[EstadosTurno] Fecha de script: 1/7/2026 16:58:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstadosTurno](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Medico_Especialidades] Fecha de script: 1/7/2026 16:58:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Medico_Especialidades](
	[MedicoId] [int] NOT NULL,
	[EspecialidadId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MedicoId] ASC,
	[EspecialidadId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Medicos] Fecha de script: 1/7/2026 16:58:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Medicos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Apellido] [varchar](100) NOT NULL,
	[DNI] [varchar](20) NOT NULL,
	[Matricula] [varchar](50) NOT NULL,
	[Email] [varchar](150) NOT NULL,
	[Telefono] [varchar](30) NULL,
	[TurnoTrabajoId] [int] NOT NULL,
	[UsuarioId] [int] NULL,
	[Activo] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Matricula] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[DNI] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Pacientes] Fecha de script: 1/7/2026 16:58:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pacientes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Apellido] [varchar](100) NOT NULL,
	[DNI] [varchar](20) NOT NULL,
	[FechaNacimiento] [date] NOT NULL,
	[Sexo] [char](1) NOT NULL,
	[Email] [varchar](150) NULL,
	[Telefono] [varchar](30) NULL,
	[Direccion] [varchar](250) NULL,
	[ObraSocial] [varchar](100) NULL,
	[NroAfiliado] [varchar](50) NULL,
	[Activo] [bit] NOT NULL,
	[FechaAlta] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[DNI] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Perfiles] Fecha de script: 1/7/2026 16:58:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Perfiles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Descripcion] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Turnos] Fecha de script: 1/7/2026 16:58:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Turnos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Numero] [varchar](20) NOT NULL,
	[PacienteId] [int] NOT NULL,
	[MedicoId] [int] NOT NULL,
	[EspecialidadId] [int] NOT NULL,
	[Fecha] [date] NOT NULL,
	[HoraInicio] [time](7) NOT NULL,
	[HoraFin] [time](7) NOT NULL,
	[EstadoId] [int] NOT NULL,
	[ObservacionesPaciente] [varchar](500) NULL,
	[ObservacionesMedico] [varchar](1000) NULL,
	[FechaAlta] [datetime] NOT NULL,
	[UsuarioAltaId] [int] NULL,
	[FechaModificacion] [datetime] NULL,
	[UsuarioModifId] [int] NULL,
	[TurnoOriginalId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Numero] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Turno_Medico_FechaHora] UNIQUE NONCLUSTERED 
(
	[MedicoId] ASC,
	[Fecha] ASC,
	[HoraInicio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Turno_Paciente_FechaHora] UNIQUE NONCLUSTERED 
(
	[PacienteId] ASC,
	[Fecha] ASC,
	[HoraInicio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[TurnosTrabajo] Fecha de script: 1/7/2026 16:58:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TurnosTrabajo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[HoraEntrada] [time](7) NOT NULL,
	[HoraSalida] [time](7) NOT NULL,
	[Descripcion] [varchar](200) NULL,
	[Activo] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Usuarios] Fecha de script: 1/7/2026 16:58:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Email] [varchar](150) NOT NULL,
	[PasswordHash] [varchar](256) NOT NULL,
	[PerfilId] [int] NOT NULL,
	[Activo] [bit] NOT NULL,
	[FechaAlta] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Especialidades] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Medicos] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Pacientes] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Pacientes] ADD  DEFAULT (getdate()) FOR [FechaAlta]
GO
ALTER TABLE [dbo].[Turnos] ADD  DEFAULT (getdate()) FOR [FechaAlta]
GO
ALTER TABLE [dbo].[TurnosTrabajo] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Usuarios] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Usuarios] ADD  DEFAULT (getdate()) FOR [FechaAlta]
GO
ALTER TABLE [dbo].[Medico_Especialidades]  WITH CHECK ADD  CONSTRAINT [FK_ME_Especialidades] FOREIGN KEY([EspecialidadId])
REFERENCES [dbo].[Especialidades] ([Id])
GO
ALTER TABLE [dbo].[Medico_Especialidades] CHECK CONSTRAINT [FK_ME_Especialidades]
GO
ALTER TABLE [dbo].[Medico_Especialidades]  WITH CHECK ADD  CONSTRAINT [FK_ME_Medicos] FOREIGN KEY([MedicoId])
REFERENCES [dbo].[Medicos] ([Id])
GO
ALTER TABLE [dbo].[Medico_Especialidades] CHECK CONSTRAINT [FK_ME_Medicos]
GO
ALTER TABLE [dbo].[Medicos]  WITH CHECK ADD  CONSTRAINT [FK_Medicos_TurnosTrabajo] FOREIGN KEY([TurnoTrabajoId])
REFERENCES [dbo].[TurnosTrabajo] ([Id])
GO
ALTER TABLE [dbo].[Medicos] CHECK CONSTRAINT [FK_Medicos_TurnosTrabajo]
GO
ALTER TABLE [dbo].[Medicos]  WITH CHECK ADD  CONSTRAINT [FK_Medicos_Usuarios] FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[Usuarios] ([Id])
GO
ALTER TABLE [dbo].[Medicos] CHECK CONSTRAINT [FK_Medicos_Usuarios]
GO
ALTER TABLE [dbo].[Turnos]  WITH CHECK ADD  CONSTRAINT [FK_Turnos_Especialidades] FOREIGN KEY([EspecialidadId])
REFERENCES [dbo].[Especialidades] ([Id])
GO
ALTER TABLE [dbo].[Turnos] CHECK CONSTRAINT [FK_Turnos_Especialidades]
GO
ALTER TABLE [dbo].[Turnos]  WITH CHECK ADD  CONSTRAINT [FK_Turnos_Estados] FOREIGN KEY([EstadoId])
REFERENCES [dbo].[EstadosTurno] ([Id])
GO
ALTER TABLE [dbo].[Turnos] CHECK CONSTRAINT [FK_Turnos_Estados]
GO
ALTER TABLE [dbo].[Turnos]  WITH CHECK ADD  CONSTRAINT [FK_Turnos_Medicos] FOREIGN KEY([MedicoId])
REFERENCES [dbo].[Medicos] ([Id])
GO
ALTER TABLE [dbo].[Turnos] CHECK CONSTRAINT [FK_Turnos_Medicos]
GO
ALTER TABLE [dbo].[Turnos]  WITH CHECK ADD  CONSTRAINT [FK_Turnos_Original] FOREIGN KEY([TurnoOriginalId])
REFERENCES [dbo].[Turnos] ([Id])
GO
ALTER TABLE [dbo].[Turnos] CHECK CONSTRAINT [FK_Turnos_Original]
GO
ALTER TABLE [dbo].[Turnos]  WITH CHECK ADD  CONSTRAINT [FK_Turnos_Pacientes] FOREIGN KEY([PacienteId])
REFERENCES [dbo].[Pacientes] ([Id])
GO
ALTER TABLE [dbo].[Turnos] CHECK CONSTRAINT [FK_Turnos_Pacientes]
GO
ALTER TABLE [dbo].[Turnos]  WITH CHECK ADD  CONSTRAINT [FK_Turnos_UsuarioAlta] FOREIGN KEY([UsuarioAltaId])
REFERENCES [dbo].[Usuarios] ([Id])
GO
ALTER TABLE [dbo].[Turnos] CHECK CONSTRAINT [FK_Turnos_UsuarioAlta]
GO
ALTER TABLE [dbo].[Turnos]  WITH CHECK ADD  CONSTRAINT [FK_Turnos_UsuarioModif] FOREIGN KEY([UsuarioModifId])
REFERENCES [dbo].[Usuarios] ([Id])
GO
ALTER TABLE [dbo].[Turnos] CHECK CONSTRAINT [FK_Turnos_UsuarioModif]
GO
ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD  CONSTRAINT [FK_Usuarios_Perfiles] FOREIGN KEY([PerfilId])
REFERENCES [dbo].[Perfiles] ([Id])
GO
ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [FK_Usuarios_Perfiles]
GO
ALTER TABLE [dbo].[Pacientes]  WITH CHECK ADD CHECK  (([Sexo]='O' OR [Sexo]='F' OR [Sexo]='M'))
GO
/****** Objeto: StoredProcedure [dbo].[SP_GenerarNumeroTurno] Fecha de script: 1/7/2026 16:58:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Generar numero de turno correlativo
CREATE PROCEDURE [dbo].[SP_GenerarNumeroTurno]
    @NroTurno VARCHAR(20) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Anio CHAR(4) = CAST(YEAR(GETDATE()) AS CHAR(4));
    DECLARE @Count INT;
    SELECT @Count = COUNT(*) + 1 FROM Turnos WHERE YEAR(FechaAlta) = YEAR(GETDATE());
    SET @NroTurno = 'T' + @Anio + '-' + RIGHT('00000' + CAST(@Count AS VARCHAR), 5);
END;

GO
/****** Objeto: StoredProcedure [dbo].[SP_ObtenerHorariosDisponibles] Fecha de script: 1/7/2026 16:58:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================
-- STORED PROCEDURES
-- ============================================================

-- Obtener horarios disponibles de un medico en una fecha
CREATE PROCEDURE [dbo].[SP_ObtenerHorariosDisponibles]
    @MedicoId INT,
    @Fecha    DATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Obtener el turno de trabajo del medico
    DECLARE @HoraEntrada TIME, @HoraSalida TIME;
    SELECT @HoraEntrada = tt.HoraEntrada, @HoraSalida = tt.HoraSalida
    FROM Medicos m
    JOIN TurnosTrabajo tt ON tt.Id = m.TurnoTrabajoId
    WHERE m.Id = @MedicoId AND m.Activo = 1;

    -- Generar slots de 1 hora y excluir los ya ocupados
    WITH Slots AS (
        SELECT @HoraEntrada AS HoraSlot
        UNION ALL
        SELECT DATEADD(HOUR, 1, HoraSlot)
        FROM Slots
        WHERE DATEADD(HOUR, 1, HoraSlot) < @HoraSalida
    )
    SELECT
        s.HoraSlot                    AS HoraInicio,
        DATEADD(HOUR, 1, s.HoraSlot) AS HoraFin,
        CASE WHEN t.Id IS NULL THEN 1 ELSE 0 END AS Disponible
    FROM Slots s
    LEFT JOIN Turnos t ON t.MedicoId = @MedicoId
                       AND t.Fecha = @Fecha
                       AND t.HoraInicio = s.HoraSlot
                       AND t.EstadoId NOT IN (3) -- excluir cancelados
    OPTION (MAXRECURSION 24);
END;

GO
/****** Objeto: StoredProcedure [dbo].[SP_SugerirTurnos] Fecha de script: 1/7/2026 16:58:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Sugerir medicos y horarios por especialidad (3 sugerencias)
CREATE PROCEDURE [dbo].[SP_SugerirTurnos]
    @EspecialidadId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Proximos 7 dias, buscar slots disponibles por especialidad
    WITH FechasProximas AS (
        SELECT CAST(DATEADD(DAY, n, GETDATE()) AS DATE) AS Fecha
        FROM (VALUES(1),(2),(3),(4),(5),(6),(7)) AS D(n)
    ),
    MedicosEsp AS (
        SELECT m.Id AS MedicoId, m.Nombre + ' ' + m.Apellido AS NombreMedico,
               tt.HoraEntrada, tt.HoraSalida
        FROM Medico_Especialidades me
        JOIN Medicos m ON m.Id = me.MedicoId AND m.Activo = 1
        JOIN TurnosTrabajo tt ON tt.Id = m.TurnoTrabajoId
        WHERE me.EspecialidadId = @EspecialidadId
    ),
    Combinaciones AS (
        SELECT me.MedicoId, me.NombreMedico, fp.Fecha,
               me.HoraEntrada AS HoraInicio,
               DATEADD(HOUR, 1, me.HoraEntrada) AS HoraFin
        FROM MedicosEsp me
        CROSS JOIN FechasProximas fp
    )
    SELECT TOP 3
        c.MedicoId, c.NombreMedico, c.Fecha, c.HoraInicio, c.HoraFin
    FROM Combinaciones c
    WHERE NOT EXISTS (
        SELECT 1 FROM Turnos t
        WHERE t.MedicoId = c.MedicoId
          AND t.Fecha = c.Fecha
          AND t.HoraInicio = c.HoraInicio
          AND t.EstadoId NOT IN (3)
    )
    ORDER BY c.Fecha, c.HoraInicio;
END;

GO
USE [master]
GO
ALTER DATABASE [ClinicaTurnos] SET  READ_WRITE 
GO
