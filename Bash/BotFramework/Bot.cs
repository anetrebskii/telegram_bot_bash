using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputMessageContents;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bash
{
	public class Bot
	{
#if DEBUG
		private readonly TelegramBotClient _bot = new TelegramBotClient("telegram_id");
#else
		private readonly TelegramBotClient _bot = new TelegramBotClient("telegram_id");
#endif

		private IUserChatInitializer _userChatInitializer;
		private ConcurrentDictionary<long, UserChat> _userChats = new ConcurrentDictionary<long, UserChat>();

		public Bot(IUserChatInitializer userChatInitializer)
		{
			_userChatInitializer = userChatInitializer;
		}

		public void Start()
		{
			_bot.OnMessage += BotOnMessageReceived;
			_bot.OnMessageEdited += BotOnMessageReceived;

			var me = _bot.GetMeAsync().Result;

			Console.Title = me.Username;

			_bot.StartReceiving();
		}

		public void Stop()
		{
			_bot.OnMessage -= BotOnMessageReceived;
			_bot.OnMessageEdited -= BotOnMessageReceived;

			_bot.StopReceiving();
		}

		private async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
		{
			handerRequest(messageEventArgs.Message);
		}

		private void handerRequest(Message message)
		{
			var userChat = _userChats.GetOrAdd(message.Chat.Id, (arg) =>
			{
				return new UserChat(message.From.Id, _userChatInitializer, _bot);
			});
			userChat.Handle(message);
		}
	}
}
