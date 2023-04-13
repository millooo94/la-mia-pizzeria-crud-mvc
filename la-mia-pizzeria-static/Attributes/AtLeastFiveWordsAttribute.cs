using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.Attributes
{
	public class AtLeastFiveWordsAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object? value, ValidationContext _)
		{
			var input = value as string;

			if (input is null || input.Trim().Split(' ').Length < 5)
			{
				return new ValidationResult(ErrorMessage ?? $"Please provide at least 5 words");
			}

			return ValidationResult.Success!;
		}
	}
}
