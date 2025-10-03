using Fractions;
using MatthL.PhysicalUnits.Core.Enums;

namespace MatthL.PhysicalUnits.DimensionalFormulas.Helpers
{
    /// <summary>
    /// Order a simplified raw units (with rawunitsSimplifier) and return ano ordered list of BaseUnitType and Fraction
    /// </summary>
    public static class RawUnitsOrderer
    {
        /// <summary>
        /// Order the dictionary
        /// TODO : Correctly Order depending on BaseUnitTypes
        /// </summary>
        /// <param name="FormulaDictionary"></param>
        /// <returns> ordered list </returns>
        public static List<(BaseUnitType, Fraction)> OrderFormula(Dictionary<BaseUnitType, Fraction> FormulaDictionary)
        {
            var positive = FormulaDictionary.Where(d => d.Value > 0).OrderBy(d => d.Key);
            var negative = FormulaDictionary.Where(d => d.Value < 0).OrderBy(d => d.Key);

            var parts = new List<(BaseUnitType, Fraction)>();

            // Ajouter les termes positifs
            foreach (var dim in positive)
            {
                parts.Add((dim.Key, dim.Value));
            }
            foreach (var dim in negative)
            {
                parts.Add((dim.Key, dim.Value));
            }
            return parts;
        }
    }
}