using System.Threading.Tasks;

namespace Json_Article_Website.Helper
{
    public class FileService(IWebHostEnvironment environment)
    {
        public readonly string _appDataPath = Path.Combine(environment.ContentRootPath, "App_Data");

        public async Task<string> SaveFileAsync(string filePath, byte[] bytes)
        {
            //var fullPath = Path.Combine(_appDataPath, filePath);
            var directory = Path.GetDirectoryName(filePath);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            await File.WriteAllBytesAsync(filePath, bytes);
            return filePath;
        }

        public async Task<byte[]?> ReadFileAsync(string filePath)
        {
            //var fullPath = Path.Combine(_appDataPath, filePath);
 
            return File.Exists(filePath) 
                ? await File.ReadAllBytesAsync(filePath) 
                : default;
        }

        public async Task<bool> DeleteFileAsync(string filePath)
        {
            //var fullPath = Path.Combine(_appDataPath, filePath);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
                return true;
            }
            return false;
        }

        #region Helper Methods

        public string GetIndexPath(int pageno)
        {
            return Path.Combine(_appDataPath, "index", $"index_{pageno}.json");
        }

        public string GetArticlePath(int id)
        {
            return Path.Combine(_appDataPath, "articles", $"article_{id}.json");
        }

        #endregion
    }
}
