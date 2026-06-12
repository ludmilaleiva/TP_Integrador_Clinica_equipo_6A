using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Perfil
    {
        public int Id { get; set; }
        public string Nombre { get; set; } // Administrador, Recepcionista, Medico
        public string Descripcion { get; set; }
    }
}