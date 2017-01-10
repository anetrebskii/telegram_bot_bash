using System;
using System.Linq;
using System.IO;
using System.Net;
using HtmlAgilityPack;
using System.Globalization;
using System.Data.Entity.Migrations.Sql;
using System.Data.Entity.Migrations.Model;

namespace Bash
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			BashRegistry.Default.Initialize();

			Bot bot = new Bot(new BashChatInitializer());
			bot.Start();

			Console.WriteLine("Please press Enter...");
			Console.ReadLine();
			bot.Stop();
		}
	}
}
