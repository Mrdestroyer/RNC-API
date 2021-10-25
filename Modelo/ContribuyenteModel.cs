using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RNC_API.Modelo
{
    public class ContribuyenteModel
    {
        private companias_dbContext _context;
        public ContribuyenteModel()
        {
            _context = new companias_dbContext();
        }

        public List<Contribuyente> GetListaContribuyentes()
        {
            return _context.Contribuyentes.ToList();
        }
        /**
         *  Busca un contribuyente por su razon social
         */
        public Contribuyente GetContribuyentePorRazonSocial(String razonSocial)
        {
            return _context.Contribuyentes.Where(s => s.RazonSocial.Equals(razonSocial)).FirstOrDefault<Contribuyente>();
        }
        public Contribuyente GetContribuyentePorRnc(String rnc)
        {
            return _context.Contribuyentes
                .Where(s => s.Rnc == rnc).FirstOrDefault<Contribuyente>();

        }
        /*
         * 
         * Inserta un contribuyente
         * 
         */
        public void InsertaContribuyente(Contribuyente contribuyente)
        {
            this._context.Contribuyentes.Add(contribuyente);
            this._context.SaveChanges();
        }

        /*
         * INSERTA UNA LISTA DE CONTRIBUYENTE
         */
        public void InsertaContribuyente(List<Contribuyente> listaContribuyentes)
        {
            int cuenta = 0;
            Debug.WriteLine("Insertando contribuyentes ...");
            foreach (Contribuyente contribuyente in listaContribuyentes)
            {
                if (cuenta % 10000 == 0) //guardar cambios cada 10,000 registros
                {
                    this._context.SaveChanges();
                }
                this._context.Contribuyentes.Add(contribuyente);
                cuenta++;
            }
            this._context.SaveChanges();

            Debug.WriteLine("Listas guardadas");
;        }
    }
}
