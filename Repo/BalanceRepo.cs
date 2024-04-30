using BankData;
using Microsoft.EntityFrameworkCore;
using OnlineBank.Models;

namespace OnlineBank.Repo
{
	public class BalanceRepo
	{
		private readonly BankDbContext _context;

		public BalanceRepo(BankDbContext context)
        {
			_context = context;
		}

		public async Task<BalanceModel> GetBalanceAsync()
		{
			var bal = await _context.Balances.FirstAsync();
			return new BalanceModel { Id = bal.Id, Amount = bal.Amount };

		}

		public async Task UpdateBal(decimal amount)
		{
			var bal = await _context.Balances.FirstOrDefaultAsync();
			bal.Amount = bal.Amount+amount;
			await _context.SaveChangesAsync();
		}
    }
}
