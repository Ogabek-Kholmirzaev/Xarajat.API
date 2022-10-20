using Microsoft.EntityFrameworkCore;
using Xarajat.API.Entities;

namespace Xarajat.API.Data;

public class XarajatDbContext : DbContext
{
    public XarajatDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
}