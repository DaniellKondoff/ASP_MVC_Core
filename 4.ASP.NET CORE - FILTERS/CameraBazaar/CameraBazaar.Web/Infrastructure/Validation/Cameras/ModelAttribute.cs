using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CameraBazaar.Web.Infrastructure.Validation.Cameras
{
    public class ModelVAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var model = value as string;

            if (model == null)
            {
                return true;
            }

            return model.Any(c => char.IsUpper(c))
                || model.Contains("-")
                || model.Any(c => char.IsDigit(c));
        }

        public override string FormatErrorMessage(string name)
        {
            return $"Model {name} must contain only uppercase letters, digits and dash(“-“).";
        }
    }
}
