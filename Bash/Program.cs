using System;
using System.IO;
using System.Net;
using HtmlAgilityPack;

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
