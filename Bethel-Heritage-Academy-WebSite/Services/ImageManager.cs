using Microsoft.AspNetCore.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;


namespace Bethel_Heritage_Academy_WebSite.Services
{
    public interface IImageManager
    {
        Task<string> CompressAndSaveImageAsync(IFormFile imageFile, string outputPath, int targetSizeKB = 120, string? oldImagePath = null);
    }

    public class ImageManager : IImageManager
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public ImageManager(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<string> CompressAndSaveImageAsync(IFormFile imageFile, string outputPath, int targetSizeKB = 120, string? oldImagePath = null)
        {

            if (!string.IsNullOrEmpty(oldImagePath))
            {
                string deleteOldImage = System.IO.Path.Combine(webHostEnvironment.WebRootPath, oldImagePath);
                if (System.IO.File.Exists(deleteOldImage))
                {
                    System.IO.File.Delete(deleteOldImage);
                }
            }


            using var imageStream = imageFile.OpenReadStream();
            using var image = await Image.LoadAsync(imageStream);
            string fileName = Guid.NewGuid().ToString();
            var exten = System.IO.Path.GetExtension(imageFile.FileName);
            var upload = webHostEnvironment.WebRootPath + "/" + outputPath;
            // Check the initial file size
            if (imageFile.Length / 1024 <= targetSizeKB)
            {
                // Save the image directly if it's already less than the target size
                var filePath = System.IO.Path.Combine(upload, fileName + exten);
                using var fileStream = new FileStream(filePath, System.IO.FileMode.Create);
                await imageFile.CopyToAsync(fileStream);
                return outputPath + fileName + exten;
            }
            var resizeOptions = new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(image.Width / 2, image.Height / 2) // Adjust the size as needed
            };
            image.Mutate(x => x.Resize(resizeOptions));

            var encoder = new JpegEncoder();

            // Compress the image until it meets the target size
            int quality = 90;
            long fileSize;
            do
            {
                using var memoryStream = new MemoryStream();
                await image.SaveAsJpegAsync(memoryStream, new JpegEncoder { Quality = quality });
                fileSize = memoryStream.Length / 1024; // Size in KB
                quality -= 5; // Reduce quality incrementally
            } while (fileSize > targetSizeKB && quality > 2);
            quality = quality < 1 ? 1 : quality;
            // Save the compressed image to the specified output path
            var filePath1 = System.IO.Path.Combine(upload, fileName + exten);
            var compressedFilePath = System.IO.Path.Combine(outputPath, fileName + exten);
            await image.SaveAsJpegAsync(filePath1, new JpegEncoder { Quality = quality });

            return outputPath + fileName + exten;
        }
    }
    
}
