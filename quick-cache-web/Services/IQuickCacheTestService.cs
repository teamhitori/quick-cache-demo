
using Microsoft.Extensions.Caching.Distributed;
using TeamHitori.QuickCacheWeb.ViewModel;

public interface IQuickCacheTestService
{
    bool IsRunning { get; }
    TestResponse Results { get; }
    void StartTest();
}