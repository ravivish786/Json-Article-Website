using System.Collections.Concurrent;
using Json_Article_Website.BGS;
using Newtonsoft.Json;

namespace Json_Article_Website.Service.Views
{
    public class InMemoryViewCounterService(
        ILogger<InMemoryViewCounterService> logger
        ) : IViewCounterService
    {
        private readonly ConcurrentDictionary<int, int> _viewCounts = new();
        private readonly ILogger<InMemoryViewCounterService> _logger = logger;
        public void IncrementView(int articleId)
        {
            _viewCounts.AddOrUpdate(articleId, 1, (_, count) => count + 1);
        }

        public Dictionary<int, int> GetAndResetCounts()
        {
            var snapshot = new Dictionary<int, int>(_viewCounts);
            _logger.LogInformation("Counts : " + JsonConvert.SerializeObject(snapshot));
            _viewCounts.Clear();
            return snapshot;
        }
    }

}
