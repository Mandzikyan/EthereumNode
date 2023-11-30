namespace EthereumNode
{
    public class JsonRequestModel
    {
        public string Jsonrpc { get; set; }
        public string Method { get; set; }
        public List<object> Parameters { get; set; }
        public string Id { get; set; }
    }

}
