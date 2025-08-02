using System.Collections.Concurrent;

namespace Json_Article_Website.Service.Views
{
    public class InMemoryViewCounterService : IViewCounterService
    {
        private readonly ConcurrentDictionary<int, int> _viewCounts = new();

        public void IncrementView(int articleId)
        {
            _viewCounts.AddOrUpdate(articleId, 1, (_, count) => count + 1);
        }

        public Dictionary<int, int> GetAndResetCounts()
        {
            var snapshot = new Dictionary<int, int>(_viewCounts);
            _viewCounts.Clear();
            return snapshot;
        }
    }

}
