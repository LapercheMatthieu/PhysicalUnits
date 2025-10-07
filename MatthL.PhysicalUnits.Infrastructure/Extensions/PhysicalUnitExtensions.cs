using Fractions;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.EquationModels;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.DimensionalFormulas.Helpers;
using MatthL.PhysicalUnits.Infrastructure.Extensions;
using MatthL.PhysicalUnits.Infrastructure.Repositories;

namespace MatthL.PhysicalUnits.Infrastructure.Extensions
{
    public static class PhysicalUnitExtensions
    {

        public static bool HasPrefixes(this PhysicalUnit unit)
        {
            foreach (var baseunit in unit.BaseUnits)
            {
                if (baseunit.Prefix != null && baseunit.Prefix != Prefix.SI)
                {
                    return true;
                }
            }
            return false;
        }

        public static PhysicalUnitTerm ToTerm(this PhysicalUnit unit, Fraction Exponent)
        {
            return new PhysicalUnitTerm(unit, Exponent);
        }

        public static PhysicalUnit Add(this PhysicalUnit unit, PhysicalUnit unitToAdd)
        {
            if (unitToAdd == null || unitToAdd.BaseUnits == null) return unit;
            foreach (var baseunit in unitToAdd.BaseUnits)
            {
                unit.BaseUnits.Add(baseunit);
            }
            return unit;
        }

        public static PhysicalUnit Clone(this PhysicalUnit CopyUnit)
        {
            if (CopyUnit == null) return new PhysicalUnit();
            var result = new PhysicalUnit()
            {
                UnitType = CopyUnit.UnitType,
            };
            // Cloner les RawUnits
            foreach (var unit in CopyUnit.BaseUnits)
            {
                result.BaseUnits.Add(unit.Clone());
            }
            return result;
        }

        /// <summary>
        /// this function send back the SI Unit of another unit
        /// share the unittype or set it to unknown
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static PhysicalUnit GetSIUnit(this PhysicalUnit unit)
        {
            // Cas 1 : Si l'unité est déjà SI sans préfixes, la retourner
            if (unit.IsSI && !unit.HasPrefixes()) return unit;

            // Cas 2 : Toujours reconstruire l'unité SI à partir des BaseUnits
            var siUnit = new PhysicalUnit
            {
                UnitType = unit.UnitType
            };

            foreach (var baseUnit in unit.BaseUnits)
            {
                // Obtenir l'unité SI équivalente pour ce BaseUnit
                var siBaseUnit = RepositorySearchEngine.GetSIUnitsOfType(baseUnit.UnitType);
                if (siBaseUnit == null) continue;

                // Cloner et appliquer l'exposant
                var clonedBase = siBaseUnit.BaseUnits.First().Clone();
                clonedBase.Exponent = baseUnit.Exponent;
                clonedBase.PhysicalUnit = siUnit;
                siUnit.BaseUnits.Add(clonedBase);
            }

            return siUnit;
        }


        /// <summary>
        /// Simplify a Physical Unit by regrouping the common base units
        /// </summary>
        public static PhysicalUnit Simplify(this PhysicalUnit unit)
        {
            var result = new PhysicalUnit
            {
                UnitType = unit.UnitType
            };

            // Grouper les BaseUnits par type et système
            var groupedUnits = unit.BaseUnits
                .GroupBy(b => new { b.UnitType, b.UnitSystem, b.Symbol })
                .ToList();

            foreach (var group in groupedUnits)
            {
                var totalExponent = group.Sum(b => b.Exponent.ToDouble());

                if (Math.Abs(totalExponent) < 0.0001) // Proche de zéro
                    continue;

                var firstUnit = group.First();
                var newBaseUnit = firstUnit.Clone();
                newBaseUnit.Exponent = new Fraction(totalExponent);
                newBaseUnit.PhysicalUnit = result;
                result.BaseUnits.Add(newBaseUnit);
            }

            return result;
        }
    }
}