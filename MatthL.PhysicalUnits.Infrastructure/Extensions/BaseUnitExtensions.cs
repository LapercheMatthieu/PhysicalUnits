using MatthL.PhysicalUnits.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Infrastructure.Extensions
{
    public static class BaseUnitExtensions
    {
        public static BaseUnit Clone(this BaseUnit unit)
        {
            return new BaseUnit()
            {
                ConversionFactor = unit.ConversionFactor,
                Exponent = unit.Exponent,
                IsSI = unit.IsSI,
                Name = unit.Name,
                Offset = unit.Offset,
                Prefix = unit.Prefix,
                Symbol = unit.Symbol,
                RawUnits = unit.RawUnits,
                UnitSystem = unit.UnitSystem,
                UnitType = unit.UnitType,
                PhysicalUnit = unit.PhysicalUnit,
            };
        }

        public static BaseUnit AddPrefix(this BaseUnit unit, Prefix Prefix)
        {
            var newBase = unit.Clone();
            newBase.Prefix = Prefix;
            return newBase;
        }

    }
}
