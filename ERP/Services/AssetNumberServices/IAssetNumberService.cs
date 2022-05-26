using ERP.Models;

namespace ERP.Services.AssetNumberServices
{
    public interface IAssetNumberService
    {
        Task<List<string>> GenerateAssetNumbers(int itemId, int count);
    }
}
