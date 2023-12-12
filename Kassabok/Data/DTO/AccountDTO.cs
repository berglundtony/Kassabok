using Kassabok.Entity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Kassabok.Data.DTO
{
    public class AccountDTO
    {
        public int Balance { get; set; }
    }
    public class AccountInsertDTO
    {
        public string? AccountTypeName { get; set; }
        public string? Type { get; set; }
        public int Balance { get; set; }
    }
    public class AccountRequestDTO
    {
        public string? AccountName { get; set; }
        public string? Balance { get; set; }
    }
    public class AccountTypeDTO
    {
        public int TypeId { get; set; }
        public string? Name { get; set; }
        public string Type { get; set; }
    }
    public class AccountTypeRequestDTO
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
    }
    public class TransactionDTO
    {
        public string? FromAccount { get; set; }
        public string? ToAccount { get; set; }
        public int Amount { get; set; }
    }
}
