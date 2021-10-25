using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RNC_API.Servicios
{
    public interface IServicio
    {
        public void Run();
        public void Stop();
    }
}
