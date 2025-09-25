using MatthL.PhysicalUnits.Enums;
using MatthL.PhysicalUnits.Models;
using MatthL.PhysicalUnits.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MatthL.PhysicalUnits.Tools
{
    /// <summary>
    /// Classe qui permet de retrouver des unités similaires et faire des suggestions intelligentes
    /// </summary>
    public static class PhysicalUnitMatch
    {
        /// <summary>
        /// Structure pour une suggestion d'unité avec contexte et score
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
        }

        /// <summary>
        /// Trouve toutes les unités correspondant à une formule dimensionnelle
        /// </summary>
        public static List<PhysicalUnit> FindUnitsForTerms(params PhysicalUnitTerm[] terms)
        {
            var dimensionalFormula = DimensionalFormulaHelper.GetFormulaString(terms);
            var matchingUnits = PhysicalUnitStorage.GetUnitsFromDimensionalFormula(dimensionalFormula);

            // Si aucune unité trouvée, retourner une unité "inconnue"
            if (!matchingUnits.Any())
            {
                return new List<PhysicalUnit> { CreateUnknownUnit(terms) };
            }

            return matchingUnits;
        }

        /// <summary>
        /// Obtient des suggestions d'unités avec scoring intelligent
        /// </summary>
        public static List<UnitSuggestion> GetUnitSuggestions(params PhysicalUnitTerm[] terms)
        {
            var units = FindUnitsForTerms(terms);
            var suggestions = new List<UnitSuggestion>();

            foreach (var unit in units)
            {
                var score = CalculateRelevanceScore(unit, terms);
                var explanation = GenerateExplanation(unit, terms);
                suggestions.Add(new UnitSuggestion(
                    unit,
                    DimensionalFormulaHelper.GetFormulaString(terms),
                    score,
                    explanation
                ));
            }

            return suggestions.OrderByDescending(s => s.RelevanceScore).ToList();
        }

        /// <summary>
        /// Calcule un score de pertinence intelligent
        /// </summary>
        private static double CalculateRelevanceScore(PhysicalUnit unit, PhysicalUnitTerm[] terms)
        {
            double score = 1.0;

            // 1. Favoriser les unités SI
            if (unit.IsSI) score += 3.0;

            // 2. Analyser le contexte de l'équation
            var context = AnalyzeEquationContext(terms);

            // 3. Appliquer des règles métier spécifiques
            score += ApplyDomainSpecificRules(unit, context);

            // 4. Pénaliser les unités exotiques ou peu communes
            if (unit.UnitSystem == StandardUnitSystem.Astronomique) score -= 1.0;
            if (unit.UnitSystem == StandardUnitSystem.Autre) score -= 0.5;

            // 5. Favoriser la cohérence des domaines
            var domains = terms.Select(t => t.Unit.UnitType.GetDomain()).Distinct().ToList();
            if (domains.Count == 1 && unit.UnitType.GetDomain() == domains[0]) score += 1.5;

            return Math.Max(0, score);
        }

        /// <summary>
        /// Analyse le contexte de l'équation pour mieux scorer
        /// </summary>
        private static EquationContext AnalyzeEquationContext(PhysicalUnitTerm[] terms)
        {
            var context = new EquationContext();

            foreach (var term in terms)
            {
                var unitType = term.Unit.UnitType;
                context.UnitTypes.Add(unitType);
                context.Domains.Add(unitType.GetDomain());

                // Analyser les types d'unités présents
                string typeStr = unitType.ToString();
                if (typeStr.Contains("Force")) context.HasForce = true;
                if (typeStr.Contains("Length")) context.HasLength = true;
                if (typeStr.Contains("Time")) context.HasTime = true;
                if (typeStr.Contains("Mass")) context.HasMass = true;
                if (typeStr.Contains("Temperature")) context.HasTemperature = true;
                if (typeStr.Contains("Electric")) context.HasElectric = true;
                if (typeStr.Contains("Volume")) context.HasVolume = true;
                if (typeStr.Contains("Area")) context.HasArea = true;
                if (typeStr.Contains("Speed")) context.HasSpeed = true;
                if (typeStr.Contains("Acceleration")) context.HasAcceleration = true;
                if (typeStr.Contains("Energy")) context.HasEnergy = true;
                if (typeStr.Contains("Pressure")) context.HasPressure = true;
                if (typeStr.Contains("Density")) context.HasDensity = true;
            }

            return context;
        }

        /// <summary>
        /// Applique des règles spécifiques au domaine
        /// </summary>
        private static double ApplyDomainSpecificRules(PhysicalUnit unit, EquationContext context)
        {
            double bonus = 0.0;
            string unitTypeStr = unit.UnitType.ToString();

            // Force × Distance
            if (context.HasForce && context.HasLength)
            {
                if (unitTypeStr.Contains("Energy"))
                {
                    bonus += 2.0; // Travail/Énergie est le cas le plus courant
                }
                else if (unitTypeStr.Contains("Torque"))
                {
                    bonus += 1.5; // Couple est aussi très courant
                }
            }

            // Mass × Acceleration
            if (context.HasMass && context.HasAcceleration)
            {
                if (unitTypeStr.Contains("Force")) bonus += 3.0; // Newton's 2nd law
            }

            // Force / Area
            if (context.HasForce && context.HasArea)
            {
                if (unitTypeStr.Contains("Pressure")) bonus += 3.0;
            }

            // Volume / Time
            if (context.HasVolume && context.HasTime)
            {
                if (unitTypeStr.Contains("VolumeFlow")) bonus += 3.0;
            }

            // Distance / Time
            if (context.HasLength && context.HasTime)
            {
                if (unitTypeStr.Contains("Speed")) bonus += 3.0;
            }

            // Energy / Time
            if (context.HasEnergy && context.HasTime)
            {
                if (unitTypeStr.Contains("Power")) bonus += 3.0;
            }

            // Électricité : V × A = W
            if (context.UnitTypes.Any(t => t.ToString().Contains("ElectricPotential")) &&
                context.UnitTypes.Any(t => t.ToString().Contains("ElectricCurrent")))
            {
                if (unitTypeStr.Contains("Power")) bonus += 3.0;
            }

            // Thermodynamique
            if (context.HasTemperature)
            {
                if (context.Domains.Contains(PhysicalUnitDomain.Thermodynamique))
                {
                    if (unit.UnitType.GetDomain() == PhysicalUnitDomain.Thermodynamique) bonus += 2.0;
                }
            }

            // Mécanique des fluides
            if (context.HasDensity && context.HasSpeed)
            {
                if (unitTypeStr.Contains("Pressure")) bonus += 2.0; // Pression dynamique
            }

            // Pression × Volume = Énergie
            if (context.HasPressure && context.HasVolume)
            {
                if (unitTypeStr.Contains("Energy")) bonus += 2.5;
            }

            return bonus;
        }

        /// <summary>
        /// Génère une explication pour la suggestion
        /// </summary>
        private static string GenerateExplanation(PhysicalUnit unit, PhysicalUnitTerm[] terms)
        {
            var context = AnalyzeEquationContext(terms);
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
            var formula = DimensionalFormulaHelper.GetFormulaString(terms);
            return $"Résultat de l'opération : {formula}";
        }

        /// <summary>
        /// Crée une unité "inconnue" pour les cas non gérés
        /// </summary>
        private static PhysicalUnit CreateUnknownUnit(PhysicalUnitTerm[] terms)
        {
            var result = PhysicalUnitEquation.Multiply(terms);
            result.UnitType = UnitType.Unknown_Special;
            return result;
        }

        /// <summary>
        /// Classe helper pour analyser le contexte
        /// </summary>
        private class EquationContext
        {
            public HashSet<UnitType> UnitTypes { get; } = new HashSet<UnitType>();
            public HashSet<PhysicalUnitDomain> Domains { get; } = new HashSet<PhysicalUnitDomain>();

            public bool HasForce { get; set; }
            public bool HasLength { get; set; }
            public bool HasTime { get; set; }
            public bool HasMass { get; set; }
            public bool HasTemperature { get; set; }
            public bool HasElectric { get; set; }
            public bool HasVolume { get; set; }
            public bool HasArea { get; set; }
            public bool HasSpeed { get; set; }
            public bool HasAcceleration { get; set; }
            public bool HasEnergy { get; set; }
            public bool HasPressure { get; set; }
            public bool HasDensity { get; set; }
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

        /// <summary>
        /// Trouve la meilleure unité pour un ensemble de termes
        /// </summary>
        public static PhysicalUnit FindBestUnit(params PhysicalUnitTerm[] terms)
        {
            var suggestions = GetUnitSuggestions(terms);
            return suggestions.FirstOrDefault()?.Unit ?? CreateUnknownUnit(terms);
        }
    }
}