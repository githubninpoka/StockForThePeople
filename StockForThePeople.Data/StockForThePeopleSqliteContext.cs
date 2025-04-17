using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyModel;
using StockForThePeople.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StockForThePeople.Data;

public class StockForThePeopleSqliteContext : DbContext, IStockForThePeopleContext
{
    public DbSet<Asset> Assets { get; set; }
    public DbSet<MarketData> MarketData { get; set; }
    public DbSet<UserTransaction> UserTransactions { get; set; }

    public StockForThePeopleSqliteContext(DbContextOptions<StockForThePeopleSqliteContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public async Task SaveChangesAsync()
    {
        await base.SaveChangesAsync();
    }
}
