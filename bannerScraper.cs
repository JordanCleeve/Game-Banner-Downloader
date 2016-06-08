using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Net;
using System;
using System.Linq;

class Program
{
    static void Main()
    {
		List<string> games = new List<string>();
		Console.WriteLine("Enter game names one at a time followed by the enter key. Enter done with finished with input");
		Console.Write("-> ");
		string input = Console.ReadLine();
		while (input != "done") {
			games.Add(input);
			Console.Write("-> ");
			input = Console.ReadLine();
		}
		Console.WriteLine("=Downloading=");
		//List<string> games = new List<string> {"Path of Exile","Blade and Soul","Hyper Light Drifter","Battlefield 4","Space Engineers","kingdom","League of Legends", "Mini Metro", "Skyrim", "Chivalry Medieval Warfare", "Supreme Commander"};

		WebClient w = new WebClient();
		foreach (string game in games)
		{
			string s1 = w.DownloadString("http://steambanners.booru.org/index.php?page=post&s=list&tags=" + game.Replace(" ", "+"));
			List<string> ids = FindID(s1);
			int i = 0;
			foreach (string id in ids){
				i++;
				string s2 = w.DownloadString("http://steambanners.booru.org/index.php?page=post&s=view&id=" + id);
				string link = FindImgLink(s2);
				w.DownloadFile(link, game + " " + i + "." + link.Substring(link.Length - 3));
			}
			
		}
		Console.WriteLine("Finished, press any key to exit");
		Console.ReadKey();
    }

	public static List<string> FindID(string file)
	{
		List<string> list = new List<string>();

		MatchCollection m1 = Regex.Matches(file, @"(<a.*?>.*?</a>)", RegexOptions.Singleline);

		foreach (Match m in m1)
		{
			string value = m.Groups[1].Value;
			Match m2 = Regex.Match(value, @"href=\""(index.php\?page=post&amp;s=view&amp;id=[0-9]+)\""",
			RegexOptions.Singleline);
			if (m2.Success)
			{
				string v = m2.Groups[1].Value;
				Match m3 = Regex.Match(v, @"[0-9]+", RegexOptions.Singleline);
				if (m3.Success)
				{
					list.Add(m3.Value);
				}
			}
		}
		return list;
	}
	
	public static string FindImgLink(string file)
	{
		string link = null;

		Match m1 = Regex.Match(file, @"http://img\.booru\.org/steambanners//images.*?(jpg|png)",
			RegexOptions.Singleline);
		if (m1.Success)
		{
			link = m1.Value;
		}
		return link;
	}
}

