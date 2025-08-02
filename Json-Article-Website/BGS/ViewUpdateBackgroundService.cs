using System;
using Json_Article_Website.Interface;
using Json_Article_Website.Service;
using Json_Article_Website.Service.Views;

namespace Json_Article_Website.BGS
{
    public class ViewUpdateBackgroundService(
        IServiceProvider serviceProvider,
        IViewCounterService viewCounter,
        ILogger<ViewUpdateBackgroundService> logger
        ) : BackgroundService
    {
        private readonly ILogger<ViewUpdateBackgroundService> _logger = logger;
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly IViewCounterService _viewCounter = viewCounter;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(1); // Change as needed

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_interval, stoppingToken);
                await UpdateViewsAsync(stoppingToken);
            }
        }

        private async Task UpdateViewsAsync(CancellationToken cancellationToken)
        {
            var counts = _viewCounter.GetAndResetCounts();

            if (counts.Count == 0) 
                return;

            using var scope = _serviceProvider.CreateScope();
            var articleService = scope.ServiceProvider.GetRequiredService<IArticleService>();

            foreach (var (articleId, viewsToAdd) in counts)
            {
                //var article = await dbContext.Articles
                //    .FindAsync(new object[] { articleId }, cancellationToken);

                var articleDetails = await articleService.GetArticleDetailsAsync(articleId, cancellationToken);

                if (articleDetails != null)
                {
                    articleDetails.Views += viewsToAdd;
                    await articleService.PutArticleAsync(articleId, articleDetails,  cancellationToken);
                }

            }

            //await dbContext.SaveChangesAsync(cancellationToken);
            

            _logger.LogInformation("Updated view counts for {Count} articles", counts.Count);
        }
    }

}
