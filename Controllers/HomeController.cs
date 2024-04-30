using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineBank.Models;
using OnlineBank.Repo;
using OnlineBank.ViewModels;

namespace OnlineBank.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
	private readonly BalanceRepo _balanceRepo;
	private readonly TransactionRepo _transactionRepo;
	private readonly TagRepo _tagRepo;

	public HomeController(ILogger<HomeController> logger,
					   BalanceRepo balanceRepo,
					   TransactionRepo transactionRepo , 
                       TagRepo tagRepo)
    {
        _logger = logger;
		_balanceRepo = balanceRepo;
		_transactionRepo = transactionRepo;
		_tagRepo = tagRepo;
	}

    public async Task<IActionResult> Index()
    {
        HomePageViewModel home = new();
        var bal = await _balanceRepo.GetBalanceAsync();
        var transactions = await _transactionRepo.GetAllTransactionsAsync();
        transactions.Reverse();
        home.Transactions = transactions;
        home.Bal = bal;


        var tags =await _tagRepo.GetTagsAsync();
		List<SelectListItem> tagsOptions = new();
		foreach (var tag in tags)
		{
			tagsOptions.Add(new(tag.Name, tag.Id.ToString()));
		}
		ViewBag.Tags = tagsOptions;

		return View(home);
    }

    [HttpPost]
    public async Task<IActionResult> ChangeTag(int id , HomePageViewModel model)
    {
        await _transactionRepo.UpdateTag(id, model.TagId);

        return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

