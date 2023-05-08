using TelegramBot;

public class ExchangeRatesFacade
{
    private PrivatBankExchange _privatBank;
    private TelegramBots _telegramBot;

    public ExchangeRatesFacade(TelegramBots telegramBots)
    {
        _telegramBot = telegramBots;
    }

    public void StartBot()
    {
        _telegramBot.StartBot();

    }

}
