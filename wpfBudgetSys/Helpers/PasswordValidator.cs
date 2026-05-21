using System.Text.RegularExpressions;

namespace wpfBudgetSys.Helpers
{
    public static class PasswordValidator
    {
        public static PasswordValidationResult Validate(string? password)
        {
            var result = new PasswordValidationResult();
            if (string.IsNullOrEmpty(password))
            {
                result.Errors.Add("Password is required.");
                return result;
            }

            if (password.Length < 8)
                result.Errors.Add("At least 8 characters.");
            if (!Regex.IsMatch(password, @"[A-Z]"))
                result.Errors.Add("At least one uppercase letter.");
            if (!Regex.IsMatch(password, @"[a-z]"))
                result.Errors.Add("At least one lowercase letter.");
            if (!Regex.IsMatch(password, @"[0-9]"))
                result.Errors.Add("At least one number.");
            if (!Regex.IsMatch(password, @"[^a-zA-Z0-9]"))
                result.Errors.Add("At least one special character.");

            result.IsValid = result.Errors.Count == 0;
            return result;
        }

        public static bool PasswordsMatch(string? password, string? confirm) =>
            !string.IsNullOrEmpty(password) && password == confirm;
    }

    public class PasswordValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; } = new();
        public string Summary => IsValid ? "Password meets all requirements." : string.Join(" ", Errors);
    }
}
