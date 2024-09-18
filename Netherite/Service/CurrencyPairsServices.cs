using Netherite.Domain;
using Netherite.Interface;

namespace Netherite.Service;

public class CurrencyPairsServices : ICurrencyPairsService
{
    private readonly ICurrencyPairsRepository _currencyPairsRepository;

    public CurrencyPairsServices(ICurrencyPairsRepository currencyPairsRepository)
    {
        this._currencyPairsRepository = currencyPairsRepository;
    }

    public Task<Guid> CreateCurrencyPairs(CurrencyPairs currencyPairs)
    {
        return this._currencyPairsRepository.Create(currencyPairs);
    }

    public Task<bool> DeleteCurrencyPairs(Guid currencyPairsId)
    {
        return this._currencyPairsRepository.Delete(currencyPairsId);
    }

    public Task<List<CurrencyPairs>> GetCurrencyPairs()
    {
        return this._currencyPairsRepository.GetCurrencyPairs();
    }

    public Task<bool> UpdateCurrencyPairs(Guid currencyPairsId, CurrencyPairs currencyPairs)
    {
        return this._currencyPairsRepository.Update(currencyPairsId, currencyPairs);
    }

    public async Task<bool> UploadIcon(Guid currencyPairId, string fileUrl)
    {
        bool flag = await this._currencyPairsRepository.UploadIcon(currencyPairId, fileUrl);
        return flag;
    }
}