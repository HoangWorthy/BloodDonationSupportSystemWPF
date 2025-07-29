using BLL.Services;
using BLL.Services.Implementations;
using DAL.Entities;
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
    /// Interaction logic for MemberWindow.xaml
    /// </summary>
    public partial class MemberWindow : Window
    {
        private readonly IDonationEventService donationEventService;
        private readonly IEventRegistrationService eventRegistrationService;
        
        public long CurrentUserId { get; set; } = 1; // This should be set from the logged-in user
        private List<DonationEvent> allEvents;
        private List<DonationEvent> filteredEvents;

        public MemberWindow()
        {
            InitializeComponent();
            donationEventService = new DonationEventService();
            eventRegistrationService = new EventRegistrationService();
            LoadEvents();
            SetupEventHandlers();
        }

        private void SetupEventHandlers()
        {
            if (EventSearchTextBox != null)
            {
                EventSearchTextBox.TextChanged += EventSearchTextBox_TextChanged;
            }

            if (CityFilterComboBox != null)
            {
                CityFilterComboBox.SelectionChanged += CityFilterComboBox_SelectionChanged;
            }

            if (SearchButton != null)
            {
                SearchButton.Click += SearchButton_Click;
            }
        }

        private void LoadEvents()
        {
            try
            {
                allEvents = donationEventService.GetDonationEvents()
                    .Where(e => e.Status == "ACTIVE" && e.DonationDate >= DateOnly.FromDateTime(DateTime.Today))
                    .ToList();

                filteredEvents = new List<DonationEvent>(allEvents);
                DisplayEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách sự kiện: {ex.Message}", "Lỗi", 
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisplayEvents()
        {
            if (EventsListView == null) return;

            EventsListView.ItemsSource = filteredEvents.Select(eventItem => new EventDisplayModel
            {
                Event = eventItem,
                Name = eventItem.Name,
                Location = eventItem.Location,
                Date = eventItem.DonationDate.ToString("dd/MM/yyyy"),
                TimeRange = GetEventTimeRange(eventItem),
                RegistrationCount = $"{eventRegistrationService.GetRegistrationCountByEvent(eventItem.Id)}/{eventItem.TotalMemberCount}",
                IsFull = eventRegistrationService.IsEventFull(eventItem.Id),
                IsUserRegistered = eventRegistrationService.IsUserRegisteredForEvent(CurrentUserId, eventItem.Id)
            });
        }

        private string GetEventTimeRange(DonationEvent donationEvent)
        {
            if (donationEvent.DonationTimeSlots == null || !donationEvent.DonationTimeSlots.Any())
                return "Chưa có lịch trình";

            var firstSlot = donationEvent.DonationTimeSlots.OrderBy(x => x.StartTime).First();
            var lastSlot = donationEvent.DonationTimeSlots.OrderByDescending(x => x.EndTime).First();

            return $"{firstSlot.StartTime:HH:mm} - {lastSlot.EndTime:HH:mm}";
        }

        private void EventSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void CityFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (allEvents == null) return;

            var searchText = EventSearchTextBox?.Text?.ToLower() ?? "";
            var selectedCity = CityFilterComboBox?.SelectedItem as ComboBoxItem;

            filteredEvents = allEvents.Where(eventItem =>
            {
                // Search filter
                bool matchesSearch = string.IsNullOrEmpty(searchText) ||
                                   eventItem.Name.ToLower().Contains(searchText) ||
                                   eventItem.Hospital?.ToLower().Contains(searchText) == true ||
                                   eventItem.Location.ToLower().Contains(searchText);

                // City filter
                bool matchesCity = selectedCity == null || 
                                 selectedCity.Content.ToString() == "Tất cả thành phố" ||
                                 eventItem.City == selectedCity.Content.ToString();

                return matchesSearch && matchesCity;
            }).ToList();

            DisplayEvents();
        }

        public void RegisterForEvent_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var eventModel = button?.DataContext as EventDisplayModel;

            if (eventModel?.Event == null) return;

            try
            {
                // Check if user is already registered
                if (eventModel.IsUserRegistered)
                {
                    MessageBox.Show("Bạn đã đăng ký cho sự kiện này.", "Thông báo", 
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Check if event is full
                if (eventModel.IsFull)
                {
                    MessageBox.Show("Sự kiện đã đầy.", "Thông báo", 
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Open registration window
                var registrationWindow = new EventRegistrationWindow();
                registrationWindow.CurrentUserId = CurrentUserId;
                registrationWindow.InitializeEvent(eventModel.Event);
                registrationWindow.Owner = this;

                if (registrationWindow.ShowDialog() == true && registrationWindow.RegistrationSuccessful)
                {
                    // Refresh the events list
                    LoadEvents();
                    MessageBox.Show("Đăng ký thành công!", "Thành công", 
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Lỗi", 
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ViewEventDetails_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var eventModel = button?.DataContext as EventDisplayModel;

            if (eventModel?.Event == null) return;

            // Show event details in a message box (you can create a separate window for this)
            var details = $"Tên sự kiện: {eventModel.Event.Name}\n" +
                         $"Địa điểm: {eventModel.Event.Location}\n" +
                         $"Ngày tổ chức: {eventModel.Event.DonationDate:dd/MM/yyyy}\n" +
                         $"Loại hiến máu: {eventModel.Event.DonationType}\n" +
                         $"Trạng thái: {eventModel.Event.Status}\n" +
                         $"Số lượng đăng ký: {eventRegistrationService.GetRegistrationCountByEvent(eventModel.Event.Id)}/{eventModel.Event.TotalMemberCount}";

            MessageBox.Show(details, "Chi tiết sự kiện", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    // ViewModel for event display
    public class EventDisplayModel
    {
        public DonationEvent Event { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Date { get; set; }
        public string TimeRange { get; set; }
        public string RegistrationCount { get; set; }
        public bool IsFull { get; set; }
        public bool IsUserRegistered { get; set; }
        public bool CanRegister => !IsFull && !IsUserRegistered;
    }
}
