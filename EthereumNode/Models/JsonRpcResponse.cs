namespace EthereumNode.Models
{
    public class JsonRpcResponse
    {
        public int Id { get; set; }
        public string Jsonrpc { get; set; }
        public string Result { get; set; }
    }
}