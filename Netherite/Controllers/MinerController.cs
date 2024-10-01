using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netherite.Domain;
using Netherite.Interface;

namespace Netherite.Controllers;

[Authorize]
 [ApiController]
  [Route("[controller]")]
  public class MinerController : ControllerBase
  {
    private readonly IMinerServices _minerServices;

    public MinerController(IMinerServices minerServices) => this._minerServices = minerServices;

    /// <summary>
    /// Получает оставшееся время майнинга определенного пользователя
    /// </summary>
    /// <param name="userId">ID пользователя</param>
    /// <returns>Оставшееся время в секундах</returns>
    [HttpGet("time/{userId}")]
    public async Task<ActionResult<double>> GetTimeOfMining(Guid userId)
    {
      try
      {
        (double, bool) valueTuple1 = await this._minerServices.GetCurrentTime(userId);
        (double, bool) valueTuple2 = valueTuple1;
        double remainingTime = valueTuple2.Item1;
        bool isFound = valueTuple2.Item2;
        return isFound ? (ActionResult<double>) (ActionResult) this.Ok((object) remainingTime) : (ActionResult<double>) (ActionResult) this.NotFound((object) "Майнинг не начат.");
      }
      catch (Exception ex)
      {
        return (ActionResult<double>) (ActionResult) this.BadRequest((object) ex.Message);
      }
    }

    /// <summary>Запускает майнинг для опредленного пользователя</summary>
    /// <param name="userId">ID пользователя</param>
    /// <returns>Булево значение, указывающее, успешно ли начался майнинг.</returns>
    [HttpPost("start/{userId}")]
    public async Task<ActionResult<bool>> StartMining(Guid userId)
    {
      try
      {
        Miner miner = Miner.Create(Guid.NewGuid(), userId, 100, DateTime.UtcNow, DateTime.UtcNow.AddSeconds(40.0));
        bool result = await this._minerServices.StartMining(miner, 40);
        return result ? (ActionResult<bool>) (ActionResult) this.Ok((object) result) : (ActionResult<bool>) (ActionResult) this.BadRequest((object) "Майнинг уже начат.");
      }
      catch (Exception ex)
      {
        return (ActionResult<bool>) (ActionResult) this.BadRequest((object) ex.Message);
      }
    }

    /// <summary>Заканчивет майнинг определенного пользователя.</summary>
    /// <param name="userId">ID пользователя</param>
    /// <returns>Булево значение, указывающее, успешно ли закончен майнинг</returns>
    [HttpDelete("end/{userId}")]
    public async Task<ActionResult<bool>> EndMining(Guid userId)
    {
      try
      {
        bool result = await this._minerServices.EndMining(userId);
        return result ? (ActionResult<bool>) (ActionResult) this.Ok((object) result) : (ActionResult<bool>) (ActionResult) this.BadRequest((object) "Не удалось завершить майнинг. Либо майнинг не найден, либо время не равно нулю.");
      }
      catch (Exception ex)
      {
        return (ActionResult<bool>) (ActionResult) this.BadRequest((object) ex.Message);
      }
    }
  }