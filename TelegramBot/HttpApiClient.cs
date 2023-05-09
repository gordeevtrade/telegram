using System.Net;

public class HttpApiClient
{
    // Метод для отправки HTTP-запроса к API и получения ответа в виде строки
    public string SendHttpRequest(string url)
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
}