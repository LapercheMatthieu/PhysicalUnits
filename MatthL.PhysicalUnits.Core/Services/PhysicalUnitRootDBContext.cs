using MatthL.PhysicalUnits.Models;
using Microsoft.EntityFrameworkCore;
using SQLiteManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Services
{
    public class PhysicalUnitRootDBContext : RootDbContext
    {
        public DbSet<PhysicalUnit> PhysicalUnits { get; set; }
        public DbSet<BaseUnit> BaseUnits { get; set; }
        public DbSet<RawUnit> RawUnits { get; set; }
    }
}
