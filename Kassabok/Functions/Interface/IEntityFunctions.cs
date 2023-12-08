using Kassabok.Data.DTO;
using System.Transactions;

namespace Kassabok.Functions.Interface
{
    public interface IEntityFunctions
    {
        bool CheckDistincedTypeName(string? name);
        int FindAccountIdByTypeID(int? typeId);
        int FindTypeIdByName(string? name);
        int GetBalanceByTypeId(int? typeId);
        void GetIdAndBalanceValues(TransactionDTO transactionDTO,
            out int fromAccountTypeId, out int toAccountTypeId,
            out int balanceFromAccount, out int balanceToAccount,
            out int fromAccountId, out int toAccountId);
    }
}
