using Netherite.Domain;

namespace Netherite.Interface;

public interface ICurrencyPairsService
{
    Task<List<CurrencyPairs>> GetCurrencyPairs();

    Task<Guid> CreateCurrencyPairs(CurrencyPairs currencyPairs);

    Task<bool> DeleteCurrencyPairs(Guid currencyPairsId);

    Task<bool> UpdateCurrencyPairs(Guid currencyPairsId, CurrencyPairs currencyPairs);

    Task<bool> UploadIcon(Guid currencyPairId, string fileUrl);
}