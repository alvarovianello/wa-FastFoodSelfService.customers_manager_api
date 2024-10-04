using Application.DTOs;
using Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly RegisterCustomer _registerCustomer;
        private readonly UpdateCustomer _updateCustomer;
        private readonly GetCustomerByCpf _getCustomerByCpf;
        private readonly GetCustomerById _getCustomerById;
        private readonly GetAllCustomers _getAllCustomers;
        private readonly RemoveCustomer _removeCustomer;

        public CustomerController(
            RegisterCustomer registerCustomer,
            UpdateCustomer updateCustomer,
            GetCustomerByCpf getCustomerByCpf,
            GetCustomerById getCustomerById,
            GetAllCustomers getAllCustomers,
            RemoveCustomer removeCustomer)
        {
            _registerCustomer = registerCustomer;
            _updateCustomer = updateCustomer;
            _getCustomerByCpf = getCustomerByCpf;
            _getCustomerById = getCustomerById;
            _getAllCustomers = getAllCustomers;
            _removeCustomer = removeCustomer;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCustomer([FromBody] CustomerDto customerDto)
        {
            try
            {
                await _registerCustomer.ExecuteAsync(customerDto);
                return CreatedAtAction(nameof(GetCustomerByCpf), new { cpf = customerDto.Cpf }, customerDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerDto customerDto)
        {
            if (id != customerDto.Id) return BadRequest("ID do cliente não corresponde.");

            try
            {
                await _updateCustomer.ExecuteAsync(customerDto);
                return Ok(customerDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("cpf/{cpf}")]
        public async Task<IActionResult> GetCustomerByCpf(string cpf)
        {
            var customer = await _getCustomerByCpf.ExecuteAsync(cpf);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var customer = await _getCustomerById.ExecuteAsync(id);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _getAllCustomers.ExecuteAsync();
            return Ok(customers);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveCustomer(int id)
        {
            try
            {
                await _removeCustomer.ExecuteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
