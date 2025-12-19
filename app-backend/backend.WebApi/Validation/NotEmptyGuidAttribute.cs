using System.ComponentModel.DataAnnotations;

namespace Backend.WebApi.Validation
{
    public class NotEmptyGuidAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }

            if (value is Guid guid)
            {
                return guid != Guid.Empty;
            }

            return false;
        }
    }
}
