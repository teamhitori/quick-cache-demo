using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Prometheus;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using TeamHitori.QuickCacheWeb.ViewModel;

public partial class QuickCacheTestService : IQuickCacheTestService
{
    private static QuickCacheTestService? _testServiceInstance = null;
    private static ICache? _cache;
    private static readonly Histogram _redisDurationHist = Metrics
        .CreateHistogram("test_redis_duration_hist", "Histogram of redis call call durations.");

    private static readonly Gauge _redisDurationGauge = Metrics
        .CreateGauge("test_redis_duration_gauge", "Gauge of redis call call durations.");

    private ConcurrentBag<float> _results = new ConcurrentBag<float>();
    private bool _isParallel;
    private int _blockSize;

    public TestResponse Results
    {
        get
        {
            return new TestResponse(IsRunning, _results.ToArray());
        }
    }

    public bool IsRunning
    {
        get;
        private set;
    }

    private QuickCacheTestService()
    {
    }

    public static QuickCacheTestService Instance(ICache cache)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _testServiceInstance ??= new QuickCacheTestService();
        return _testServiceInstance;
    }


    public void StartTest(bool isParallel, int blockSize)
    {
        if (!IsRunning)
        {
            this._isParallel = isParallel;
            this._blockSize = blockSize;
            IsRunning = true;
            Task.Run(() => LoadTest());

        }
    }

    private async Task LoadTest()
    {
        
        _results = new ConcurrentBag<float>();

        Stopwatch stopWatch = new Stopwatch();

        if (this._isParallel)
        {
            var tasks = new List<Task>();

            // Act
            for (int i = 0; i < 1000; i++)
            {
                tasks.Add(Task.Run(() => ReadWrite(stopWatch, i)));
            }

            await Task.WhenAll(tasks.ToArray());

        }
        else
        {
            for (int i = 0; i < 1000; i++)
            {
                ReadWrite(stopWatch, i);
            }
        }

        this.IsRunning = false;
    }

    private void ReadWrite(Stopwatch stopWatch, int i)
    {
        using (_redisDurationHist.NewTimer())
        {
            using (_redisDurationGauge.NewTimer())
            {
                stopWatch.Start();
                _cache.Set($"blob-{i % 20}", new MyStruct() { Arr = new int[_blockSize] });
                var res = _cache.Get<MyStruct>($"blob-{i}");
                stopWatch.Stop();
            }
        }

        TimeSpan ts = stopWatch.Elapsed;
        stopWatch.Reset();

        this._results.Add(ts.Seconds * 1000 + ts.Milliseconds);

        Thread.Sleep(10);
    }
}