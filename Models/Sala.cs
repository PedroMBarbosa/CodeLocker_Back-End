using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class Salas
    {
        public int id { get;set; }
        public string? nome { get;set; }
        public int? id_usuario { get;set; }
        public int status_sala { get;set; }
    }
}