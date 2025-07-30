using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IEventRegistrationRepository
    {
        List<EventRegistration> GetEventRegistrations();
        List<EventRegistration> GetRegistrationsByEvent(long eventId);
        List<EventRegistration> GetRegistrationsByUser(long userId);
        List<EventRegistration> GetRegistrationsByTimeSlot(long timeSlotId);
        EventRegistration GetRegistrationById(long registrationId);
        bool IsUserRegisteredForEvent(long userId, long eventId);
        bool IsTimeSlotFull(long timeSlotId);
        void AddEventRegistration(EventRegistration registration);
        void UpdateEventRegistration(EventRegistration registration);
        void RemoveEventRegistration(EventRegistration registration);
        int GetRegistrationCountByEvent(long eventId);
        int GetRegistrationCountByTimeSlot(long timeSlotId);
        int GetAvailableCapacityForTimeSlot(long timeSlotId);
    }
} 