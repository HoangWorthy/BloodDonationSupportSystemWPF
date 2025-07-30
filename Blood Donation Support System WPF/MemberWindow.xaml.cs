using BLL.Services;
using BLL.Services.Implementations;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            // Handle the placeholder text for search box
            EventSearchTextBox.GotFocus += (s, e) =>
            {
                if (EventSearchTextBox.Text == "Tìm kiếm sự kiện...")
                {
                    EventSearchTextBox.Text = "";
                    EventSearchTextBox.Foreground = Brushes.Black;
                }
            };

            EventSearchTextBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(EventSearchTextBox.Text))
                {
                    EventSearchTextBox.Text = "Tìm kiếm sự kiện...";
                    EventSearchTextBox.Foreground = new SolidColorBrush(Color.FromRgb(0x99, 0x99, 0x99));
                }
            };

            EventSearchTextBox.TextChanged += EventSearchTextBox_TextChanged;
            CityFilterComboBox.SelectionChanged += CityFilterComboBox_SelectionChanged;
            SearchButton.Click += SearchButton_Click;
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
                UpdateEventCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách sự kiện: {ex.Message}", "Lỗi",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisplayEvents()
        {
            EventsListView.ItemsSource = filteredEvents.Select(eventItem => new EventDisplayModel
            {
                Event = eventItem,
                Name = eventItem.Name,
                Location = GetFullLocation(eventItem),
                Date = eventItem.DonationDate.ToString("dd/MM/yyyy"),
                TimeRange = GetEventTimeRange(eventItem),
                RegistrationCount = GetRegistrationCountString(eventItem),
                IsFull = IsEventFull(eventItem),
                IsUserRegistered = IsUserRegisteredForEvent(eventItem)
            }).ToList();
        }

        private string GetFullLocation(DonationEvent eventItem)
        {
            var location = eventItem.Address;
            if (!string.IsNullOrEmpty(eventItem.Ward))
                location += $", {eventItem.Ward}";
            if (!string.IsNullOrEmpty(eventItem.District))
                location += $", {eventItem.District}";
            if (!string.IsNullOrEmpty(eventItem.City))
                location += $", {eventItem.City}";
            return location;
        }

        private string GetEventTimeRange(DonationEvent donationEvent)
        {
            if (donationEvent.DonationTimeSlots == null || !donationEvent.DonationTimeSlots.Any())
                return "Chưa có lịch trình";

            var firstSlot = donationEvent.DonationTimeSlots.OrderBy(x => x.StartTime).First();
            var lastSlot = donationEvent.DonationTimeSlots.OrderByDescending(x => x.EndTime).First();

            return $"{firstSlot.StartTime:HH:mm} đến {lastSlot.EndTime:HH:mm}";
        }

        private string GetRegistrationCountString(DonationEvent eventItem)
        {
            try
            {
                int registeredCount = eventRegistrationService?.GetRegistrationCountByEvent(eventItem.Id) ?? eventItem.RegisteredMemberCount;
                return $"{registeredCount}/{eventItem.TotalMemberCount}";
            }
            catch
            {
                return $"{eventItem.RegisteredMemberCount}/{eventItem.TotalMemberCount}";
            }
        }

        private bool IsEventFull(DonationEvent eventItem)
        {
            try
            {
                int registeredCount = eventRegistrationService?.GetRegistrationCountByEvent(eventItem.Id) ?? eventItem.RegisteredMemberCount;
                return registeredCount >= eventItem.TotalMemberCount;
            }
            catch
            {
                return eventItem.RegisteredMemberCount >= eventItem.TotalMemberCount;
            }
        }

        private bool IsUserRegisteredForEvent(DonationEvent eventItem)
        {
            try
            {
                return eventRegistrationService?.IsUserRegisteredForEvent(CurrentUserId, eventItem.Id) ?? false;
            }
            catch
            {
                return false;
            }
        }

        private void UpdateEventCount()
        {
            EventCountText.Text = filteredEvents.Count.ToString();
        }

        private void EventSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (EventSearchTextBox.Text != "Tìm kiếm sự kiện...")
            {
                ApplyFilters();
            }
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

            var searchText = EventSearchTextBox.Text?.ToLower() ?? "";
            if (searchText == "tìm kiếm sự kiện...") searchText = "";

            var selectedCity = CityFilterComboBox.SelectedItem as ComboBoxItem;

            filteredEvents = allEvents.Where(eventItem =>
            {
                // Search filter
                bool matchesSearch = string.IsNullOrEmpty(searchText) ||
                                   eventItem.Name.ToLower().Contains(searchText) ||
                                   eventItem.Hospital?.ToLower().Contains(searchText) == true ||
                                   eventItem.Address.ToLower().Contains(searchText);

                // City filter
                bool matchesCity = selectedCity == null ||
                                 selectedCity.Content.ToString() == "Tất cả thành phố" ||
                                 eventItem.City == selectedCity.Content.ToString();

                return matchesSearch && matchesCity;
            }).ToList();

            DisplayEvents();
            UpdateEventCount();
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

                // Open registration window (if you have one)
                var registrationWindow = new EventRegistrationWindow();
                registrationWindow.CurrentUserId = CurrentUserId;
                registrationWindow.InitializeEvent(eventModel.Event);
                registrationWindow.Owner = this;

                if (registrationWindow.ShowDialog() == true)
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
                         $"Địa điểm: {eventModel.Location}\n" +
                         $"Ngày tổ chức: {eventModel.Event.DonationDate:dd/MM/yyyy}\n" +
                         $"Thời gian: {eventModel.TimeRange}\n" +
                         $"Loại hiến máu: {eventModel.Event.DonationType}\n" +
                         $"Bệnh viện: {eventModel.Event.Hospital}\n" +
                         $"Trạng thái: {eventModel.Event.Status}\n" +
                         $"Số lượng đăng ký: {eventModel.RegistrationCount}";

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

    // Converter class for boolean to visibility
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public static readonly BooleanToVisibilityConverter Instance = new BooleanToVisibilityConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }
            return false;
        }
    }
}