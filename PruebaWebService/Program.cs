using PruebaWebService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace PruebaWebService
{
    class Program
    {
        static void Main(string[] args)
        {
            
            IList<Consulta> consultas = new List<Consulta>();
            int semilla = 156268;

            for (int i = 0; i < 30; i++) {
                Consulta consulta = new Consulta();

                consulta.campos_busqueda = Convert.ToString(semilla + i);
                consulta.cod_operacion = "c";
                consultas.Add(consulta);
            }

            //Aqui se debe colocar la direccion del endPoint que quiero consultar ejemplo https://...BancoAccionSocialService.svc/restapi/consulta
            string url = "";

            TimeSpan stopAll;
            TimeSpan startAll = new TimeSpan(DateTime.Now.Ticks);

            foreach (Consulta item in consultas) {
                
                TimeSpan stop;
                TimeSpan start = new TimeSpan(DateTime.Now.Ticks);


                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.Method = "POST";
                httpWebRequest.Accept = "application/json; charset=utf-8";
                httpWebRequest.Credentials = CredentialCache.DefaultCredentials;

                string json = JsonConvert.SerializeObject(item);

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    json = json.Replace("\r\n", "");
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    string re = result.ToString();
                    Console.WriteLine("Busqueda: "+item.campos_busqueda.ToString(),"\n");
                    Console.WriteLine(re);

                    stop = new TimeSpan(DateTime.Now.Ticks);
                    Console.WriteLine("Duración: " + stop.Subtract(start).TotalMilliseconds);

                    Console.WriteLine("\n");

      
                }
            }
            stopAll = new TimeSpan(DateTime.Now.Ticks);
            Console.WriteLine("Duración Total: " + stopAll.Subtract(startAll).TotalMilliseconds);

            Console.ReadLine();
        }
    }
}
