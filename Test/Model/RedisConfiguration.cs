using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        public string ConfigurationString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ConnectTimeout { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PoolSize { get; set; }

    }
}
