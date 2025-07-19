using Fractions;
using  PhysicalUnitManagement.Enums;
using  PhysicalUnitManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  PhysicalUnitManagement.Tools
{
    public static class PhysicalUnitNameHelper
    {
        /// <summary>
        /// Génère le nom complet d'une PhysicalUnit basé sur ses BaseUnits
        /// </summary>
        public static string GetPhysicalUnitName(PhysicalUnit unit)
        {
            if (unit == null || !unit.BaseUnits.Any())
                return string.Empty;

            // Séparer les unités au numérateur (exposant > 0) et au dénominateur (exposant < 0)
            var numeratorUnits = unit.BaseUnits.Where(b => b.Exponent > 0).ToList();
            var denominatorUnits = unit.BaseUnits.Where(b => b.Exponent < 0).ToList();

            var nameBuilder = new StringBuilder();

            // Construire le numérateur
            if (numeratorUnits.Any())
            {
                nameBuilder.Append(BuildUnitsName(numeratorUnits, false));
            }

            // Ajouter le dénominateur si présent
            if (denominatorUnits.Any())
            {
                if (numeratorUnits.Any())
                {
                    nameBuilder.Append(" Per ");
                }
                nameBuilder.Append(BuildUnitsName(denominatorUnits, true));
            }

            return nameBuilder.ToString();
        }

        public static string GetPhysicalUnitSymbol(PhysicalUnit unit)
        {

           // return string.Join("·", BaseUnits.Select(b => b.ToString()));

            if (unit == null || !unit.BaseUnits.Any())
                return "SI";

            // Séparer les unités au numérateur (exposant > 0) et au dénominateur (exposant < 0)
            var numeratorUnits = OrderUnits(unit.BaseUnits.Where(b => b.Exponent > 0).ToList());
            var denominatorUnits = OrderUnits(unit.BaseUnits.Where(b => b.Exponent < 0).ToList());
            var finalList = numeratorUnits.Concat(denominatorUnits).ToList();

            var nameBuilder = new StringBuilder();

            for(int i = 0; i< finalList.Count-1; i++)
            {
                nameBuilder.Append($"{finalList[i].ToString()}·");
            }
            nameBuilder.Append($"{finalList.Last().ToString()}");

            return nameBuilder.ToString();
        }


        private static List<BaseUnit> OrderUnits(List<BaseUnit> ListToOrder)
        {
            Initialize();

            return ListToOrder.OrderBy(unit =>
            {
                // Règle spéciale : le temps va toujours en dernier
                bool isTimeUnit = unit.RawUnits.Any() && unit.RawUnits.All(r => r.UnitType == BaseUnitType.Time);
                if (isTimeUnit) return int.MaxValue;

                // Calculer la complexité (nombre de RawUnits distincts)
                int complexity = unit.RawUnits.Count();

                // Obtenir l'ordre du plus petit élément de base
                int smallestOrder = 999; // Valeur par défaut

                if (unit.RawUnits.Any())
                {
                    if (complexity == 1)
                    {
                        // Si complexité 1, prendre directement l'ordre de base
                        var rawUnit = unit.RawUnits.First();
                        BaseUnitOrders.TryGetValue(rawUnit.UnitType, out smallestOrder);
                    }
                    else
                    {
                        // Si complexité > 1, prendre le plus petit ordre parmi les RawUnits
                        smallestOrder = unit.RawUnits
                            .Select(r => BaseUnitOrders.TryGetValue(r.UnitType, out int order) ? order : 999)
                            .Min();
                    }
                }

                // Retourner un score qui priorise:
                // 1. D'abord par complexité décroissante (multiplié par -1000)
                // 2. Ensuite par ordre conventionnel
                return -complexity * 1000 + smallestOrder;
            })
            .ToList();
        }

        private static Dictionary<BaseUnitType, int> BaseUnitOrders;
        private static bool IsInitialized = false;

        private static void Initialize()
        {
            if (IsInitialized) return;
            BaseUnitOrders = new Dictionary<BaseUnitType, int>
                {
                    { BaseUnitType.Mass, 1 },
                    { BaseUnitType.Length, 2 },
                    { BaseUnitType.Time, 3 },
                    { BaseUnitType.ElectricCurrent, 4 },
                    { BaseUnitType.Temperature, 5 },
                    { BaseUnitType.AmountOfSubstance, 6 },
                    { BaseUnitType.LuminousIntensity, 7 },
                    { BaseUnitType.Angle, 8 },
                    { BaseUnitType.Ratio, 9 },
                    { BaseUnitType.Currency, 10 },
                    { BaseUnitType.Information, 11 }
                };
        }

        /// <summary>
        /// Construit le nom pour une liste d'unités
        /// </summary>
        private static string BuildUnitsName(List<BaseUnit> units, bool isDenominator)
        {
            var parts = new List<string>();
            var OrderedUnits = OrderUnits(units);
            foreach (var unit in OrderedUnits)
            {
                var absExponent = isDenominator ? -unit.Exponent : unit.Exponent;
                var unitName = unit.PrefixedName;

                // Vérifier si l'exposant est un entier
                if (absExponent.Denominator == 1)
                {
                    // C'est un entier
                    var intExponent = (int)absExponent.Numerator;

                    if (intExponent == 1)
                    {
                        parts.Add(unitName);
                    }
                    else if (intExponent == 2)
                    {
                        parts.Add($"Square {unitName}");
                    }
                    else if (intExponent == 3)
                    {
                        parts.Add($"Cubic {unitName}");
                    }
                    else
                    {
                        parts.Add($"{unitName} To The {GetOrdinal(intExponent)}");
                    }
                }
                else
                {
                    // C'est une fraction, utiliser le format avec exposant
                    var exponentStr = EquationToStringHelper.ToSuperscript(absExponent);
                    parts.Add($"{unitName}{exponentStr}");
                }
            }

            return string.Join(" ", parts);
        }

        /// <summary>
        /// Convertit un nombre en ordinal anglais
        /// </summary>
        private static string GetOrdinal(int number)
        {
            if (number <= 0)
                return number.ToString();

            switch (number % 100)
            {
                case 11:
                case 12:
                case 13:
                    return $"{number}th";
            }

            switch (number % 10)
            {
                case 1:
                    return $"{number}st";
                case 2:
                    return $"{number}nd";
                case 3:
                    return $"{number}rd";
                default:
                    return $"{number}th";
            }
        }

    }
}
