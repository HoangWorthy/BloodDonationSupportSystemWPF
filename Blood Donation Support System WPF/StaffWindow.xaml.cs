using BLL.Services;
using BLL.Services.Implementations;
using BLL.Services.Implementations;
using BloodDonationSupportSystemWPF;
using DAL.Entities;
using DAL.Entities;
using DAL.Repositories;
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
        private readonly IBlogService _blogService;
        public Account account{ get; set; }
        public StaffWindow()
        {
            InitializeComponent();
            donationEventService = new DonationEventService();
            LoadDonationEvent();
            _bloodRequestService = new BloodRequestService();
            _bloodStockService = new BloodStockService();
            _blogService = new BlogService();
            LoadBloodRequest();
            LoadBlog();
            CreateSampleData(); // Tạo dữ liệu mẫu nếu cần
            CheckDatabaseTables(); // Kiểm tra database khi khởi động
        }
        private void LoadDonationEvent()
        {
            EventDataGrid.ItemsSource = donationEventService.GetDonationEvents();
        }
        private void AddEventButton_Click(object sender, RoutedEventArgs e)
        {
            var modal = new AddDonationEventWindow();
            modal.Owner = this;
            modal.CurrentUserId = account.Id;
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

        private void LoadBlog()
        {
            BlogDataGrid.ItemsSource = _blogService.GetBlogs();
        }

        private void CreateSampleData()
        {
            try
            {
                using (var context = new DAL.Entities.BloodDonationSupportSystemContext())
                {
                    // Kiểm tra xem đã có data chưa
                    if (context.Accounts.Any())
                    {
                        return; // Đã có data, không cần tạo thêm
                    }

                    // Tạo Profile mẫu
                    var sampleProfile = new DAL.Entities.Profile
                    {
                        Name = "Staff",
                        Phone = "0123456789",
                        Address = "123 Test Street",
                        DateOfBirth = System.DateOnly.FromDateTime(DateTime.Now.AddYears(-25))
                    };
                    context.Profiles.Add(sampleProfile);
                    context.SaveChanges();

                    // Tạo Account mẫu
                    var sampleAccount = new DAL.Entities.Account
                    {
                        Email = "staff@test.com",
                        Password = "123456",
                        Role = "STAFF",
                        Status = "ENABLE",
                        ProfileId = sampleProfile.Id
                    };
                    context.Accounts.Add(sampleAccount);
                    context.SaveChanges();

                    MessageBox.Show("Đã tạo dữ liệu mẫu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo dữ liệu mẫu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CheckDatabaseTables()
        {
            try
            {
                using (var context = new DAL.Entities.BloodDonationSupportSystemContext())
                {
                    var accountCount = context.Accounts.Count();
                    var blogCount = context.Blogs.Count();
                    
                    var info = $"Database Status:\n- Accounts: {accountCount}\n- Blogs: {blogCount}";
                    MessageBox.Show(info, "Database Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi kiểm tra database: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private long GetValidAuthorId()
        {
            try
            {
                using (var context = new DAL.Entities.BloodDonationSupportSystemContext())
                {
                    // Kiểm tra kết nối database
                    if (!context.Database.CanConnect())
                    {
                        MessageBox.Show("Không thể kết nối đến database!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return 1;
                    }
                    
                    var accounts = context.Accounts.ToList();
                    if (!accounts.Any())
                    {
                        MessageBox.Show("Không có account nào trong database!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return 1;
                    }
                    
                    var firstAccount = accounts.First();
                    return firstAccount.Id;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy AuthorId: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return 1; // Fallback value
            }
        }

        private void AddBlogButton_Click(object sender, RoutedEventArgs e)
        {

            // Truyền vào cửa sổ
            var addWindow = new AddBlogWindow();
            var result = addWindow.ShowDialog();

            if (result == true)
            {
                LoadBlog(); // Hoặc hàm bạn cần gọi lại
            }
        }




        private void UpdateBlogButton_Click(object sender, RoutedEventArgs e)
        {
            if (BlogDataGrid.SelectedItem is DAL.Entities.Blog selectedBlog)
            {
                // TODO: Hiển thị cửa sổ chỉnh sửa Blog (tùy chỉnh giao diện theo ý bạn)
                selectedBlog.Title = "Tiêu đề đã sửa";
                selectedBlog.LastModifiedDate = System.DateOnly.FromDateTime(DateTime.Now);
                _blogService.UpdateBlog(selectedBlog);
                LoadBlog();
            }
        }

        private void DeleteBlogButton_Click(object sender, RoutedEventArgs e)
        {
            if (BlogDataGrid.SelectedItem is DAL.Entities.Blog selectedBlog)
            {
                var confirm = MessageBox.Show($"Xác nhận xóa bài viết '{selectedBlog.Title}'?", "Xác nhận", MessageBoxButton.YesNo);
                if (confirm == MessageBoxResult.Yes)
                {
                    _blogService.RemoveBlog(selectedBlog);
                    LoadBlog();
                }
            }
        }

    }
}
