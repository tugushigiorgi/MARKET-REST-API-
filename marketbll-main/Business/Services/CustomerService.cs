using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;

namespace Business.Services;

public class CustomerService : ICustomerService
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly AutoMapper.IMapper _mapper;
    
    
    public CustomerService( IUnitOfWork unitOfWork,AutoMapper.IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;

    }
    
    public async Task<IEnumerable<CustomerModel>> GetAllAsync()
    {
        var data = await _unitOfWork.CustomerRepository.GetAllWithDetailsAsync();
 
        var customerModels = _mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerModel>>(data);
        return customerModels;
       
    }

    public async Task<CustomerModel> GetByIdAsync(int id)
    {
          var data = await _unitOfWork.CustomerRepository.GetByIdWithDetailsAsync(id);
          var customerModel = _mapper.Map<Customer, CustomerModel>(data);
          return customerModel;
          
          
    }

    public async Task AddAsync(CustomerModel model)
    {
        
        var lowerBound= new DateTime(1000, 1, 1, 0, 0, 0, DateTimeKind.Local);
        var upperBound = new DateTime(2099, 10, 10, 0, 0, 0, DateTimeKind.Local);
        
        if (  model==null || string.IsNullOrEmpty(model.Name) || model.BirthDate<=lowerBound || model.BirthDate>=upperBound )
        {
            throw new MarketException("dawd");
        }

        var Customer = _mapper.Map<CustomerModel, Customer>(model);
        await _unitOfWork.CustomerRepository.AddAsync(Customer);
       await  _unitOfWork.SaveAsync();


    }

    public    Task UpdateAsync(CustomerModel model)
    {
        
        var lowerBound= new DateTime(1330, 1, 1, 0, 0, 0, DateTimeKind.Local);
        var upperBound = new DateTime(2050, 1, 1, 0, 0, 0, DateTimeKind.Local);
        if (  model==null || string.IsNullOrEmpty(model.Name) ||  String.IsNullOrEmpty(model.Surname) ||  model.BirthDate<=lowerBound || model.BirthDate>=upperBound )
        {
            throw new MarketException("dawd");
        }
        var Customer = _mapper.Map<CustomerModel, Customer>(model);
        _unitOfWork.CustomerRepository.Update(Customer);
      return       _unitOfWork.SaveAsync();
      
        
    }

    public    Task  DeleteAsync(int modelId)
    {
        _unitOfWork.CustomerRepository.DeleteByIdAsync(modelId);
        return       _unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId)
    {
        var customers = await _unitOfWork.CustomerRepository.GetAllWithDetailsAsync();
       var filtered=   customers.Where(c => c.Receipts.Any(r => r.ReceiptDetails.Any(rd => rd.ProductId == productId))).ToList();
       return  _mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerModel>>( filtered);
 
            

    }
}