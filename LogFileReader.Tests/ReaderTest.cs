using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Reflection;

namespace LogFileReader.Tests
{
	[TestClass]
	public class ReaderTest
	{
		[TestMethod]
		public void TestUniqueIPs()
		{
			var fileReader = new LogFileParser(AppContext.BaseDirectory, @"logs\programming-task-example-data.log");

			fileReader.Parse();
			
			Assert.IsTrue(fileReader.IpList.Distinct().Count() == 8, $"expected 8 found {fileReader.IpList.Distinct().Count()}");
		}

		[TestMethod]
		public void Testtop3IPs()
		{

			var fileReader = new LogFileParser(AppContext.BaseDirectory, @"logs\programming-task-example-data.log");

			fileReader.Parse();

			var top3ips = fileReader.GetTopThreeActiveIPs();

			Assert.AreEqual($"IPAddress:{top3ips[0].Key},Count:{top3ips[0].Value}",
				"IPAddress:168.41.191.40,Count:5", top3ips[0].ToString());

			Assert.AreEqual($"IPAddress:{top3ips[1].Key},Count:{top3ips[1].Value}",
	"IPAddress:177.71.128.21,Count:4", top3ips[1].ToString());

			Assert.AreEqual($"IPAddress:{top3ips[2].Key},Count:{top3ips[2].Value}",
	"IPAddress:72.44.32.10,Count:3", top3ips[1].ToString());
		}

		[TestMethod]
		public void Testtop3URLs()
		{
			var fileReader = new LogFileParser(AppContext.BaseDirectory, @"logs\programming-task-example-data.log");

			fileReader.Parse();

			var top3urls = fileReader.GetTopThreeVisitedURLs();

			Assert.AreEqual($"url:{top3urls[0].Key},Count:{top3urls[0].Value}",
				"url:GET /intranet-analytics/ ,Count:2", top3urls[0].ToString());

			Assert.AreEqual($"url:{top3urls[1].Key},Count:{top3urls[1].Value}",
	"url:GET http://example.net/faq/ ,Count:2", top3urls[1].ToString());

			Assert.AreEqual($"url:{top3urls[2].Key},Count:{top3urls[2].Value}",
	"url:GET /this/page/does/not/exist/ ,Count:2", top3urls[1].ToString());
		}
	}
}
