using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;

namespace TelegramBot
{
    public class PrivatBankExchange
    {
        private const string apiUrl = "https://api.privatbank.ua/p24api/exchange_rates";

        // Метод для отправки HTTP-запроса к API и получения ответа в виде строки
        private string SendHttpRequest(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return null;
                    }
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(dataStream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                throw new Exception("По вашему запросу нет данных. Попробуйте еще раз.", ex);
            }
        }

        // Метод для получения курса валюты на указанную дату
        public string GetExchangeRate2(string currencyCode, string formattedDatedate)
        {
            try
            {
                string url = $"{apiUrl}?json&date={formattedDatedate}";
                string responseFromServer = SendHttpRequest(url);
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
                        return $"Курс {item["currency"]} на {formattedDatedate}: {item["saleRateNB"]}/{item["purchaseRateNB"]}";
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
