using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FriendlySetTime
{

    public class NumberValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                int number = Int32.Parse((String)value);
                if (number < 0)
                {
                    return new ValidationResult(false, "value cannot be less than 0");
                }
                return ValidationResult.ValidResult;
            } catch (Exception e)
            { //TODO: this isn't working/being used
                return new ValidationResult(false, $"{value} is not a number");
            }
        }
    }
}
