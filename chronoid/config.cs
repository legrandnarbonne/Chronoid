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
		public static string[] LogsPath { get; set; }
        public static string User { get; set; }
        public static string Password { get; set; }
        public static string Domain { get; set; }

        public static string LastErreur { get; set; }

        public static bool charge()
		{
			
			FileInfo fi = new FileInfo("chronoid.ini");
			
			if (fi.Exists) 
			{
				StreamReader sr = new StreamReader("chronoid.ini", Encoding.Default);
				string s = sr.ReadToEnd();
				sr.Close();
				string[] conf=s.Split('\n','=');
				int cmpt=0;
				
				
				while (cmpt<(conf.Length-1))
				{
					string val;
					try
					{
						val=conf[cmpt+1].Trim();
						
						switch (conf[cmpt].ToLower())
						{
							case "urls":
								URL=val.Split(';');
								break;
							case "logs":
								LogsPath=val.Split(';');
								break;
							case "domaine":
								Domain=val;
								break;
							case "utilisateur":
								User=val;
								break;
							case "motdepasse":
								Password=val;
								break;
						}
					}
					catch(Exception e){
						LastErreur=e.Message;
						return false;
					}
					cmpt++;
				}
				return true;
			}
			else
			{
				LastErreur="Fichier chronoid.ini introuvable.";
				return false;
			}
		}
	}
}
