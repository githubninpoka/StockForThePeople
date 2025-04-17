using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StockForThePeople.ExternalData;
using StockForThePeople.ExternalData.DTO;
using StockForThePeople.WebApiExecuter;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StockForThePeople.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataManagementController : ControllerBase
    {
        private IExternalDataService _externalDataService;
        private ILogger<DataManagementController> _logger;
        public DataManagementController(
            IExternalDataService externalDataService,
            ILogger<DataManagementController> logger
            )
        {
            _logger = logger;
            _externalDataService = externalDataService;
            
        }
        // GET: api/<DataManagementController>
        [HttpGet]
        [SwaggerOperation(Summary = "The endpoint for incrementally updating external data into the database.", Description = "The process will update from 30 days ago until as recent as possible.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UpdateDataAsync()
        {
            _logger.LogInformation("Calling the service.");
            await _externalDataService.UpdateDataAsync(); //I should probably not await here.
            _logger.LogInformation("Ending the servicecall.");
            return Ok("Updating data...");
            

        }

        // GET api/<DataManagementController>/5
        [HttpGet("{numberOfDays}")]
        [SwaggerOperation(Summary = "The endpoint for cleaning the database and refreshing all historical data.", Description = "You can choose a positive number from 1 to 1000")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        //public string Get(int id)
        public async Task<IActionResult> LoadHistoricalAsync(int numberOfDays)
        {
            if (numberOfDays < 1 || numberOfDays > 1000)
            {
                return BadRequest("Try a described value");
            }
            _logger.LogInformation("{var} - { var2} - Updating for {var3} days", nameof(DataManagementController),nameof(LoadHistoricalAsync),numberOfDays);
            await _externalDataService.LoadHistoricalDataAsync(); //I should probably not await here.
            _logger.LogInformation("{var} - { var2} - Done updating for {var3} days", nameof(DataManagementController), nameof(LoadHistoricalAsync), numberOfDays);
            return Ok("Updating data...");
        }


    }
}
