namespace Json_Article_Website.Helper
{
    public class FileService(IWebHostEnvironment environment)
    {
        private readonly string _appDataPath = Path.Combine(environment.ContentRootPath, "App_Data");

        public string SaveFile(string filePath, byte[] bytes)
        {
            var fullPath = Path.Combine(_appDataPath, filePath);
            var directory = Path.GetDirectoryName(fullPath);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.WriteAllBytes(fullPath, bytes);
            return fullPath;
        }

        public byte[]? ReadFile(string filePath)
        {
            var fullPath = Path.Combine(_appDataPath, filePath);
 
            return File.Exists(fullPath) ? File.ReadAllBytes(fullPath) : default;
        }
    }
}
