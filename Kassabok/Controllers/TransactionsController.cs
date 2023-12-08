using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kassabok.Data;
using Kassabok.Entity;
using System.Transactions;
using Kassabok.Data.DTO;
using Microsoft.EntityFrameworkCore.Storage;
using Kassabok.Functions.Interface;
using Kassabok.Enum;

namespace Kassabok.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionsDbContext _context;
        private readonly IFunctions _functions;
        private readonly IEntityFunctions _entityFunctions;

        public TransactionsController(TransactionsDbContext context, IFunctions functions, IEntityFunctions entityFunctions)
        {
            _context = context;
            _functions = functions;
            _entityFunctions = entityFunctions;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kassabok.Entity.Transaction>>> GetTransactions()
        {
          if (_context.Transactions == null)
          {
              return NotFound();
          }
            return await _context.Transactions.ToListAsync();
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Kassabok.Entity.Transaction>> GetTransaction(int id)
        {
          if (_context.Transactions == null)
          {
              return NotFound();
          }
            var transactionDTO = await _context.Transactions.FindAsync(id);

            if (transactionDTO == null)
            {
                return NotFound();
            }

            return transactionDTO;
        }

        // PUT: api/Transactions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(int id, TransactionDTO transactionDTO)
        {

            _context.Entry(transactionDTO).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Transactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TransactionDTO>> PostTransaction(TransactionDTO transactionDTO)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                int fromAccountTypeId, toAccountTypeId, balanceFromAccount, balanceToAccount, fromAccountId, toAccountId;

                _entityFunctions.GetIdAndBalanceValues(transactionDTO, out fromAccountTypeId, out toAccountTypeId,
                    out balanceFromAccount, out balanceToAccount,
                    out fromAccountId, out toAccountId);

                // It will just get transactions if Accounts exists
                if (fromAccountId == 0 || toAccountId == 0)
                {
                    return BadRequest("From account and/or to account doesnt exists");
                }
                // Get the current account,accountType and Transaction entity by the fromAccontId from the database.
                var account = _context.Accounts.Include(t => t.AccountType).FirstOrDefault(e => e.AccountId == fromAccountId);

                // Here sets the values for transaction
                var typeId = _entityFunctions.FindTypeIdByName(transactionDTO.ToAccount);
                Entity.Transaction transaction = _functions.SetValuesForTransaction(transactionDTO, toAccountId, typeId);
                _context.Transactions.Add(transaction);

                // Update account table with the reduced balance after transaction
                account = _functions.SetPropertyValuesFromAccount(fromAccountTypeId, balanceFromAccount, fromAccountId, transaction, account);
                if(account == null)
                {
                    return BadRequest($"No transaction will occur there is to less money in the fromAccount called '{transactionDTO.FromAccount}' ");
                }
                _context.Accounts.Update(account);

                // Get the current account,accountType and Transaction entity by the to accontId from the database.
                Account? accountEntity = _context.Accounts.Include(t => t.AccountType).FirstOrDefault(e => e.AccountId == toAccountId);
            
                // Update account table with the increased balance after transaction
                accountEntity = _functions.SetPropertyValuesToAccount(toAccountTypeId, balanceToAccount, toAccountId, transaction, accountEntity);
                _context.Accounts.Update(accountEntity);

                await _context.SaveChangesAsync();

                transactionScope.Complete();

                return Ok("transaction successful");
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            if (_context.Transactions == null)
            {
                return NotFound();
            }
            var transactionDTO = await _context.Transactions.FindAsync(id);
            if (transactionDTO == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transactionDTO);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionExists(int id)
        {
            return (_context.Transactions?.Any(e => e.TransactionId == id)).GetValueOrDefault();
        }
    }
}
