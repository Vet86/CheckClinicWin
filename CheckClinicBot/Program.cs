using System;

namespace CheckClinic.Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Token must be set as first parameter");
                return;
            }

            try
            {
                var telegramProcessor = new TelegramProcessor(args[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.ReadKey();
        }
    }
}
