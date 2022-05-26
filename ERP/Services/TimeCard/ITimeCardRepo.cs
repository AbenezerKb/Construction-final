using ERP.Models;

namespace ERP.Services
{
    public interface ITimeCardRepo
    {


        bool SaveChanges();

        IEnumerable<TimeCard> GetAllTimeCards();
        TimeCard GetTimeCard(string id);
        void CreateTimeCard(TimeCard timeCard);
        void DeleteTimeCard(string id);
        void UpdateTimeCard(string id, TimeCard updatedTimeCard);
    }
}
