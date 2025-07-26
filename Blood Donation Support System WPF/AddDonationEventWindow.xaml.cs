using DAL.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AddDonationEventWindow.xaml
    /// </summary>
    public partial class AddDonationEventWindow : Window
    {
        public DonationEvent NewEvent { get; set; }
        public bool isUpdate { get; set; } = false;
        public long CurrentUserId { get; set; } = 1; // This should be set from the logged-in user

        // Collection to hold time slots
        private ObservableCollection<TimeSlotViewModel> timeSlots;

        public AddDonationEventWindow()
        {
            InitializeComponent();
        }

        public void InitializeDefaults()
        {
            if (NewEvent != null)
            {
                EventNameTextBox.Text = NewEvent?.Name;
                HospitalTextBox.Text = NewEvent?.Hospital;
                DonationTypeComboBox.SelectedItem = NewEvent?.DonationType;
                DonationDatePicker.Text = NewEvent?.DonationDate.ToString("yyyy-MM-dd");
                AddressTextBox.Text = NewEvent?.Address;
                WardTextBox.Text = NewEvent?.Ward;
                DistrictTextBox.Text = NewEvent?.District;
                CityComboBox.SelectedItem = NewEvent?.City;
                TimeSlotsListView.ItemsSource = NewEvent?.DonationTimeSlots;
                TotalMemberCountTextBox.Text = NewEvent?.TotalMemberCount.ToString();
                StatusComboBox.SelectedItem = NewEvent?.Status;
                timeSlots = new ObservableCollection<TimeSlotViewModel>(NewEvent.DonationTimeSlots.Select(ts => new TimeSlotViewModel
                {
                    StartTime = ts.StartTime,
                    EndTime = ts.EndTime,
                    MaxCapacity = ts.MaxCapacity,
                    CurrentRegistrations = ts.CurrentRegistrations
                }));
                return;
            }

            // Default setup
            timeSlots = new ObservableCollection<TimeSlotViewModel>();
            TimeSlotsListView.ItemsSource = timeSlots;

            DonationDatePicker.SelectedDate = DateTime.Today.AddDays(7);
            TotalMemberCountTextBox.Text = "100";
            SlotCapacityTextBox.Text = "20";

            TotalMemberCountTextBox.PreviewTextInput += NumericOnly_PreviewTextInput;
            SlotCapacityTextBox.PreviewTextInput += NumericOnly_PreviewTextInput;

            StartTimeComboBox.SelectedIndex = 0;
            EndTimeComboBox.SelectedIndex = 1;
        }

        private void NumericOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Only allow numeric input
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private void AddTimeSlotButton_Click(object sender, RoutedEventArgs e)
        {
            TimeSlotExpander.IsExpanded = true;
        }

        private void ConfirmAddTimeSlotButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate time slot inputs
                if (!ValidateTimeSlotInputs())
                {
                    return;
                }

                var startTimeStr = ((ComboBoxItem)StartTimeComboBox.SelectedItem).Content.ToString();
                var endTimeStr = ((ComboBoxItem)EndTimeComboBox.SelectedItem).Content.ToString();
                var maxCapacity = int.Parse(SlotCapacityTextBox.Text);

                var startTime = TimeOnly.Parse(startTimeStr);
                var endTime = TimeOnly.Parse(endTimeStr);

                // Check for overlapping time slots
                if (HasOverlappingTimeSlot(startTime, endTime))
                {
                    MessageBox.Show("Khung thời gian này bị trùng với khung thời gian đã có!",
                                   "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Add new time slot
                var newTimeSlot = new TimeSlotViewModel
                {
                    StartTime = startTime,
                    EndTime = endTime,
                    MaxCapacity = maxCapacity,
                    CurrentRegistrations = 0
                };

                timeSlots.Add(newTimeSlot);

                // Reset form
                StartTimeComboBox.SelectedIndex = 0;
                EndTimeComboBox.SelectedIndex = 1;
                SlotCapacityTextBox.Text = "20";
                TimeSlotExpander.IsExpanded = false;

                MessageBox.Show("Khung thời gian đã được thêm thành công!", "Thành công",
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi thêm khung thời gian: {ex.Message}", "Lỗi",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveTimeSlotButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var timeSlot = button.Tag as TimeSlotViewModel;

            if (timeSlot != null)
            {
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa khung thời gian này?",
                                           "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    timeSlots.Remove(timeSlot);
                }
            }
        }

        private bool ValidateTimeSlotInputs()
        {
            var errorMessages = new List<string>();

            // Validate start time
            if (StartTimeComboBox.SelectedItem == null)
            {
                errorMessages.Add("• Vui lòng chọn thời gian bắt đầu");
            }

            // Validate end time
            if (EndTimeComboBox.SelectedItem == null)
            {
                errorMessages.Add("• Vui lòng chọn thời gian kết thúc");
            }

            // Validate time order
            if (StartTimeComboBox.SelectedItem != null && EndTimeComboBox.SelectedItem != null)
            {
                var startTimeStr = ((ComboBoxItem)StartTimeComboBox.SelectedItem).Content.ToString();
                var endTimeStr = ((ComboBoxItem)EndTimeComboBox.SelectedItem).Content.ToString();

                var startTime = TimeOnly.Parse(startTimeStr);
                var endTime = TimeOnly.Parse(endTimeStr);

                if (endTime <= startTime)
                {
                    errorMessages.Add("• Thời gian kết thúc phải sau thời gian bắt đầu");
                }
            }

            // Validate capacity
            if (string.IsNullOrWhiteSpace(SlotCapacityTextBox.Text) ||
                !int.TryParse(SlotCapacityTextBox.Text, out int capacity) ||
                capacity <= 0)
            {
                errorMessages.Add("• Sức chứa tối đa phải là số nguyên dương");
            }
            else if (capacity > 100)
            {
                errorMessages.Add("• Sức chứa tối đa cho một khung giờ không được vượt quá 100");
            }

            // Show validation errors if any
            if (errorMessages.Any())
            {
                string message = "Vui lòng sửa các lỗi sau:\n\n" + string.Join("\n", errorMessages);
                MessageBox.Show(message, "Lỗi xác thực", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private bool HasOverlappingTimeSlot(TimeOnly startTime, TimeOnly endTime)
        {
            return timeSlots.Any(slot =>
                (startTime >= slot.StartTime && startTime < slot.EndTime) ||
                (endTime > slot.StartTime && endTime <= slot.EndTime) ||
                (startTime <= slot.StartTime && endTime >= slot.EndTime));
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate required fields
                if (!ValidateInputs())
                {
                    return;
                }

                // Create new DonationEvent
                NewEvent = new DonationEvent
                {
                    Name = EventNameTextBox.Text.Trim(),
                    Hospital = string.IsNullOrWhiteSpace(HospitalTextBox.Text) ? null : HospitalTextBox.Text.Trim(),
                    Address = AddressTextBox.Text.Trim(),
                    Ward = string.IsNullOrWhiteSpace(WardTextBox.Text) ? null : WardTextBox.Text.Trim(),
                    District = string.IsNullOrWhiteSpace(DistrictTextBox.Text) ? null : DistrictTextBox.Text.Trim(),
                    City = CityComboBox.SelectedItem != null ? ((ComboBoxItem)CityComboBox.SelectedItem).Content.ToString() : null,
                    DonationDate = DateOnly.FromDateTime(DonationDatePicker.SelectedDate.Value),
                    DonationType = ((ComboBoxItem)DonationTypeComboBox.SelectedItem).Content.ToString(),
                    TotalMemberCount = int.Parse(TotalMemberCountTextBox.Text),
                    RegisteredMemberCount = 0, // Initially 0
                    Status = ((ComboBoxItem)StatusComboBox.SelectedItem).Content.ToString(),
                    CreatedBy = CurrentUserId,
                    CreatedDate = DateOnly.FromDateTime(DateTime.Today)
                };

                // Create time slots for the event
                NewEvent.DonationTimeSlots = timeSlots.Select(ts => new DonationTimeSlot
                {
                    StartTime = ts.StartTime,
                    EndTime = ts.EndTime,
                    MaxCapacity = ts.MaxCapacity,
                    CurrentRegistrations = 0,
                    Event = NewEvent
                }).ToList();


                // Show success message
                if (isUpdate)
                {
                    MessageBox.Show($"Sự kiện hiến máu đã được lưu thành công với {timeSlots.Count} khung thời gian!",
                               "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                } else
                {
                    MessageBox.Show($"Sự kiện hiến máu đã được tạo thành công với {timeSlots.Count} khung thời gian!",
                                   "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi tạo sự kiện: {ex.Message}", "Lỗi",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInputs()
        {
            var errorMessages = new List<string>();
            int.TryParse(TotalMemberCountTextBox.Text,out int totalCount);
            // Validate Event Name
            if (string.IsNullOrWhiteSpace(EventNameTextBox.Text))
            {
                errorMessages.Add("• Tên sự kiện không được để trống");
                EventNameTextBox.BorderBrush = Brushes.Red;
            }
            else
            {
                EventNameTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(0xCC, 0xCC, 0xCC));
            }

            // Validate Address
            if (string.IsNullOrWhiteSpace(AddressTextBox.Text))
            {
                errorMessages.Add("• Địa chỉ không được để trống");
                AddressTextBox.BorderBrush = Brushes.Red;
            }
            else
            {
                AddressTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(0xCC, 0xCC, 0xCC));
            }

            // Validate Donation Date
            if (!DonationDatePicker.SelectedDate.HasValue)
            {
                errorMessages.Add("• Ngày tổ chức sự kiện không được để trống");
                DonationDatePicker.BorderBrush = Brushes.Red;
            }
            else if (DonationDatePicker.SelectedDate.Value.Date < DateTime.Today)
            {
                errorMessages.Add("• Ngày tổ chức sự kiện không thể là ngày trong quá khứ");
                DonationDatePicker.BorderBrush = Brushes.Red;
            }
            else
            {
                DonationDatePicker.BorderBrush = new SolidColorBrush(Color.FromRgb(0xCC, 0xCC, 0xCC));
            }

            // Validate Total Member Count
            if (string.IsNullOrWhiteSpace(TotalMemberCountTextBox.Text) ||
                totalCount <= 0)
            {
                errorMessages.Add("• Số lượng người hiến máu tối đa phải là số nguyên dương");
                TotalMemberCountTextBox.BorderBrush = Brushes.Red;
            }
            else if (totalCount > 1000)
            {
                errorMessages.Add("• Số lượng người hiến máu tối đa không được vượt quá 1000");
                TotalMemberCountTextBox.BorderBrush = Brushes.Red;
            }
            else
            {
                TotalMemberCountTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(0xCC, 0xCC, 0xCC));
            }

            // Validate Donation Type
            if (DonationTypeComboBox.SelectedItem == null)
            {
                errorMessages.Add("• Vui lòng chọn loại hiến máu");
            }

            // Validate Status
            if (StatusComboBox.SelectedItem == null)
            {
                errorMessages.Add("• Vui lòng chọn trạng thái sự kiện");
            }

            // Validate Time Slots
            if (timeSlots.Count == 0)
            {
                errorMessages.Add("• Vui lòng thêm ít nhất một khung thời gian cho sự kiện");
            }

            // Validate total capacity consistency
            if (timeSlots.Count > 0 && totalCount > 0)
            {
                int totalSlotCapacity = timeSlots.Sum(ts => ts.MaxCapacity);
                if (totalSlotCapacity > totalCount)
                {
                    errorMessages.Add($"• Tổng sức chứa của các khung giờ ({totalSlotCapacity}) không được vượt quá sức chứa tối đa của sự kiện ({totalCount})");
                }
            }

            // Show validation errors if any
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
            // Confirm before closing if user has entered data
            if (HasUserEnteredData())
            {
                var result = MessageBox.Show("Bạn có chắc chắn muốn hủy? Tất cả dữ liệu đã nhập sẽ bị mất.",
                                           "Xác nhận hủy", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result != MessageBoxResult.Yes)
                {
                    return;
                }
            }

            DialogResult = false;
            Close();
        }

        private bool HasUserEnteredData()
        {
            return !string.IsNullOrWhiteSpace(EventNameTextBox.Text) ||
                   !string.IsNullOrWhiteSpace(HospitalTextBox.Text) ||
                   !string.IsNullOrWhiteSpace(AddressTextBox.Text) ||
                   !string.IsNullOrWhiteSpace(WardTextBox.Text) ||
                   !string.IsNullOrWhiteSpace(DistrictTextBox.Text) ||
                   CityComboBox.SelectedItem != null ||
                   DonationDatePicker.SelectedDate.HasValue ||
                   TotalMemberCountTextBox.Text != "100" ||
                   timeSlots.Count > 0;
        }

        // Event handlers for real-time validation feedback
        private void EventNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(EventNameTextBox.Text))
            {
                EventNameTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(0xCC, 0xCC, 0xCC));
            }
        }

        private void AddressTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(AddressTextBox.Text))
            {
                AddressTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(0xCC, 0xCC, 0xCC));
            }
        }
    }

    // ViewModel class for time slots
    public class TimeSlotViewModel
    {
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int MaxCapacity { get; set; }
        public int CurrentRegistrations { get; set; }

        public override string ToString()
        {
            return $"{StartTime:HH:mm} - {EndTime:HH:mm} (Sức chứa: {MaxCapacity})";
        }
    }
}