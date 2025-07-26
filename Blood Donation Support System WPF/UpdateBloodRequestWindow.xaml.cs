
using BLL.Services.Implementations;
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
        private readonly BloodStockService _bloodStockService;
        private readonly BloodRequest _bloodRequest;

        public UpdateBloodRequestWindow(BloodRequest bloodRequest)
        {
            InitializeComponent();

            _bloodRequestService = new BloodRequestService();
            _componentRequestService = new ComponentRequestService();
            _bloodStockService = new BloodStockService();
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
            // Set the selected blood type
            foreach (ComboBoxItem item in BloodTypeComboBox.Items)
            {
                if (item.Content.ToString() == _bloodRequest.BloodType)
                {
                    BloodTypeComboBox.SelectedItem = item;
                    break;
                }
            }

            // Set the selected status
            foreach (ComboBoxItem item in StatusComboBox.Items)
            {
                if (item.Content.ToString() == _bloodRequest.Status)
                {
                    StatusComboBox.SelectedItem = item;
                    break;
                }
            }
        }
       



        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedBloodTypeItem = BloodTypeComboBox.SelectedItem as ComboBoxItem;
            _bloodRequest.BloodType = selectedBloodTypeItem?.Content.ToString();

            var selectedStatusItem = StatusComboBox.SelectedItem as ComboBoxItem;
            _bloodRequest.Status = selectedStatusItem?.Content.ToString();

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
