using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Prometheus;
using System.Diagnostics;
using System.Text.Json;
using TeamHitori.QuickCacheWeb.ViewModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace quick_cache_web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisTestController : ControllerBase
    {

        private readonly ILogger<HomeController> _logger;
        private IList<byte[]> _blob;
        private readonly IRedisTestService _redisTestService;
        private readonly IDistributedCache _redisCache;
        private static readonly Histogram _redisDurationHist = Metrics
            .CreateHistogram("test_redis_duration_hist", "Histogram of redis call call durations.");

        private static readonly Gauge _redisDurationGauge = Metrics
            .CreateGauge("test_redis_duration_gauge", "Gauge of redis call call durations.");

        public RedisTestController(
            ILogger<HomeController> logger, 
            IDistributedCache cache, 
            IRedisTestService redisTestService,
            IList<byte[]> blob)
        {
            _redisCache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger;
            _blob = blob;
            _redisTestService = redisTestService;
        }
        // GET: api/<LoadTest>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            
            return ["Hello World"];
        }

        // GET api/<LoadTest>/5
        [HttpGet("{id}")]
        public bool Get(int id)
        {

            switch (id)
            {
                case 0:
                    _redisTestService.StartTest();

                    break;
            }

             return true;
        }

        // Create an api GET method that calls RedisTestService Results and returns the results as JSON.
        // GET api/<LoadTest>/results
        [HttpGet("results")]
        public TestResponse GetResults()
        {
            return _redisTestService.Results;
        }

        // POST api/<LoadTest>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<LoadTest>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LoadTest>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
