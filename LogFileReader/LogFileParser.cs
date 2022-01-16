using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LogFileReader
{
	public class LogFileParser
	{
		private readonly string BaseDirectory = "";
		private readonly string FileName = "";
		public List<string> IpList { get; private set; }
		private List<string> UrlList;
		public LogFileParser(string baseDirectory, string fileName)
		{
			if (string.IsNullOrEmpty(baseDirectory))
			{
				throw new ArgumentNullException(nameof(baseDirectory), "Value cannot be null");
			}

			if (!Directory.Exists(baseDirectory))
			{
				throw new ArgumentException($"The base directory {baseDirectory} does not exist", nameof(baseDirectory));
			}

			if (string.IsNullOrWhiteSpace(fileName))
			{
				throw new ArgumentException("Filename is blank", nameof(fileName));
			}

			BaseDirectory = baseDirectory;
			FileName = fileName;
		}

		public void Parse()
		{
			IpList = new List<string>();
			UrlList = new List<string>();
			// an ip address should be thrice dot seperated and numbers should be from 0 to 255
			string ippattern = @"\b(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\." +
				 @"(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\." +
				 @"(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\." +
				 @"(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\b";

			string pattern = @"(\"".*?\"")|(\[.*?\])|(\S.*?\s)";
			var alldata = File.ReadAllLines(@$"{BaseDirectory}\{FileName}");

			foreach (var input in alldata)
			{
				foreach (Match m in Regex.Matches(input, pattern))
				{
					var ipaddress = Regex.Match(m.Value, ippattern);
					if (!string.IsNullOrWhiteSpace(ipaddress.Value))
					{
						IpList.Add(ipaddress.Value);
					}
					else
					{
						// just after http verbs urls are mentioned in the log file.
						// same url with different verb will be treated unique.
						string verbpattern = "(GET)|(POST)|(PUT)|(DELETE)|(PATCH)";

						var foundverb = Regex.Match(m.Value, verbpattern);

						if (foundverb.Index > 0)
						{
							string httppattern = "(HTTP)";
							var found = Regex.Match(m.Value, httppattern);
							UrlList.Add(m.Value.Substring(foundverb.Index, found.Index - foundverb.Index));
							break;
						}
					}
				}
			}
		}

		public List<KeyValuePair<string, int>> GetTopThreeVisitedURLs()
		{
			var uniqueUrls = UrlList.GroupBy(u => u).Select(group => new
			KeyValuePair<string, int>(group.Key, group.Count()))
				.OrderByDescending(x => x.Value).Take(3).ToList();

			return uniqueUrls;
		}

		public List<KeyValuePair<string, int>> GetTopThreeActiveIPs()
		{
			var activeips = IpList.GroupBy(u => u).Select(group => new
			KeyValuePair<string, int>(group.Key, group.Count()))
				.OrderByDescending(x => x.Value).Take(3).ToList();

			return activeips;
		}
	}
}
