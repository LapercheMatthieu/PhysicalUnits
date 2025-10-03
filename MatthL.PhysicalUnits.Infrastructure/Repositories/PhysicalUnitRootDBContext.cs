using MatthL.PhysicalUnits.Core.Models;
using MatthL.SqliteEF.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace MatthL.PhysicalUnits.Infrastructure.Repositories
{
    public class PhysicalUnitRootDBContext : RootDbContext
    {
        public DbSet<PhysicalUnit> PhysicalUnits { get; set; }
        public DbSet<BaseUnit> BaseUnits { get; set; }
        public DbSet<RawUnit> RawUnits { get; set; }
    }
}