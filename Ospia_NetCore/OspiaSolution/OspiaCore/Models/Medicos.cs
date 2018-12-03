using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OspiaCore.Models
{
    public class Medicos
    {
        [Key]
        public int NumeroMatricula { get; set; }
        public int DNI { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string Especialidad { get; set; }
        public string Celular { get; set; }
        public string Domicilio { get; set; }
    }
}
