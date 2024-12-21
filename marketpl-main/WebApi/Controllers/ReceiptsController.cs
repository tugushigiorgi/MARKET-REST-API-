using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;


[Route("api/[controller]")]
[ApiController]
public class ReceiptsController :ControllerBase
{
    private readonly IReceiptService _receiptService;

    

    public ReceiptsController(IReceiptService receiptService)
    {
        _receiptService = receiptService;
    }
    
    
     
    [HttpGet ]
    public async Task<ActionResult<IEnumerable<ReceiptModel>>> GetAsync()
    {
        try
        { 
            var result2 = await _receiptService.GetAllAsync();
            if (result2.Any())
            {
                return Ok(result2);
            }
            return NotFound();  
        }
        catch (MarketException e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }
    
    
    
    
    [HttpGet("period") ]
    public async Task<ActionResult<IEnumerable<ReceiptModel>>> GetByPerdionAsync([FromQuery] DateTime  startDate, [FromQuery] DateTime  endDate)
    { try
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date cannot be after end date.");
            }
            var result2 = await _receiptService.GetReceiptsByPeriodAsync(
                
               startDate ,
                endDate);
            
            if (result2.Any())
            {
                return Ok(result2);
            }
            return NotFound();  
        }
        catch (MarketException e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int Id, [FromBody] ReceiptModel value)
    {
        try
        {
              
            await _receiptService.UpdateAsync( value);
            return Ok( value  );
        }
        catch (MarketException e)
        {
            Console.WriteLine(e);
            return BadRequest(400);
        }
      
    }
    
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReceiptModel>> GetByReceiptId(int id)
    {

        var result = await _receiptService.GetByIdAsync(id);
        if (result != null)
        {
            return Ok(result);
        }
            
        return NotFound();
    }
    [HttpGet("{id:int}/details")]
    public async Task<ActionResult<ReceiptDetailModel>> GetByReceiptIdWithDetails(int id)
    {

        var result = await _receiptService.GetReceiptDetailsAsync(id);
        if (result != null)
        {
            return Ok(result);
        }
            
        return NotFound();
    }
    [HttpGet("{id:int}/sum")]
    public async Task<ActionResult<ReceiptModel>> GetByReceiptIdWithSum(int id)
    {
        
        var receipt  = await _receiptService.GetByIdAsync(id); 
        if (receipt==null)
        {
            return NotFound();
        }
        var result = await _receiptService.ToPayAsync(id);
       
            return Ok(result);
        
            
        
    }
    [HttpPut("{id:int}/products/add/{productId}/{quantity}")]
    public async Task<ActionResult> AddProductToReceipt(int Id, int productId,int quantity)
    {
 
        try
        {

            await _receiptService.AddProductAsync(productId, Id, quantity);
            return Ok(    );
        }
        catch (MarketException e)
        {
            Console.WriteLine(e);
            return BadRequest(400);
        }
         
             
    }
    
    
    [HttpPut("{id}/products/remove/{productId}/{quantity}")]
    public async Task<ActionResult> RemoveProductFromReceipt(int Id, int productId,int quantity)
    {
            
        try
        {
          await    _receiptService.RemoveProductAsync(productId, Id, quantity);

          return Ok();
        }
        catch (MarketException e)
        {
            Console.WriteLine(e);
            return BadRequest();
        }
             
    }   
    
    [HttpPut("{id:int}/checkout")]
    public async Task<ActionResult> checkout(int Id )
    {
            
        try
        {
            await _receiptService.CheckOutAsync(Id);

          return Ok();
        }
        catch (MarketException e)
        {
            Console.WriteLine(e);
            return BadRequest();
        }
             
    }
    
    
    
    
    
    
    
    
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] ReceiptModel value)
    {

        try
        {


            if (!ModelState.IsValid) throw new MarketException("a");

            if (value==null)
            {
                return BadRequest(400);
            }
            await _receiptService.AddAsync(value);
                
            return Ok( value);
 

        }
        catch (MarketException e)
        {
            Console.WriteLine(e);
            return BadRequest(400);
        }
         
             
    }
    
     
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
            
        try
        {
            await _receiptService.DeleteAsync(id);
            return Ok(   );
        }
        catch (MarketException e)
        {
            Console.WriteLine(e);
            return BadRequest();
        }
             
    }
    
    
    
}
    
    
    
 