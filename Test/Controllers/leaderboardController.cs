using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using Test.Service;

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public leaderboardController(ILogger<leaderboardController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Based on the customer ID, retrieve the top or bottom few entries from the ranking list
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="high"></param>
        /// <param name="low"></param>
        /// <returns></returns>
        [HttpGet("{customerid}")]
        public IActionResult GetLeaderboardByCustomId(long customerid, [Range(0, int.MaxValue)] int high = 0, [Range(0, int.MaxValue)] int low = 0)
        {
            var list = SingletonConcurrentCache.Instance.List;
            int index = list.FindIndex(p => p.CustomerID == customerid);
            int from = Interlocked.Add(ref index, -high) < 0 ? 0 : index;
            var datas = list.Skip(from).Take(Interlocked.Add(ref high,low + 1)).ToList();
            return Ok(datas);
        }
        /// <summary>
        /// Obtain the leaderboard based on rankings
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [HttpGet("/GetLeaderboard")]
        public IActionResult GetLeaderboard([Required, Range(1, int.MaxValue)] int start, [Required, Range(1, int.MaxValue)] int end)
        {
            var list = SingletonConcurrentCache.Instance.List.Where(p => p.Rank >= start && p.Rank <= end).ToList();
            return Ok(list);
        }
    }
}
