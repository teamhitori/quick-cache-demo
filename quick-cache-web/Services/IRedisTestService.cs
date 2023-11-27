
using Microsoft.Extensions.Caching.Distributed;
using TeamHitori.QuickCacheWeb.ViewModel;

public interface IRedisTestService
{
    bool IsRunning { get; }
    TestResponse Results { get; }
    void StartTest(bool isParallel, int blockSize);
}