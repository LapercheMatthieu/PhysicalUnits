using Fractions;
using MatthL.PhysicalUnits.Core.Abstractions;
using MatthL.PhysicalUnits.Core.EnumHelpers;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.EquationModels;
using MatthL.PhysicalUnits.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MatthL.PhysicalUnits.Core.Formulas.EquationToStringHelper;

namespace MatthL.PhysicalUnits.Core.Formulas
{
    /// <summary>
    /// Simply transform all models into a base unit type dictionary which is the formula
    /// </summary>
    public static class FormulaHelper
    {
        /// <summary>
        /// Need homogeneity in the list
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static string GetDimensionalFormula(params IDimensionableUnit[] unit)
        {
            var formula = BuildDimensionalFormula(unit);
            var ordered = OrderFormula(formula);
            return CreateFormulaString(ordered);
        }
        public static string GetDimensionalFormula(this IDimensionableUnit unit)
        {
            return GetDimensionalFormula(unit);
        }

        private static Dictionary<BaseUnitType, Fraction> BuildDimensionalFormula(params IDimensionableUnit[] units)
        {
            switch (units) 
            {
                case PhysicalUnitTerm[] Physicalterm: return CalculateDimensionalFormula(Physicalterm);
                case PhysicalUnit[] physicalunits: return CalculateDimensionalFormula(physicalunits);
                case BaseUnit[] Units: return CalculateDimensionalFormula(Units);
                default: return new Dictionary<BaseUnitType, Fraction>();
            }
        }
        private static Dictionary<BaseUnitType, Fraction> CalculateDimensionalFormula(params PhysicalUnitTerm[] Units)
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
        private static Dictionary<BaseUnitType, Fraction> CalculateDimensionalFormula(params PhysicalUnit[] Units)
        {
            var newRawUnits = new List<RawUnit>();

            foreach(var mainUnit in Units)
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
        private static Dictionary<BaseUnitType, Fraction> CalculateDimensionalFormula(params BaseUnit[] Units)
        {
            var newRawUnits = new List<RawUnit>();

            foreach (var unit in Units)
            {
                foreach(var rawunit in unit.RawUnits)
                {
                    var newRaw = rawunit.Power(unit.Exponent);
                    newRawUnits.Add(newRaw);
                }
            }
            return SimplifyFormula(newRawUnits.ToArray());
        }
        public static List<RawUnit> SimplifyUnits(params RawUnit[] units)
        {
            var dictionary = SimplifyFormula(units);
            List<RawUnit> newList = new List<RawUnit>();
            foreach (var unit in dictionary)
            {
                newList.Add(new RawUnit(unit.Key, unit.Value));
            }
            return newList;
        }

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

        /// <summary>
        /// Order the dictionary
        /// TODO : Correctly Order depending on BaseUnitTypes
        /// </summary>
        /// <param name="FormulaDictionary"></param>
        /// <returns> ordered list </returns>
        public static List<(BaseUnitType,Fraction)> OrderFormula(Dictionary<BaseUnitType, Fraction> FormulaDictionary)
        {
            var positive = FormulaDictionary.Where(d => d.Value > 0).OrderBy(d => d.Key);
            var negative = FormulaDictionary.Where(d => d.Value < 0).OrderBy(d => d.Key);

            var parts = new List<(BaseUnitType, Fraction)>();

            // Ajouter les termes positifs
            foreach (var dim in positive)
            {
                parts.Add((dim.Key,dim.Value));
            }
            foreach (var dim in negative)
            {
                parts.Add((dim.Key, dim.Value));
            }
            return parts;
        }

        /// <summary>
        /// Create the string from an ordered dictionary
        /// </summary>
        private static string CreateFormulaString(List<(BaseUnitType, Fraction)> OrderedUnits)
        {
            var builder = new StringBuilder();
            bool isinNegative = false;
            bool isFirst = true;
            foreach (var unit in OrderedUnits)
            {
                if(unit.Item2 < 0 && isinNegative == false)
                {
                    builder.Append("/");
                    isinNegative = true;
                }
                else if(isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    builder.Append("·");
                }
                if (unit.Item2 > 0)
                {
                    builder.Append(FormatWithExponent(BaseUnitTypeExtensions.GetBaseSymbol(unit.Item1), unit.Item2));
                }
                else
                {
                    builder.Append(FormatWithExponent(BaseUnitTypeExtensions.GetBaseSymbol(unit.Item1), -unit.Item2));
                }
            }
            return builder.ToString();
        }
    }
}
