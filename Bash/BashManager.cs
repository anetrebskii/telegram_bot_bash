using System;
using HtmlAgilityPack;

namespace Bash
{
	public class BashManager
	{
		public BashManager()
		{
		}

		public int MaxQuoteId
		{
			get;
			set;
		}

		public string Read(User user)
		{
			HtmlWeb web = new HtmlWeb();
			web.OverrideEncoding = System.Text.Encoding.GetEncoding(1251);

			int quoteIdToRead = -1;
			if (user.HasRead)
			{
				if (user.MaxReadQuoteId < MaxQuoteId)
				{
					user.MaxReadQuoteId++;
					quoteIdToRead = user.MaxReadQuoteId;
				}
				else
				{
					user.MinReadQuoteId--;
					quoteIdToRead = user.MinReadQuoteId;
				}
			}
			else
			{
				quoteIdToRead = MaxQuoteId;
				user.MaxReadQuoteId = user.MinReadQuoteId = MaxQuoteId;
			}
			HtmlDocument document = web.Load($"http://bash.im/quote/{quoteIdToRead}");

			var quote = document.DocumentNode.SelectSingleNode("//div[@class='text']");
			return quote.InnerHtml.Replace("<br>", "\n");
		}

		public void UpdateMaxQuoteId()
		{
			HtmlWeb web = new HtmlWeb();
			web.OverrideEncoding = System.Text.Encoding.GetEncoding(1251);
			HtmlDocument defaultDocument = web.Load($"http://bash.im/");
			var maxIdNode = defaultDocument.DocumentNode.SelectSingleNode("//a[@class='id']");
			MaxQuoteId = Int32.Parse(maxIdNode.InnerText.Substring(1));
		}
	}
}
