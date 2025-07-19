using Fractions;
using  PhysicalUnitManagement.Enums;
using  PhysicalUnitManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace  PhysicalUnitManagement.Services
{
    /// <summary>
    /// Factory pour créer facilement les unités physiques
    /// </summary>
    public static class PhysicalUnitLibraryFactory
    {
        // Stockage des PhysicalUnits de base pour les références dans les composites
        private static readonly Dictionary<string, PhysicalUnit> _registeredUnits = new Dictionary<string, PhysicalUnit>();

        /// <summary>
        /// Enregistre une PhysicalUnit pour utilisation dans les composites
        /// </summary>
        public static void RegisterUnit(string key, PhysicalUnit unit)
        {
            _registeredUnits[key] = unit;
        }

        /// <summary>
        /// Récupère une PhysicalUnit enregistrée
        /// </summary>
        public static PhysicalUnit GetRegisteredUnit(string key)
        {
            return _registeredUnits.TryGetValue(key, out var unit) ? unit : null;
        }

        /// <summary>
        /// Crée une PhysicalUnit simple (un seul BaseUnit)
        /// </summary>
        public static PhysicalUnit CreateSimpleUnit(
            UnitType unitType,
            string name,
            string symbol,
            StandardUnitSystem system,
            bool isSI,
            decimal conversionFactor,
            double offset,
            Prefix prefix,
            params (BaseUnitType type, Fraction exponent)[] rawUnits)
        {
            // Créer le BaseUnit
            var baseUnit = new BaseUnit
            {
                UnitType = unitType,
                Name = name,
                Symbol = symbol,
                Exponent = 1, // Par défaut pour une unité simple
                UnitSystem = system,
                IsSI = isSI,
                ConversionFactor = new Fraction(conversionFactor),
                Offset = offset,
                Prefix = prefix,
                RawUnits = new List<RawUnit>()
            };

            // Ajouter les RawUnits
            foreach (var (type, exponent) in rawUnits)
            {
                baseUnit.RawUnits.Add(new RawUnit(type, exponent));
            }

            // Créer la PhysicalUnit qui contient ce BaseUnit
            var physicalUnit = new PhysicalUnit
            {
                UnitType = unitType
            };
            physicalUnit.BaseUnits.Add(baseUnit);

            // Lier le BaseUnit à la PhysicalUnit (pour la DB)
            baseUnit.PhysicalUnit = physicalUnit;

            return physicalUnit;
        }

        /// <summary>
        /// Crée une unité SI
        /// </summary>
        public static PhysicalUnit SI(UnitType type, string name, string symbol, params (BaseUnitType, Fraction)[] dimensions)
        {
            return CreateSimpleUnit(type, name, symbol, StandardUnitSystem.SI, true, 1m, 0.0, Prefix.SI, dimensions);
        }

        /// <summary>
        /// Crée une unité métrique (non-SI)
        /// </summary>
        public static PhysicalUnit Metric(UnitType type, string name, string symbol, decimal factor, params (BaseUnitType, Fraction)[] dimensions)
        {
            return CreateSimpleUnit(type, name, symbol, StandardUnitSystem.Metrique, false, factor, 0.0, Prefix.SI, dimensions);
        }

        /// <summary>
        /// Crée une unité métrique avec préfixe
        /// </summary>
        public static PhysicalUnit MetricWithPrefix(UnitType type, string name, string symbol, Prefix prefix, params (BaseUnitType, Fraction)[] dimensions)
        {
            var factor = PrefixHelper.GetSize(prefix);
            return CreateSimpleUnit(type, name, symbol, StandardUnitSystem.Metrique, false, factor, 0.0, prefix, dimensions);
        }

        /// <summary>
        /// Crée une unité impériale
        /// </summary>
        public static PhysicalUnit Imperial(UnitType type, string name, string symbol, decimal factor, params (BaseUnitType, Fraction)[] dimensions)
        {
            return CreateSimpleUnit(type, name, symbol, StandardUnitSystem.Imperial, false, factor, 0.0, Prefix.SI, dimensions);
        }

        /// <summary>
        /// Crée une unité avec offset (températures)
        /// </summary>
        public static PhysicalUnit WithOffset(UnitType type, string name, string symbol, decimal factor, double offset, StandardUnitSystem system, params (BaseUnitType, Fraction)[] dimensions)
        {
            return CreateSimpleUnit(type, name, symbol, system, false, factor, offset, Prefix.SI, dimensions);
        }

        /// <summary>
        /// Crée une unité astronomique
        /// </summary>
        public static PhysicalUnit Astro(UnitType type, string name, string symbol, decimal factor, params (BaseUnitType, Fraction)[] dimensions)
        {
            return CreateSimpleUnit(type, name, symbol, StandardUnitSystem.Astronomique, false, factor, 0.0, Prefix.SI, dimensions);
        }

        /// <summary>
        /// Crée une unité autre
        /// </summary>
        public static PhysicalUnit Other(UnitType type, string name, string symbol, decimal factor, params (BaseUnitType, Fraction)[] dimensions)
        {
            return CreateSimpleUnit(type, name, symbol, StandardUnitSystem.Autre, false, factor, 0.0, Prefix.SI, dimensions);
        }

        /// <summary>
        /// Crée une unité d'angle
        /// </summary>
        public static PhysicalUnit Angle(UnitType type, string name, string symbol, decimal factor = 1m)
        {
            var system = factor == 1m ? StandardUnitSystem.SI : StandardUnitSystem.Autre;
            return CreateSimpleUnit(type, name, symbol, system, factor == 1m, factor, 0.0, Prefix.SI, (BaseUnitType.Angle, 1));
        }

        /// <summary>
        /// Crée une unité de ratio/pourcentage
        /// </summary>
        public static PhysicalUnit Ratio(UnitType type, string name, string symbol, decimal factor = 1m)
        {
            return CreateSimpleUnit(type, name, symbol, StandardUnitSystem.Autre, false, factor, 0.0, Prefix.SI, (BaseUnitType.Ratio, 1));
        }

        /// <summary>
        /// Crée une unité monétaire
        /// </summary>
        public static PhysicalUnit Currency(UnitType type, string name, string symbol, decimal factor = 1m)
        {
            var system = name.Contains("Dollar") ? StandardUnitSystem.US : StandardUnitSystem.Autre;
            return CreateSimpleUnit(type, name, symbol, system, false, factor, 0.0, Prefix.SI, (BaseUnitType.Currency, 1));
        }

        /// <summary>
        /// Crée une unité d'information
        /// </summary>
        public static PhysicalUnit Info(UnitType type, string name, string symbol, decimal factor = 1m)
        {
            return CreateSimpleUnit(type, name, symbol, StandardUnitSystem.Autre, false, factor, 0.0, Prefix.SI, (BaseUnitType.Information, 1));
        }

        /// <summary>
        /// Crée une PhysicalUnit composite à partir d'unités enregistrées
        /// </summary>
        public static PhysicalUnit Composite(UnitType compositeType, params (string unitKey, int exponent)[] components)
        {
            var physicalUnit = new PhysicalUnit
            {
                UnitType = compositeType
            };

            foreach (var (key, exponent) in components)
            {
                var registeredUnit = GetRegisteredUnit(key);
                if (registeredUnit != null && registeredUnit.BaseUnits.Count == 1)
                {
                    // Cloner le BaseUnit de l'unité enregistrée
                    var originalBase = registeredUnit.BaseUnits.First();
                    var clonedBase = new BaseUnit
                    {
                        UnitType = originalBase.UnitType,
                        Name = originalBase.Name,
                        Symbol = originalBase.Symbol,
                        Exponent = exponent, // L'exposant dans la composition
                        UnitSystem = originalBase.UnitSystem,
                        IsSI = originalBase.IsSI,
                        ConversionFactor = originalBase.ConversionFactor,
                        Offset = originalBase.Offset,
                        Prefix = originalBase.Prefix,
                        RawUnits = new List<RawUnit>()
                    };

                    // Cloner les RawUnits
                    foreach (var rawUnit in originalBase.RawUnits)
                    {
                        clonedBase.RawUnits.Add(new RawUnit(rawUnit.UnitType, rawUnit.Exponent));
                    }

                    // Lier à la nouvelle PhysicalUnit
                    clonedBase.PhysicalUnit = physicalUnit;
                    physicalUnit.BaseUnits.Add(clonedBase);
                }
                else
                {
                    throw new ArgumentException($"Unit '{key}' not found or is not a simple unit.");
                }
            }

            return physicalUnit;
        }

        /// <summary>
        /// Crée une PhysicalUnit avec préfixe à partir d'une unité existante
        /// </summary>
        public static PhysicalUnit WithPrefix(string baseUnitKey, Prefix prefix)
        {
            var baseUnit = GetRegisteredUnit(baseUnitKey);
            if (baseUnit == null || baseUnit.BaseUnits.Count != 1)
                throw new ArgumentException($"Unit '{baseUnitKey}' not found or is not a simple unit.");

            var original = baseUnit.BaseUnits.First();

            // Créer une nouvelle PhysicalUnit avec le préfixe
            return CreateSimpleUnit(
                original.UnitType,
                original.Name,
                original.Symbol,
                original.UnitSystem,
                original.IsSI,
                original.ConversionFactor.ToDecimal(),
                original.Offset,
                prefix,
                original.RawUnits.Select(r => (r.UnitType, r.Exponent)).ToArray()
            );
        }

        /// <summary>
        /// Réinitialise le registre (utile pour les tests)
        /// </summary>
        public static void ClearRegistry()
        {
            _registeredUnits.Clear();
        }
    }
}
