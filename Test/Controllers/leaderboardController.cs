using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceStack.Redis;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace Test.Controllers
{
    /// <summary>
    /// leaderboardController
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class leaderboardController : ControllerBase
    {
        private readonly ILogger<leaderboardController> _logger;
        
        private readonly IRedisClientsManager _redisClientsManager;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="redisClientsManager"></param>
        public leaderboardController(ILogger<leaderboardController> logger, IRedisClientsManager redisClientsManager)
        {
            _logger = logger;
            _redisClientsManager = redisClientsManager;
        }

        /// <summary>
        /// Obtain the leaderboard based on rankings
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [HttpGet()]
        public IActionResult GetLeaderboard([Required, Range(1, int.MaxValue)] int start, [Required, Range(1, int.MaxValue)] int end)
        {
            using (var client = _redisClientsManager.GetClient())
            {
                var datas= client.GetRangeWithScoresFromSortedSetDesc("Leaderboard", Interlocked.Decrement(ref start), Interlocked.Decrement(ref end));
                List<dynamic> ret = new List<dynamic>();
                foreach (KeyValuePair<string, double> data in datas)
                {
                    ret.Add(new { CustomerID = data.Key, Score = data.Value, Rank = Interlocked.Increment(ref start) });
                }
                return Ok(ret); ;
            }
        }
        /// <summary>
        /// Based on the customer ID, retrieve the top or bottom few entries from the ranking list
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="high"></param>
        /// <param name="low"></param>
        /// <returns></returns>
        [HttpGet("{customerid}")]
        public IActionResult GetLeaderboardByCustomId(long customerid,int high=0,int low=0)
        {
            using (var client = _redisClientsManager.GetClient())
            {
                int index = (int)client.GetItemIndexInSortedSetDesc("Leaderboard", customerid.ToString());
                int from = Interlocked.Add(ref index, -high)<0?0:index;
                int to = Interlocked.Add(ref index, Interlocked.Add(ref low,high));
                var datas = client.GetRangeWithScoresFromSortedSetDesc("Leaderboard", from, to);
                List<dynamic> ret = new List<dynamic>();
                foreach (KeyValuePair<string,double> data in datas)
                {
                    ret.Add(new { CustomerID=data.Key, Score=data.Value, Rank= Interlocked.Increment(ref from)});
                }
                return  Ok(ret); ;
            }
        }
    }
}
