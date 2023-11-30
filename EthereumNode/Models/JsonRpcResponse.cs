using Nethereum.Hex.HexTypes;

namespace EthereumNode.Models
{
    public class JsonRpcResponse
    {
        public int Id { get; set; }
        public string JsonRpc { get; set; }
        public HexBigInteger Result { get; set; }
    }
}