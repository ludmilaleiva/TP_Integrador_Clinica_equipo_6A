using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Medico
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string DNI { get; set; }
        public string Matricula { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public TurnoTrabajo TurnoTrabajo { get; set; } // Relación con el turno asignado
        public Usuario Usuario { get; set; } // Puede ser NULL si el médico no entra al sistema
        // Relación N:N de Medico_Especialidades: una lista con todas sus especialidades
        public List<Especialidad> Especialidades { get; set; }
        public bool Activo { get; set; }
    }
}
