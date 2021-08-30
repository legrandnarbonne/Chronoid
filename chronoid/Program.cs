/*
 * Created by SharpDevelop.
 * User: DAPOJERO
 * Date: 27/01/2013
 * Time: 18:22
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net;
using System.Xml;
using System.Reflection;
using System.IO;

namespace chronoid
{
	class Program
	{
        static String _appPath;
		public static void Main(string[] args)
		{
            _appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var configFile = args.Length > 0 ? args[0] : null;

            if (config.charge(configFile))
			{
				for(int a=0;a<config.URL.Length;a++)
				{
					log(getServiceResult(config.URL[a],config.Method[a]),config.LogsPath[a]);
				}
			}
			else
				log(config.LastErreur, _appPath);
			
		}
		private static void log(string msg,string chemin)
		{
            var dir = new DirectoryInfo(chemin);

            dir.Create();

			chemin+="\\chronoid.log";
			Console.Write(msg);
			
			StreamWriter sw=new StreamWriter(chemin,true);
			sw.WriteLine(DateTime.Now+" "+msg);
			sw.Close();
			
		}

		public static string getServiceResult(string serviceUrl,string method) {

            log("Requesting :" + serviceUrl, _appPath);
            try
            {
                HttpWebRequest HttpWReq;
                HttpWebResponse HttpWResp;
                HttpWReq = (HttpWebRequest)WebRequest.Create(serviceUrl);

                CredentialCache cc = new CredentialCache();


                HttpWReq.Credentials = CredentialCache.DefaultCredentials;

                if (!string.IsNullOrEmpty(config.User))
                {
                    HttpWReq.Credentials = BuildCredentials(serviceUrl, config.User, config.Password, "NTLM");
                    log(config.LastErreur, _appPath);
                }

                HttpWReq.Method =string.IsNullOrEmpty(method)? "Post":method;
                HttpWReq.ContentLength = 0;



                HttpWResp = (HttpWebResponse)HttpWReq.GetResponse();

                if (HttpWResp.StatusCode == HttpStatusCode.OK)
                {
                    //Consume webservice with basic XML reading, assumes it returns (one) string
                    XmlReader reader = XmlReader.Create(HttpWResp.GetResponseStream());
                    while (reader.Read())
                    {
                        reader.MoveToFirstAttribute();
                        if (reader.NodeType == XmlNodeType.Text)
                        {
                            return reader.Value;
                        }
                    }
                    return string.Empty;
                }
                else
                {
                    log("Getting response :" + HttpWResp.StatusCode.ToString(), _appPath);
                    throw new Exception("Error on remote : " + HttpWResp.StatusCode.ToString());
                }
            }
            catch(Exception e)
            {
                log("Erreur :" + e.ToString(), _appPath);
                return null;
            }
		}

        private static ICredentials BuildCredentials(string siteurl, string username, string password, string authtype)
        {
            NetworkCredential cred;
            if (username.Contains(@"\"))
            {
                string domain = username.Substring(0, username.IndexOf(@"\", StringComparison.InvariantCulture));
                username = username.Substring(username.IndexOf(@"\", StringComparison.InvariantCulture) + 1);
                cred = new NetworkCredential(username, password, domain);
            }
            else
            {
                cred = new NetworkCredential(username, password);
            }
            CredentialCache cache = new CredentialCache();
            if (authtype.Contains(":"))
            {
                authtype = authtype.Substring(authtype.IndexOf(":",StringComparison.InvariantCulture) + 1); //remove the TMG: prefix
            }
            cache.Add(new Uri(siteurl), authtype, cred);
            return cache;
        }
    }
}