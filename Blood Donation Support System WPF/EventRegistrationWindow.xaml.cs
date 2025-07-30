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
    /// Interaction logic for EventRegistrationWindow.xaml
    /// </summary>
    public partial class EventRegistrationWindow : Window
    {
        private readonly IEventRegistrationService eventRegistrationService;
        private readonly IDonationEventService donationEventService;
        
        public DonationEvent SelectedEvent { get; set; }
        public long CurrentUserId { get; set; }
        public bool RegistrationSuccessful { get; set; } = false;

        public EventRegistrationWindow()
        {
            InitializeComponent();
            eventRegistrationService = new EventRegistrationService();
            donationEventService = new DonationEventService();
        }

        public void InitializeEvent(DonationEvent donationEvent)
        {
            SelectedEvent = donationEvent;
            LoadEventInformation();
            LoadAvailableTimeSlots();
        }

        private void LoadEventInformation()
        {
            if (SelectedEvent == null) return;

            EventNameTextBlock.Text = $"Tên sự kiện: {SelectedEvent.Name}";
            EventLocationTextBlock.Text = $"Địa điểm: {SelectedEvent.Location}";
            EventDateTextBlock.Text = $"Ngày tổ chức: {SelectedEvent.DonationDate:dd/MM/yyyy}";
            EventStatusTextBlock.Text = $"Trạng thái: {SelectedEvent.Status}";

            var currentRegistrations = eventRegistrationService.GetRegistrationCountByEvent(SelectedEvent.Id);
            RegistrationCountTextBlock.Text = $"Số lượng đăng ký: {currentRegistrations}/{SelectedEvent.TotalMemberCount} người";

            if (currentRegistrations >= SelectedEvent.TotalMemberCount)
            {
                RegistrationCountTextBlock.Foreground = Brushes.Red;
            }
            else if (currentRegistrations >= SelectedEvent.TotalMemberCount * 0.8)
            {
                RegistrationCountTextBlock.Foreground = Brushes.Orange;
            }
            else
            {
                RegistrationCountTextBlock.Foreground = new SolidColorBrush(Color.FromRgb(0x4C, 0xAF, 0x50));
            }
        }

        private void LoadAvailableTimeSlots()
        {
            if (SelectedEvent == null) return;

            TimeSlotComboBox.Items.Clear();
            var availableTimeSlots = eventRegistrationService.GetAvailableTimeSlots(SelectedEvent.Id);

            if (!availableTimeSlots.Any())
            {
                TimeSlotComboBox.IsEnabled = false;
                TimeSlotInfoTextBlock.Text = "Không có khung thời gian khả dụng cho sự kiện này.";
                TimeSlotInfoTextBlock.Foreground = Brushes.Red;
                return;
            }

            foreach (var timeSlot in availableTimeSlots)
            {
                var availableSlots = eventRegistrationService.GetAvailableCapacityForTimeSlot(timeSlot.Id);
                
                var item = new ComboBoxItem
                {
                    Content = $"{timeSlot.StartTime:HH:mm} - {timeSlot.EndTime:HH:mm} (Còn {availableSlots} chỗ)",
                    Tag = timeSlot
                };
                
                TimeSlotComboBox.Items.Add(item);
            }

            TimeSlotComboBox.SelectedIndex = 0;
            UpdateTimeSlotInfo();
        }

        private void UpdateTimeSlotInfo()
        {
            if (TimeSlotComboBox.SelectedItem == null) return;

            var selectedItem = TimeSlotComboBox.SelectedItem as ComboBoxItem;
            var timeSlot = selectedItem?.Tag as DonationTimeSlot;

            if (timeSlot != null)
            {
                var currentRegistrations = eventRegistrationService.GetRegistrationCountByTimeSlot(timeSlot.Id);
                var availableSlots = eventRegistrationService.GetAvailableCapacityForTimeSlot(timeSlot.Id);

                TimeSlotInfoTextBlock.Text = $"Khung thời gian: {timeSlot.StartTime:HH:mm} - {timeSlot.EndTime:HH:mm}\n" +
                                           $"Sức chứa: {currentRegistrations}/{timeSlot.MaxCapacity} người\n" +
                                           $"Còn lại: {availableSlots} chỗ";

                if (availableSlots <= 0)
                {
                    TimeSlotInfoTextBlock.Foreground = Brushes.Red;
                }
                else if (availableSlots <= 5)
                {
                    TimeSlotInfoTextBlock.Foreground = Brushes.Orange;
                }
                else
                {
                    TimeSlotInfoTextBlock.Foreground = new SolidColorBrush(Color.FromRgb(0x2E, 0x7D, 0x32));
                }
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateInputs())
                {
                    return;
                }

                var bloodType = ((ComboBoxItem)BloodTypeComboBox.SelectedItem)?.Content?.ToString() ?? "";
                var donationType = ((ComboBoxItem)DonationTypeComboBox.SelectedItem)?.Content?.ToString() ?? "";
                var selectedTimeSlot = (TimeSlotComboBox.SelectedItem as ComboBoxItem)?.Tag as DonationTimeSlot;

                if (selectedTimeSlot == null)
                {
                    MessageBox.Show("Vui lòng chọn khung thời gian.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var success = eventRegistrationService.RegisterForEvent(
                    CurrentUserId, 
                    SelectedEvent.Id, 
                    selectedTimeSlot.Id, 
                    bloodType, 
                    donationType
                );

                if (success)
                {
                    MessageBox.Show("Đăng ký thành công! Vui lòng đến đúng thời gian và địa điểm đã đăng ký.", 
                                  "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    RegistrationSuccessful = true;
                    DialogResult = true;
                    Close();
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi đăng ký", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi đăng ký: {ex.Message}", "Lỗi", 
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInputs()
        {
            var errorMessages = new List<string>();

            if (BloodTypeComboBox.SelectedItem == null)
            {
                errorMessages.Add("• Vui lòng chọn nhóm máu");
            }

            if (DonationTypeComboBox.SelectedItem == null)
            {
                errorMessages.Add("• Vui lòng chọn loại hiến máu");
            }

            if (TimeSlotComboBox.SelectedItem == null)
            {
                errorMessages.Add("• Vui lòng chọn khung thời gian");
            }

            if (!eventRegistrationService.IsEventActive(SelectedEvent.Id))
            {
                errorMessages.Add("• Sự kiện không còn hoạt động");
            }

            if (eventRegistrationService.IsUserRegisteredForEvent(CurrentUserId, SelectedEvent.Id))
            {
                errorMessages.Add("• Bạn đã đăng ký cho sự kiện này");
            }

            if (errorMessages.Any())
            {
                string message = "Vui lòng sửa các lỗi sau:\n\n" + string.Join("\n", errorMessages);
                MessageBox.Show(message, "Lỗi xác thực", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void TimeSlotComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTimeSlotInfo();
        }
    }
} 