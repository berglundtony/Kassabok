using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kassabok.Data;
using Kassabok.Entity;
using Kassabok.Data.DTO;
using Kassabok.Enum;
using Kassabok.Functions.Interface;

namespace Kassabok.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly TransactionsDbContext _context;
        private readonly IEntityFunctions _entityFunctions;
        private readonly IFunctions _functions;

        public AccountController(TransactionsDbContext context, IEntityFunctions entityFunctions, IFunctions functions)
        {
            _context = context;
            _entityFunctions = entityFunctions;
            _functions = functions;
        }

        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountRequestDTO>>> GetAccounts()
        {
          var accountList = new List<AccountRequestDTO>();
          if (_context.Accounts == null)
          {
              return NotFound();
          }

            var result = await _context.AccountTypes
            .Include(a => a.Account)
            .Select(types => new 
            {
                Name = types.Name,
                Balance = types.Account.Balance.ToString(),
                Type = types.Type
            })
            .OrderBy(t => t.Name)
            .ToListAsync();

            foreach (var account in result)
            {
                if (account.Name != null && account.Balance != null)
                {
                    accountList.Add(new AccountRequestDTO { AccountName = account.Name, Balance = account.Balance.ToString() }); 
                }        
            }
            return Ok(accountList);
         }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            if (_context.Accounts == null)
            {
              return NotFound();
            }
            var account = await _context.Accounts
           .Include(a => a.AccountType)
           .Select(types => new Account
           {
               AccountId = types.AccountId,
               Balance = types.Balance,
               AccountType = types.AccountType,
           }).FirstOrDefaultAsync();

            if (account == null)
            {
                return NotFound();
            }
           return account;
        }

        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(int id, [FromBody] AccountDTO accountDTO)
        {
            var accountEntity = await _context.Accounts.FindAsync(id);
            accountEntity.Balance = accountDTO.Balance;
            _context.Entry(accountEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                var account = await GetAccount(accountEntity.AccountId);
                accountEntity.AccountType = account.Value.AccountType;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

           return CreatedAtAction("GetAccount", new { id = accountEntity.AccountId }, accountEntity);
        }

        // POST: api/Accounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount(AccountInsertDTO accountInsertDTO)
        {

            var newAccount = new Account
            {
                Balance = accountInsertDTO.Balance,
                AccountType = new AccountType
                {
                    Name = accountInsertDTO.AccountTypeName,
                    Type = _functions.GetValue(accountInsertDTO.Type)
                }
            };
            if (newAccount.AccountType.Type == null)
                return BadRequest("The accountType in not correct.");
            if (newAccount.AccountType.Name?.Length > 20)
                newAccount.AccountType.Name = _functions.GetTwentyLettersFromAccountTypeName(newAccount.AccountType.Name);
            if (_entityFunctions.CheckDistincedTypeName(newAccount.AccountType.Name))
                _context.Add(newAccount);
            else if (newAccount == null)
                return BadRequest("Invalid account data");
            else
                return BadRequest("The accountName already exists.");

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccount", new { id = newAccount.AccountId, Type = newAccount.Balance }, newAccount);
        }

        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            if (_context.Accounts == null)
            {
                return NotFound();
            }
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountExists(int id)
        {
            return (_context.Accounts?.Any(e => e.AccountId == id)).GetValueOrDefault();
        }
    }
}
