using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace LearningSystem.Web.Infrastructure.Extensions
{
    public static class FormFileExtensions
    {
        public static async Task<byte[]> ToByteArrayAsync(this IFormFile formFIle)
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFIle.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
