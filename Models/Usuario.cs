using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class Usuario
    {
        public int id { get;set; }
        public string? nome { get;set; }
        public string? email { get;set; }
        public string? telefone{ get;set; }
        public string? senha { get;set; }
        public string? qrcode { get;set; }
        public int tipo { get;set; }
    }
}