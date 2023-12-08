using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kassabok.Entity;
using Kassabok.Enum;

namespace Kassabok.Entity
{
    public class AccountType
    {
        [Key]
        public int TypeId { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string? Name { get; set; }
        public TransactionType? Type { get; set; }

        // Navigation Proroperty
        public Account Account { get; set; }

    }
}
