using NewTiceAI.Models;

namespace NewTiceAI.Helpers
{
    public static class FileUploader
    {
        //public static readonly string DefaultProfilePictureUrl = "/img/DefaultUserImage.png";
        public static readonly string DefaultUserImage = "/img/DefaultContactImage.png";
        public static readonly string DefaultBlogImage = "/img/DefaultBlogImage.jpg";
        public static readonly string DefaultCategoryImage = "/img/DefaultCategoryImage.png";

        public static async Task<FileUpload> GetFileUploadAsync(IFormFile file)
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            byte[] data = ms.ToArray();

            if (ms.Length > 5 * 1024 * 1024)
            {
                throw new IOException("Images must be less than 5MB");
            }

            FileUpload fileUpload = new()
            {
                Id = Guid.NewGuid(),
                Data = data,
                Type = file.ContentType
            };

            return fileUpload;
        }
    }
}
