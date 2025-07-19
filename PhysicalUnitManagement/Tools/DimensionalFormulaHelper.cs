using Fractions;
using MahApps.Metro.Controls;
using  PhysicalUnitManagement.Enums;
using  PhysicalUnitManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static  PhysicalUnitManagement.Tools.EquationToStringHelper;

namespace  PhysicalUnitManagement.Tools
{
    /// <summary>
    /// Classe qui simplifie la création et l'afffichage des formules dimensionnelles
    /// </summary>
    public static class DimensionalFormulaHelper
    {
        #region STRING BUILDING 
        public static string GetFormulaString(params PhysicalUnitTerm[] Units)
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

            return GetFormulaString(newRawUnits.ToArray());
        }

        public static string GetFormulaString(params PhysicalUnit[] Units)
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

            return GetFormulaString(newRawUnits.ToArray());
        }

        public static string GetFormulaString(params BaseUnit[] Units)
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
            return GetFormulaString(newRawUnits.ToArray());
        }
        /// <summary>
        /// Calcule la formule dimensionnelle à partir d'une liste de RawUnits
        /// </summary>
        public static string GetFormulaString(params RawUnit[] rawUnits)
        {
            var dimensions = SimplifyFormula(rawUnits);
            var positive = dimensions.Where(d => d.Value > 0).OrderBy(d => d.Key);
            var negative = dimensions.Where(d => d.Value < 0).OrderBy(d => d.Key);

            var parts = new List<string>();

            // Ajouter les termes positifs
            foreach (var dim in positive)
            {
                parts.Add(FormatWithExponent(GetBaseSymbol(dim.Key), dim.Value));
            }

            // Si on a des termes négatifs, les ajouter après un slash
            if (negative.Any())
            {
                var negParts = new List<string>();
                foreach (var dim in negative)
                {
                    // Inverser le signe pour l'affichage après le slash
                    negParts.Add(FormatWithExponent(GetBaseSymbol(dim.Key), -dim.Value));
                }

                return string.Join("·", parts) + "/" + string.Join("·", negParts);
            }

            return string.Join("·", parts);
        }

        #endregion

        #region Formula building !!!!!
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

        public static Dictionary<BaseUnitType, Fraction> CalculateDimensionalFormula(params BaseUnit[] Units)
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


        #endregion

    }
}
