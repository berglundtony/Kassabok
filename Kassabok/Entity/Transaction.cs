using System.ComponentModel.DataAnnotations;

namespace Kassabok.Entity
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public int Amount { get; set; }

        //ForignKey
        public int? AccountId { get; set; }
        public int? TypeId { get; set; }

        //Navigation property
        public Account Account { get; set; }
    }
}
