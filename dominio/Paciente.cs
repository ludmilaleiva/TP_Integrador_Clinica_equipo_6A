using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string DNI { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public char Sexo { get; set; } // 'M', 'F', 'O'
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string ObraSocial { get; set; }
        public string NroAfiliado { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaAlta { get; set; }
            
    

        public string DatosCompletosCombo
        {
            get
            {
                // Esto va a armar la cadena: "Pérez, Juan Carlos (DNI: 35123456)"
                return $"{Apellido}, {Nombre} (DNI: {DNI})";
            }
        }
    }


}
