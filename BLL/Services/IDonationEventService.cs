using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IDonationEventService
    {
        List<DonationEvent> GetDonationEvents();
        void AddDonationEvent(DonationEvent donationEvent);
        void RemoveDonationEvent(DonationEvent donationEvent);
        void UpdateDonationEvent(DonationEvent donationEvent);
    }
}
