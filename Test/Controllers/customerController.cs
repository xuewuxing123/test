using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceStack.Redis;
using System.ComponentModel.DataAnnotations;

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
        private readonly IRedisClientsManager _redisClientsManager;
        /// <summary>
        /// customerController
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="redisClientsManager"></param>
        public customerController(ILogger<customerController> logger,IRedisClientsManager redisClientsManager)
        {
            this._logger = logger;
            this._redisClientsManager = redisClientsManager;
        }
        /// <summary>
        /// Add or modify user scores
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        [HttpPost("{customerid}/score/{score}")]
        public IActionResult UpdateScore([Range(1, int.MaxValue)] long? customerid, [Range(-1000, 1000)] decimal? score)
        {
            using (var client = _redisClientsManager.GetClient())
            {
                var ret = client.IncrementItemInSortedSet("Leaderboard", customerid.ToString(), (double)score);
                return Ok(ret);
            }
        }
    }
}
