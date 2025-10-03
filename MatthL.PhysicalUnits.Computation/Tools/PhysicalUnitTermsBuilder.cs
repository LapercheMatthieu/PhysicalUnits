using Fractions;
using MatthL.PhysicalUnits.Computation;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.EquationModels;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.DimensionalFormulas;

namespace MatthL.PhysicalUnits.Computation.Tools
{
    /// <summary>
    /// Builder to facilitate the production of PhysicalUnitTerms
    /// </summary>
    public static class PhysicalUnitTermsBuilder
    {
        /// <summary>
        /// Méthodes de convenance pour créer des PhysicalUnitTerms
        /// </summary>
        public static PhysicalUnitTerm[] CreateTerms(params (PhysicalUnit unit, double exponent)[] terms)
        {
            return terms.Select(t => new PhysicalUnitTerm
            {
                Unit = t.unit,
                Exponent = new Fraction(t.exponent)
            }).ToArray();
        }
    }
}