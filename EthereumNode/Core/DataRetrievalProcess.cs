using System.Reflection.PortableExecutable;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using EthereumNode.Models;

namespace EthereumNode.Core
{
    public class DataRetrievalProcess
    {
        private readonly List<string> nodeUrls;
        private readonly HttpClient client;

        public DataRetrievalProcess()
        {
        //    this.nodeUrls = nodeUrls;
            client = new HttpClient();
        }
        public async Task<JsonRpcResponse> Process()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://eth-mainnet.g.alchemy.com/v2/docs-demo"),
                Headers =
    {
        { "accept", "application/json" },
    },
                Content = new StringContent("{\"id\":1,\"jsonrpc\":\"2.0\",\"method\":\"eth_gasPrice\"}")
                {
                    Headers =
        {
            ContentType = new MediaTypeHeaderValue("application/json")
        }
                }
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return new JsonRpcResponse { };
            }
        }
    }
}