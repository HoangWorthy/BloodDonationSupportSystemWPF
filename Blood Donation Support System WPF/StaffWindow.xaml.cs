using BLL.Services;
using BLL.Services.Implementations;
using DAL.Entities;
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
    /// Interaction logic for StaffWindow.xaml
    /// </summary>
    public partial class StaffWindow : Window
    {
        private readonly IDonationEventService donationEventService;
        private readonly BloodRequestService _bloodRequestService;
        private readonly BloodStockService _bloodStockService;

        public StaffWindow()
        {
            InitializeComponent();
            donationEventService = new DonationEventService();
            LoadDonationEvent();
            _bloodRequestService = new BloodRequestService();
            _bloodStockService = new BloodStockService();
            LoadBloodRequest();
        }
        private void LoadDonationEvent()
        {
            EventDataGrid.ItemsSource = donationEventService.GetDonationEvents();
        }
        private void AddEventButton_Click(object sender, RoutedEventArgs e)
        {
            var modal = new AddDonationEventWindow();
            modal.Owner = this;
            modal.InitializeDefaults();

            if (modal.ShowDialog() == true)
            {
                DonationEvent newEvent = modal.NewEvent;
                donationEventService.AddDonationEvent(newEvent);
                LoadDonationEvent();
            }
        }

        private void btnUpdateEvent_Click(object sender, RoutedEventArgs e)
        {
            var modal = new AddDonationEventWindow();
            DonationEvent donationEvent = (DonationEvent)EventDataGrid.SelectedItem;
            modal.NewEvent = donationEvent;
            modal.isUpdate = true;
            modal.InitializeDefaults();
            modal.Owner = this;

            if (modal.ShowDialog() == true)
            {
                DonationEvent donation = modal.NewEvent;
                donationEvent.Name = donation.Name;
                donationEvent.Address = donation.Address;
                donationEvent.Ward = donation.Ward;
                donationEvent.District = donation.District;
                donationEvent.City = donation.City;
                donationEvent.DonationDate = donation.DonationDate;
                donationEvent.RegisteredMemberCount = donation.RegisteredMemberCount;
                donationEvent.Status = donation.Status;
                donationEvent.DonationType = donation.DonationType;
                donationEvent.DonationTimeSlots = donation.DonationTimeSlots;
                donationEventService.UpdateDonationEvent(donationEvent);
                LoadDonationEvent();
            }
        }

        private void btnDeleteEvent_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure to remove this?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(result == MessageBoxResult.Yes)
            {
                DonationEvent donationEvent = (DonationEvent)EventDataGrid.SelectedItem;
                donationEventService.RemoveDonationEvent(donationEvent);
                MessageBox.Show("Delete successfully!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadDonationEvent();
            }
        }
            
        private async void LoadBloodRequest()
        {
            var requests = await _bloodRequestService.GetAllAsync();

            foreach (var request in requests)
            {
                await ProcessBloodRequestIfStockAvailableAsync(request);
            }

            // Load lại dữ liệu sau khi cập nhật
            BloodRequestDataGrid.ItemsSource = await _bloodRequestService.GetAllAsync();
        }

        private async Task ProcessBloodRequestIfStockAvailableAsync(BloodRequest request)
        {
            if (request.Status != "Pending" && request.Status != "Processing")
                return;

            var allStocks = await _bloodStockService.GetAllAsync();
            var matchingStock = allStocks.FirstOrDefault(stock =>
                stock.BloodType.Trim().Equals(request.BloodType.Trim(), StringComparison.OrdinalIgnoreCase) &&
                stock.Volume > 0);
          
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
                var detailWindow = new BloodRequestDetailWindow(request);
                detailWindow.ShowDialog();
            }
        }

    }
}
