using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HipchatScraper
{


    public class HipchatClient
    {
        private HttpClient _httpClient;

        public HipchatClient(string pat)
        {
            var handler = new HttpClientHandler();
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri("https://api.hipchat.com/");
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + pat);
        }

        public Task<HttpResponseMessage> GetAsync(string url)
        {
            Console.WriteLine(url);
            var result = _httpClient.GetAsync(url);
            result.Wait();
            var content = result.Result.Content.ReadAsStringAsync();
            content.Wait();
            Trace.WriteLine(content);

            Thread.Sleep(TimeSpan.FromSeconds(3));
            if ((int) result.Result.StatusCode == 429)
            {
                Console.WriteLine("429 - Backing off...");
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }

            return result;
        }

    }
}