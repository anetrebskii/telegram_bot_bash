using System;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bash
{
	public class UserChat : IDisposable
	{
		private IState _currentState;
		private TelegramBotClient _bot;

		public UserChat(long chatId, IUserChatInitializer initializer, TelegramBotClient bot)
		{
			_currentState = initializer.CreateInitialState(chatId);
			_currentState.NewState += _currentState_NewState;
			_bot = bot;
		}

		public void Dispose()
		{
			(_currentState as IDisposable)?.Dispose();
			_currentState = null;
		}

		private void _currentState_NewState(object sender, NewStateEventArgs e)
		{
			_currentState.NewState -= _currentState_NewState;
			(_currentState as IDisposable)?.Dispose();
			_currentState = e.NewState;
		}

		public void Handle(Message message)
		{
			_currentState.HandleMessage(message, _bot);
		}
	}
}
