# Trade Market - BLL


## Domain description

Supermarkets sell goods of various categories. The customers can shop anonymously or by logging in. When buying, a receipt is created with a list of goods purchased in a particular market.



- The folder **Models** contains classes of logic’s models
- The folder **Interfaces** contains BLL service interfaces.
- The folder **Services** contains service classes that implement interfaces from the folder **Interfaces** –
- The root folder of the project contains **AutomapperProfile.cs** file to display DAL entities in the BLL model. And opposite 
```
Product <-> ProductModel
Customer <-> CustomerModel
Receipt <-> ReceiptModel
ReceiptDetail <-> ReceiptDetailModel,
ProductCategory <-> ProductCategoryModel
```
 
in the AutomapperProfile class.
- The folder **Validation** which contains **MarketException.cs** file – make the class of user exception **MarketException**.


