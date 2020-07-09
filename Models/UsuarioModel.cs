using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq; 
using System.Threading.Tasks;
using System.Web;


namespace webApiATSA.Models
{
    public class PersonalModel
    {
        //[Required]
        public int id { get; set; }
        //[Required]
        public String NombresCompletos { get; set; }
        public String apellidos { get; set; }
        public String Email { get; set; }
        public String CodigoSAP { get; set; }
        public Byte activo { get; set; }
        public Byte MecanicoLinea { get; set; }
        public int id_gerencia { get; set; }
        public String dni { get; set; }
        public String puestoTrabajo { get; set; }
        public String licencia { get; set; }
    }

    public class UsuarioModel
    {
        public int id { get; set; }
        public String Usr { get; set; }
        public String Pwd { get; set; }
        public DateTime FecReg { get; set; }
        public int id_personal { get; set; }
    }

    public class UsuarioPerfilModel
    {
        public int id { get; set; }
        public int id_usuario { get; set; }
        public int id_perfil { get; set; }
    }
}
