using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netherite.Contracts;
using Netherite.Domain;
using Netherite.Interface;

namespace Netherite.Controllers;

[Authorize]
 [ApiController]
  [Route("[controller]")]
  public class IntervalController : ControllerBase
  {
    private readonly IIntervalServices _intervalServices;

    public IntervalController(IIntervalServices intervalServices)
    {
      this._intervalServices = intervalServices;
    }

    /// <summary>Получение списка интервалов валютной пары</summary>
    /// <param name="pairsId">ID валютной пары</param>
    [HttpGet("{pairsId}")]
    public async Task<ActionResult<List<IntervalsResponse>>> GetIntervalsByPairsId(Guid pairsId)
    {
      try
      {
        List<Interval> intervals = await this._intervalServices.GetIntervalByPairsId(pairsId);
        if (intervals == null)
          return (ActionResult<List<IntervalsResponse>>) (ActionResult) this.NotFound();
        IEnumerable<IntervalsResponse> response = intervals.Select<Interval, IntervalsResponse>((Func<Interval, IntervalsResponse>) (t => new IntervalsResponse(t.Id, t.Time)));
        return (ActionResult<List<IntervalsResponse>>) (ActionResult) this.Ok((object) response);
      }
      catch (Exception ex)
      {
        return (ActionResult<List<IntervalsResponse>>) (ActionResult) this.BadRequest((object) ex.Message);
      }
    }

    /// <summary>Создание интервала</summary>
    /// <param name="pairsId">ID валютной пары для привязки интервала</param>
    /// <param name="request">Запрос на создание интервала содержит время интервала.</param>
    [HttpPost("{pairsId}")]
    public async Task<ActionResult<Guid>> CreateInterval(Guid pairsId, [FromBody] IntervalsRequest request)
    {
      try
      {
        Interval interval = Interval.Create(Guid.NewGuid(), request.Time);
        Guid guid = await this._intervalServices.CreateInterval(interval, pairsId);
        Guid intervalId = guid;
        guid = new Guid();
        return !(intervalId == Guid.Empty) ? (ActionResult<Guid>) (ActionResult) this.Ok((object) intervalId) : (ActionResult<Guid>) (ActionResult) this.NotFound((object) "Валютная пара для привязки интервала не найдена");
      }
      catch (Exception ex)
      {
        return (ActionResult<Guid>) (ActionResult) this.BadRequest((object) ex.Message);
      }
    }

    /// <summary>Обновление данных интервала</summary>
    /// <param name="intervalId">ID интервала</param>
    /// <param name="request">Запрос на обновление интервала содержит время интервала.</param>
    [HttpPut("{intervalId}")]
    public async Task<ActionResult<bool>> UpdateInterval(Guid intervalId, [FromBody] IntervalsRequest request)
    {
      try
      {
        Interval interval = Interval.Create(intervalId, request.Time);
        bool result = await this._intervalServices.UpdateInterval(intervalId, interval);
        return result ? (ActionResult<bool>) (ActionResult) this.Ok((object) result) : (ActionResult<bool>) (ActionResult) this.NotFound((object) "Interval not found or could not be updated.");
      }
      catch (Exception ex)
      {
        return (ActionResult<bool>) (ActionResult) this.BadRequest((object) ex.Message);
      }
    }

    /// <summary>Удаление интервала</summary>
    /// <param name="intervalId">ID интервала</param>
    [HttpDelete("{intervalId}")]
    public async Task<ActionResult<bool>> DeleteInterval(Guid intervalId)
    {
      try
      {
        bool result = await this._intervalServices.DeleteInterval(intervalId);
        return result ? (ActionResult<bool>) (ActionResult) this.Ok((object) result) : (ActionResult<bool>) (ActionResult) this.NotFound((object) "Interval not found or could not be updated.");
      }
      catch (Exception ex)
      {
        return (ActionResult<bool>) (ActionResult) this.BadRequest((object) ex.Message);
      }
    }
  }