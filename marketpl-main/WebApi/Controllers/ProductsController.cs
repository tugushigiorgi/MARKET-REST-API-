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
public class ProductsController: ControllerBase
{
     private readonly IProductService _productService;
     public ProductsController( IProductService productService)
    {
        _productService = productService;
    }
 

    [HttpGet("{id:int}")]
    public async Task<ActionResult<IEnumerable<ProductModel>>> GetByIdAsync([FromRoute] int id)
    {

        try
        {
            var result = await _productService.GetByIdAsync(id);

            if (result!=null)
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
    [HttpGet()]
    public async Task<ActionResult<IEnumerable<ProductModel>>> GetAsync(
        
        [FromQuery] FilterSearchModel searchModel
    
    )
    {
        try
        {
            
            if (searchModel.MaxPrice == 0 && searchModel.MinPrice == 0 && searchModel.CategoryId == 0)
            {
                var result  = await _productService.GetAllAsync();
                if (result .Any())
                {
                    return Ok(result );
                }
            }
             var result2 = await _productService.GetByFilterAsync(searchModel);

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
  
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductModel value)
    {

        try
        {
            if (!ModelState.IsValid) throw new MarketException("a");
            if (value==null)
            {
                return BadRequest(400);
            }
            await _productService.AddAsync(value);
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
                await _productService.DeleteAsync(id);
                return Ok( );
            }
            catch (MarketException e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
             
        }
        [HttpGet("categories")]
        public async Task<ActionResult<CustomerModel>> GetAllCategories(int id)
        {

            var result = await _productService.GetAllProductCategoriesAsync();
            if (result != null)
            {
                return Ok(result);
            }
            
            return NotFound();
        }
        [HttpPost("categories")]
        public async Task<ActionResult> PostCategory([FromBody] ProductCategoryModel  value)
        {
            try
            {


                if (!ModelState.IsValid) throw new MarketException("a");

                if (value == null)
                {
                    return BadRequest(400);
                }

                await _productService.AddCategoryAsync(value);

                return Ok(value);


            }
            catch (MarketException e)
            {
                Console.WriteLine(e);
                return BadRequest(400);
            }
        }
        [HttpDelete("categories/{id:int}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            
            try
            {
                await _productService.RemoveCategoryAsync(id);
                return Ok( );
            }
            catch (MarketException e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
             
        }
        [HttpPut("categories/{id:int}")]
        public async Task<ActionResult> Put(int Id, [FromBody] ProductCategoryModel value)
        {
            try
            {

                await _productService.UpdateCategoryAsync(value);
                return Ok( value  );
            }
            catch (MarketException e)
            {
                Console.WriteLine(e);
                return BadRequest(400);
            }
        }
        
        [HttpPut("{id:int}")] 
        public async Task<ActionResult> Put(int id, [FromBody] ProductModel value)
        {
            try
            {

                await _productService.UpdateAsync(value);
                return Ok(value);
            }
            catch (MarketException e)
            {
                Console.WriteLine(e);
                return BadRequest(400);
            }
        }
        
       
}