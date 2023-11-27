﻿using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Prometheus;
using System.Diagnostics;
using System.Text.Json;
using TeamHitori.QuickCacheWeb.ViewModel;

public class QuickCacheTestService : IQuickCacheTestService
{
    private static QuickCacheTestService? _testServiceInstance = null;
    private static ICache? _cache;
    private static readonly Histogram _redisDurationHist = Metrics
        .CreateHistogram("test_redis_duration_hist", "Histogram of redis call call durations.");

    private static readonly Gauge _redisDurationGauge = Metrics
        .CreateGauge("test_redis_duration_gauge", "Gauge of redis call call durations.");

    private List<float> _results = new List<float>();

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


    public void StartTest()
    {
        if (!IsRunning)
        {
            IsRunning = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(RedisLoadTest));

        }
    }

    private void RedisLoadTest(object state)
    {
        _results = new List<float>();

        Stopwatch stopWatch = new Stopwatch();

        for (int i = 0; i < 5000; i++)
        {
            using (_redisDurationHist.NewTimer())
            {
                using (_redisDurationGauge.NewTimer())
                {
                    stopWatch.Start();
                    _cache.Set($"blob-{i}", new MyStruct() { Arr = new int[1024] });
                    var res = _cache.Get<MyStruct>($"blob-{i}");
                    stopWatch.Stop();
                }
            }

            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = $"{ts.Seconds}.{ts.Milliseconds}";

            this._results.Add(ts.Seconds + ((float)ts.Milliseconds / 1000));
        }

        this.IsRunning = false;
    }

    record struct MyStruct
    {
        public int[] Arr;
    }


}