using System.ComponentModel.DataAnnotations;

namespace CameraBazaar.Web.Infrastructure.Validation.Cameras
{
    public class URLAttribute : ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return "URL must start with http:// or https://";
        }

        public override bool IsValid(object value)
        {
            var url = value as string;

            if (url == null)
            {
                return true;
            }

            return url.StartsWith("http://") || url.StartsWith("https://");
        }
    }
}
