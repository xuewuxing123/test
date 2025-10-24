using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using Test.Service;

namespace Test.Controllers
{
    /// <summary>
    /// testController
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class testController : ControllerBase
    {
        private readonly ILogger<testController> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <summary>
        /// testController
        /// </summary>
        /// <param name="logger"></param>
        public testController(ILogger<testController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// delete the leaderboard
        /// </summary>
        [HttpDelete]
        public void delete()
        {
            SingletonConcurrentCache.Instance.RemoveAll();
        }
    }
}
