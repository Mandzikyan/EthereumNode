using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace EthereumNode.Connection
{
    public class ApiConnection
    {

        private HttpClient httpClient { get; }
        private string baseUrl { get; }
        public ApiConnection(string baseUrl)
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseUrl);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.Timeout = new TimeSpan(1, 0, 0);
            this.baseUrl = baseUrl;
        }
        public async Task<T> GetAsync<T>(string url)
        {
            var response = await httpClient.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }
        public async Task<T> PostAsync<T>(string url, object content, CancellationToken cancellationToken)
        {
            var myContent = JsonConvert.SerializeObject(content, Formatting.Indented);
            var stringContent = new StringContent(myContent, Encoding.UTF8, "application/json" + "");
            var response = await httpClient.PostAsync(url, stringContent, cancellationToken);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}
