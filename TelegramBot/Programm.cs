using Telegram.Bot;
using Telegram.Bot.Args;

namespace TelegramBot
{
    public class Programm
    {
        private static ITelegramBotClient botClient;
        private static bool isAwaitingCurrencyInput = false;
        private static bool isAwaitingDateInput = false;
        private static InputState currentInputState = InputState.None;
        private static string recentlyEnteredCurrency = null;



        private static PrivatBankExchange privatBank = new PrivatBankExchange();

        public static async Task Main(string[] args)
        {
            var token = "5694254616:AAGThfBKk019Uqqk8Zv7AAy_XAAz3uAm6Lc";
            botClient = new TelegramBotClient(token);

            var me = await botClient.GetMeAsync();
            Console.WriteLine($"Bot ID: {me.Id}. Bot Name: {me.FirstName}");

            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            botClient.StopReceiving();
        }

        private static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                await HandleUserInput2(e.Message.Chat.Id, e.Message.Text);
            }
        }


        private static async Task HandleUserInput2(long chatId, string userInput)
        {

            if (userInput.Equals("/start", StringComparison.OrdinalIgnoreCase))
            {
                currentInputState = InputState.AwaitingCurrencyInput;
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "ОК, давайте начнем! Введите название валюты:");
            }
            else
            {
                switch (currentInputState)
                {
                    case InputState.AwaitingCurrencyInput:
                        await HandleCurrencyInput(chatId, userInput);
                        break;
                    case InputState.AwaitingDateInput:
                        await HandleDateInput(chatId, userInput); // Создайте новый метод для обработки ввода даты
                        break;
                    default:
                        if (userInput.Equals("/stop", StringComparison.OrdinalIgnoreCase))
                        {
                            await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: "Бот Остановлен!"
                            );
                            botClient.StopReceiving();
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: "Пожалуйста, введите /start для начала работы с ботом.");
                        }
                        break;
                }
            }
        }

        private static async Task Answer(long chatId, string currencyName)
        {
            await botClient.SendTextMessageAsync(
           chatId: chatId,
           text: "ОК, давайте начнем!");

            await botClient.SendTextMessageAsync(
            chatId: chatId,
             text: "Введите название Валюты   ( Пример: USD,usd)");
            currentInputState = InputState.AwaitingCurrencyInput;
        }


        private static async Task HandleCurrencyInput(long chatId, string currencyName)
        {
            currentInputState = InputState.AwaitingDateInput;
            recentlyEnteredCurrency = currencyName;

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $" Теперь введите дату в формате дд.мм.гггг (Например, 05.05.2023):"
            );
        }

        private static async Task HandleDateInput(long chatId, string dateInput)
        {
            currentInputState = InputState.None;

            var privat = new PrivatBankExchange();
            string exchangeRateResult = privat.GetExchangeRate2(recentlyEnteredCurrency, dateInput);


            await botClient.SendTextMessageAsync(
                          chatId: chatId,
                          text: exchangeRateResult
                      );
        }


    }
}
