using DAL.Entities;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implementations
{
    public class EventRegistrationRepository : IEventRegistrationRepository
    {
        private readonly BloodDonationSupportSystemContext donationSupportSystemContext;

        public EventRegistrationRepository()
        {
            donationSupportSystemContext = new BloodDonationSupportSystemContext();
        }

        public void AddEventRegistration(EventRegistration registration)
        {
            try
            {
                donationSupportSystemContext.EventRegistrations.Add(registration);
                donationSupportSystemContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Không thể thêm đăng ký: {ex.Message}");
            }
        }

        public List<EventRegistration> GetEventRegistrations()
        {
            return donationSupportSystemContext.EventRegistrations
                .Include(x => x.Account)
                .Include(x => x.Profile)
                .Include(x => x.Event)
                .Include(x => x.TimeSlot)
                .ToList();
        }

        public List<EventRegistration> GetRegistrationsByEvent(long eventId)
        {
            return donationSupportSystemContext.EventRegistrations
                .Include(x => x.Account)
                .Include(x => x.Profile)
                .Include(x => x.Event)
                .Include(x => x.TimeSlot)
                .Where(x => x.EventId == eventId)
                .ToList();
        }

        public List<EventRegistration> GetRegistrationsByUser(long userId)
        {
            return donationSupportSystemContext.EventRegistrations
                .Include(x => x.Account)
                .Include(x => x.Profile)
                .Include(x => x.Event)
                .Include(x => x.TimeSlot)
                .Where(x => x.AccountId == userId)
                .ToList();
        }

        public List<EventRegistration> GetRegistrationsByTimeSlot(long timeSlotId)
        {
            return donationSupportSystemContext.EventRegistrations
                .Include(x => x.Account)
                .Include(x => x.Profile)
                .Include(x => x.Event)
                .Include(x => x.TimeSlot)
                .Where(x => x.TimeSlotId == timeSlotId)
                .ToList();
        }

        public EventRegistration GetRegistrationById(long registrationId)
        {
            return donationSupportSystemContext.EventRegistrations
                .Include(x => x.Account)
                .Include(x => x.Profile)
                .Include(x => x.Event)
                .Include(x => x.TimeSlot)
                .FirstOrDefault(x => x.Id == registrationId);
        }

        public bool IsUserRegisteredForEvent(long userId, long eventId)
        {
            return donationSupportSystemContext.EventRegistrations
                .Any(x => x.AccountId == userId && x.EventId == eventId);
        }

        public bool IsTimeSlotFull(long timeSlotId)
        {
            var timeSlot = donationSupportSystemContext.DonationTimeSlots
                .FirstOrDefault(x => x.Id == timeSlotId);
            
            if (timeSlot == null) return true;

            var currentRegistrations = donationSupportSystemContext.EventRegistrations
                .Count(x => x.TimeSlotId == timeSlotId);

            return currentRegistrations >= timeSlot.MaxCapacity;
        }

        public void RemoveEventRegistration(EventRegistration registration)
        {
            donationSupportSystemContext.EventRegistrations.Remove(registration);
            donationSupportSystemContext.SaveChanges();
        }

        public void UpdateEventRegistration(EventRegistration registration)
        {
            donationSupportSystemContext.EventRegistrations.Update(registration);
            donationSupportSystemContext.SaveChanges();
        }

        public int GetRegistrationCountByEvent(long eventId)
        {
            return donationSupportSystemContext.EventRegistrations
                .Count(x => x.EventId == eventId);
        }

        public int GetRegistrationCountByTimeSlot(long timeSlotId)
        {
            return donationSupportSystemContext.EventRegistrations
                .Count(x => x.TimeSlotId == timeSlotId);
        }

        public int GetAvailableCapacityForTimeSlot(long timeSlotId)
        {
            var timeSlot = donationSupportSystemContext.DonationTimeSlots
                .FirstOrDefault(x => x.Id == timeSlotId);
            
            if (timeSlot == null) return 0;

            var currentRegistrations = GetRegistrationCountByTimeSlot(timeSlotId);
            return Math.Max(0, timeSlot.MaxCapacity - currentRegistrations);
        }
    }
} 