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
        private readonly EthereumNodeConfiguration _ethereumNodeConfiguration;

        public EthereumNodeController(IOptions<EthereumNodeConfiguration> ethereumNodeConfiguration)
        {
            _ethereumNodeConfiguration = ethereumNodeConfiguration.Value;
        }

        [HttpPost]
        [Route("eth_gasPrice")]
        public IActionResult GetGasPrice([FromBody] JObject jsonRequest)
        {
            try
            {
                if (jsonRequest == null)
                {
                    return BadRequest("Invalid JSON-RPC payload");
                }


                // Send the JSON-RPC request to the Ethereum node
                var gasPriceResponse = await _web3.Client.JObject(gasPriceRequest);
                    
                // Check if the response contains a result
                if (gasPriceResponse.HasResult)
                {
                    // Extract the gas price from the response
                    var gasPriceHex = gasPriceResponse.Result.ToString();

                    // Convert gas price from hex to decimal
                    var gasPriceDecimal = Web3.Convert.FromWei(Nethereum.Hex.HexConvertors.HexBigIntegerConverter.Instance.ConvertFromHex(gasPriceHex));

                    // Return the gas price in the response
                    return Ok(new { GasPrice = gasPriceDecimal });
                }
                else
                {
                    // Handle the case when the response does not contain a result
                    return BadRequest("Unable to retrieve gas price from the Ethereum node.");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, log errors, etc.
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
