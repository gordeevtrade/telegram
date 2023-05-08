using Telegram.Bot;
using Telegram.Bot.Args;
using TelegramBot;

public class TelegramBots
{
    private ITelegramBotClient botClient;
    private InputState currentInputState = InputState.None;
    private string recentlyEnteredCurrency = "";
    public TelegramBots(string token)
    {
        botClient = new TelegramBotClient(token);

    }

    public TelegramBots()
    {

    }

    public void StartBot()
    {
        botClient.OnMessage += Bot_OnMessage;
        botClient.StartReceiving();
        Console.WriteLine("Bot is started");
        Console.ReadLine();
        botClient.StopReceiving();
        Console.WriteLine("Bot is stopped");
    }


    private async void Bot_OnMessage(object sender, MessageEventArgs e)
    {
        if (e.Message.Text != null)
        {
            await HandleUserInput(e.Message.Chat.Id, e.Message.Text);
        }
    }

    private async Task HandleUserInput(long chatId, string userInput)
    {
        if (userInput.Equals("/start", StringComparison.OrdinalIgnoreCase))
        {
            Answer(chatId, userInput);
        }
        else if (userInput.Equals("/stop", StringComparison.OrdinalIgnoreCase))
        {
            StopBot(chatId);
        }
        else
        {
            switch (currentInputState)
            {
                case InputState.AwaitingCurrencyInput:
                    await HandleCurrencyInput(chatId, userInput);
                    break;
                case InputState.AwaitingDateInput:
                    await HandleDateInput(chatId, userInput);
                    break;
                default:
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Пожалуйста, введите /start для начала работы с ботом.");
                    break;
            }
        }
    }

    private async Task StopBot(long chatId)
    {
        await botClient.SendTextMessageAsync(
                                         chatId: chatId,
                                         text: "Бот Остановлен!");
        botClient.StopReceiving();

        await botClient.SendTextMessageAsync(
                                     chatId: chatId,
                                     text: "Для продолжения введите /start!");
        botClient.StartReceiving();
        currentInputState = InputState.None;
    }

    private async Task Answer(long chatId, string currencyName)
    {
        currentInputState = InputState.AwaitingCurrencyInput;

        await botClient.SendTextMessageAsync(
       chatId: chatId,
       text: "Небольшая инструкция перед  началом. Сначала вводите Название Валюты после Дату за какой срок вы хотите получить актуальный курс!"
       );
        await Task.Delay(1000);

        await botClient.SendTextMessageAsync(
        chatId: chatId,
         text: "Укажите название  ( Например: USD,usd; Eur,eur)  Вводите как в примере что бы получить актуальные котировки");
        currentInputState = InputState.AwaitingCurrencyInput;
    }


    private async Task HandleCurrencyInput(long chatId, string currencyName)
    {
        currentInputState = InputState.AwaitingDateInput;
        recentlyEnteredCurrency = currencyName;

        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: $" Теперь укажите дату в формате ДД.ММ.ГГГГ (Папример: 05.05.2023)  Вводите как в примере что бы получить актуальные котировки"
        );
    }

    private async Task HandleDateInput(long chatId, string dateInput)
    {
        currentInputState = InputState.AwaitingCurrencyInput;
        var privat = new PrivatBankExchange();
        string exchangeRateResult = privat.GetExchangeRate2(recentlyEnteredCurrency, dateInput);
        exchangeRateResult = "Продажа/Покупка: " + exchangeRateResult;
        ReturnExchage(chatId, exchangeRateResult);
    }

    private async Task ReturnExchage(long chatId, string exchangeRateResult)
    {
        await botClient.SendTextMessageAsync(
                  chatId: chatId,
                  text: exchangeRateResult
              );
    }
}
