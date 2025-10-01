using MatthL.PhysicalUnits.Core.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Core.Abstractions
{
    public interface IBaseUnit : IDimensionableUnit
    {
        public int Id { get; set; }
        public UnitType UnitType { get; }
        public string Name { get; }
        public bool IsSI { get; }
        public StandardUnitSystem UnitSystem { get; }

    }
}
