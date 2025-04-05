using StockForThePeople.InternalData.DTO;

namespace StockForThePeople.InternalData
{
    public interface IInternalDataService
    {
        Task<List<AssetGetDtoList>> GetAllAssetsAsync();
        Task<AssetGetDto> GetAssetByTickerAsync(string ticker);
        Task<AssetWithMarketGetDto> GetMarketForAssetAsync(string ticker);
        Task<SingleAssetWithMarketAndInformationListGetDto> GetMarketWithInformationForAssetAsync(string ticker, InformationOptions informationOptions, int numberOfDays = 30, DateTime lastDateTime = default);
    }
}