using AutoMapper;
using Business.Models;

using System.Linq;
using Data.Entities;

namespace Business
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Receipt, ReceiptModel>()
                .ForMember(rm => rm.ReceiptDetailsIds, r => r
                    .MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap();
                 CreateMap<Product, ProductModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.ReceiptDetailIds, opt => opt.MapFrom(src => src.ReceiptDetails.Select(rd => rd.Id)));
                 CreateMap<ProductModel, Product>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => new ProductCategory { CategoryName = src.CategoryName })) // Map Category from CategoryName
                .ForMember(dest => dest.ReceiptDetails, opt => opt.Ignore()); // Ignore ReceiptDetails
             CreateMap<ReceiptDetail, ReceiptDetailModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DiscountUnitPrice, opt => opt.MapFrom(src => src.DiscountUnitPrice))
                .ForMember(dest => dest.ReceiptId, opt => opt.MapFrom(src => src.ReceiptId))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
             CreateMap<ReceiptDetailModel, ReceiptDetail>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ReceiptId, opt => opt.MapFrom(src => src.ReceiptId))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.DiscountUnitPrice, opt => opt.MapFrom(src => src.DiscountUnitPrice))
                .ForMember(dest => dest.Receipt, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore()).ReverseMap();
          CreateMap<Customer, CustomerModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Person.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Person.Surname))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.Person.BirthDate))
                .ForMember(dest => dest.DiscountValue, opt => opt.MapFrom(src => src.DiscountValue))
                .ForMember(dest => dest.ReceiptsIds, opt => opt.MapFrom(src => src.Receipts.Select(r => r.Id)));
         CreateMap<CustomerModel, Customer>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Person, opt => opt.Ignore()) // Ignore Person mapping for now
                .ForMember(dest => dest.DiscountValue, opt => opt.MapFrom(src => src.DiscountValue))
                .ForMember(dest => dest.Receipts, opt => opt.Ignore()); // Ignore Receipts mapping for now
         CreateMap<CustomerModel, Person>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignore Id mapping for Person
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate));
            CreateMap<ProductCategory, ProductCategoryModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
                .ForMember(dest => dest.ProductIds, opt => opt.MapFrom(src => src.Products.Select(p => p.Id)));
            CreateMap<ProductCategoryModel, ProductCategory>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
                .ForMember(dest => dest.Products, opt => opt.Ignore()); 
        }
    }
}