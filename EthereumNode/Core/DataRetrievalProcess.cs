using EthereumNode.Models;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;

namespace EthereumNode.Core
{
    public class DataRetrievalProcess
    {
        private readonly List<string> nodeUrls;
        private List<string> blackList;
        private readonly int nodeTimeoutMinutes;

        public DataRetrievalProcess(List<string> nodeUrls, int nodeTimeoutMinutes)
        {
            this.nodeUrls = nodeUrls;
            blackList = new List<string>();
            this.nodeTimeoutMinutes = nodeTimeoutMinutes;
        }

        public async void CheckForPenalityAsync(JsonRpcRequest jsonRpcRequest)
        {
            var results = await GetGasFromNodesAsync(jsonRpcRequest);
            if (results.Count == nodeUrls.Count)
            {
                var distinctGasPrices = results.Where(r => r != null).Select(r => r.Response.Result).Distinct().ToList();

                if (!(distinctGasPrices.Count == 1))
                {
                    var nonMatchingUrl = results.Where(r => !distinctGasPrices.Contains(r.Response.Result))
                                                 .Select(r => r.Url)
                                                 .ToList();
                    blackList.Clear();
                    blackList.AddRange(nonMatchingUrl);
                }
            }
        }

        public async Task<RpcRequestResult> GetGasPriceAsync(JsonRpcRequest jsonRpcRequest)
        {
            var tasks = nodeUrls.Select(node => GetGasFromNode(node, jsonRpcRequest)).ToList();

            while (tasks.Count > 0)
            {
                var completedTask = await Task.WhenAny(tasks);
                tasks.Remove(completedTask);

                if (!(completedTask == null && blackList.Contains(completedTask.Result.Url)))
                    return completedTask.Result;
            }
            return null;
        }

        private async Task<List<RpcRequestResult>> GetGasFromNodesAsync(JsonRpcRequest jsonRpcRequest)
        {
            var tasks = nodeUrls.Select(url => GetGasFromNode(url, jsonRpcRequest)).ToList();
            await Task.WhenAll(tasks);
            return tasks.Select(t => t.Result).ToList();
        }

        private async Task<RpcRequestResult> GetGasFromNode(string nodeUrl, JsonRpcRequest jsonRpcRequest)
        {
            var web3 = new Web3(nodeUrl);
            var timeoutTask = Task.Delay(TimeSpan.FromMinutes(nodeTimeoutMinutes));
            var gasPrice = web3.Eth.GasPrice.SendRequestAsync();
            var completedTask = await Task.WhenAny(gasPrice, timeoutTask);

            if (completedTask == timeoutTask)
            {
                blackList.Add(nodeUrl);
                return null;
            }
            if (gasPrice != null)
                return new RpcRequestResult()
                {
                    Response = new JsonRpcResponse()
                    {
                        Result = gasPrice.Result,
                    },
                    Url = nodeUrl
                };
            else
                return null;
            }
    }
}