# NPrismy - A Lightweight .NET Core ORM Framework

** *This repository is under construction. This is a draft Readme.* **

NPrismy is lightweight ORM for ASP.NET Core Web Applications. 

- [ ] Add ChangeTracker.
- [ ] Add MySql support
- [ ] Add Oracle Db support

## Table Of Contents
1. [Features](#features)
2. [Installation](#installation)
3. [Usage](#usage)
4. [Examples](#examples)
5. [Benchmark](#benchmark)

## Features
* Simple CRUD operations with sugar syntax.
* Using database transactions
* Customized entity mappings
* Managing persistance concerns with a high level language

## Installation
Install NPrismy NuGet package by executing following command on .NET Core CLI:

`dotnet add package NPrismy`

and

`PM> Install-Package NPrismy` on NuGet Package Manager Console.

## Usage

Step by step initialization.

### 1) Create a Database class

Create a database class that inherited from NPrismy.Database abstract class. (i.e. CarFactoryDatabase) Database classes are represents the remote database. (Like EF DbContext)

```
public class WeatherForecastDatabase : NPrismy.Database
{   
}   
```

### 2) Add NPrismy support

Insert the following code to your Startup.cs ConfigureServices() method to register your database object to ServiceCollection. 
```
services.AddNPrismy<WeatherForecastDatabase>()
        .UseProvider(PersistanceProvider.SqlServer)
        .ConnectionString("#your-connection-string#");
```

1. Specify the connection string by passing it to `ConnectionString()` method.  
2. Specify the your persistance provider (SqlServer, Oracle DB, MySql) by calling `UseProvider()` method. (ONLY MICROSOFT SQL SERVER SUPPORTED AT THE MOMENT)

### 3) Specify your tables

Add `EntityTable<T>`properties to your database class to specify your database tables.

```
 public class WeatherForecastDatabase : NPrismy.Database
 {   
    public EntityTable<City> Cities { get; set; }
    public EntityTable<Forecast> Forecasts { get; set; }
 }
```

### 4) Specify table names and schemas (optional)

As default, NPrismy pluralizes the property types of database class. And assumes they are in the `dbo` schema. In the above example, NPrismy assumes there are two different tables named `dbo.Cities` and `dbo.Forecasts`. Regardless of the names of properties, NPrismy pluralizes the types `City` and `Foreacast`.

If you override these definitions, simply put `[TableName]` and `[Schema]` attributes as following.

```
public class WeatherForecastDatabase : NPrismy.Database
{  
    [TableName("MyCities")]
    [Schema("weatherForecast")]
    public EntityTable<City> Cities { get; set; }
       
    [TableName("AwesomeForecasts")]
    [Schema("weatherForecast")]
    public EntityTable<Forecast> Forecasts { get; set; }

}   
```
### 5) Do your CRUD!

Modify your controllers as accepts `WeatherForecastDatabase` (how you name it) and you're done. Querying data can be performed as following:

`var lovelyCities = _db.Cities.Query(c => c.Name == "Istanbul" || c.Name == "Copenhagen");`

## Examples

### Query Data


## Benchmark
