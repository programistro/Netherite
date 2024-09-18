using Microsoft.AspNetCore.Mvc;
using Netherite.Contracts;
using Netherite.Domain;
using Netherite.Interface;

namespace Netherite.Controllers;

[ApiController]
  [Route("[controller]")]
  public class FavoritesController : ControllerBase
  {
    private readonly IFavoritesService _favoritesService;

    public FavoritesController(IFavoritesService favoritesService)
    {
      this._favoritesService = favoritesService;
    }

    /// <summary>Получение списка избранных пользователя</summary>
    /// <param name="userId">ID пользователя</param>
    [HttpGet("{userId}")]
    public async Task<ActionResult<List<TasksResponse>>> GetFavoritesCurrencyPairs(Guid userId)
    {
      try
      {
        List<CurrencyPairs> favorites = await this._favoritesService.GetFavorites(userId);
        IEnumerable<CurrencyPairsResponse> response = favorites.Select<CurrencyPairs, CurrencyPairsResponse>((Func<CurrencyPairs, CurrencyPairsResponse>) (t => new CurrencyPairsResponse(t.Id, t.Name, t.NameTwo, t.Icon, t.InterestRate)));
        return (ActionResult<List<TasksResponse>>) (ActionResult) this.Ok((object) response);
      }
      catch (Exception ex)
      {
        return (ActionResult<List<TasksResponse>>) (ActionResult) this.BadRequest((object) ex.Message);
      }
    }

    /// <summary>Создание избранной валютной пары</summary>
    /// <param name="userId">ID пользователя</param>
    /// <param name="currencyPairsId">ID валютной пары</param>
    [HttpPost("add/{userId}/{currencyPairsId}")]
    public async Task<ActionResult<Guid>> CreateFavorite(Guid userId, Guid currencyPairsId)
    {
      try
      {
        Guid guid = await this._favoritesService.CreateFavorites(userId, currencyPairsId);
        Guid favoritesId = guid;
        guid = new Guid();
        return (ActionResult<Guid>) (ActionResult) this.Ok((object) favoritesId);
      }
      catch (Exception ex)
      {
        return (ActionResult<Guid>) (ActionResult) this.BadRequest((object) ex.Message);
      }
    }

    /// <summary>Удаление валютной пары из избранного</summary>
    /// <param name="userId">ID пользователя</param>
    /// <param name="currencyPairsId">ID валютной пары</param>
    [HttpDelete("remove/{userId}/{currencyPairsId}")]
    public async Task<ActionResult<bool>> DeleteFavorite(Guid userId, Guid currencyPairsId)
    {
      try
      {
        bool result = await this._favoritesService.DeleteFavorites(userId, currencyPairsId);
        return result ? (ActionResult<bool>) (ActionResult) this.Ok((object) result) : (ActionResult<bool>) (ActionResult) this.NotFound((object) "Favorites not found or could not be updated.");
      }
      catch (Exception ex)
      {
        return (ActionResult<bool>) (ActionResult) this.BadRequest((object) ex.Message);
      }
    }
  }