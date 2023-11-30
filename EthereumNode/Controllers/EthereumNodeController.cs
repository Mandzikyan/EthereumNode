using EthereumNode.Configuration;
using EthereumNode.Core;
using EthereumNode.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using Newtonsoft.Json.Linq;

namespace EthereumNode.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EthereumNodeController : ControllerBase
    {
        private readonly EthereumNodeConfig _ethereumNodeConfiguration;

        public EthereumNodeController(IOptions<EthereumNodeConfig> ethereumNodeConfiguration)
        {
            _ethereumNodeConfiguration = ethereumNodeConfiguration.Value;
        }

        [HttpPost]
        [Route("eth_gasPrice")]
        public IActionResult GetGasPrice()
        {
     
            var Class = new DataRetrievalProcess();

            var response = Class.Process();

            
            return Ok(response);
        }
    }
}
