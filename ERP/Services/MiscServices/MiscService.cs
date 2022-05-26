using ERP.Context;
using ERP.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.MiscServices
{
    public class MiscService : IMiscService
    {
        private readonly DataContext _context;

        public MiscService(DataContext context)
        {
            _context = context;
        }


        public async Task<CompanyNamePrefixDTO> GetCompanyNamePrefix()
        {
            var companyNamePrefixDTO = new CompanyNamePrefixDTO();

            var nameData = await _context.Miscellaneouses
                .Where(m => m.Key == MICELLANEOUSKEYS.COMPANYNAME)
                .FirstOrDefaultAsync();

            if (nameData != null)
                companyNamePrefixDTO.Name = nameData.Value;

            var prefixData = await _context.Miscellaneouses
                .Where(m => m.Key == MICELLANEOUSKEYS.COMPANYPREFIX)
                .FirstOrDefaultAsync();

            if (prefixData != null)
                companyNamePrefixDTO.Prefix = prefixData.Value;

            return companyNamePrefixDTO;

        }

        public async Task<bool> SetCompanyNamePrefix(CompanyNamePrefixDTO companyDTO)
        {
            var nameData = await _context.Miscellaneouses
                .Where(m => m.Key == MICELLANEOUSKEYS.COMPANYNAME)
                .FirstOrDefaultAsync();

            if (nameData == null)
            {
                nameData = new Models.Miscellaneous
                {
                    Key = MICELLANEOUSKEYS.COMPANYNAME,
                    Value = companyDTO.Name
                };

                _context.Add(nameData);
            }
            else
            {
                nameData.Value = companyDTO.Name;
            }



            var prefixData = await _context.Miscellaneouses
                .Where(m => m.Key == MICELLANEOUSKEYS.COMPANYPREFIX)
                .FirstOrDefaultAsync();

            if (prefixData == null)
            {
                prefixData = new Models.Miscellaneous
                {
                    Key = MICELLANEOUSKEYS.COMPANYPREFIX,
                    Value = companyDTO.Prefix
                };

                _context.Add(prefixData);
            }
            else
            {
                prefixData.Value = companyDTO.Prefix;
            }

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
