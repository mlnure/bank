using OnlineBank.Models;

namespace OnlineBank.ViewModels
{
	public class HomePageViewModel
	{
        public BalanceModel Bal { get; set; }
        public IEnumerable<TransactionModel> Transactions { get; set; }
        public int TagId { get; set; }
    }
}
