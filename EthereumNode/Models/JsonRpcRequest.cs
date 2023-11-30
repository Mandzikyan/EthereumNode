namespace EthereumNode.Models
{
    public class JsonRpcRequest
    {
        public int Id { get; set; }
        public string Jsonrpc { get; set; }
        public string Method { get; set; }
    }
}
