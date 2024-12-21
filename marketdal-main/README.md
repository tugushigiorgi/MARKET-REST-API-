# Trade Market - DAL


## Domain description

Supermarkets sell goods of various categories. The customers can shop anonymously or by logging in. When buying, a receipt is created with a list of goods purchased in a particular market.


## Task

(DAL) for an electronic system **"Trade Market"** with Three-Layer Architecture in dynamic library form named “Data”.

- The folder **Entities** contains classes of entities 
- The folder **Interfaces** contains repository interfaces of entities and the interface of their merge point.
- The folder **Repositories** contains repository classes that implement interfaces from the folder **Interfaces** 
- The root folder of the project contains **MarketDBContext.cs** file for project entity context 
- The root folder of the project contains **UnitOfWork.cs** file. This class is entry point for all repositories to get access to DAL from the business logic.  
- The folder **Migrations** contains project database migration files   

