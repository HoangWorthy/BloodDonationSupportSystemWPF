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
    /// Interaction logic for UpdateBloodRequestWindow.xaml
    /// </summary>
    public partial class UpdateBloodRequestWindow : Window
    {
        private readonly BloodRequestService _bloodRequestService;
        private readonly ComponentRequestService _componentRequestService;
        private BloodRequest _bloodRequest;

        public UpdateBloodRequestWindow(BloodRequest bloodRequest)
        {
            InitializeComponent();

            _bloodRequestService = new BloodRequestService();
            _componentRequestService = new ComponentRequestService();

            _bloodRequest = bloodRequest;

            LoadComponents();
            LoadBloodRequestData();
        }

        private async void LoadComponents()
        {
            var components = await _componentRequestService.GetAllAsync();
            ComponentComboBox.ItemsSource = components;
            ComponentComboBox.DisplayMemberPath = "ComponentType";
            ComponentComboBox.SelectedValuePath = "Id";

            // Set selected item if exists
            if (_bloodRequest.ComponentRequestId.HasValue)
                ComponentComboBox.SelectedValue = _bloodRequest.ComponentRequestId.Value;
        }

        private void LoadBloodRequestData()
        {
            BloodTypeTextBox.Text = _bloodRequest.BloodType;
            StatusTextBox.Text = _bloodRequest.Status;
        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            _bloodRequest.BloodType = BloodTypeTextBox.Text.Trim();
            _bloodRequest.Status = StatusTextBox.Text.Trim();

            if (ComponentComboBox.SelectedValue is int selectedComponentId)
                _bloodRequest.ComponentRequestId = selectedComponentId;

            try
            {
                await _bloodRequestService.UpdateAsync(_bloodRequest);
                MessageBox.Show("Cập nhật thành công!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật: {ex.Message}");
            }
        }
    }
}
