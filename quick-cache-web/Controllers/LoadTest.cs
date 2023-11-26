using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Prometheus;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace quick_cache_web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoadTest : ControllerBase
    {

        private readonly ILogger<HomeController> _logger;
        private IList<byte[]> _blob;
        private readonly IDistributedCache _redisCache;
        private static readonly Histogram _redisDuration = Metrics
            .CreateHistogram("test_redis_duration_seconds", "Histogram of redis call call durations.");

        public LoadTest(ILogger<HomeController> logger, IDistributedCache cache, IList<byte[]> blob)
        {
            _redisCache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger;
            _blob = blob;
        }
        // GET: api/<LoadTest>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var cache = new quick_cache.QuickCache();
            return ["Hello World"];
        }

        // GET api/<LoadTest>/5
        [HttpGet("{id}")]
        public async Task<IEnumerable<string[]>> Get(int id)
        {
            switch(id)
            {
                case 0:
                    for (int i = 0; i < 100; i++)
                    {
                        using (_redisDuration.NewTimer())
                        {
                            await _redisCache.SetStringAsync($"blob-{i}", JsonSerializer.Serialize(new byte[1024]));
                            var res = await _redisCache.GetStringAsync($"blob-{i}");
                        }
                    }
                    var res2 = _redisDuration.GetAllLabelValues();
                    return res2;
                    break;
            }

            return [[""]];
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
