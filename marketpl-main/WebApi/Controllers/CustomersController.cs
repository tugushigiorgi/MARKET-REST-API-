using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MarketException = Business.Validation.MarketException;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        //Inject customer service via constructor

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        
        
        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerModel>>> Get()
        {
            var result = await _customerService.GetAllAsync();

            if (result.Any())
            {
                return     Ok( result);  
                
            }
            return NotFound();
        }

        //GET: api/customers/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CustomerModel>> GetById(int id)
        {
           
            
            var result = await _customerService.GetByIdAsync(id);
             
            if (result!=null)
            {
                return Ok(result);
            }
            return NotFound(404);
        }
        
       
        [HttpGet("products/{id:int}")]
        public async Task<ActionResult<CustomerModel>> GetByProductId(int id)
        {

            var result = await _customerService.GetCustomersByProductIdAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            
            return NotFound();
        }

     
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CustomerModel value)
        {

            try
            {


                if (!ModelState.IsValid) throw new MarketException("a");

                if (value==null)
                {
                    return BadRequest(400);
                }
                await _customerService.AddAsync(value);
                
                return Ok( value);
 

            }
            catch (MarketException e)
            {
                Console.WriteLine(e);
                return BadRequest(400);
            }
         
             
        }

       
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int Id, [FromBody] CustomerModel value)
        {
            try
            {
              
                await _customerService.UpdateAsync( value);
               return Ok( value  );
            }
            catch (MarketException e)
            {         Console.WriteLine(e);
                return BadRequest(400);
            }

         
            
        }

     
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            
            try
            {
                await _customerService.DeleteAsync(id);
                return Ok( );
            }
            catch (MarketException e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
             
        }
    }
}
