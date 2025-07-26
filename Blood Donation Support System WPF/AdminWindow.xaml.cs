using BLL.Services.Implementations;
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
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private readonly BloodRequestService _bloodRequestService;
        private readonly BloodStockService _bloodStockService;
        public AdminWindow()
        {
            _bloodStockService = new BloodStockService();
            _bloodRequestService = new BloodRequestService();
            LoadBloodRequest();
        }
        private async void LoadBloodRequest()
        {
            var requests = await _bloodRequestService.GetAllAsync();

            foreach (var request in requests)
            {
                await ProcessBloodRequestIfStockAvailableAsync(request);
            }

            // Load lại dữ liệu sau khi cập nhật
            AdminBloodRequestDataGrid.ItemsSource = await _bloodRequestService.GetAllAsync();
        }

        private async Task ProcessBloodRequestIfStockAvailableAsync(BloodRequest request)
        {
            if (request.Status != "Pending" && request.Status != "Processing")
                return;

            var allStocks = await _bloodStockService.GetAllAsync();

            var matchingStock = allStocks.FirstOrDefault(stock =>
                stock.BloodType == request.BloodType && stock.Volume > 0);

            if (matchingStock != null)
            {
                // Reduce stock and mark request as fulfilled
                matchingStock.Volume -= 1;
                await _bloodStockService.UpdateAsync(matchingStock);

                request.Status = "Fulfilled";
                await _bloodRequestService.UpdateAsync(request);

                MessageBox.Show($"Yêu cầu đã được xử lý với nhóm máu {request.BloodType}. Trạng thái chuyển thành 'Fulfilled'.");
            }
          
        }



        private void BloodRequestDetailButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is BloodRequest request)
            {
                var detailWindow = new BloodRequestDetailWindow(request);
                detailWindow.ShowDialog();
            }
        }
    }
}
