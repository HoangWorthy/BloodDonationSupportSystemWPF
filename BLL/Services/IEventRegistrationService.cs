using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IEventRegistrationService
    {
        List<EventRegistration> GetEventRegistrations();
        List<EventRegistration> GetRegistrationsByEvent(long eventId);
        List<EventRegistration> GetRegistrationsByUser(long userId);
        List<EventRegistration> GetRegistrationsByTimeSlot(long timeSlotId);
        EventRegistration GetRegistrationById(long registrationId);
        
        // Business Logic Methods
        bool RegisterForEvent(long userId, long eventId, long timeSlotId, string bloodType, string donationType);
        bool CancelRegistration(long registrationId);
        bool UpdateRegistration(EventRegistration registration);
        
        // Validation Methods
        bool IsUserRegisteredForEvent(long userId, long eventId);
        bool IsTimeSlotFull(long timeSlotId);
        bool IsEventFull(long eventId);
        bool IsEventActive(long eventId);
        bool IsTimeSlotValid(long timeSlotId, long eventId);
        
        // Statistics Methods
        int GetRegistrationCountByEvent(long eventId);
        int GetRegistrationCountByTimeSlot(long timeSlotId);
        int GetAvailableCapacityForTimeSlot(long timeSlotId);
        List<DonationTimeSlot> GetAvailableTimeSlots(long eventId);
    }
} 