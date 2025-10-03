using MatthL.PhysicalUnits.Computation.Models;
using MatthL.PhysicalUnits.Computation.Tools;
using MatthL.PhysicalUnits.Core.EquationModels;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.DimensionalFormulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Computation.Helpers
{
    public class UnitSuggestionHelper
    {
        /// <summary>
        /// Get the list of unit suggestions for physical Unit Terms
        /// </summary>
        public static List<UnitSuggestion> GetUnitSuggestions(params PhysicalUnitTerm[] terms)
        {
            var units = PhysicalUnitBuilder.FindUnitsForTerms(terms);
            var suggestions = new List<UnitSuggestion>();

            foreach (var unit in units)
            {

                var score = UnitSuggestion.CalculateRelevanceScore(unit, terms);
                var explanation = UnitSuggestion.GenerateExplanation(unit, terms);
                suggestions.Add(new UnitSuggestion(
                    unit,
                    FormulaBuilder.GetDimensionalFormula(terms),
                    score,
                    explanation
                ));
            }

            return suggestions.OrderByDescending(s => s.RelevanceScore).ToList();
        }

        /// <summary>
        /// Méthodes de convenance pour les opérations courantes
        /// </summary>
        public static List<UnitSuggestion> MultiplyUnits(PhysicalUnit unit1, PhysicalUnit unit2)
        {
            var terms = new[]
            {
                new PhysicalUnitTerm { Unit = unit1, Exponent = 1 },
                new PhysicalUnitTerm { Unit = unit2, Exponent = 1 }
            };
            return GetUnitSuggestions(terms);
        }

        public static List<UnitSuggestion> DivideUnits(PhysicalUnit numerator, PhysicalUnit denominator)
        {
            var terms = new[]
            {
                new PhysicalUnitTerm { Unit = numerator, Exponent = 1 },
                new PhysicalUnitTerm { Unit = denominator, Exponent = -1 }
            };
            return GetUnitSuggestions(terms);
        }

        public static List<UnitSuggestion> PowerUnit(PhysicalUnit unit, double exponent)
        {
            var terms = new[]
            {
                new PhysicalUnitTerm { Unit = unit, Exponent = new Fractions.Fraction(exponent) }
            };
            return GetUnitSuggestions(terms);
        }

    }
}
