using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.Models;

namespace MatthL.PhysicalUnits.Infrastructure.Extensions
{
    public static class BaseUnitExtensions
    {
        public static BaseUnit Clone(this BaseUnit unit)
        {
            var cloned = new BaseUnit()
            {
                ConversionFactor = unit.ConversionFactor,
                Exponent = unit.Exponent,
                IsSI = unit.IsSI,
                Name = unit.Name,
                Offset = unit.Offset,
                Prefix = unit.Prefix,
                Symbol = unit.Symbol,
                UnitSystem = unit.UnitSystem,
                UnitType = unit.UnitType,
                // Ne pas copier PhysicalUnit et PhysicalUnitId pour éviter les références circulaires
            };

            // Cloner les RawUnits
            foreach (var rawUnit in unit.RawUnits)
            {
                cloned.RawUnits.Add(rawUnit.Clone());
            }

            return cloned;
        }

        public static BaseUnit AddPrefix(this BaseUnit unit, Prefix Prefix)
        {
            var newBase = unit.Clone();
            newBase.Prefix = Prefix;
            return newBase;
        }
    }
}