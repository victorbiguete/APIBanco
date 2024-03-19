using System.ComponentModel;
namespace APIBanco.Domain.Enums;

public enum TransactionType
{
    [Description(description: "Deposit")]
    Deposit = 0,
    [Description(description: "Withdraw")]
    Withdraw = 1,
    [Description(description: "TransferOutcome")]
    TransferOutcome = 2,
    [Description(description: "TransferIncome")]
    TransferIncome = 3
}
