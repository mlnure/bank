using BankData;
using BankData.Entities;
using Microsoft.EntityFrameworkCore;
using OnlineBank.Models;

namespace OnlineBank.Repo
{
	public class TransactionRepo
	{
		private readonly BankDbContext _context;

		public TransactionRepo(BankDbContext context)
        {
			_context = context;
		}

		public async Task<List<TransactionModel>> GetAllTransactionsAsync()
		{
			var trans = await _context.Transactions.Include(t => t.Tag).ToListAsync();
			List<TransactionModel> transactions = new();

			trans.ForEach(t => transactions.Add(new TransactionModel
			{
				Id = t.Id,
				Amount = t.Amount,
				Date = t.Date,
				Sender = t.Sender,
				Recipient = t.Recipient,
				TagId = t.TagId,
				Tag = new TagModel
				{
					Id = t.Tag.Id,
					Name = t.Tag.Name
				},
				IsIncome = t.IsIncome
			}));

			return transactions;
		}


		public	async Task CreateTransaction(TransactionModel transaction)
		{
			Transaction newTransaction = new()
			{
				Recipient = transaction.Recipient,
				Sender = transaction.Sender,
				Date = transaction.Date,
				Amount = transaction.Amount,
				TagId = transaction.TagId,
				IsIncome = transaction.IsIncome
			};

			await _context.Transactions.AddAsync(newTransaction);
			await _context.SaveChangesAsync();
		}


		public async Task UpdateTag(int tranId , int tagId)
		{
			var tran = await _context.Transactions.FindAsync(tranId);
			tran.TagId = tagId;
			await _context.SaveChangesAsync();
		}
    }
}
