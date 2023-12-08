using System.ComponentModel.DataAnnotations;

namespace Kassabok.Entity
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }
        public int Balance { get; set; } = 0;
        public List<Transaction> Transactions { get; set; }

        // ForignKey
        public int TypeId { get; set; }

        // Navigation property
        public AccountType AccountType { get; set; }
    }
}
