using ERP.Context;
using ERP.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.SiteServices
{
    public class SiteService : ISiteService
    {
        private readonly DataContext _context;

        public SiteService(DataContext context)
        {
            _context = context;
        }

        public async Task<Site> GetOne(int id)
        {
            var site = _context.Sites.Where(s => s.SiteId == id)
                       .FirstOrDefault();

            if (site == null) throw new KeyNotFoundException("Site Not Found.");

            return site;

        }
        
        public async Task<List<Site>> GetAll()
        {
            var sites = await _context.Sites.ToListAsync();

            return sites;

        }

        public async Task<Site> AddSite(Site request)
        {
            Site site = new();
            site.Name = request.Name;
            site.Location = request.Location;
            site.PettyCashLimit = request.PettyCashLimit;

            _context.Sites.Add(site);

            await _context.SaveChangesAsync();

            return site;
        }
        
        public async Task<Site> EditSite(Site request)
        {
            var site = _context.Sites.Where(s => s.SiteId == request.SiteId)
               .FirstOrDefault();
            
            if (site == null) throw new KeyNotFoundException("Site Not Found.");

            site.Name = request.Name;
            site.Location = request.Location;
            site.PettyCashLimit = request.PettyCashLimit;

            await _context.SaveChangesAsync();

            return site;
        }

    }
}
