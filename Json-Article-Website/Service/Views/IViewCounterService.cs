namespace Json_Article_Website.Service.Views
{
    public interface IViewCounterService
    {
        void IncrementView(int articleId);
        Dictionary<int, int> GetAndResetCounts();
    }

}
