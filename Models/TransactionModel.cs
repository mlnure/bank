namespace OnlineBank.Models;

public class TransactionModel
{
    public int Id { get; set; }
    public decimal Amount { get; set; } // Сумма транзакции
    public DateTime Date { get; set; } = DateTime.Now;// Дата и время транзакции
    public string? Sender { get; set; } // Отправитель транзакции
    public string? Recipient { get; set; } // Получатель транзакции
    public int TagId { get; set; }
    public TagModel? Tag { get; set; } // Тег транзакции (если есть)
    public bool IsIncome { get; set; } // Приход (true) или уход (false) средств
}