using DAL.Entities;
using DAL.Repositories;
using DAL.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Implementations
{
    public class EventRegistrationService : IEventRegistrationService
    {
        private readonly IEventRegistrationRepository eventRegistrationRepository;
        private readonly IDonationEventRepository donationEventRepository;

        public EventRegistrationService()
        {
            eventRegistrationRepository = new EventRegistrationRepository();
            donationEventRepository = new DonationEventRepository();
        }

        public void AddEventRegistration(EventRegistration registration)
        {
            eventRegistrationRepository.AddEventRegistration(registration);
        }

        public bool CancelRegistration(long registrationId)
        {
            try
            {
                var registration = eventRegistrationRepository.GetRegistrationById(registrationId);
                if (registration == null) return false;

                eventRegistrationRepository.RemoveEventRegistration(registration);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<EventRegistration> GetEventRegistrations()
        {
            return eventRegistrationRepository.GetEventRegistrations();
        }

        public List<EventRegistration> GetRegistrationsByEvent(long eventId)
        {
            return eventRegistrationRepository.GetRegistrationsByEvent(eventId);
        }

        public List<EventRegistration> GetRegistrationsByUser(long userId)
        {
            return eventRegistrationRepository.GetRegistrationsByUser(userId);
        }

        public List<EventRegistration> GetRegistrationsByTimeSlot(long timeSlotId)
        {
            return eventRegistrationRepository.GetRegistrationsByTimeSlot(timeSlotId);
        }

        public EventRegistration GetRegistrationById(long registrationId)
        {
            return eventRegistrationRepository.GetRegistrationById(registrationId);
        }

        public bool IsEventActive(long eventId)
        {
            var donationEvent = donationEventRepository.GetDonationEvents()
                .FirstOrDefault(x => x.Id == eventId);
            
            return donationEvent != null && donationEvent.Status == "ACTIVE";
        }

        public bool IsEventFull(long eventId)
        {
            var donationEvent = donationEventRepository.GetDonationEvents()
                .FirstOrDefault(x => x.Id == eventId);
            
            if (donationEvent == null) return true;

            var currentRegistrations = GetRegistrationCountByEvent(eventId);
            return currentRegistrations >= donationEvent.TotalMemberCount;
        }

        public bool IsTimeSlotFull(long timeSlotId)
        {
            return eventRegistrationRepository.IsTimeSlotFull(timeSlotId);
        }

        public bool IsTimeSlotValid(long timeSlotId, long eventId)
        {
            var donationEvent = donationEventRepository.GetDonationEvents()
                .FirstOrDefault(x => x.Id == eventId);
            
            if (donationEvent == null) return false;

            return donationEvent.DonationTimeSlots.Any(x => x.Id == timeSlotId);
        }

        public bool IsUserRegisteredForEvent(long userId, long eventId)
        {
            return eventRegistrationRepository.IsUserRegisteredForEvent(userId, eventId);
        }

        public bool RegisterForEvent(long userId, long eventId, long timeSlotId, string bloodType, string donationType)
        {
            try
            {
                // Validate input parameters
                if (string.IsNullOrWhiteSpace(bloodType))
                {
                    throw new InvalidOperationException("Nhóm máu không được để trống.");
                }

                if (string.IsNullOrWhiteSpace(donationType))
                {
                    throw new InvalidOperationException("Loại hiến máu không được để trống.");
                }

                // Validation checks
                if (!IsEventActive(eventId))
                {
                    throw new InvalidOperationException("Sự kiện không còn hoạt động.");
                }

                if (IsEventFull(eventId))
                {
                    throw new InvalidOperationException("Sự kiện đã đầy.");
                }

                if (IsUserRegisteredForEvent(userId, eventId))
                {
                    throw new InvalidOperationException("Bạn đã đăng ký cho sự kiện này.");
                }

                if (!IsTimeSlotValid(timeSlotId, eventId))
                {
                    throw new InvalidOperationException("Khung thời gian không hợp lệ.");
                }

                if (IsTimeSlotFull(timeSlotId))
                {
                    throw new InvalidOperationException("Khung thời gian đã đầy.");
                }

                // Create new registration
                var registration = new EventRegistration
                {
                    AccountId = userId,
                    EventId = eventId,
                    TimeSlotId = timeSlotId,
                    BloodType = bloodType,
                    DonationType = donationType,
                    RegistrationDate = DateOnly.FromDateTime(DateTime.Today),
                    Status = "REGISTERED"
                };

                // Get ProfileId from Account (assuming Account has Profile relationship)
                var context = new BloodDonationSupportSystemContext();
                var account = context.Accounts
                    .Include(x => x.Profile)
                    .FirstOrDefault(x => x.Id == userId);
                
                if (account?.Profile != null)
                {
                    registration.ProfileId = account.Profile.Id;
                }
                else
                {
                    throw new InvalidOperationException("Không tìm thấy thông tin profile của người dùng.");
                }

                eventRegistrationRepository.AddEventRegistration(registration);
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Không thể đăng ký: {ex.Message}");
            }
        }

        public bool UpdateRegistration(EventRegistration registration)
        {
            try
            {
                eventRegistrationRepository.UpdateEventRegistration(registration);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int GetRegistrationCountByEvent(long eventId)
        {
            return eventRegistrationRepository.GetRegistrationCountByEvent(eventId);
        }

        public int GetRegistrationCountByTimeSlot(long timeSlotId)
        {
            return eventRegistrationRepository.GetRegistrationCountByTimeSlot(timeSlotId);
        }

        public int GetAvailableCapacityForTimeSlot(long timeSlotId)
        {
            return eventRegistrationRepository.GetAvailableCapacityForTimeSlot(timeSlotId);
        }

        public List<DonationTimeSlot> GetAvailableTimeSlots(long eventId)
        {
            var donationEvent = donationEventRepository.GetDonationEvents()
                .FirstOrDefault(x => x.Id == eventId);
            
            if (donationEvent == null) return new List<DonationTimeSlot>();

            var availableTimeSlots = new List<DonationTimeSlot>();
            
            foreach (var timeSlot in donationEvent.DonationTimeSlots)
            {
                var currentRegistrations = GetRegistrationCountByTimeSlot(timeSlot.Id);
                if (currentRegistrations < timeSlot.MaxCapacity)
                {
                    availableTimeSlots.Add(timeSlot);
                }
            }

            return availableTimeSlots;
        }
    }
} 