using MatthL.PhysicalUnits.Computation.Extensions;
using MatthL.PhysicalUnits.Computation.Helpers;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.EquationModels;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.DimensionalFormulas.Extensions;
using MatthL.PhysicalUnits.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Computation.Tools
{
    public static class PhysicalUnitBuilder
    {
        /// <summary>
        /// Crée une unité "inconnue" pour les cas non gérés
        /// </summary>
        private static PhysicalUnit CreateUnknownUnit(PhysicalUnitTerm[] terms)
        {
            var result = new PhysicalUnit();
            result.UnitType = UnitType.Unknown_Special;
            return result.Multiply(terms);
        }

        /// <summary>
        /// Trouve toutes les unités correspondant à une formule dimensionnelle
        /// </summary>
        public static List<PhysicalUnit> FindUnitsForTerms(params PhysicalUnitTerm[] terms)
        {
            var equation = new EquationTerms(terms);
            var dimensionalFormula = equation.GetDimensionalFormula();
            var matchingUnits = RepositorySearchEngine.GetUnitsFromDimensionalFormula(dimensionalFormula);

            // Si aucune unité trouvée, retourner une unité "inconnue"
            if (!matchingUnits.Any())
            {
                return new List<PhysicalUnit> { CreateUnknownUnit(terms) };
            }

            return matchingUnits;
        }

        /// <summary>
        /// Trouve la meilleure unité pour un ensemble de termes
        /// </summary>
        public static PhysicalUnit FindBestUnit(params PhysicalUnitTerm[] terms)
        {
            var suggestions = UnitSuggestionHelper.GetUnitSuggestions(terms);
            return suggestions.FirstOrDefault()?.Unit ?? CreateUnknownUnit(terms);
        }
    }
}
