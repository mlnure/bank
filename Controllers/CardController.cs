using BankData.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineBank.Models;
using OnlineBank.Repo;

namespace OnlineBank.Controllers;

public class CardController : Controller
{
	private readonly BalanceRepo _balanceRepo;
	private readonly TransactionRepo _transactionRepo;
	private readonly TagRepo _tagRepo;

	public CardController(BalanceRepo balanceRepo, TransactionRepo transactionRepo, TagRepo tagRepo)
	{
		_balanceRepo = balanceRepo;
		_transactionRepo = transactionRepo;
		_tagRepo = tagRepo;
	}



	// GET: Card/Recharge
	public async Task<IActionResult> Recharge()
	{

		var bal = await _balanceRepo.GetBalanceAsync();
		ViewBag.Bal = bal.Amount;
		var tags = await _tagRepo.GetTagsAsync();

		List<SelectListItem> tagsOptions = new();
		foreach (var tag in tags)
		{
			tagsOptions.Add(new(tag.Name, tag.Id.ToString()));
		}
		ViewBag.Tags = tagsOptions;


		TransactionModel model = new();

		return View(model);
	}

	[HttpPost]
	public async Task<IActionResult> Recharge(TransactionModel transaction)
	{
		transaction.Sender = "You";
		transaction.Recipient = "You";
		transaction.IsIncome = true;
		if (ModelState.IsValid)
		{

			await _transactionRepo.CreateTransaction(transaction);
			await _balanceRepo.UpdateBal(transaction.Amount);
			return RedirectToAction("Index", "Home");

		}

        var bal = await _balanceRepo.GetBalanceAsync();
        ViewBag.Bal = bal.Amount;
        var tags = await _tagRepo.GetTagsAsync();

        List<SelectListItem> tagsOptions = new();
        foreach (var tag in tags)
        {
            tagsOptions.Add(new(tag.Name, tag.Id.ToString()));
        }
        ViewBag.Tags = tagsOptions;

        return View(transaction);
	}


	// GET: Card/Statistics
	public async Task<ActionResult> Statistics()
	{

		var tags = await _tagRepo.GetTagsAsync();
		var trans = await _transactionRepo.GetAllTransactionsAsync();
		foreach(var tag in tags)
		{
			
            var negativeTrans = trans.Where(tr => tr.IsIncome == false && tr.TagId == tag.Id);
			var positiveTrans = trans.Where(tr => tr.IsIncome  && tr.TagId == tag.Id);
			
			tag.Outcome = negativeTrans.Select(ntr => ntr.Amount).Sum();
			tag.Income = positiveTrans.Select(ntr => ntr.Amount).Sum();
		}

		return View(tags);
	}

	public async Task<IActionResult> DeleteTag(int id)
	{
		await _tagRepo.DeleteTag(id);
		return RedirectToAction("Statistics");
	}

    public IActionResult Back()
    {
        return RedirectToAction("Index", "Home");
    }
}