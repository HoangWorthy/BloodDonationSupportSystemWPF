using BLL.Services;
using BLL.Services.Implementations;
using DAL.Entities;
using DAL.Repositories;
using DAL.Repositories.Implementations;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Blood_Donation_Support_System_WPF
{
    public partial class AuthenticationWindow : Window
    {
        private readonly IAccountService _accountService;

        public AuthenticationWindow()
        {
            InitializeComponent();

            var context = new BloodDonationSupportSystemContext();
            var accountRepo = new AccountRepository(context);
            _accountService = new AccountService(accountRepo);

            LoginButton.Click += LoginButton_Click;
            RegisterButton.Click += RegisterButton_Click;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var email = LoginEmailTextBox.Text.Trim();
            var password = LoginPasswordBox.Password;

            var account = _accountService.Login(email, password);
            if (account != null)
            {
                Window nextWindow;
                if (account.Role == "MEMBER")
                {
                    nextWindow = new MemberWindow();
                }
                else if (account.Role == "STAFF" || account.Role == "ADMIN")
                {
                    nextWindow = new StaffWindow();
                }
                else
                {
                    MessageBox.Show("Invalid role.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                nextWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Incorrect email or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var email = RegisterEmailTextBox.Text.Trim();
            var password = RegisterPasswordBox.Password;
            var fullName = FullNameTextBox.Text.Trim();
            var personalId = PersonalIdTextBox.Text.Trim();
            var phone = PhoneTextBox.Text.Trim();
            var birthDate = BirthDatePicker.SelectedDate;
            var gender = (GenderComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
            var bloodType = (BloodTypeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
            var role = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
            var address = AddressTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(personalId) ||
                string.IsNullOrWhiteSpace(phone) ||
                birthDate == null ||
                string.IsNullOrWhiteSpace(gender) ||
                string.IsNullOrWhiteSpace(bloodType) ||
                string.IsNullOrWhiteSpace(role) ||
                string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("Please fill in all required information.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Invalid email format.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password.Length < 5)
            {
                MessageBox.Show("Password must be at least 5 characters long.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (birthDate > DateTime.Today)
            {
                MessageBox.Show("Birthdate cannot be in the future.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_accountService.EmailExists(email))
            {
                MessageBox.Show("Email already exists. Please choose another one.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateOnly birthDateOnly = DateOnly.FromDateTime(birthDate.Value);

            var profile = new Profile
            {
                Name = fullName,
                Gender = gender,
                BloodType = bloodType,
                DateOfBirth = birthDateOnly,
                PersonalId = personalId,
                Phone = phone,
                Address = address
            };

            var account = new Account
            {
                Email = email,
                Password = password,
                Role = role ?? "MEMBER",
                Status = "ACTIVE",
                Profile = profile
            };

            bool success = _accountService.Register(account);

            if (success)
            {
                MessageBox.Show("Registration successful! You can now log in.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("An error occurred during registration. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
