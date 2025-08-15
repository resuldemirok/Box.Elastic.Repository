# Box.Elastic.Repository

Box.Elastic.Repository is a .NET library that brings Entity Framework Core-like LINQ querying and Repository pattern support to Elasticsearch using the [NEST](https://www.nuget.org/packages/NEST) client.

With Box.Elastic.Repository, you can:
- Query Elasticsearch using LINQ syntax
- Use Generic Repository for CRUD operations
- Create Specific Repositories for custom queries
- Work with Elasticsearch in a strongly-typed, EF Core-style API

---

## Installation

You can install the package via NuGet:

```powershell
dotnet add package Box.Elastic.Repository
```

---

## Example Usage

Below is an example of how to use Box.Elastic.Repository to perform CRUD operations and LINQ queries with Elasticsearch:

```csharp
using System;
using System.Linq;
using Box.Elastic.Repository.Context;
using Box.Elastic.Repository.Repository;
using ElasticTestApp.Models;

class Program
{
    static void Main()
    {
        // 1️⃣ Elasticsearch connection
        var context = new ElasticDbContext("http://localhost:9200");

        // 2️⃣ Create generic repository
        var repo = new GenericRepository<Product>(context);

        // 3️⃣ Add product
        var product = new Product
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Gaming Mouse",
            Price = 650,
            Category = "Electronics"
        };
        repo.Add(product);
        Console.WriteLine("Product added!");

        // 4️⃣ LINQ sample
        var expensiveProducts = repo
            .Where(p => p.Price > 500)
            .ToList();

        Console.WriteLine("Expensive products:");
        foreach (var p in expensiveProducts)
        {
            Console.WriteLine($"{p.Name} - {p.Price} - {p.Category}");
        }

        // 5️⃣ Update product
        product.Price = 600;
        repo.Update(product);
        Console.WriteLine("Product updated!");

        // 6️⃣ Delete product
        repo.Delete(product.Id);
        Console.WriteLine("Product deleted!");
    }
}
```