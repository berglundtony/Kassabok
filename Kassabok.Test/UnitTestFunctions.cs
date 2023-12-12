using Kassabok.Functions.Interface;
using Kassabok.Entity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit.Abstractions;

namespace Kassabok.Test
{
    public class UnitTestFunctions : IClassFixture<ServiceProviderFixture>
    {
        private readonly IFunctions _functions;
        private readonly ITestOutputHelper _output;

        public UnitTestFunctions(ServiceProviderFixture fixture, ITestOutputHelper output)
        {
            this._functions = fixture.ServiceProvider.GetRequiredService<IFunctions>();
            _output = output;
            _output = output;
        }

        [Fact]
        public void CheckIfEnumValueIsCorrect()
        {
            //Arrange
            string transactionType = "check";
            var accountNames = _functions.GetValue(transactionType);

            Assert.Equal(transactionType, accountNames.ToString());
        }

        [Fact]
        public void CheckIfBalanceIsCorrectFromAccountTypeIncome()
        {
            //Arrange
            var accounttype_fromaccount = new AccountType { Name = "Lön", Type = Enum.TransactionType.income, TypeId = 4 };
            var fromaccount = new Account { AccountId = 4, Balance = 1000, TypeId = 4, AccountType = accounttype_fromaccount };

            var accounttype = new AccountType { Name = "Bankkonto", Type = Enum.TransactionType.check, TypeId = 3 };
            var toaccount = new Account { AccountId = 3, Balance = 2000, TypeId = 3, AccountType = accounttype };
            var transaction = new Transaction { Account = toaccount, AccountId = 3, Amount = 1000, FromAccount = "Lön", ToAccount = "Bankkonto", TransactionId = 0, TypeId = 3 };

            var expected = 2000;
            //Act
            var result = _functions.SetPropertyValuesFromAccount(4, 1000, 4, transaction, fromaccount);
            //Assert
            Assert.Equal(expected, result.Balance);
        }

        [Fact]
        public void CheckIfBalanceIsCorrectToAccountypeCheck()
        {
            //Arrange
            var accounttype = new AccountType { Name = "Bankkonto", Type = Enum.TransactionType.check, TypeId = 3 };
            var toaccount = new Account { AccountId = 3, Balance = 2000, TypeId = 3, AccountType = accounttype };
            var transaction = new Transaction { Account = toaccount, AccountId = 3, Amount = 1000, FromAccount = "Lön", ToAccount = "Bankkonto", TransactionId = 0, TypeId = 3 };

            var expected = 3000;
            //Act
            var result = _functions.SetPropertyValuesToAccount(3, 2000, 3, transaction, toaccount);
            //Assert
            Assert.Equal(expected, result.Balance);
        }

        [Fact]
        public void CheckIfBalanceIsCorrectFromAccountIsTypeCheck()
        {
            //Arrange
            var accounttype_fromaccount = new AccountType { Name = "Bankkonto", Type = Enum.TransactionType.check, TypeId = 3 };
            var fromaccount = new Account { AccountId = 3, Balance = 1000, TypeId = 3, AccountType = accounttype_fromaccount };

            var accounttype = new AccountType { Name = "Livsmedel", Type = Enum.TransactionType.expense, TypeId = 1 };
            var toaccount = new Account { AccountId = 1, Balance = -50, TypeId = 1, AccountType = accounttype };
            var transaction = new Transaction { Account= toaccount, AccountId = 1, Amount = 100, FromAccount = "Bankkonto", ToAccount = "Livsmedel", TransactionId = 0, TypeId = 1 };
            var expected = 900;
            //Act
            var result = _functions.SetPropertyValuesFromAccount(3,1000,3, transaction, fromaccount);
            //Assert
            Assert.Equal(expected, result.Balance);
        }
        [Fact]
        public void CheckIfBalanceIsCorrectToAccountTypeExpense()
        {
            //Arrange
            var accounttype = new AccountType { Name = "Livsmedel", Type = Enum.TransactionType.expense, TypeId = 1 };
            var toaccount = new Account { AccountId = 1, Balance = -50, TypeId = 1, AccountType = accounttype };
            var transaction = new Transaction { Account = toaccount, AccountId = 1, Amount = 100, FromAccount = "Bankkonto", ToAccount = "Livsmedel", TransactionId = 0, TypeId = 1 };
            var expected = -150;

            //Act
            var result = _functions.SetPropertyValuesToAccount(1, -50, 1, transaction, toaccount);
            //Assert
            Assert.Equal(expected, result.Balance);
        }

        [Fact]
        public void CheckIfBalanceIsCorrectFromAccountTypeExpense()
        {
            //Arrange
            var accounttype_fromaccount = new AccountType { Name = "Livsmedel", Type = Enum.TransactionType.expense, TypeId = 1 };
            var fromaccount = new Account { AccountId = 1, Balance = -50, TypeId = 1, AccountType = accounttype_fromaccount };

            var accounttype = new AccountType { Name = "Hyra", Type = Enum.TransactionType.expense, TypeId = 2 };
            var toaccount = new Account { AccountId = 2, Balance = 0, TypeId = 1, AccountType = accounttype };
            var transaction = new Transaction { Account = toaccount, AccountId = 2, Amount = 50, FromAccount = "Livsmedel", ToAccount = "Hyra", TransactionId = 0, TypeId = 2 };
            var expected = 0;

            //Act
            var result = _functions.SetPropertyValuesFromAccount(1, -50, 1, transaction, fromaccount);
            //Assert
            Assert.Equal(expected, result.Balance);
        }

        [Fact]
        public void CheckIfBalanceIsCorrectToAccountTypeExpenseFromAccountTypeExpense()
        {
            //Arrange
            var accounttype = new AccountType { Name = "Hyra", Type = Enum.TransactionType.expense, TypeId = 2 };
            var toaccount = new Account { AccountId = 2, Balance = 0, TypeId = 2, AccountType = accounttype };
            var transaction = new Transaction { Account = toaccount, AccountId = 2, Amount = 50, FromAccount = "Livsmedel", ToAccount = "Hyra", TransactionId = 0, TypeId = 2 };
            var expected = -50;

            //Act
            var result = _functions.SetPropertyValuesToAccount(2, 0, 2, transaction, toaccount);
            //Assert
            Assert.Equal(expected, result.Balance);
        }

        [Fact]
        public void CheckTypeNameIsShortenedIfStringIsLongerThanTwentyLetters()
        {
            //Arrange
            string inputvalue = "Livsmedel och luncher på stan";
            string expectedresult = "Livsmedel och lunche";

            //Act
            string result = _functions.GetTwentyLettersFromAccountTypeName(inputvalue);
            //Assert
            Assert.Equal(expectedresult, result);
        }
    }
}