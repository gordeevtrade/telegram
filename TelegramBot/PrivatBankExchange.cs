using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;

namespace TelegramBot
{
    public class PrivatBankExchange
    {
        private const string apiUrl = "https://api.privatbank.ua/p24api/exchange_rates";

        HttpApiClient apiClient = new HttpApiClient();

        //получает ответ от API и извлекает из него курс валюты на указанную дату.
        public string GetExchangeRate2(string currencyCode, string formattedDatedate)
        {
            try
            {
                string url = $"{apiUrl}?json&date={formattedDatedate}";
                string responseFromServer = apiClient.SendHttpRequest(url);
                if (responseFromServer == null)
                {
                    return "Произошла ошибка при вводе  данных. Попробуйте еще раз";
                }
                JObject data = JObject.Parse(responseFromServer);
                JArray exchangeRates = (JArray)data["exchangeRate"];

                //    bool foundCurrency = false;
                foreach (JObject item in exchangeRates)
                {
                    if ((string)item["currency"] == currencyCode.ToUpper())
                    {
                        return $"Продажа/Покупка:  Курс {item["currency"]} на {formattedDatedate}: {item["saleRateNB"]}/{item["purchaseRateNB"]}";
                    }
                }
                return "Курс не найден. Попробуйте позже или укажите другие данные";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
