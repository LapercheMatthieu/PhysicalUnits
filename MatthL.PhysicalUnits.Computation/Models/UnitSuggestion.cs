using MatthL.PhysicalUnits.Core.EnumHelpers;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.EquationModels;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.DimensionalFormulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Computation.Models
{
    /// <summary>
    /// Structure of a suggested unit with its relevance score and explanation.
    /// </summary>
    public class UnitSuggestion
    {
        public PhysicalUnit Unit { get; set; }
        public string Context { get; set; }
        public double RelevanceScore { get; set; }
        public string Explanation { get; set; }

        public UnitSuggestion(PhysicalUnit unit, string context, double score, string explanation = "")
        {
            Unit = unit;
            Context = context;
            RelevanceScore = score;
            Explanation = explanation;
        }

        /// <summary>
        /// Génère une explication pour la suggestion
        /// </summary>
        public static string GenerateExplanation(PhysicalUnit unit, PhysicalUnitTerm[] terms)
        {

            var context = new EquationContext(terms);
            string unitTypeStr = unit.UnitType.ToString();

            // Cas spéciaux avec explications
            if (context.HasForce && context.HasLength)
            {
                if (unitTypeStr.Contains("Energy"))
                    return "Travail effectué par une force sur une distance (W = F × d)";
                if (unitTypeStr.Contains("Torque"))
                    return "Moment de force (couple) (τ = F × r)";
            }

            if (context.HasMass && context.HasAcceleration)
            {
                if (unitTypeStr.Contains("Force"))
                    return "2ème loi de Newton : F = m × a";
            }

            if (context.UnitTypes.Any(t => t.ToString().Contains("ElectricPotential")) &&
                context.UnitTypes.Any(t => t.ToString().Contains("ElectricCurrent")))
            {
                if (unitTypeStr.Contains("Power"))
                    return "Puissance électrique : P = U × I";
            }

            if (context.HasPressure && context.HasVolume)
            {
                if (unitTypeStr.Contains("Energy"))
                    return "Énergie = Pression × Volume (travail de compression/détente)";
            }

            if (context.HasEnergy && context.HasTime)
            {
                if (unitTypeStr.Contains("Power"))
                    return "Puissance = Énergie / Temps";
            }

            if (context.HasForce && context.HasArea)
            {
                if (unitTypeStr.Contains("Pressure"))
                    return "Pression = Force / Surface";
            }

            // Explication générique basée sur la formule
            var formula = FormulaBuilder.GetDimensionalFormula(terms);
            return $"Résultat de l'opération : {formula}";
        }

        /// <summary>
        /// Calculate a relevence score
        /// </summary>
        public static double CalculateRelevanceScore(PhysicalUnit unit, PhysicalUnitTerm[] terms)
        {
            double score = 1.0;

            // 1. Favoriser les unités SI
            if (unit.IsSI) score += 3.0;

            // 2. Analyser le contexte de l'équation
            var context = new EquationContext(terms); 
            // 3. Appliquer des règles métier spécifiques
            score += context.CheckScore(unit);

            // 4. Pénaliser les unités exotiques ou peu communes
            if (unit.UnitSystem == StandardUnitSystem.Astronomical) score -= 1.0;
            if (unit.UnitSystem == StandardUnitSystem.Other) score -= 0.5;

            // 5. Favoriser la cohérence des domaines
            var domains = terms.Select(t => t.Unit.UnitType.GetDomain()).Distinct().ToList();
            if (domains.Count == 1 && unit.UnitType.GetDomain() == domains[0]) score += 1.5;

            return Math.Max(0, score);
        }

    }


}
