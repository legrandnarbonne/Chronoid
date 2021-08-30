/*
 * Created by SharpDevelop.
 * User: DAPOJERO
 * Date: 19/03/2009
 * Time: 09:49
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Text;
using System.IO;

namespace chronoid
{
    /// <summary>
    /// Description of config.
    /// </summary>
    public static class config
    {
        public static string[] URL { get; set; }
        public static string[] Method { get; set; }
        public static string[] LogsPath { get; set; }
        public static string User { get; set; }
        public static string Password { get; set; }
        public static string Domain { get; set; }

        public static string LastErreur { get; set; }

        public static bool charge(string fileName)
        {

            if (string.IsNullOrEmpty(fileName)) fileName = "chronoid.ini";


            FileInfo fi = new FileInfo(fileName);

            if (fi.Exists)
            {
                StreamReader sr = new StreamReader(fileName, Encoding.Default);
                string s = sr.ReadToEnd();
                sr.Close();
                string[] conf = s.Split('\n');
                int cmpt = 0;


                while (cmpt < (conf.Length - 1))
                {
                    string val = conf[cmpt].Trim();
                    var part = val.Split(new char[] { '=' }, 2);
                    if (part.Length > 1)
                        try
                        {
                            switch (part[0].ToLower())
                            {
                                case "urls":
                                    URL = part[1].Split(';');
                                    break;
                                case "metode":
                                    Method = part[1].Split(';');
                                    break;
                                case "logs":
                                    LogsPath = part[1].Split(';');
                                    break;
                                case "domaine":
                                    Domain = val;
                                    break;
                                case "utilisateur":
                                    User = part[1];
                                    break;
                                case "motdepasse":
                                    Password = part[1];
                                    break;
                            }
                        }
                        catch (Exception e)
                        {
                            LastErreur = e.Message;
                            return false;
                        }
                    cmpt++;
                }
                return true;
            }
            else
            {
                LastErreur = $"Fichier {fileName} introuvable.";
                return false;
            }
        }
    }
}
