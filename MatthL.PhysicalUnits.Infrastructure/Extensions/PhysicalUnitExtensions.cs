
using Fractions;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.Infrastructure.Extensions;
using MatthL.PhysicalUnits.Infrastructure.Repositories;

namespace MatthL.PhysicalUnits.Infrastructure.Extensions
{
    public static class PhysicalUnitExtensions
    {
        public static void AddPrefix(this PhysicalUnit unit, Prefix preFix)
        {
            
        }

        public static bool HasPrefixes(this PhysicalUnit unit)
        {
            foreach(var baseunit in unit.BaseUnits)
            {
                if(baseunit.Prefix != null && baseunit.Prefix != Prefix.SI)
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
            foreach(var baseunit in unitToAdd.BaseUnits)
            {
                unit.BaseUnits.Add(baseunit);
            }
            return unit;
        }

        /// <summary>
        /// cette fonction récupère l'unité SI d'une unité définie. 
        /// l'unité SI est l'unité SI qui partage le même UnitType 
        /// Si Unit Type est unknown on retourne la meme unité
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
                var siBaseUnit = PhysicalUnitStorage.GetSIUnitsOfType(baseUnit.UnitType);
                if (siBaseUnit == null) continue;

                // Cloner et appliquer l'exposant
                var clonedBase = BaseUnit.Clone(siBaseUnit.BaseUnits.First());
                clonedBase.Exponent = baseUnit.Exponent;
                clonedBase.PhysicalUnit = siUnit;
                siUnit.BaseUnits.Add(clonedBase);
            }

            return siUnit;
        }
    }
}
