using RNC_API.Modelo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace RNC_API.Servicios
{
    public class ContribuyenteService : IServicio
    {
        private static ContribuyenteService contribuyenteService;
        private String nombreArchivo = "DGII_RNC.TXT";
        private String rutaGuardado = @"./TMP/";
        public string rutaArchivoDatos;

        private String rutaArchivoZip;

        private bool Extraido;

        public const String URL_ARCHIVO_DATA = @"https://dgii.gov.do/app/WebApps/Consultas/RNC/DGII_RNC.zip";
        private const String RUTA_ARCHIVO_ZIP = @"./DGII_RNC.zip";

        //private companias_dbContext _context;
        private ContribuyenteModel contribuyenteModel;
        private ArchivoHelper archivoHelper;
        private ContribuyenteService()
        {
            rutaArchivoDatos = rutaGuardado + nombreArchivo;
           
            Extraido = false;
            /* _context = new companias_dbContext();*/
            contribuyenteModel = new ContribuyenteModel();
            archivoHelper = new ArchivoHelper();
        }

        public static IServicio GetInstance()
        {
            if(contribuyenteService == null)
            {
                contribuyenteService = new ContribuyenteService();
            }
            return contribuyenteService;
        }

        public void Run()
        {
            try
            {
                while (true)
                {
                    DateTime f = DateTime.Now;
                    int hora = f.Hour;
                    int min = f.Minute;

                    if (hora == 0 && min >= 0 && min < 4) //Todos los dias a las 12AM
                    {
                        if (Extraido == false)
                        {
                            Debug.WriteLine("Las condiciones se cumplen, procesando...");
                            List<Contribuyente> contribuyentesFromArhivo = this.ExtraeListadoContribuyentes();
                            List<Contribuyente> contribuyentesFromBBBDD = contribuyenteModel.GetListaContribuyentes();

                            //INICIAR LA COMPARACION
                            List<Contribuyente> listaFaltante = this.GetListaFaltante(contribuyentesFromArhivo, contribuyentesFromBBBDD);
                            contribuyenteModel.InsertaContribuyente(listaFaltante);
                            Extraido = true;
                        }
                        
                    }
                    if(min >= 4)
                    {
                        Extraido = false;
                    }
                    Thread.Sleep(30000);
                }
            }catch(Exception e)
            {
                Console.WriteLine(e);
                Thread.Sleep(10000);
                Run();
            }
        }
        public void Stop()
        {

        }

        /*  
         *  Devuelve una lista de contribuyentes que estan en el conjuntoA que no se encuentran en el conjuntoB
         *  tomando como referencia su RNC
         */
        public List<Contribuyente> GetListaFaltante(List<Contribuyente> conjuntoA, List<Contribuyente> conjuntoB)
        {
            Debug.WriteLine("Obteniendo lista faltante ");
            List<Contribuyente> faltantes = new List<Contribuyente>();

            bool encontrado;
            for (int i = 0; i < conjuntoA.Count; i++)
            {
                encontrado = false;
                for (int j = 0; j < conjuntoB.Count; j++)
                {
                    if (conjuntoA[i].Rnc.Equals(conjuntoB[j].Rnc))
                    {
                        encontrado = true;
                        //Eliminar registro del registro 2 para acortar las busquedas siguientes
                        conjuntoB.RemoveAt(j);
                        break;
                    }
                }
                if (encontrado == false)
                {
                    faltantes.Add(conjuntoA[i]);
                }
            }

            Debug.WriteLine("Cantidad de nueos registros: {0}", faltantes.Count);
            for(int i = 0; i < faltantes.Count; i++)
            {
                Debug.WriteLine(faltantes[i].Rnc);
            }
            return faltantes;
        }


        private List<Contribuyente> ExtraeListadoContribuyentes()
        {
            //Descargar archivo
            Task h = Task.Factory.StartNew(() => this.archivoHelper.DescargaArchivo(URL_ARCHIVO_DATA, RUTA_ARCHIVO_ZIP) );
            //Descargar archivo
            Task.WaitAll(h);

            Task desc = Task.Factory.StartNew(() => this.archivoHelper.DescomprimeArchivoZip(RUTA_ARCHIVO_ZIP, @"./"));
            Task.WaitAll(desc);
            //this.DescargarArchivoDatos(this.rutaGuardado);


            Debug.WriteLine("Obteniendo listado ...");
            List<Contribuyente> listaContribuy = new List<Contribuyente>();

            using (StreamReader sr = new StreamReader(rutaArchivoDatos, Encoding.GetEncoding("iso-8859-1")))
            {
                String s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    Contribuyente contribuyente = new Contribuyente();
                    String[] data = s.Split('|');
                    if (!data[0].Trim().Equals("")) //ignorar empresas sin RNC
                    {
                        contribuyente.Rnc = this.EliminarSobreEspacio(data[0]);
                        contribuyente.RazonSocial = this.EliminarSobreEspacio(data[1]);

                        if (data.Length > 2)
                        {
                            if (!data[2].Trim().Equals(""))
                            {
                                contribuyente.NombreComercial = this.EliminarSobreEspacio(data[2]);
                            }
                            else
                            {
                                contribuyente.NombreComercial = "N/A";
                            }
                        }
                        else
                        {
                            contribuyente.NombreComercial = "N/A";
                        }

                        listaContribuy.Add(contribuyente);
                    }
                }
            }

            return listaContribuy;
        }

       

        public void ExportInformacionContribuyentes(List<Contribuyente> empresas, String rutaArchivo)
        {
            Console.WriteLine("Generando");
            using (StreamWriter sw = File.AppendText(rutaArchivo))
            {
                for (int i = 0; i < empresas.Count; i++)
                {
                    sw.WriteLine(empresas[i].Rnc + "|" + empresas[i].RazonSocial + "|" + empresas[i].NombreComercial);
                }
            }

        }

        /**
            * RECIBE UN TEXTO Y LO DEVUELVE CON LOS SOBRE-ESPACIOS ELIMINADOS
        */
        private String EliminarSobreEspacio(String texto)
        {
            RegexOptions opciones = RegexOptions.None;
            Regex regex = new Regex(@"[ ]{2,}", opciones);

            String nuevoTexto = regex.Replace(texto, " ");

            return nuevoTexto;
        }
    }
}
