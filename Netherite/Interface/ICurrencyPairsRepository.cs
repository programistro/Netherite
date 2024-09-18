using Netherite.Domain;

namespace Netherite.Interface;

public interface ICurrencyPairsRepository
{
    Task<List<CurrencyPairs>> GetCurrencyPairs();

    Task<Guid> Create(CurrencyPairs currencyPairs);

    Task<bool> Delete(Guid currencyPairsId);

    Task<bool> Update(Guid currencyPairsId, CurrencyPairs currencyPairs);

    Task<bool> UploadIcon(Guid currencyPairId, string fileUrl);
}