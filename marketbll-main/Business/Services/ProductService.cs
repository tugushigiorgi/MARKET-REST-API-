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

public class ProductService :IProductService
{
    
    
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly AutoMapper.IMapper _mapper;
    
    
    public  ProductService( IUnitOfWork unitOfWork,AutoMapper.IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;

    }
    
    public async Task<IEnumerable<ProductModel>> GetAllAsync()
    {
        var data = await _unitOfWork.ProductRepository.GetAllWithDetailsAsync();
        var ProductModels = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductModel>>(data);
        return ProductModels;
    }

    public async Task<ProductModel> GetByIdAsync(int id)
    {
        var data = await _unitOfWork.ProductRepository.GetByIdWithDetailsAsync(id);
        
        var ProductModel = _mapper.Map< Product , ProductModel >(data);

        return ProductModel;
        



    }

    public async Task AddAsync(ProductModel model)
    {
        if(model==null || model.Price<=0 || String.IsNullOrEmpty( model.ProductName) )
        {
            throw new MarketException("excpetion");

        }
        var productItem = _mapper.Map<ProductModel,Product>(model);
       await  _unitOfWork.ProductRepository.AddAsync(productItem);
        await _unitOfWork.SaveAsync();

    }

    public Task UpdateAsync(ProductModel model)
    {
        if (model == null || string.IsNullOrEmpty(model.ProductName) || string.IsNullOrEmpty(model.CategoryName))
        {
            throw new MarketException("excpetion");
        }
        var ProductItem = _mapper.Map<ProductModel,Product>(model);
        
         _unitOfWork.ProductRepository.Update(ProductItem);
         return _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int modelId)
    {
        
        
       await  _unitOfWork.ProductRepository.DeleteByIdAsync(modelId);
      await _unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch)
    {
        
       
        var productList = await _unitOfWork.ProductRepository.GetAllWithDetailsAsync();
        var filteredList = productList.Where(d =>
            (!filterSearch.CategoryId.HasValue || d.ProductCategoryId == filterSearch.CategoryId.Value) &&
            (!filterSearch.MinPrice.HasValue || d.Price >= filterSearch.MinPrice.Value) &&
            (!filterSearch.MaxPrice.HasValue || d.Price <= filterSearch.MaxPrice.Value)
        ).ToList();
        
       
        
       
       
        
        return  _mapper.Map<IEnumerable<Product>, IEnumerable<ProductModel>>(filteredList);




    }

    public async Task<IEnumerable<ProductCategoryModel>> GetAllProductCategoriesAsync()
    {
        var list = await _unitOfWork.ProductCategoryRepository.GetAllAsync();
        return  _mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryModel>>(list);

        
    }

    public  async Task AddCategoryAsync(ProductCategoryModel categoryModel)
    {
        if (categoryModel == null || String.IsNullOrEmpty(categoryModel.CategoryName))
        {
            throw new MarketException("excpetion");
        }
        
        
        var mapped = _mapper.Map<ProductCategoryModel, ProductCategory>(categoryModel);
        await  _unitOfWork.ProductCategoryRepository.AddAsync(mapped);
        await  _unitOfWork.SaveAsync();

    }

    public Task UpdateCategoryAsync(ProductCategoryModel categoryModel)
    {

     
        
        if (categoryModel == null || string.IsNullOrEmpty(categoryModel.CategoryName) || categoryModel.Id == 0  
        )   {
            throw new MarketException("excpetion");
        }
        var mapped = _mapper.Map<ProductCategoryModel, ProductCategory>(categoryModel);
        _unitOfWork.ProductCategoryRepository.Update(mapped);
        return _unitOfWork.SaveAsync();
    }

    public Task RemoveCategoryAsync(int categoryId)
    {
        
        _unitOfWork.ProductCategoryRepository.DeleteByIdAsync(categoryId);
        return _unitOfWork.SaveAsync();
    }
}