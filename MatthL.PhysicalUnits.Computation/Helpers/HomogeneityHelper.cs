using MatthL.PhysicalUnits.Computation.Extensions;
using MatthL.PhysicalUnits.Core.EquationModels;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.DimensionalFormulas;
using MatthL.PhysicalUnits.DimensionalFormulas.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Computation.Helpers
{
    public static class HomogeneityHelper
    {
        /// <summary>
        /// Return true if all inputs are homogenuous
        /// </summary>
        public static bool VerifyHomogeneity(params PhysicalUnitTerm[] terms)
        {
            if (terms == null || terms.Length < 2)
                return true;

            // Calculate Dictionary
            var referenceFormula = RawUnitsSimplifier.CalculateDimensionalFormula(terms[0]);

            // Ignore all non physical units
            referenceFormula = referenceFormula.FilterPhysicalDimensions();

            // Comparer avec les autres termes
            for (int i = 1; i < terms.Length; i++)
            {
                var currentFormula = RawUnitsSimplifier.CalculateDimensionalFormula(terms[i]);
                currentFormula = currentFormula.FilterPhysicalDimensions();

                // Vérifier que toutes les dimensions sont identiques
                if (!referenceFormula.IsDimensionsEqualTo(currentFormula))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Check Homogeneity between different Equation Terms, considering multiplication of all physicalUnitTerms
        /// </summary>
        public static bool VerifyHomogeneity(params EquationTerms[] terms)
        {
            if (terms == null || terms.Length < 2)
                return true;

            //First multiply all PhysicalUnitTerms in each terms to compare only resulting PhysicalUnitTerms
            var PhysicalUnitList = new List<PhysicalUnit>();

            foreach(var term in terms)
            {
                var resultingUnit = new PhysicalUnit()
                {
                    UnitType = Core.Enums.UnitType.Unknown_Special
                };
                PhysicalUnitList.Add(resultingUnit.Multiply(term.Terms.ToArray()));
            }
            return VerifyHomogeneity(PhysicalUnitList.ToArray());
        }

        /// <summary>
        /// Vérifie l'homogénéité entre plusieurs PhysicalUnits
        /// </summary>
        public static bool VerifyHomogeneity(params PhysicalUnit[] units)
        {
            var terms = units.Select(u => new PhysicalUnitTerm { Unit = u, Exponent = 1 }).ToArray();
            return VerifyHomogeneity(terms);
        }
    }
}
