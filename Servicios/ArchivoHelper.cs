using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RNC_API.Servicios
{
    public class ArchivoHelper
    {
        public ArchivoHelper() { }

        /**
         *  Descomprime archivo zip en la ruta Especificada
         */
        public void DescomprimeArchivoZip(String rutaArchivo, String rutaDestino)
        {
            try
            {
                Debug.WriteLine("Descomprimiendo archivo");
                ZipFile.ExtractToDirectory(rutaArchivo, rutaDestino);
                Debug.WriteLine("Archivo descomprimido");
            }catch(Exception e)
            {
                Debug.WriteLine("Error {0}", e);
            }
        }
        /*
         *  Descomprime archivo zip en la ruta Especificada y si eliminaArchivoDespuesTerminar es = True elimina el archivo zip
         *  luego de su extraccion
         */
        public void DescomprimeArchivoZip(String rutaArchivo, String rutaDestino, bool eliminaArchivoDespuesTerminar)
        {
            try
            {
                Debug.WriteLine("Descomprimiendo archivo");
                ZipFile.ExtractToDirectory(rutaArchivo, rutaDestino);
                if (eliminaArchivoDespuesTerminar)
                {
                    File.Delete(rutaDestino);
                }
                Debug.WriteLine("Archivo descomprimido");
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error {0}", e);
            }
        }


        public void DescargaArchivo(String urlDescarga, String rutaGuardadoArchivo)
        {
            Debug.WriteLine("Descargando archivo archivo helper");
            if (File.Exists(rutaGuardadoArchivo))
            {
                File.Delete(rutaGuardadoArchivo); // ELIMINAR EL ARCHIVO DE DATOS PARA SER REEMPLAZADO POR EL NUEVO
            }

            DateTime startTime = DateTime.UtcNow;
            WebRequest request = WebRequest.Create(urlDescarga);
            WebResponse response = request.GetResponse();
            
            using (Stream responseStream = response.GetResponseStream())
            {
                using (Stream fileStream = File.OpenWrite(rutaGuardadoArchivo))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead = responseStream.Read(buffer, 0, 4096);
                    while (bytesRead > 0)
                    {
                        fileStream.Write(buffer, 0, bytesRead);
                        DateTime nowTime = DateTime.UtcNow;
                        if ((nowTime - startTime).TotalMinutes > 5)
                        {
                            throw new ApplicationException(
                                "Download timed out");
                        }
                        bytesRead = responseStream.Read(buffer, 0, 4096);
                    }

                    fileStream.Close();
                }

                responseStream.Close();
            }
            Debug.WriteLine("Fin descarga en archivoHelper");
        }
    }
}
