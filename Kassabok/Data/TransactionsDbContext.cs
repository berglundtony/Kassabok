using Kassabok.Entity;
using Microsoft.EntityFrameworkCore;
using Kassabok.Enum;

namespace Kassabok.Data
{
    public class TransactionsDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public TransactionsDbContext(DbContextOptions<TransactionsDbContext> options) : base(options)
        {

        }
     
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entitiesToModify = ChangeTracker.Entries<AccountType>()
                .Where(entry =>  entry.State == EntityState.Added)
                .ToList();

            foreach (var entry in entitiesToModify)
            {
                if (entry.State == EntityState.Added)
                {
                    // Access the entity being saved
                    var entity = entry.Entity;

                    // Check the property value to decide whether to apply custom logic
                 
                        var addedAccountTypes = ChangeTracker.Entries<AccountType>()
                        .Where(e => e.State == EntityState.Added)
                        .Select(e => e.Entity)
                        .First();

                        //return await base.SaveChangesAsync();
                        // Now that AccountTypes are saved, you can safely create associated Accounts

                        AccountTypes.Add(addedAccountTypes);
                        await base.SaveChangesAsync();

                        var existingAccountType = AccountTypes.Find(addedAccountTypes.TypeId);
                        var newAccount = new Account
                        {
                            TypeId = existingAccountType.TypeId,
                            Balance = 0
                        };

                        Accounts.Add(newAccount);

                        // For example, modify another property based on the condition
                        //entity.AnotherProperty = "ModifiedValue"
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountId)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Account>()
               .HasOne(a => a.AccountType)
               .WithOne(a => a.Account)
               .HasForeignKey<Account>(a => a.TypeId)
               .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<AccountType>()
                .Property(e => e.Type)
                .HasConversion<string>();

            modelBuilder.Entity<AccountType>()
            .HasOne(t => t.Account)
            .WithOne(a => a.AccountType)
            .HasForeignKey<Account>(a => a.TypeId)
            .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
