using DAL.Entities;
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
    /// Interaction logic for StaffWindow.xaml
    /// </summary>
    public partial class StaffWindow : Window
    {
        private readonly BloodRequestService _bloodRequestService;
        public StaffWindow()
        {
            InitializeComponent();
            _bloodRequestService = new BloodRequestService();
            LoadBloodRequest();
        }
        private async void LoadBloodRequest()
        {
            var requests = await _bloodRequestService.GetAllAsync();
            BloodRequestDataGrid.ItemsSource = requests;
        }



        private void AddBloodRequestButton_Click_1(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddBloodRequestWindow();
            var result = addWindow.ShowDialog(); // wait until closed

            // Reload data after the add window is closed
            if (result == true) // optional: only if success
            {
                LoadBloodRequest(); // Replace with your method to refresh data
            }
        }

        private void UpdateBloodRequest_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is BloodRequest request)
            {
                var updateWindow = new UpdateBloodRequestWindow(request);
                updateWindow.ShowDialog();

                LoadBloodRequest(); // reload list after dialog closes
            }
        }

        private async void DeleteBloodRequest_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is BloodRequest request)
            {
                var confirm = MessageBox.Show($"Xác nhận xóa yêu cầu {request.Id}?", "Xác nhận", MessageBoxButton.YesNo);
                if (confirm == MessageBoxResult.Yes)
                {
                    try
                    {
                        var service = new BloodRequestService();
                        await service.DeleteAsync(request.Id);
                        MessageBox.Show("Xóa thành công!");
                        LoadBloodRequest(); // Refresh the list after delete
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa: {ex.Message}");
                    }
                }
            }
        }


        private void DetailBloodRequest_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is BloodRequest request)
            {
                MessageBox.Show($"Chi tiết yêu cầu: {request.Id}\nNhóm máu: {request.BloodType}");
            }
        }
    }
}
