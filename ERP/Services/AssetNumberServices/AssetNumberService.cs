using ERP.Context;
using ERP.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ERP.Services.AssetNumberServices
{
    public class AssetNumberService : IAssetNumberService
    {
        private readonly DataContext _context;
        private const int numberOfLetters = 2;
        private const int numberOfDigits = 7;

        public AssetNumberService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<string>>GenerateAssetNumbers(int itemId, int count)
        {
            var assetNumberId = await GetAssetPrefix(itemId);
            if (assetNumberId == null) 
                throw new InvalidOperationException("Could Not Generate Asset Number Prefix");

            string assetPrefix = assetNumberId.Prefix;
            int lastId = assetNumberId.LastId;
            string companyPrefix = await GetCompanyPrefix();
            
            List<string> assetNumbers = new();

            for(int i = 0; i < count; i++)
            {
                lastId++;

                assetNumbers.Add(
                    companyPrefix +
                    assetPrefix + 
                    lastId.ToString().PadLeft(numberOfDigits, '0'));
            }

            assetNumberId.LastId = lastId;
            await _context.SaveChangesAsync();

            return assetNumbers;
        }

        private async Task<string> GetCompanyPrefix()
        {
            var companyMisc = await _context.Miscellaneouses.
                Where(m => m.Key == MICELLANEOUSKEYS.COMPANYPREFIX)
                .FirstOrDefaultAsync();

            if (companyMisc != null)
                return companyMisc.Value;

            return "XX";
        }

        private async Task<AssetNumberId> GetAssetPrefix (int itemId)
        {
            var item = _context.Items
                .Where(i => i.ItemId == itemId)
                .FirstOrDefault();

            if (item == null) throw new KeyNotFoundException("Item Id Not Found");

            var assetNumberId = await _context.AssetNumberIds
                .Where(i => i.ItemId == itemId)
                .FirstOrDefaultAsync();

            if (assetNumberId != null) return assetNumberId;

            string itemName = item.Name.ToUpper();
            itemName = Regex.Replace(itemName, @"(\d|\s)+", "");

            //use combination of letters in the name
            IEnumerable<string> prefixes = PrefixGenerator.GetCombinations(numberOfLetters - 1, itemName.Substring(1));
            List<string> assets = new List<string>();
            string prefix = "";

            foreach (string prefixEnd in prefixes)
            {
                prefix = itemName.Substring(0, 1) + prefixEnd;

                if (await AssetPrefixExists(prefix)) continue;

                return await AddAssetPrefix(prefix, itemId);

            }

            //if the combination of letters in the name did not return try every letter
            prefixes = PrefixGenerator.GetCombinations(numberOfLetters - 1);
            assets = new List<string>();

            foreach (string prefixEnd in prefixes)
            {
                prefix = itemName.Substring(0, 1) + prefixEnd;

                if (await AssetPrefixExists(prefix)) continue;

                return await AddAssetPrefix(prefix, itemId);

            }


            return null;

        }

        private async Task<AssetNumberId> AddAssetPrefix(string prefix, int itemId)
        {
            AssetNumberId assetNumberId = new();

            assetNumberId.ItemId = itemId;
            assetNumberId.Prefix = prefix;
            
            _context.AssetNumberIds.Add(assetNumberId);

            await _context.SaveChangesAsync();

            return assetNumberId;
        }

        private async Task<bool> AssetPrefixExists(string prefix)
        {
            var assetPrefix = await _context.AssetNumberIds
                .Where(an => an.Prefix == prefix)
                .FirstOrDefaultAsync();

            if (assetPrefix == null) return false;

            return true;

        }

    }


    public class PrefixGenerator
    {
        public static IEnumerable<string> GetCombinations(int numberOfCharacters, string itemName)
        {
            string pattern = new string('A', numberOfCharacters);
            string stringA = new string(itemName.ToCharArray().Distinct().ToArray());

            var sets = pattern.Select(charsetCode => charset(charsetCode, stringA));

            return Combine(sets).Select(x => new String(x.ToArray()));
        }

        public static IEnumerable<string> GetCombinations(int numberOfCharacters)
        {
            string pattern = new string('B', numberOfCharacters);
            string stringA = String.Empty;

            var sets = pattern.Select(charsetCode => charset(charsetCode, stringA));

            return Combine(sets).Select(x => new String(x.ToArray()));
        }

        private static string charset(char charsetCode, string stringA)
        {
            switch (charsetCode)
            {
                case 'A': return stringA;
                case 'B': return "ABCEDEFGHIJKLMNOPQRSTUVWXYZ";

                default: throw new InvalidOperationException("Bad charset code: " + charsetCode);
            }
        }

        private static IEnumerable<IEnumerable<T>> Combine<T>(IEnumerable<IEnumerable<T>> sequences)
        {
            IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };

            return sequences.Aggregate(
                emptyProduct,
                (accumulator, sequence) =>
                    from accseq in accumulator
                    from item in sequence
                    select accseq.Concat(new[] { item }));
        }
    }

}
