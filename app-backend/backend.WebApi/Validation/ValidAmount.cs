using System.ComponentModel.DataAnnotations;

namespace Backend.WebApi.Validation
{
    public class ValidAmount : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is int number && number != 0 && number <= 1000000 && number >= -1000000)
            {
                return true;
            }

            return false;
        }
    }
}

