using P03_SalesDatabase.Data;

var context = new SalesContext();
context.Database.EnsureCreated();