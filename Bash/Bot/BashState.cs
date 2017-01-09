using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bash
{
	public class BashState : IState
	{
		private UserInfo _userInfo = new UserInfo();
		private int _a = 0;

		public event EventHandler<NewStateEventArgs> NewState;

		public void HandleMessage(Message message, TelegramBotClient bot)
		{
			if (Interlocked.CompareExchange(ref _a, 1, 0) == 1)
			{
				return;
			}
			string quote = BashRegistry.Default.BashManager.Read(_userInfo);
			BashRegistry.Default.UserThrottleSaver.AddToSaving(_userInfo.Clone());
			string response = $@"{quote}
---
/next - дальше";
			bot.SendTextMessageAsync(message.Chat.Id, response).ContinueWith((arg) =>
			{
				Interlocked.Exchange(ref _a, 0);
			});
		}
	}
}
