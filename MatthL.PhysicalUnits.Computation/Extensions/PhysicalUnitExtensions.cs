using Fractions;
using MatthL.PhysicalUnits.Computation.Helpers;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.EquationModels;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.DimensionalFormulas;
using MatthL.PhysicalUnits.Infrastructure.Extensions;


namespace MatthL.PhysicalUnits.Computation.Extensions
{
    public static class PhysicalUnitExtensions
    {
        /// <summary>
        /// Divide a physical unit with an other
        /// </summary>
        public static PhysicalUnit Divide(this PhysicalUnit numerator, PhysicalUnit denominator)
        {
            return numerator.Multiply(
                new PhysicalUnitTerm { Unit = numerator, Exponent = 1 },
                new PhysicalUnitTerm { Unit = denominator, Exponent = -1 }
            );
        }

        /// <summary>
        /// Multiply a physical unit with Physical Unit Terms
        /// </summary>
        public static PhysicalUnit Multiply(this PhysicalUnit firstitem, params PhysicalUnitTerm[] terms)
        {
            if (terms == null || terms.Length == 0)
                throw new ArgumentException("Au moins un terme est requis");

            // Créer une nouvelle PhysicalUnit composite
            var result = new PhysicalUnit
            {
                UnitType = UnitType.Unknown_Special // Sera déterminé par PhysicalUnitMatch
            };

            //Ajouter les termes de l'unité initiale
            foreach (var baseUnit in firstitem.BaseUnits)
            {
                // Cloner le BaseUnit avec le nouvel exposant
                var newBaseUnit = baseUnit.Clone();
                newBaseUnit.Exponent = baseUnit.Exponent;
                newBaseUnit.PhysicalUnit = result;
                result.BaseUnits.Add(newBaseUnit);
            }

            // Pour chaque terme, ajouter ses BaseUnits avec les exposants appropriés
            foreach (var term in terms)
            {
                foreach (var baseUnit in term.Unit.BaseUnits)
                {
                    // Cloner le BaseUnit avec le nouvel exposant
                    var newBaseUnit = baseUnit.Clone();
                    newBaseUnit.Exponent = baseUnit.Exponent * term.Exponent;
                    newBaseUnit.PhysicalUnit = result;
                    result.BaseUnits.Add(newBaseUnit);
                }
            }

            return result.Simplify();
        }

        /// <summary>
        /// send a new physical unit raised to a certain power
        /// </summary>
        public static PhysicalUnit Pow(this PhysicalUnit unit, Fraction exponent)
        {
            // Créer une nouvelle PhysicalUnit composite
            var result = new PhysicalUnit
            {
                UnitType = unit.UnitType
            };

            //Ajouter les termes de l'unité initiale
            foreach (var baseUnit in unit.BaseUnits)
            {
                // Cloner le BaseUnit avec le nouvel exposant
                var newBaseUnit = baseUnit.Clone();
                newBaseUnit.Exponent = baseUnit.Exponent;
                newBaseUnit.PhysicalUnit = result;
                result.BaseUnits.Add(newBaseUnit);
            }

            return result.Simplify();
        }
    }
}
