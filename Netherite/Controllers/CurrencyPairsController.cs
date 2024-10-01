using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netherite.Contracts;
using Netherite.Data;
using Netherite.Domain;
using Netherite.Interface;

namespace Netherite.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class CurrencyPairsController : ControllerBase
{
  private readonly ICurrencyPairsService _currencyPairsService;

  private readonly NetheriteDbContext _context;

  public CurrencyPairsController(ICurrencyPairsService currencyPairsService, NetheriteDbContext context)
  {
    this._currencyPairsService = currencyPairsService;
    _context = context;
  }

  /// <summary>Получение списка валютных пар</summary>
  [HttpGet]
  [Authorize]
  public async Task<ActionResult<List<CurrencyPairsResponse>>> GetCurrencyPairs()
  {
    try
    {
      List<CurrencyPairs> currencyPairs = await this._currencyPairsService.GetCurrencyPairs();
      if (currencyPairs == null)
        return (ActionResult<List<CurrencyPairsResponse>>)(ActionResult)this.NotFound();
      IEnumerable<CurrencyPairsResponse> response = currencyPairs.Select<CurrencyPairs, CurrencyPairsResponse>(
        (Func<CurrencyPairs, CurrencyPairsResponse>)(cp =>
          new CurrencyPairsResponse(cp.Id, cp.Name, cp.NameTwo, cp.Icon, cp.InterestRate)));
      return (ActionResult<List<CurrencyPairsResponse>>)(ActionResult)this.Ok((object)response);
    }
    catch (Exception ex)
    {
      return (ActionResult<List<CurrencyPairsResponse>>)(ActionResult)this.BadRequest((object)ex.Message);
    }
  }

  /// <summary>Создание валютной пары</summary>
  /// <param name="request">Запрос на создание валютной пары содержит имя валютной пары, процент прибыли валюнтой пары.</param>
  /// <param name="file">Изображение валютной пары.</param>
  [HttpPost]
  public async Task<ActionResult<Guid>> CreateCurrencyPairs(
    [FromForm] CurrencyPairsRequest request,
    IFormFile file)
  {
    try
    {
      if (file == null || file.Length == 0L)
        return BadRequest("Файл не выбран или он пустой");

      // Создаем имя файла
      string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
      string filePath = Path.Combine("storage", fileName);

      // Создаем директорию, если она не существует
      if (!Directory.Exists("storage"))
        Directory.CreateDirectory("storage");

      // Сохраняем файл
      using (FileStream stream = new FileStream(filePath, FileMode.Create))
      {
        await file.CopyToAsync(stream);
      }

      // Создаем URL файла
      string fileUrl = $"https://{this.Request.Host}/storage/{fileName}";

      // Создаем объект CurrencyPairs
      CurrencyPairs currencyPair = CurrencyPairs.Create(
        Guid.NewGuid(),
        request.Name,
        request.NameTwo,
        fileUrl,
        request.InterestRate);

      // Сохраняем пару валют
      Guid currencyPairId = await _currencyPairsService.CreateCurrencyPairs(currencyPair);

      return Ok(currencyPairId);
    }
    catch (Exception ex)
    {
      return StatusCode(500, "Внутренняя ошибка сервера");
    }
  }

  /// <summary>Обновление валютной пары</summary>
  /// <param name="currencyPairId">ID валютной пары</param>
  /// <param name="request">Запрос на обновление валютной пары содержит имя валютной пары, процент прибыли валюнтой пары.</param>
  [HttpPut("{currencyPairId}")]
  public async Task<ActionResult<bool>> UpdateCurrencyPairs(
    Guid currencyPairId,
    [FromBody] CurrencyPairsRequest request,
    IFormFile file)
  {
    try
    {
      string fileUrl = "";

      if (file != null && file.Length > 0)
      {
        // Создаем имя файла
        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        string filePath = Path.Combine("storage", fileName);

        // Создаем директорию, если она не существует
        if (!Directory.Exists("storage"))
          Directory.CreateDirectory("storage");

        // Сохраняем файл
        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
          await file.CopyToAsync(stream);
        }

        // Создаем URL файла
        fileUrl = $"{this.Request.Scheme}://{this.Request.Host}/storage/{fileName}";
      }

      // Создаем объект CurrencyPairs
      CurrencyPairs currencyPair = CurrencyPairs.Create(
        currencyPairId,
        request.Name,
        request.NameTwo,
        fileUrl,
        request.InterestRate);

      // Обновляем пару валют
      bool result = await _currencyPairsService.UpdateCurrencyPairs(currencyPairId, currencyPair);

      if (result)
        return Ok(result);
      else
        return NotFound("Валютная пара не найдена или не может быть обновлена");
    }
    catch (Exception ex)
    {
      return StatusCode(500, "Внутренняя ошибка сервера");
    }
  }

  [HttpPut("update-icon-pair")]
  public async Task<IActionResult> UpdateIconCurrencyPairs(Guid id, IFormFile file)
  {
    var pair = _context.CurrencyPairs.FirstOrDefault(x => x.Id == id);

    if (pair == null)
    {
      return NotFound();
    }
    
    try
    {
      if (file == null || file.Length == 0L)
        return BadRequest("Файл не выбран или он пустой");

      // Создаем имя файла
      string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
      string filePath = Path.Combine("storage", fileName);

      // Создаем директорию, если она не существует
      if (!Directory.Exists("storage"))
        Directory.CreateDirectory("storage");

      // Сохраняем файл
      using (FileStream stream = new FileStream(filePath, FileMode.Create))
      {
        await file.CopyToAsync(stream);
      }

      // Создаем URL файла
      string fileUrl = $"https://{this.Request.Host}/storage/{fileName}";

      pair.Icon = fileUrl;

      _context.CurrencyPairs.Update(pair);
      await _context.SaveChangesAsync();

      return Ok(pair);
    }
    catch (Exception ex)
    {
      return StatusCode(500, "Внутренняя ошибка сервера");
    }
  }

  /// <summary>Удаление валютной пары</summary>
  /// <param name="currencyPairId">ID валютной пары</param>
  [HttpDelete("{currencyPairId}")]
  public async Task<ActionResult<bool>> DeleteCurrencyPairs(Guid currencyPairId)
  {
    try
    {
      bool result = await this._currencyPairsService.DeleteCurrencyPairs(currencyPairId);
      return result
        ? (ActionResult<bool>)(ActionResult)this.Ok((object)result)
        : (ActionResult<bool>)(ActionResult)this.NotFound((object)"Валютная пара не найдена");
    }
    catch (Exception ex)
    {
      return (ActionResult<bool>)(ActionResult)this.BadRequest((object)ex.Message);
    }
  }

  [HttpPost("{currencyPairId}/upload")]
  public async Task<ActionResult<bool>> UploadIcon(Guid currencyPairId, IFormFile file)
  {
    try
    {
      if (file == null || file.Length == 0L)
        return BadRequest("Файл не выбран или он пустой");

      // Создаем имя файла
      string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
      string filePath = Path.Combine("storage", fileName);

      // Создаем директорию, если она не существует
      if (!Directory.Exists("storage"))
        Directory.CreateDirectory("storage");

      // Сохраняем файл
      using (FileStream stream = new FileStream(filePath, FileMode.Create))
      {
        await file.CopyToAsync(stream);
      }

      // Создаем URL файла
      string fileUrl = $"{this.Request.Scheme}://{this.Request.Host}/storage/{fileName}";

      // Загружаем иконку
      bool result = await _currencyPairsService.UploadIcon(currencyPairId, fileUrl);

      if (result)
        return Ok(result);
      else
        return NotFound("Валютная пара не найдена или не может быть обновлена");
    }
    catch (Exception ex)
    {
      return StatusCode(500, "Внутренняя ошибка сервера");
    }
  }
}