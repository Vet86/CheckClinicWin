using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace CheckClinic.Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            var telegramProcessor = new TelegramProcessor();
            //var me = botClient.GetMeAsync().Result;
            //Console.WriteLine(me.Username);
            //var markup = new ReplyKeyboardMarkup(new[]
            //{
            //        new KeyboardButton("Privet"),
            //        new KeyboardButton("Hello"),
            //        new KeyboardButton("Zdarova"),
            //});
            //markup.OneTimeKeyboard = true;
            //botClient.SendTextMessageAsync(chatId, "Hello", replyMarkup: markup);
            Console.ReadKey();
        }

        private static void BotClient_OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
        }

        private static void onMessage(object sender, MessageEventArgs e)
        {
        }

        static void HandleMessage(object sender, MessageEventArgs messageEventArgs)
        {
            
        }
    }
}
