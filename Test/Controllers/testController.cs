using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceStack.Redis;

namespace Test.Controllers
{
    /// <summary>
    /// testController
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class testController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<testController> _logger;
        /// <summary>
        /// 
        /// </summary>
        private readonly IRedisClientsManager _redisClientsManager;
        /// <summary>
        /// testController
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="redisClientsManager"></param>
        public testController(ILogger<testController> logger, IRedisClientsManager redisClientsManager)
        {
            _logger = logger;
            _redisClientsManager = redisClientsManager;
        }
        /// <summary>
        /// delete the leaderboard
        /// </summary>
        [HttpGet]
        public void delete()
        {
            using (var client = _redisClientsManager.GetClient())
            {
                client.RemoveRangeFromSortedSet("Leaderboard",0,-1);
            }
        }
    }
}
