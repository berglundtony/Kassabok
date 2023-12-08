using Kassabok.Entity;
using Kassabok.Repository;
using Kassabok.Data;
using Kassabok.Data.DTO;
using Kassabok.Enum;
using Kassabok.Functions.Interface;

namespace Kassabok.Functions
{
    public class EntityFunctions : IEntityFunctions
    {
        private readonly TransactionsDbContext _context;

        public EntityFunctions(TransactionsDbContext context)
        {
            _context = context;
        }

        public bool CheckDistincedTypeName(string? name)
        {
           bool excistingTypeName = _context.AccountTypes.Any(n => n.Name == name) ? false : true;
           return excistingTypeName;
        }

        public int FindTypeIdByName(string? name)
        {
            var accountType = _context.AccountTypes.FirstOrDefault(n => n.Name == name);
            return accountType == null ? 0 : accountType.TypeId;
        }

        public int GetBalanceByTypeId(int? typeId)
        {
            var balance = _context.Accounts.FirstOrDefault(n => n.TypeId == typeId);
            return balance == null ? 0 : balance.Balance;
        }

        public int FindAccountIdByTypeID(int? typeId)
        {
            var accountId = _context.Accounts.FirstOrDefault(n => n.TypeId == typeId);
            return accountId == null ? 0 : accountId.AccountId;
        }

        public void GetIdAndBalanceValues(TransactionDTO transactionDTO, out int fromAccountTypeId, out int toAccountTypeId,
                    out int balanceFromAccount, out int balanceToAccount, out int fromAccountId, out int toAccountId)
        {
            // Get Id from from AccountType name
            fromAccountTypeId = FindTypeIdByName(transactionDTO.FromAccount);
            toAccountTypeId = FindTypeIdByName(transactionDTO.ToAccount);

            // Get balance by typeId from Account table
            balanceFromAccount = GetBalanceByTypeId(fromAccountTypeId);
            balanceToAccount = GetBalanceByTypeId(toAccountTypeId);

            // Get current accountId
            fromAccountId = FindAccountIdByTypeID(fromAccountTypeId);
            toAccountId = FindAccountIdByTypeID(toAccountTypeId);
        }
    }
}
