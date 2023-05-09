using TelegramBot;

public class ExchangeRateService
{

    public string GetExchangeRate(string currency, string date)
    {
        var privat = new PrivatBankExchange();
        string exchangeRateResult = privat.GetExchangeRate2(currency, date);
        return "Продажа/Покупка: " + exchangeRateResult;
    }
}
