using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WinToolDrawingRefresh
{
    class Program
    {
        static void Main(string[] args)
        {
            string basePath = System.Configuration.ConfigurationManager.AppSettings["WinChillDrawingDirectory"];
            string webApi = System.Configuration.ConfigurationManager.AppSettings["WinToolUpdateDrawingApi"];

            DirectoryInfo dic = new DirectoryInfo(basePath);
            var files = dic.GetFiles();
            int idx = 0;
            foreach (var file in files)
            {
                string fileName = file.Name.Trim();
                string content = string.Format($"[\"{fileName}\"]");

                string result = PostResponseJson(webApi, content);

                Console.WriteLine($"[{++idx}] {fileName}: {content}: {result}");
            }
            Console.WriteLine("All files refresh completed...");
            Console.ReadKey();
        }

        public static string PostResponseJson(string url, string requestJson)
        {
            HttpContent httpContent = new StringContent(requestJson);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpClient httpClient = new HttpClient();

            HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;

            if (response.IsSuccessStatusCode)
            {
                string responseJson = response.Content.ReadAsStringAsync().Result;
                return responseJson;
            }
            else
            {
                return "Error,StatusCode:" + response.StatusCode.ToString();
            }
        }
    }
}
