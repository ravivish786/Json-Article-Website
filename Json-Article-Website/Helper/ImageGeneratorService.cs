using System.Drawing;
using Json_Article_Website.Extention;
using SkiaSharp;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Json_Article_Website.Helper
{
    public class ImageGeneratorService(IWebHostEnvironment _env)
    {
        public byte[] GenerateArticleImageWebP(string title, string authorName, string siteName, int width = 1200, int height = 800)
        {
            using var bitmap = new SKBitmap(width, height);
            using var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);

            // Background
            var bgPaint = new SKPaint
            {
                Color = SKColors.LightGray,
                Style = SKPaintStyle.Fill
            };
            canvas.DrawRect(new SKRect(0, 0, width, height), bgPaint);

            // Title
            var titlePaint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 60,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold)
            };
            var titleX = 60;
            var titleY = 280;
            canvas.DrawText(title, titleX, titleY, titlePaint);

            // Author Name
            var authorPaint = new SKPaint
            {
                Color = SKColors.DarkSlateGray,
                TextSize = 40,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Normal)
            };
            var authorY = titleY + 80;
            canvas.DrawText($"By {authorName}", titleX, authorY, authorPaint);

            // Site Footer
            var footerPaint = new SKPaint
            {
                Color = SKColors.DimGray,
                TextSize = 32,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Italic)
            };
            canvas.DrawText(siteName, titleX, height - 60, footerPaint);

            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Webp, 90);
            return data.ToArray();
        }

        public byte[] GenerateArticleImagePng(string title, string authorName, string siteName, int width = 1200, int height = 800)
        {
            using var bitmap = new SKBitmap(width, height);
            using var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);
            // Background
            var bgPaint = new SKPaint
            {
                Color = SKColors.LightGray,
                Style = SKPaintStyle.Fill
            };
            canvas.DrawRect(new SKRect(0, 0, width, height), bgPaint);
            // Title
            var titlePaint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 60,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold)
            };
            var titleX = 60;
            var titleY = 280;
            canvas.DrawText(title, titleX, titleY, titlePaint);
            // Author Name
            var authorPaint = new SKPaint
            {
                Color = SKColors.DarkSlateGray,
                TextSize = 40,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Normal)
            };
            var authorY = titleY + 80;
            canvas.DrawText($"By {authorName}", titleX, authorY, authorPaint);
            // Site Footer
            var footerPaint = new SKPaint
            {
                Color = SKColors.DimGray,
                TextSize = 32,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Italic)
            };
            canvas.DrawText(siteName, titleX, height - 60, footerPaint);
            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 90);
            return data.ToArray();
        }


        public byte[] GenerateArticleImageJpeg(string title, string authorName, string siteName, int width = 1200, int height = 800)
        {
            using var bitmap = new SKBitmap(width, height);
            using var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);
            // Background
            var bgPaint = new SKPaint
            {
                Color = SKColors.LightGray,
                Style = SKPaintStyle.Fill
            };
            canvas.DrawRect(new SKRect(0, 0, width, height), bgPaint);
            // Title
            var titlePaint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 60,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold)
            };
            var titleX = 60;
            var titleY = 280;
            canvas.DrawText(title, titleX, titleY, titlePaint);
            // Author Name
            var authorPaint = new SKPaint
            {
                Color = SKColors.DarkSlateGray,
                TextSize = 40,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Normal)
            };
            var authorY = titleY + 80;
            canvas.DrawText($"By {authorName}", titleX, authorY, authorPaint);
            // Site Footer
            var footerPaint = new SKPaint
            {
                Color = SKColors.DimGray,
                TextSize = 32,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Italic)
            };
            canvas.DrawText(siteName, titleX, height - 60, footerPaint);
            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Jpeg, 90);
            return data.ToArray();
        }


        public string SaveImageToFile(byte[] imageBytes, string filePath)
        {
            if (imageBytes == null || imageBytes.Length == 0)
            {
                throw new ArgumentException("Image bytes cannot be null or empty.", nameof(imageBytes));
            }
            var directory = Path.GetDirectoryName(filePath);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.WriteAllBytes(filePath, imageBytes);
            return filePath;


        }


        public void DeleteImageFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            else
            {
                throw new FileNotFoundException("Image file not found.", filePath);
            }
        }

        public byte[]? ReadImageFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllBytes(filePath);
            }
            return null; // or throw an exception if preferred
        }
         
        public string SanitizeFileName(string input)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                input = input.Replace(c, '_');
            }
            return input.Replace(" ", "-").ToLower();
        }

        public string GenerateDefaultImage(string title, string authorName, string siteName )
        {
            var sanitizedTitle = title;
            var sanitizedAuthor = authorName;
            var sanitizedSite = SanitizeFileName(siteName);

            // Create safe filename
            string randomImageName = DateTime.Now.EncodeTime();
            string fileName = $"{SanitizeFileName($"default{randomImageName}")}.webp";
            string folderPath = Path.Combine(_env.WebRootPath, "generated-images");
            Directory.CreateDirectory(folderPath);

            string fullPath = Path.Combine(folderPath, fileName);


            byte[] imageBytes = GenerateArticleImageWebP(title, authorName, siteName);
            SaveImageToFile(imageBytes, fullPath);
             
            // Return relative URL
            return $"/generated-images/{fileName}";
             
        }

    }
}
