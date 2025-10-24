using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Test.Service;

namespace Test.Controllers
{
    /// <summary>
    /// customerController
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class customerController : ControllerBase
    {
        private readonly ILogger<customerController> _logger;
        /// <summary>
        /// customerController
        /// </summary>
        /// <param name="logger"></param>
        public customerController(ILogger<customerController> logger)
        {
            this._logger = logger;
        }
        /// <summary>
        /// Add or modify user score
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        [HttpPost("{customerid}/score/{score}")]
        public IActionResult UpdateScore([Range(1, int.MaxValue)] long customerid, [Range(-1000, 1000)] decimal score)
        {
            var ret = SingletonConcurrentCache.Instance.AddOrUpdate(customerid, score);
            return Ok(ret);
        }
    }
}
