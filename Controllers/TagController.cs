using Microsoft.AspNetCore.Mvc;
using OnlineBank.Models;
using OnlineBank.Repo;

namespace OnlineBank.Controllers;

public class TagController : Controller
{
    private readonly TagRepo _tagRepo;

    public TagController(TagRepo tagRepo )
    {
        _tagRepo = tagRepo;
    }


    [HttpPost]
    public async Task<IActionResult> Create(TagModel tag)
    {
        // Логика для обработки создания нового тега
        if (ModelState.IsValid)
        {
            await _tagRepo.AddTagAsync(tag);
            return RedirectToAction("Statistics", "Card"); // Перенаправление на страницу создания транзакции
        }
        return RedirectToAction("Statistics", "Card"); // Перенаправление на страницу создания транзакции
    }

    public IActionResult Back()
    {
        return RedirectToAction("Index", "Home");
    }

}