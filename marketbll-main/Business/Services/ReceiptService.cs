using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class ReceiptService :IReceiptService
{
    
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly AutoMapper.IMapper _mapper;
    
    
    public  ReceiptService( IUnitOfWork unitOfWork,AutoMapper.IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;

    }
    
    
    public async Task<IEnumerable<ReceiptModel>> GetAllAsync()
    {
        var data = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();

        return  _mapper.Map<IEnumerable<Receipt>, IEnumerable<ReceiptModel>>(data);
        

    }

    public async Task<ReceiptModel> GetByIdAsync(int id)
    {
        var receiptItem=await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(id);

        return _mapper.Map<Receipt,ReceiptModel>(receiptItem);
    }

    public async Task AddAsync(ReceiptModel model)
    {
        var check = await GetByIdAsync(model.Id);

        
        
        
        if (check != null)
        {   throw new MarketException("dw");
           
        }
        var receipt=_mapper.Map<ReceiptModel,Receipt>(model);
        await _unitOfWork.ReceiptRepository.AddAsync(receipt);
        await  _unitOfWork.SaveAsync();

      


    }

    public Task UpdateAsync(ReceiptModel model)
    {
        var receipt=_mapper.Map<ReceiptModel,Receipt>(model);
        
        _unitOfWork.ReceiptRepository.Update(receipt);
       return  _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int modelId)
    {
   var getReceipt =await  _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(modelId);
   foreach (var receiptDetail in getReceipt.ReceiptDetails) {
             _unitOfWork.ReceiptDetailRepository.Delete(receiptDetail );
   }
   await _unitOfWork.ReceiptRepository.DeleteByIdAsync(modelId);
   await _unitOfWork.SaveAsync(); 
    }

    public async Task AddProductAsync(int productId, int receiptId, int quantity)
    {
        if (quantity < 0)
        {
            throw new MarketException("Invalid quantity");
        }
        var getReceipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
        if (getReceipt == null)
        {
            throw new MarketException($"Receipt not found with ID: {receiptId}");
        }
        var existingReceiptDetail = getReceipt.ReceiptDetails.FirstOrDefault(d => d.ProductId == productId);
        if (existingReceiptDetail == null)
        {
            var getProduct = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
            if (getProduct == null) throw new MarketException("product not found");
            
            
            var newReceiptDetail = new ReceiptDetail
            {
                ReceiptId = receiptId,
                ProductId = productId,
                Product = getProduct,
                Receipt = getReceipt,
                Quantity = quantity,
                UnitPrice = getProduct.Price,
                DiscountUnitPrice = CalculateDiscountedPrice(getProduct.Price, getReceipt.Customer.DiscountValue)
            };

            await _unitOfWork.ReceiptDetailRepository.AddAsync(newReceiptDetail);
        }
        else
        {
            
            int count = existingReceiptDetail.Quantity;
            existingReceiptDetail.Quantity =  count+ quantity;
            _unitOfWork.ReceiptDetailRepository.Update(existingReceiptDetail);
        }
        
     
        await _unitOfWork.SaveAsync();

    }

      
      
   

    private static decimal CalculateDiscountedPrice(decimal originalPrice, int discountValue)
    {
        // Calculate the discounted price based on the discount value
        decimal discountPercentage = (decimal)discountValue / 100;
        decimal discountedPrice = originalPrice - (originalPrice * discountPercentage);
        return discountedPrice;
    }
   
    public async Task RemoveProductAsync(int productId, int receiptId, int quantity)
    {
        var getReceipt =await  _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);

     var product=   getReceipt.ReceiptDetails.SingleOrDefault(d => d.ProductId == productId);

    var currentQuantity= product!.Quantity;

    
    
    product.Quantity = currentQuantity - quantity;
    if (product.Quantity == 0)
    {
        _unitOfWork.ReceiptDetailRepository.Delete(product);
        
    }
    
    await _unitOfWork.SaveAsync();


    }

    public async Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(int receiptId)
    {
        var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);

        var details = receipt.ReceiptDetails.ToList();
        var detailModels = _mapper.Map<IEnumerable<ReceiptDetail>, IEnumerable<ReceiptDetailModel>>(details);
    
        return detailModels;
    }

    public async Task<decimal> ToPayAsync(int receiptId)
    {
        var receipt=await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
        return receipt.ReceiptDetails.Sum(c => c.Quantity * c.DiscountUnitPrice);
    }

    public async Task CheckOutAsync(int receiptId)
    {
        if(receiptId==0)  throw new MarketException("w");
        
        var receipt=await _unitOfWork.ReceiptRepository.GetByIdAsync(receiptId);

        if (receipt == null)
        {
            throw new MarketException("w");
        }
        
        receipt.OperationDate = DateTime.Now;
         
        receipt.IsCheckedOut = true ;
        await _unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<ReceiptModel>> GetReceiptsByPeriodAsync(DateTime startDate, DateTime endDate)
    {
       
        
        var receipts=await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
        var filterData = receipts.Where(c => c.OperationDate >= startDate && c.OperationDate <= endDate).ToList();
        return _mapper.Map<IEnumerable<Receipt>, IEnumerable<ReceiptModel>>(filterData);
 
    }
}