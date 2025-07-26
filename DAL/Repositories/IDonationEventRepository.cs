using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IDonationEventRepository
    {
        List<DonationEvent> GetDonationEvents();
        void AddDonationEvent(DonationEvent donationEvent);
        void RemoveDonationEvent(DonationEvent donationEvent);
        void UpdateDonationEvent(DonationEvent donationEvent);
    }
}
