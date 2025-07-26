using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implementations
{
    public class DonationEventRepository : IDonationEventRepository
    {
        private readonly BloodDonationSupportSystemContext donationSupportSystemContext;
        public DonationEventRepository()
        {
            donationSupportSystemContext = new BloodDonationSupportSystemContext();
        }

        public void AddDonationEvent(DonationEvent donationEvent)
        {

            donationSupportSystemContext.DonationEvents.Add(donationEvent);
            donationSupportSystemContext.SaveChanges();
        }

        public List<DonationEvent> GetDonationEvents()
        {
            return donationSupportSystemContext.DonationEvents
                .Include(x => x.DonationTimeSlots)
                .Include(y => y.BloodUnits)
                .ToList();
        }

        public void RemoveDonationEvent(DonationEvent donationEvent)
        {
            donationSupportSystemContext.DonationEvents.Remove(donationEvent);
            donationSupportSystemContext.SaveChanges();
        }

        public void UpdateDonationEvent(DonationEvent donationEvent)
        {
            donationSupportSystemContext.DonationEvents.Update(donationEvent);
            donationSupportSystemContext.SaveChanges();
        }
    }
}
