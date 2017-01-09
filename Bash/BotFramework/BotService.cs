using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputMessageContents;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotFramework
{
	public class BotService
	{
		private readonly TelegramBotClient _bot = new TelegramBotClient("321733116:AAG2vUemPvqXt6qJc0pSPTAzoTvB_bC250E");

		public BotService()
		{
		}

		public void Start()
		{
			_bot.OnCallbackQuery += BotOnCallbackQueryReceived;
			_bot.OnMessage += BotOnMessageReceived;
			_bot.OnMessageEdited += BotOnMessageReceived;
			_bot.OnInlineQuery += BotOnInlineQueryReceived;
			_bot.OnInlineResultChosen += BotOnChosenInlineResultReceived;
			_bot.OnReceiveError += BotOnReceiveError;

			var me = _bot.GetMeAsync().Result;

			Console.Title = me.Username;

			_bot.StartReceiving();
		}

		/// <summary>
		/// Stop this service.
		/// </summary>
		public void Stop()
		{			
			_bot.StopReceiving();
		}

		private void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
		{
			Debugger.Break();
		}

		private void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
		{
			Console.WriteLine($"Received choosen inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
		}

		private async void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs inlineQueryEventArgs)
		{
			InlineQueryResult[] results = {
				new InlineQueryResultLocation
				{
					Id = "1",
					Latitude = 40.7058316f, // displayed result
                    Longitude = -74.2581888f,
					Title = "New York",
					InputMessageContent = new InputLocationMessageContent // message if result is selected
                    {
						Latitude = 40.7058316f,
						Longitude = -74.2581888f,
					}
				},

				new InlineQueryResultLocation
				{
					Id = "2",
					Longitude = 52.507629f, // displayed result
                    Latitude = 13.1449577f,
					Title = "Berlin",
					InputMessageContent = new InputLocationMessageContent // message if result is selected
                    {
						Longitude = 52.507629f,
						Latitude = 13.1449577f
					}
				}
			};

			await _bot.AnswerInlineQueryAsync(inlineQueryEventArgs.InlineQuery.Id, results, isPersonal: true, cacheTime: 0);
		}

		private async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
		{
			Console.WriteLine($"Message received: {messageEventArgs.Message}");
			var message = messageEventArgs.Message;

			if (message == null || message.Type != MessageType.TextMessage) return;

			if (message.Text.StartsWith("/inline")) // send inline keyboard
			{
				await _bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

				var keyboard = new InlineKeyboardMarkup(new[]
				{
					new[] // first row
                    {
						new InlineKeyboardButton("1.1"),
						new InlineKeyboardButton("1.2"),
					},
					new[] // second row
                    {
						new InlineKeyboardButton("2.1"),
						new InlineKeyboardButton("2.2"),
					}
				});

				await Task.Delay(500); // simulate longer running task

				await _bot.SendTextMessageAsync(message.Chat.Id, "Choose",
					replyMarkup: keyboard);
			}
			else if (message.Text.StartsWith("/keyboard")) // send custom keyboard
			{
				var keyboard = new ReplyKeyboardMarkup(new[]
				{
					new [] // first row
                    {
						new KeyboardButton("1.1"),
						new KeyboardButton("1.2"),
					},
					new [] // last row
                    {
						new KeyboardButton("2.1"),
						new KeyboardButton("2.2"),
					}
				});

				await _bot.SendTextMessageAsync(message.Chat.Id, "Choose",
					replyMarkup: keyboard);
			}
			else if (message.Text.StartsWith("/photo")) // send a photo
			{
				await _bot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

				const string file = @"<FilePath>";

				var fileName = file.Split('\\').Last();

				using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					var fts = new FileToSend(fileName, fileStream);

					await _bot.SendPhotoAsync(message.Chat.Id, fts, "Nice Picture");
				}
			}
			else if (message.Text.StartsWith("/request")) // request location or contact
			{
				var keyboard = new ReplyKeyboardMarkup(new[]
				{
					new KeyboardButton("Location")
					{
						RequestLocation = true
					},
					new KeyboardButton("Contact")
					{
						RequestContact = true
					},
				});

				await _bot.SendTextMessageAsync(message.Chat.Id, "Who or Where are you?", replyMarkup: keyboard);
			}
			else
			{
				var usage = @"Usage:
/inline   - send inline keyboard
/keyboard - send custom keyboard
/photo    - send a photo
/request  - request location or contact
";

				await _bot.SendTextMessageAsync(message.Chat.Id, usage,
					replyMarkup: new ReplyKeyboardHide());
			}
		}

		private async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
		{
			await _bot.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id,
				$"Received {callbackQueryEventArgs.CallbackQuery.Data}");
		}
	}
}
