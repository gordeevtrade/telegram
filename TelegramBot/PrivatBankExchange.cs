using Newtonsoft.Json.Linq;
using System.Net;

namespace TelegramBot
{
    public class PrivatBankExchange
    {
        public string GetExchangeRate2(string currencyCode, string date)
        {
            string url = $"https://api.privatbank.ua/p24api/exchange_rates?json&date={date}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return "Произошла ошибка при получении данных";
                    }
                    else
                    {
                        using (Stream dataStream = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(dataStream))
                            {
                                string responseFromServer = reader.ReadToEnd();
                                JObject data = JObject.Parse(responseFromServer);

                                JArray exchangeRates = (JArray)data["exchangeRate"];
                                bool foundCurrency = false;
                                foreach (JObject item in exchangeRates)
                                {
                                    if ((string)item["currency"] == currencyCode)
                                    {
                                        return $"Курс {item["currency"]} на {date}: {item["saleRateNB"]}/{item["purchaseRateNB"]}";
                                    }
                                }

                                return "Ничего не нашли";
                            }
                        }
                    }
                }
            }
            catch (WebException)
            {
                return "По вашему запросу нет данных.Попробуйте еще раз.";
            }
        }

        public string GetValidDateStringFromUserInput()
        {
            DateTime date = DateTime.MinValue;
            while (date == DateTime.MinValue)
            {
                Console.Write("Введите дату (в формате ДД.ММ.ГГГГ): ");
                string dateString = Console.ReadLine();
                if (DateTime.TryParseExact(dateString, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out date))
                {
                    return date.ToString("dd.MM.yyyy");
                }
            }
            return "Введите корректную дату в формате ДД.ММ.ГГГГ";
        }
    }
}