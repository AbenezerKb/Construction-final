using ERP.DTOs;

namespace ERP.Services.MiscServices
{
    public interface IMiscService
    {
        Task<CompanyNamePrefixDTO> GetCompanyNamePrefix();
        Task<bool> SetCompanyNamePrefix(CompanyNamePrefixDTO companyDTO);
    }
}
