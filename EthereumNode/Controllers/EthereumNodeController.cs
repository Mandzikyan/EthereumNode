using EthereumNode.Configuration;
using EthereumNode.Core;
using EthereumNode.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EthereumNode.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EthereumNodeController : ControllerBase
    {
        private readonly DataRetrievalProcess dataRetrievalProcess;

        public EthereumNodeController(IOptions<EthereumNodeConfig> ethereumNodeConfig)
        {
            dataRetrievalProcess = new DataRetrievalProcess(ethereumNodeConfig.Value.NodeUrls, ethereumNodeConfig.Value.NodeTimeoutMinutes);
        }

        [HttpPost]
        [Route("eth_gasPrice")]
        public async Task<IActionResult> GetGasPrice([FromBody] JsonRpcRequest jsonRpcRequest)
        {  
            var response = await dataRetrievalProcess.GetGasPrice(jsonRpcRequest);
            dataRetrievalProcess.CheckForPenality(jsonRpcRequest);
            return Ok(response);
        }
    }
}