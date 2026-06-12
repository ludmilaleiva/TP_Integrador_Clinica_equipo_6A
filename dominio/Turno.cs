using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Turno
    {
        public int Id { get; set; }
        public string Numero { get; set; } // Código auto-generado (Ej: T2026-00001)
        public Paciente Paciente { get; set; }
        public Medico Medico { get; set; }
        public Especialidad Especialidad { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public EstadoTurno Estado { get; set; }
        public string ObservacionesPaciente { get; set; } // Causa del turno
        public string ObservacionesMedico { get; set; }   // Diagnóstico médico
        public DateTime FechaAlta { get; set; }
        public int? UsuarioAltaId { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? UsuarioModifId { get; set; }
        public int? TurnoOriginalId { get; set; } // Si es reprogramado, apunta al original
    }
}
