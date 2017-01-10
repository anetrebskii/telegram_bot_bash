using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bash
{
	public class BashState : IState
	{
		private User _user = null;
		private int _messageIsHandled = 0;

		public event EventHandler<NewStateEventArgs> NewState;

		public void HandleMessage(Message message, TelegramBotClient bot)
		{
			if (Interlocked.CompareExchange(ref _messageIsHandled, 1, 0) == 1)
			{
				return;
			}

			if (_user == null)
			{
				using (var context = new BashDbContext())
				{
					_user = context.Users.Find(message.From.Id);
				}
			}

			string quote = BashRegistry.Default.BashManager.Read(_user);
			BashRegistry.Default.UserThrottleSaver.AddToSaving(_user.Clone());
			string response = $@"{quote}
---
/next - дальше";
			bot.SendTextMessageAsync(message.Chat.Id, response).ContinueWith((arg) =>
			{
				Interlocked.Exchange(ref _messageIsHandled, 0);
			});
		}
	}
}
