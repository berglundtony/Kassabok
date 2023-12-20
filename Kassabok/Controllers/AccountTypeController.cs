using Kassabok.Entity;
using Kassabok.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Kassabok.Enum;
using Kassabok.Functions.Interface;
using Kassabok.Data.DTO;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Kassabok.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountTypeController : ControllerBase
    {
        private readonly IRepository<AccountType> _accountTypeRepository;
        private readonly IEntityFunctions _entityFunctions;
        private readonly IFunctions _functions;

        public AccountTypeController(IRepository<AccountType> accountTypeRepository, IEntityFunctions entityFunctions, IFunctions functions)
        {
            _accountTypeRepository = accountTypeRepository;
            _entityFunctions = entityFunctions;
            _functions = functions;
        }


        // GET: api/<AccountController2>
        [HttpGet]
        public async Task<ActionResult<IEnumerable>> GetAccountTypes()
        {
            var accounttypes = _accountTypeRepository.GetAll().Result.OrderBy(n => n.Name);
            var output = new List<string[]>();

            foreach (var item in accounttypes)
            {
                output.Add(new string[] { item.Name, item.Type.ToString() });
            }
            return Ok(output);
        }

        // GET api/<AccountTypeController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var accounttype = await _accountTypeRepository.GetByIdAsync(id);
            if(accounttype == null)
            {
                return NotFound();
            }
            return Ok(accounttype);
        }

        // POST api/<AccountTypeController>
        [HttpPost]
        public async Task<ActionResult<AccountType>> CreateAccountType(string? name, string? transactionType)
        {
            string? shortendstring = name?.Length > 20 ? _functions.GetTwentyLettersFromAccountTypeName(name) : name;

            AccountType createdAccountTypeResponse;
            var accountTypeEntity = new AccountType()
            {
                Name = shortendstring,
                Type = _functions.GetValue(transactionType),
            };
            if (accountTypeEntity == null)
                return BadRequest("Invalid transaction data");
           
            if (_entityFunctions.CheckDistincedTypeName(shortendstring))
                createdAccountTypeResponse = await _accountTypeRepository.AddAccountAsync(accountTypeEntity);
            else
                return BadRequest("The accountName already exists.");

            return CreatedAtAction(nameof(GetById), new { id = createdAccountTypeResponse.TypeId }, createdAccountTypeResponse);
        }

        // PUT api/<AccountController2>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<AccountType>>Put(int id, [FromBody] AccountTypeRequestDTO? accountType)
        {
            var accountTypeEntity = await _accountTypeRepository.GetByIdAsync(id);
            if(accountTypeEntity == null)
            {
                return NotFound();
            }

            accountTypeEntity.Name = accountType.Name?.Length > 20 ? _functions.GetTwentyLettersFromAccountTypeName(accountType?.Name): accountType?.Name;
            accountTypeEntity.Type = _functions.GetValue(accountType.Type);
            await _accountTypeRepository.UpdateAsync(accountTypeEntity);
            return CreatedAtAction(nameof(GetById), new { id = accountTypeEntity.TypeId, type= accountType.Type }, accountTypeEntity);
        }

        // DELETE api/<AccountController2>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var accountTypeEntity = await _accountTypeRepository.GetByIdAsync(id);
            if(accountTypeEntity == null)
            {
                return NotFound();
            }
            await _accountTypeRepository.DeleteAsync(accountTypeEntity);
            return CreatedAtAction(nameof(GetById), new { id = accountTypeEntity.TypeId }, accountTypeEntity);
        }

    }
}
