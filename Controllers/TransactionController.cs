using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineBank.Models;
using OnlineBank.Repo;

namespace OnlineBank.Controllers;

public class TransactionController : Controller
{
	private readonly BalanceRepo _balanceRepo;
	private readonly TagRepo _tagRepo;
	private readonly TransactionRepo _transactionRepo;

	public TransactionController(BalanceRepo balanceRepo , TagRepo tagRepo , TransactionRepo transactionRepo)
    {
		_balanceRepo = balanceRepo;
		_tagRepo = tagRepo;
		_transactionRepo = transactionRepo;
	}

    // GET: Transaction/Create
    public async Task<ActionResult> Create()
    {
        var tags = await _tagRepo.GetTagsAsync();
        var bal = await _balanceRepo.GetBalanceAsync();
        ViewBag.Bal = bal.Amount;

        List<SelectListItem> tagsOptions = new();
        foreach (var tag in tags)
        {
            tagsOptions.Add(new(tag.Name, tag.Id.ToString()));
        }
        ViewBag.Tags = tagsOptions;


        TransactionModel model = new TransactionModel();

        return View(model);
    }

    // POST: Transaction/Create
    [HttpPost]
    public async Task<IActionResult> Create(TransactionModel transaction)
    {


		var tags = await _tagRepo.GetTagsAsync();
		var bal = await _balanceRepo.GetBalanceAsync();
		ViewBag.Bal = bal.Amount;

		List<SelectListItem> tagsOptions = new();
		foreach (var tag in tags)
		{
			tagsOptions.Add(new(tag.Name, tag.Id.ToString()));
		}
		ViewBag.Tags = tagsOptions;


		transaction.Sender = "You";
        transaction.IsIncome = false;
        // Логика для обработки создания новой транзакции

        if (string.IsNullOrWhiteSpace(transaction.Recipient))
        {
			return View(transaction);

		}

		if (ModelState.IsValid)
        {

            await _transactionRepo.CreateTransaction(transaction);
            await _balanceRepo.UpdateBal(-transaction.Amount);
            // Ваш код для сохранения транзакции в базе данных
            return RedirectToAction("Index" , "Home");
           
        }
        return View(transaction);
    }


    public async Task<IActionResult> GenerateTransaction()
    {
        TransactionModel tran = new()
        {
            IsIncome = true,
            Recipient = "You"

        };

        Random rand = new();

        var tags = await _tagRepo.GetTagsAsync();
        int randTag = rand.Next(0, tags.Count);
        tran.Tag = tags.ElementAt(randTag);
        tran.TagId = tran.Tag.Id;

        string[] senders = {
    "John Smith",
    "Emily Johnson",
    "Michael Williams",
    "Jennifer Brown",
    "William Jones",
    "Sarah Davis",
    "David Miller",
    "Ashley Wilson",
    "James Moore",
    "Jessica Taylor",
    "Robert Anderson",
    "Amanda Martinez",
    "Daniel Thomas",
    "Elizabeth Garcia",
    "Matthew Robinson",
    "Michelle Hernandez",
    "Christopher Lee",
    "Kimberly Perez",
    "Joshua Turner",
    "Nicole Scott"
};

        int randSender = rand.Next(0, senders.Length);
        tran.Sender = senders[randSender];

        decimal randAmount = rand.Next(0, 1000);
        tran.Amount = randAmount;

        await _transactionRepo.CreateTransaction(tran);
        await _balanceRepo.UpdateBal(tran.Amount);

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Back()
    {
        return RedirectToAction("Index" , "Home");
    }
}