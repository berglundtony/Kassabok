using Kassabok.Data.DTO;
using Kassabok.Entity;
using Kassabok.Enum;

namespace Kassabok.Functions.Interface
{
    public interface IFunctions
    {
        TransactionType? GetValue(string transactionType);
        Entity.Transaction SetValuesForTransaction(TransactionDTO transactionDTO, int toAccountId,int typeId);
        TransactionType checkTransactionTypeId(Transaction transaction);
        Account? SetPropertyValuesFromAccount(int fromAccountTypeId, int balanceFromAccount, int fromAccountId, Transaction transaction, Account? account);
        Account? SetPropertyValuesToAccount(int toAccountTypeId, int balanceToAccount, int toAccountId, Transaction transaction, Account? accountEntity);
        string? GetTwentyLettersFromAccountTypeName(string? accountTypeName);
    }
}