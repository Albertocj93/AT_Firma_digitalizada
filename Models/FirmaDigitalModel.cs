﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webApiATSA.Models
{
    public class FirmaDigitalModel
    {
        public String Id_firma { get; set; }
        public int id_usuario { get; set; }
        public byte[] firma { get; set; }
        public DateTime fechaRegistro { get; set; }
        public String estado { get; set; }
    }
}
