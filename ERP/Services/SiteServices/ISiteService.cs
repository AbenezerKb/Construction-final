using ERP.Models;

namespace ERP.Services.SiteServices
{
    public interface ISiteService
    {
        public Task<Site> GetOne(int id);

        Task<List<Site>> GetAll();

        public Task<Site> AddSite(Site request);

        public Task<Site> EditSite(Site request);
    }
}
