using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class TurnoTrabajo
    {
        public int Id { get; set; }
        public string Nombre { get; set; } // Mañana, Tarde, Noche, Completo
        // Usamos TimeSpan en C# porque mapea directo con el tipo TIME de SQL
        public TimeSpan HoraEntrada { get; set; }
        public TimeSpan HoraSalida { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
    } 
}
