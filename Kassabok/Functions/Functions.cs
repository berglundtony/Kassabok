using Kassabok.Data;
using Kassabok.Data.DTO;
using Kassabok.Entity;
using Kassabok.Enum;
using Kassabok.Functions.Interface;

namespace Kassabok.Functions
{
    public class Functions : IFunctions
    {
        // ToDo: Test to se if transaction type is following the enum values
        // This check that input values follows the enum values
        public TransactionType? GetValue(string? transactionType)
        {
            if (transactionType?.ToLower() == TransactionType.income.ToString())
            {
                return TransactionType.income;
            }
            else if (transactionType?.ToLower() == TransactionType.expense.ToString())
            {
                return TransactionType.expense;
            }
            else if (transactionType?.ToLower() == TransactionType.check.ToString())
            {
                return TransactionType.check;
            }
            else
            {
                return null;
            }
        }

        public TransactionType checkTransactionTypeId(Transaction transaction)
        {
            if (transaction.TypeId == (int)TransactionType.income)
            {
                return TransactionType.income;
            }
            else if (transaction.TypeId == (int)TransactionType.expense)
            {
                return TransactionType.expense;
            }
            else
            {
                return TransactionType.check;
            }
  
        }

        public Entity.Transaction SetValuesForTransaction(TransactionDTO transactionDTO, int toAccountId,int typeId)
        {
            var transaction = new Kassabok.Entity.Transaction
            {
                FromAccount = transactionDTO.FromAccount,
                ToAccount = transactionDTO.ToAccount,
                Amount = transactionDTO.Amount,
                AccountId = toAccountId,
                TypeId = (int?)typeId
            };

            return transaction;
        }

        public Account? SetPropertyValuesFromAccount(int fromAccountTypeId, int balanceFromAccount, int fromAccountId, Transaction transaction, Account? account)
        {
            if (account.AccountType.Type == TransactionType.check || account.AccountType.Type == TransactionType.income)
            {
                if (balanceFromAccount > 0 && balanceFromAccount >= transaction.Amount)
                {
                    balanceFromAccount -= transaction.Amount;
                }
                else
                {
                    account = null;
                }
            }
            else
            {
                if (balanceFromAccount < 0)
                {
                    balanceFromAccount += transaction.Amount;
                }
                else
                {
                    balanceFromAccount -= transaction.Amount;
                }
            }

            if (account != null)
            {
                account.AccountId = fromAccountId;
                account.Balance = balanceFromAccount;
                account.TypeId = fromAccountTypeId;
            }

            return account;
        }

        public Account? SetPropertyValuesToAccount(int toAccountTypeId, int balanceToAccount, int toAccountId, Entity.Transaction transaction,Account? accountEntity)
        {
            if (accountEntity != null)
            {
                var balToAcc = accountEntity.AccountType.Type == TransactionType.check || accountEntity.AccountType.Type == TransactionType.income ? balanceToAccount += transaction.Amount : balanceToAccount -= transaction.Amount;

                accountEntity.AccountId = toAccountId;
                accountEntity.Balance = balanceToAccount;
                accountEntity.TypeId = toAccountTypeId;
            }

            return accountEntity;
        }

        public string? GetTwentyLettersFromAccountTypeName(string? accounttypename)
        {
            string? twentyLetters =string.IsNullOrEmpty(accounttypename) ? accounttypename : accounttypename.Substring(0, Math.Min(20, accounttypename.Length));
            return twentyLetters;
        }
    }
}
