using Newtonsoft.Json;
using EthereumNode.Models;
using System.Text;

namespace EthereumNode.Core
{
    public class DataRetrievalProcess
    {
        private readonly List<string> nodeUrls;
        private readonly HttpClient client;

        public DataRetrievalProcess(List<string> nodeUrls)
        {
            this.nodeUrls = nodeUrls;
            client = new HttpClient();
        }
        public async Task<JsonRpcResponse> GetGasPrice(JsonRpcRequest jsonRpcRequest)
        {
            return await GetGasPriceFromNodesParallel(jsonRpcRequest);

        }
        private async Task<JsonRpcResponse> GetGasPriceFromNodesParallel(JsonRpcRequest jsonRpcRequest)
        {
            var tasks = nodeUrls.Select(node => GetGasFromNode(node, jsonRpcRequest)).ToList();

            while (tasks.Count > 0)
            {
                var completedTask = await Task.WhenAny(tasks);

                tasks.Remove(completedTask);

                if (!completedTask.IsFaulted)                
                    return completedTask.Result;                
            }

            return null;
        }
        private async Task<List<JsonRpcResponse>> GetGasFromNodes(JsonRpcRequest jsonRpcRequest)
        {
            var tasks = nodeUrls.Select(url => GetGasFromNode(url, jsonRpcRequest)).ToList();
            await Task.WhenAll(tasks);
            return tasks.Select(t => t.Result).ToList();
        }

        private async Task<JsonRpcResponse> GetGasFromNode(string nodeUrl, JsonRpcRequest jsonRpcRequest)
        {
            var jsonPayload = JsonConvert.SerializeObject(jsonRpcRequest);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(nodeUrl, content);

            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<JsonRpcResponse>(await response.Content.ReadAsStringAsync());
            else
                return new JsonRpcResponse();
        }

    }
}