using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class clsPedido
    {
        public int IdPedido
        {
            get;
            set;
        }

        public DateTime FechaPedido
        {
            get;
            set;
        }

        public string DireccionDestinatario
        {
            get;
            set;
        }

        public string CiudadDestinatario
        {
            get;
            set;
        }
    }
}

