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
    public class QuickCacheTestController : ControllerBase
    {

        private readonly ILogger<HomeController> _logger;
        private IList<byte[]> _blob;
        private readonly IQuickCacheTestService _testService;
        private readonly ICache _cache;
        private static readonly Histogram _durationHist = Metrics
            .CreateHistogram("test_quick_cache_duration_hist", "Histogram of Quick Cache call call durations.");

        private static readonly Gauge _durationGauge = Metrics
            .CreateGauge("test_quick_cache_duration_gauge", "Gauge of Quick Cache call call durations.");

        public QuickCacheTestController(
            ILogger<HomeController> logger, 
            ICache cache, 
            IQuickCacheTestService testService,
            IList<byte[]> blob)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger;
            _blob = blob;
            _testService = testService;
        }


        // GET api/<LoadTest>/5
        [HttpGet("{id}")]
        public bool Get(int id)
        {

            switch (id)
            {
                case 0:
                    _testService.StartTest(false, 1024);
                    break;
                case 1:
                    _testService.StartTest(false, 1024 * 1024);
                    break;
                case 2:
                    _testService.StartTest(true, 1024);
                    break;
                case 3:
                    _testService.StartTest(true, 1024 * 1024);

                    break;
            }

             return true;
        }

        // Create an api GET method that calls RedisTestService Results and returns the results as JSON.
        // GET api/<LoadTest>/results
        [HttpGet("results")]
        public TestResponse GetResults()
        {
            return _testService.Results;
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
