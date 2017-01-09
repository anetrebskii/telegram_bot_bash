using System;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bash
{
	public interface IState
	{
		void HandleMessage(Message message, TelegramBotClient bot);
		event EventHandler<NewStateEventArgs> NewState;
	}
}
