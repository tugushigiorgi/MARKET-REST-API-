using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AutoMapper.IMapper _mapper;

        public StatisticService(IUnitOfWork unitOfWork, AutoMapper.IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<IEnumerable<ProductModel>> GetMostPopularProductsAsync(int productCount)
        {

            var receiptDetails = await _unitOfWork.ReceiptDetailRepository.GetAllWithDetailsAsync();

            var popularProductIds = receiptDetails
                .GroupBy(rd => rd.Product )
                .Select(group => new
                {
                    ProductId = group.Key,
                    Quantity = group.Sum(d => d.Quantity)
                })
                .OrderByDescending(group => group.Quantity)
                .Select(group => group.ProductId)
                .Take(productCount)
                .ToList();
 

          return  _mapper.Map<IEnumerable<Product>, IEnumerable<ProductModel>>(popularProductIds);
           
        }
        
        
        
        

        public async Task<IEnumerable<ProductModel>> GetCustomersMostPopularProductsAsync(int productCount, int customerId)
        {
            var Details = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            var filtered = Details.Where(d => d.CustomerId == customerId).ToList();

       var data=     filtered.SelectMany(d => d.ReceiptDetails)
                .GroupBy(d => d.Product)
                .Select(c => new
                {
                    quantity = c.Sum(d => d.Quantity),
                    product = c.Key

                }).OrderByDescending(d => d.quantity)
                .Select(d => d.product)
                .Take(productCount).ToList();
       return  _mapper.Map<IEnumerable<Product>, IEnumerable<ProductModel>>(data);
        }

        
        
        public async Task<IEnumerable<CustomerActivityModel>> GetMostValuableCustomersAsync(int customerCount,
            DateTime startDate, DateTime endDate)
        {

 
      
         var customerData = await _unitOfWork.CustomerRepository.GetAllWithDetailsAsync();
       
         
         if (customerData != null)
         {
             return  customerData.SelectMany(d=>d.Receipts)   
             
             .Where(detail => detail.OperationDate >= startDate && detail.OperationDate <= endDate)
                    
             .GroupBy(detail => detail.Customer)
                    
             .Select(group => new
             {
                 CustomerId = group.Key.Id ,
                 Customername = group.Key.Person.Name+" "+group.Key.Person.Surname,
                 CustomerI = group.Select(d=>d.Customer),
                 ReceiptSum = group.SelectMany(d=>d.ReceiptDetails).Sum(d=>d.Quantity*d.DiscountUnitPrice)
             })
                    
             .OrderByDescending(d=>d.ReceiptSum)
             .Select(a=>new CustomerActivityModel
             {
                 CustomerId = a.CustomerId,
                 ReceiptSum = a.ReceiptSum,
                 CustomerName =  a. Customername    
             
             })
             .Take(customerCount)
             .ToList();
             
         }

         var receiptDetails = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
         
         
         
             return receiptDetails
                 .Where(detail => detail.OperationDate >= startDate && detail.OperationDate <= endDate)
         
                 .GroupBy(detail => detail.Customer)
                 .Select(group => new
                 {
                     CustomerId = group.Key.Id,
                     Customername = group.Key.Person.Name + " " + group.Key.Person.Surname,
                     CustomerI = group.Select(d => d.Customer),
                     ReceiptSum = group.SelectMany(d => d.ReceiptDetails).Sum(d => d.Quantity * d.DiscountUnitPrice)
                 })
                 .OrderByDescending(d => d.ReceiptSum)
                 .Select(a => new CustomerActivityModel
                 {
                     CustomerId = a.CustomerId,
                     ReceiptSum = a.ReceiptSum,
                     CustomerName = a.Customername
         
                 })
                 .Take(customerCount)
                 .ToList();
         
         
         
         
        
        
        
        
    }

        public async Task<decimal> GetIncomeOfCategoryInPeriod(int categoryId, DateTime startDate, DateTime endDate)
        { 
            
            var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
          var filtered = receipts
             .Where(detail => detail.OperationDate >= startDate && detail.OperationDate <= endDate)
             .SelectMany(product => product.ReceiptDetails)
             .Where(d => d.Product.ProductCategoryId == categoryId)
             .Sum(detail => detail.Quantity * detail.DiscountUnitPrice);
       
          return filtered;
         
 }

    }

}
 
