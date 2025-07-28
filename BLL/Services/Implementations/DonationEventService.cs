using DAL.Entities;
using DAL.Repositories;
using DAL.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Implementations
{
    public class DonationEventService : IDonationEventService
    {
        private readonly IDonationEventRepository donationEventRepository;
        public DonationEventService()
        {
            donationEventRepository = new DonationEventRepository();
        }
        public void AddDonationEvent(DonationEvent donationEvent)
        {
            donationEventRepository.AddDonationEvent(donationEvent);
        }

        public List<DonationEvent> GetDonationEvents()
        {
            return donationEventRepository.GetDonationEvents();
        }

        public void RemoveDonationEvent(DonationEvent donationEvent)
        {
            donationEventRepository.RemoveDonationEvent(donationEvent);
        }

        public void UpdateDonationEvent(DonationEvent donationEvent)
        {
            donationEventRepository.UpdateDonationEvent(donationEvent);
        }
    }
}
