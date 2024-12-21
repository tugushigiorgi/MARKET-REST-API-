using System;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[Route("api/statistics")]
[ApiController]
public class StatisticController :ControllerBase 
{
    private readonly IStatisticService _statisticService;

    public StatisticController(IStatisticService service)
    {
        _statisticService = service;

    }
    
    [HttpGet("popularProducts")]
    public async Task<ActionResult<ProductModel>> CustomersPopularProducts([FromQuery]int productCount )
    {
        var result = await _statisticService.GetMostPopularProductsAsync(productCount);
        if (result .Any())
        {
            return Ok(result);
        }
        return NotFound();
    }    

    
    
    
    [HttpGet("customer/{id:int}/{productCount:int}")]
    public async Task<ActionResult<ProductModel>> productCount([FromRoute] int id,[FromRoute] int productCount )
    {
        var result = await _statisticService.GetCustomersMostPopularProductsAsync(id, productCount);
        if (result.Any())
        {
            return Ok(result);
        }
        return NotFound();
    }    
    
    
    [HttpGet("activity/{customerCount:int}")]
    public async Task<ActionResult<CustomerActivityModel>> MostActiveCustomer( [FromRoute] int customerCount,
        [FromQuery] DateTime  startDate, [FromQuery] DateTime  endDate
        )
    {
        try
        {
            var result = await _statisticService.GetMostValuableCustomersAsync(customerCount, startDate, endDate);
        
            if (result .Any())
            {
                return Ok(result);
            }
            return NotFound();
        }
        catch (MarketException e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
       
     
    }
    
    
      [HttpGet("income/{categoryId:int}")]
    public async Task<ActionResult<decimal>> CategoryIncomeIn([FromRoute]  int categoryId,
        [FromQuery] DateTime  startDate, [FromQuery] DateTime  endDate
        )
    {
       
        
        var result = await _statisticService.GetIncomeOfCategoryInPeriod(categoryId, startDate, endDate);
        
       
            return Ok(result);
        
            
            
            
            
       }     
            
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}