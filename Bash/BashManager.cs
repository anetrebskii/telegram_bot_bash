using System;
using System.Threading;
using HtmlAgilityPack;

namespace Bash
{
	public class BashManager
	{
#if DEBUG
		private static readonly TimeSpan UPDATE_MAX_QUOTE_ID_INTERVAL = TimeSpan.FromSeconds(20);
#else
		private static readonly TimeSpan UPDATE_MAX_QUOTE_ID_INTERVAL = TimeSpan.FromMinutes(5);
#endif

		private Timer _timer;

		public BashManager()
		{
			_timer = new Timer(updateMaxQuoteId, null, UPDATE_MAX_QUOTE_ID_INTERVAL, UPDATE_MAX_QUOTE_ID_INTERVAL);
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
			updateMaxQuoteId(null);
		}

		private void updateMaxQuoteId(object state)
		{
			try
			{
				_timer.Change(Timeout.Infinite, Timeout.Infinite);
				HtmlWeb web = new HtmlWeb();
				web.OverrideEncoding = System.Text.Encoding.GetEncoding(1251);
				HtmlDocument defaultDocument = web.Load($"http://bash.im/");
				var maxIdNode = defaultDocument.DocumentNode.SelectSingleNode("//a[@class='id']");
				MaxQuoteId = Int32.Parse(maxIdNode.InnerText.Substring(1));
			}
			finally
			{
				_timer.Change(UPDATE_MAX_QUOTE_ID_INTERVAL, UPDATE_MAX_QUOTE_ID_INTERVAL);
			}
		}
	}
}
