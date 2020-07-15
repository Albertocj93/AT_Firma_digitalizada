using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webApiATSA.Models
{
    public class FirmaDigitalHistModel
    {
        public String Id_firma { get; set; }
        public int id_usuario { get; set; }
        public int codigoSerie { get; set; }
        public DateTime fecha { get; set; }
        public String nombreDocumento { get; set; }
        public String sistema { get; set; }
        public String ip { get; set; }
        public String localizacion { get; set; }
        public String estado { get; set; }
    }
}
