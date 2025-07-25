using DAL.Entities;
using Service;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Blood_Donation_Support_System_WPF
{
    /// <summary>
    /// Interaction logic for AddBloodRequestWindow.xaml
    /// </summary>
    public partial class AddBloodRequestWindow : Window
    {
        private readonly BloodRequestService _bloodRequestService;
        private readonly ComponentRequestService _componentRequestService;
        public AddBloodRequestWindow()
        {
            InitializeComponent();

            _bloodRequestService = new BloodRequestService();
            _componentRequestService = new ComponentRequestService();

            LoadComponents();
        }

        private async void LoadComponents()
        {
            var components = await _componentRequestService.GetAllAsync();
            ComponentComboBox.ItemsSource = components;
        }

        private async void AddBloodRequestButton_Click(object sender, RoutedEventArgs e)
        {
            string bloodType = BloodTypeTextBox.Text.Trim();
            string status = StatusTextBox.Text.Trim();

            if (string.IsNullOrEmpty(bloodType))
            {
                MessageBox.Show("Please enter a blood type.");
                return;
            }

            var selectedComponent = ComponentComboBox.SelectedItem as ComponentRequest;

            var newRequest = new BloodRequest
            {
                BloodType = bloodType,
                CreatedDate = DateTime.Now,
                Status = status,
                ComponentRequestId = selectedComponent?.Id
            };

            await _bloodRequestService.AddAsync(newRequest);

            MessageBox.Show("Blood request added successfully!");
            this.DialogResult = true; // triggers `ShowDialog()` return
            this.Close();

        }



    }
}
