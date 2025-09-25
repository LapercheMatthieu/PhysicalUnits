using Fractions;
using MatthL.PhysicalUnits.Enums;
using MatthL.PhysicalUnits.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MatthL.PhysicalUnits.Tools
{
    /// <summary>
    /// Helper qui gère les équations entre PhysicalUnits
    /// Permet de faire des opérations, créer de nouvelles PhysicalUnits et vérifier l'homogénéité
    /// </summary>
    public static class PhysicalUnitEquation
    {
        /// <summary>
        /// Multiplie plusieurs PhysicalUnits avec leurs exposants
        /// </summary>
        public static PhysicalUnit Multiply(params PhysicalUnitTerm[] terms)
        {
            if (terms == null || terms.Length == 0)
                throw new ArgumentException("Au moins un terme est requis");

            // Utiliser DimensionalFormulaHelper pour calculer la formule résultante
            var resultFormula = DimensionalFormulaHelper.CalculateDimensionalFormula(terms);

            // Créer une nouvelle PhysicalUnit composite
            var result = new PhysicalUnit
            {
                UnitType = UnitType.Unknown_Special // Sera déterminé par PhysicalUnitMatch
            };

            // Pour chaque terme, ajouter ses BaseUnits avec les exposants appropriés
            foreach (var term in terms)
            {
                foreach (var baseUnit in term.Unit.BaseUnits)
                {
                    // Cloner le BaseUnit avec le nouvel exposant
                    var newBaseUnit = CloneBaseUnit(baseUnit);
                    newBaseUnit.Exponent = baseUnit.Exponent * term.Exponent;
                    newBaseUnit.PhysicalUnit = result;
                    result.BaseUnits.Add(newBaseUnit);
                }
            }

            return SimplifyPhysicalUnit(result);
        }

        /// <summary>
        /// Division simple entre deux PhysicalUnits
        /// </summary>
        public static PhysicalUnit Divide(PhysicalUnit numerator, PhysicalUnit denominator)
        {
            return Multiply(
                new PhysicalUnitTerm { Unit = numerator, Exponent = 1 },
                new PhysicalUnitTerm { Unit = denominator, Exponent = -1 }
            );
        }

        /// <summary>
        /// Élève une PhysicalUnit à une puissance
        /// </summary>
        public static PhysicalUnit Power(PhysicalUnit unit, Fraction exponent)
        {
            return Multiply(new PhysicalUnitTerm { Unit = unit, Exponent = exponent });
        }

        /// <summary>
        /// Vérifie l'homogénéité physique entre plusieurs termes
        /// </summary>
        public static bool VerifyHomogeneity(params PhysicalUnitTerm[] terms)
        {
            if (terms == null || terms.Length < 2)
                return true;

            // Calculer la formule dimensionnelle du premier terme
            var referenceFormula = DimensionalFormulaHelper.CalculateDimensionalFormula(terms[0]);

            // Ignorer les dimensions "non physiques" (Angle, Ratio, etc.)
            referenceFormula = FilterPhysicalDimensions(referenceFormula);

            // Comparer avec les autres termes
            for (int i = 1; i < terms.Length; i++)
            {
                var currentFormula = DimensionalFormulaHelper.CalculateDimensionalFormula(terms[i]);
                currentFormula = FilterPhysicalDimensions(currentFormula);

                // Vérifier que toutes les dimensions sont identiques
                if (!AreDimensionsEqual(referenceFormula, currentFormula))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Vérifie l'homogénéité physique entre plusieurs termes equation 
        /// </summary>
        public static bool VerifyHomogeneity(params EquationTerms[] terms)
        {
            if (terms == null || terms.Length < 2)
                return true;

            // Calculer la formule dimensionnelle du premier terme
            var referenceFormula = DimensionalFormulaHelper.CalculateDimensionalFormula(terms[0].Terms.ToArray());

            // Ignorer les dimensions "non physiques" (Angle, Ratio, etc.)
            referenceFormula = FilterPhysicalDimensions(referenceFormula);

            // Comparer avec les autres termes
            for (int i = 1; i < terms.Length; i++)
            {
                var currentFormula = DimensionalFormulaHelper.CalculateDimensionalFormula(terms[i].Terms.ToArray());
                currentFormula = FilterPhysicalDimensions(currentFormula);

                // Vérifier que toutes les dimensions sont identiques
                if (!AreDimensionsEqual(referenceFormula, currentFormula))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Vérifie l'homogénéité entre plusieurs PhysicalUnits
        /// </summary>
        public static bool VerifyHomogeneity(params PhysicalUnit[] units)
        {
            var terms = units.Select(u => new PhysicalUnitTerm { Unit = u, Exponent = 1 }).ToArray();
            return VerifyHomogeneity(terms);
        }

        /// <summary>
        /// Obtient la formule dimensionnelle d'un ensemble de termes
        /// </summary>
        public static string GetDimensionalFormula(params PhysicalUnitTerm[] terms)
        {
            return DimensionalFormulaHelper.GetFormulaString(terms);
        }

        /// <summary>
        /// Simplifie une PhysicalUnit en regroupant les BaseUnits identiques
        /// </summary>
        private static PhysicalUnit SimplifyPhysicalUnit(PhysicalUnit unit)
        {
            var result = new PhysicalUnit
            {
                UnitType = unit.UnitType
            };

            // Grouper les BaseUnits par type et système
            var groupedUnits = unit.BaseUnits
                .GroupBy(b => new { b.UnitType, b.UnitSystem, b.Symbol })
                .ToList();

            foreach (var group in groupedUnits)
            {
                var totalExponent = group.Sum(b => b.Exponent.ToDouble());

                if (Math.Abs(totalExponent) < 0.0001) // Proche de zéro
                    continue;

                var firstUnit = group.First();
                var newBaseUnit = CloneBaseUnit(firstUnit);
                newBaseUnit.Exponent = new Fraction(totalExponent);
                newBaseUnit.PhysicalUnit = result;
                result.BaseUnits.Add(newBaseUnit);
            }

            return result;
        }

        /// <summary>
        /// Clone un BaseUnit
        /// </summary>
        private static BaseUnit CloneBaseUnit(BaseUnit original)
        {
            return new BaseUnit
            {
                UnitType = original.UnitType,
                UnitSystem = original.UnitSystem,
                Name = original.Name,
                Symbol = original.Symbol,
                Prefix = original.Prefix,
                IsSI = original.IsSI,
                ConversionFactor = original.ConversionFactor,
                Offset = original.Offset,
                Exponent = original.Exponent,
                RawUnits = original.RawUnits.Select(r => new RawUnit(r.UnitType, r.Exponent)).ToList()
            };
        }

        /// <summary>
        /// Filtre les dimensions non physiques pour la vérification d'homogénéité
        /// </summary>
        private static Dictionary<BaseUnitType, Fraction> FilterPhysicalDimensions(Dictionary<BaseUnitType, Fraction> dimensions)
        {
            var nonPhysicalDimensions = new[] {
                BaseUnitType.Angle,
                BaseUnitType.Ratio,
                BaseUnitType.Currency,
                BaseUnitType.Information
            };

            return dimensions
                .Where(d => !nonPhysicalDimensions.Contains(d.Key))
                .ToDictionary(d => d.Key, d => d.Value);
        }

        /// <summary>
        /// Compare deux ensembles de dimensions
        /// </summary>
        private static bool AreDimensionsEqual(
            Dictionary<BaseUnitType, Fraction> dim1,
            Dictionary<BaseUnitType, Fraction> dim2)
        {
            // Vérifier que toutes les clés sont identiques
            var keys1 = dim1.Keys.OrderBy(k => k).ToList();
            var keys2 = dim2.Keys.OrderBy(k => k).ToList();

            if (keys1.Count != keys2.Count)
                return false;

            for (int i = 0; i < keys1.Count; i++)
            {
                if (keys1[i] != keys2[i])
                    return false;

                // Vérifier que les exposants sont égaux
                if (dim1[keys1[i]] != dim2[keys2[i]])
                    return false;
            }

            return true;
        }

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

        /// <summary>
        /// Ajoute deux PhysicalUnits (vérifie l'homogénéité d'abord)
        /// </summary>
        public static (bool isHomogeneous, PhysicalUnit result) Add(PhysicalUnit unit1, PhysicalUnit unit2)
        {
            if (!VerifyHomogeneity(unit1, unit2))
                return (false, null);

            // Pour l'addition, on retourne simplement la première unité car elles sont homogènes
            return (true, unit1);
        }

        /// <summary>
        /// Soustrait deux PhysicalUnits (vérifie l'homogénéité d'abord)
        /// </summary>
        public static (bool isHomogeneous, PhysicalUnit result) Subtract(PhysicalUnit unit1, PhysicalUnit unit2)
        {
            return Add(unit1, unit2); // Même logique que l'addition
        }
    }
}