using Fractions;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.EquationModels;
using MatthL.PhysicalUnits.Core.Models;

namespace MatthL.PhysicalUnits.DimensionalFormulas.Helpers
{
    /// <summary>
    /// Simplify any unit into a Dictionary<BaseUnitType, Fraction>
    ///
    /// </summary>
    public static class RawUnitsSimplifier
    {
        /// <summary>
        /// Create a dictionary Unit Exponent for the list of RawUnits
        /// </summary>
        /// <param name="units"></param>
        /// <returns></returns>
        public static Dictionary<BaseUnitType, Fraction> SimplifyFormula(params RawUnit[] units)
        {
            // Grouper par type et sommer les exposants
            var dimensions = new Dictionary<BaseUnitType, Fraction>();

            foreach (var unit in units)
            {
                if (dimensions.ContainsKey(unit.UnitType))
                {
                    dimensions[unit.UnitType] += unit.Exponent;
                }
                else
                {
                    dimensions[unit.UnitType] = unit.Exponent;
                }
            }

            // Filtrer les dimensions nulles
            dimensions = dimensions.Where(d => d.Value != 0).ToDictionary(d => d.Key, d => d.Value);

            return dimensions;
        }

        public static Dictionary<BaseUnitType, Fraction> CalculateDimensionalFormula(params PhysicalUnitTerm[] Units)
        {
            var newRawUnits = new List<RawUnit>();

            foreach (var physicalterm in Units)
            {
                foreach (var baseunit in physicalterm.Unit.BaseUnits)
                {
                    foreach (var rawunit in baseunit.RawUnits)
                    {
                        var newRaw = rawunit.Power(physicalterm.Exponent).Power(baseunit.Exponent);
                        newRawUnits.Add(newRaw);
                    }
                }
            }

            return SimplifyFormula(newRawUnits.ToArray());
        }

        public static Dictionary<BaseUnitType, Fraction> CalculateDimensionalFormula(params PhysicalUnit[] Units)
        {
            var newRawUnits = new List<RawUnit>();

            foreach (var mainUnit in Units)
            {
                foreach (var unit in mainUnit.BaseUnits)
                {
                    foreach (var rawunit in unit.RawUnits)
                    {
                        var newRaw = rawunit.Power(unit.Exponent);
                        newRawUnits.Add(newRaw);
                    }
                }
            }

            return SimplifyFormula(newRawUnits.ToArray());
        }

        public static Dictionary<BaseUnitType, Fraction> CalculateDimensionalFormula(params BaseUnit[] Units)
        {
            var newRawUnits = new List<RawUnit>();

            foreach (var unit in Units)
            {
                foreach (var rawunit in unit.RawUnits)
                {
                    var newRaw = rawunit.Power(unit.Exponent);
                    newRawUnits.Add(newRaw);
                }
            }
            return SimplifyFormula(newRawUnits.ToArray());
        }

        public static Dictionary<BaseUnitType, Fraction> CalculateDimensionalFormula(params RawUnit[] Units)
        {
            return SimplifyFormula(Units);
        }
    }
}