namespace EthereumNode.Models
{
    public class RpcRequestResult
    {
        public string Url { get; set; }
        public JsonRpcResponse Response { get; set; }
    }
}