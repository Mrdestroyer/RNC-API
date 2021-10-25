using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RNC_API.Servicios
{
    public class ServicesController
    {

        public void IniciaDataEstractorService()
        {
            IServicio servicoS = ContribuyenteService.GetInstance();

            Thread HiloServicioCotribuyente = new Thread(new ThreadStart(servicoS.Run));
            HiloServicioCotribuyente.Start();
        }
    }
}
