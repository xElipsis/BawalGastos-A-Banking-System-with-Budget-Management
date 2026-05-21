using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using wpfBudgetSys.Helpers;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;
using wpfBudgetSys.View;

namespace wpfBudgetSys.ViewModel
{
    public class RegisterWindowVM : ViewModelBase
    {
        private readonly AuthServices authServices = new();

        private string firstName = string.Empty;
        public string FirstName { get => firstName; set { firstName = value; OnPropertyChanged(); } }

        private string middleName = string.Empty;
        public string MiddleName { get => middleName; set { middleName = value; OnPropertyChanged(); } }

        private string lastName = string.Empty;
        public string LastName { get => lastName; set { lastName = value; OnPropertyChanged(); } }

        private string email = string.Empty;
        public string Email { get => email; set { email = value; OnPropertyChanged(); } }

        private string phone = string.Empty;
        public string Phone { get => phone; set { phone = value; OnPropertyChanged(); } }

        private string username = string.Empty;
        public string Username { get => username; set { username = value; OnPropertyChanged(); } }

        private string password = string.Empty;
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged();
                UpdatePasswordHints();
            }
        }

        private string confirmPassword = string.Empty;
        public string ConfirmPassword
        {
            get => confirmPassword;
            set { confirmPassword = value; OnPropertyChanged(); OnPropertyChanged(nameof(PasswordsMatchText)); }
        }

        public string ReqLength { get; private set; } = "○ At least 8 characters";
        public string ReqUpper { get; private set; } = "○ One uppercase letter";
        public string ReqLower { get; private set; } = "○ One lowercase letter";
        public string ReqDigit { get; private set; } = "○ One number";
        public string ReqSpecial { get; private set; } = "○ One special character";
        public string PasswordsMatchText =>
            PasswordValidator.PasswordsMatch(Password, ConfirmPassword)
                ? "✓ Passwords match"
                : "○ Passwords must match";

        private bool isOtpSent;
        public bool IsOtpSent
        {
            get => isOtpSent;
            set { isOtpSent = value; OnPropertyChanged(); }
        }

        private string otpInput = string.Empty;
        public string OtpInput
        {
            get => otpInput;
            set { otpInput = value; OnPropertyChanged(); }
        }

        private string? message;
        public string? Message
        {
            get => message;
            set { message = value; OnPropertyChanged(); }
        }

        public ICommand ShowLoginWindowCommand { get; }
        public ICommand SendOtpCommand { get; }
        public ICommand CompleteRegistrationCommand { get; }

        public RegisterWindowVM()
        {
            ShowLoginWindowCommand = new RelayCommand(_ =>
            {
                new LoginPage().Show();
                Application.Current.Windows.OfType<Window>()
                    .FirstOrDefault(w => w is RegisterWindowView)?.Close();
            });

            SendOtpCommand = new RelayCommand(_ =>
            {
                if (!IsValidEmail(Email))
                {
                    AppDialog.Show("Enter a valid email address first.", "Verify email", AppDialogIcon.Warning);
                    return;
                }

                try
                {
                    authServices.SendRegistrationOtp(Email.Trim());
                    IsOtpSent = true;
                    AppDialog.Show($"A verification code was sent to {Email}.", "OTP sent", AppDialogIcon.Success);
                }
                catch (Exception ex)
                {
                    AppDialog.Show(ex.Message, "Could not send OTP", AppDialogIcon.Error);
                }
            });

            CompleteRegistrationCommand = new RelayCommand(_ =>
            {
                if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) ||
                    string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Phone) ||
                    string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                {
                    AppDialog.Show("Please fill in all required fields.", "Registration", AppDialogIcon.Warning);
                    return;
                }

                if (!IsOtpSent || string.IsNullOrWhiteSpace(OtpInput))
                {
                    AppDialog.Show("Send and enter the email verification code first.", "Registration", AppDialogIcon.Warning);
                    return;
                }

                var validation = PasswordValidator.Validate(Password);
                if (!validation.IsValid)
                {
                    AppDialog.Show(validation.Summary, "Password requirements", AppDialogIcon.Warning);
                    return;
                }

                if (!PasswordValidator.PasswordsMatch(Password, ConfirmPassword))
                {
                    AppDialog.Show("Password and confirm password do not match.", "Registration", AppDialogIcon.Warning);
                    return;
                }

                string fullName = $"{FirstName} {MiddleName} {LastName}".Trim().Replace("  ", " ");

                bool success = authServices.Register(fullName, Email.Trim(), Phone.Trim(), Username.Trim(), Password, OtpInput.Trim());

                if (!success)
                {
                    AppDialog.Show("Invalid or expired verification code. Request a new code and try again.",
                        "Registration", AppDialogIcon.Error);
                    return;
                }

                AppDialog.Show("Registration successful. You can sign in with your new account.",
                    "Welcome", AppDialogIcon.Success);
                ShowLoginWindowCommand.Execute(null);
            });
        }

        private void UpdatePasswordHints()
        {
            ReqLength = (Password.Length >= 8 ? "✓" : "○") + " At least 8 characters";
            ReqUpper = (Regex.IsMatch(Password, @"[A-Z]") ? "✓" : "○") + " One uppercase letter";
            ReqLower = (Regex.IsMatch(Password, @"[a-z]") ? "✓" : "○") + " One lowercase letter";
            ReqDigit = (Regex.IsMatch(Password, @"[0-9]") ? "✓" : "○") + " One number";
            ReqSpecial = (Regex.IsMatch(Password, @"[^a-zA-Z0-9]") ? "✓" : "○") + " One special character";
            OnPropertyChanged(nameof(ReqLength));
            OnPropertyChanged(nameof(ReqUpper));
            OnPropertyChanged(nameof(ReqLower));
            OnPropertyChanged(nameof(ReqDigit));
            OnPropertyChanged(nameof(ReqSpecial));
            OnPropertyChanged(nameof(PasswordsMatchText));
        }

        private static bool IsValidEmail(string email) =>
            !string.IsNullOrWhiteSpace(email) && Regex.IsMatch(email.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }
}
