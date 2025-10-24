using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Test.Service
{
    /// <summary>
    /// SingletonConcurrentCache
    /// </summary>
    public class SingletonConcurrentCache
    {
        /// <summary>
        /// instance
        /// </summary>
        private static volatile SingletonConcurrentCache instance;
        /// <summary>
        /// lockObject
        /// </summary>
        private static object lockObject = new object();
        /// <summary>
        /// Constructor funcation
        /// </summary>
        private SingletonConcurrentCache() 
        {

        }
        /// <summary>
        /// Instance
        /// </summary>
        public static SingletonConcurrentCache Instance
        {
            get
            {
                if (instance == null) 
                {
                    lock (lockObject) 
                    {
                        if (instance == null) 
                            instance = new SingletonConcurrentCache();
                    }
                }
                return instance;
            }
        }
        /// <summary>
        /// concurrentDictionary
        /// </summary>
        private ConcurrentDictionary<long, decimal> concurrentDictionary = new ConcurrentDictionary<long, decimal>();
        /// <summary>
        /// AddOrUpdate
        /// </summary>
        /// <param name="key"></param>
        /// <param name="addValue"></param>
        /// <returns></returns>
        public decimal AddOrUpdate(long key, decimal addValue)
        {
            return concurrentDictionary.AddOrUpdate(key, addValue, (key, oldValue) => oldValue + addValue);
        }
        /// <summary>
        /// RemoveAll
        /// </summary>
        public void RemoveAll()
        {
            concurrentDictionary = new ConcurrentDictionary<long, decimal>();
        }
        /// <summary>
        /// To List
        /// </summary>
        public List<dynamic> List
        {
            get
            {
                List<dynamic> retList = new List<dynamic>();
                var list = concurrentDictionary.OrderByDescending(o => o.Value).ThenBy(o => o.Key).ToList();
                int rank=0;
                foreach (var kvp in list)
                {
                    retList.Add(new { CustomerID = kvp.Key, Score = kvp.Value, Rank = Interlocked.Increment(ref rank) });
                }
                return retList;
            }
        }
    }
}
