
using MatthL.PhysicalUnits.Core.Enums.Extensions;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MatthL.PhysicalUnits.Infrastructure.Repositories
{
    /// <summary>
    /// Service de stockage et recherche des unités physiques
    /// </summary>
    public static class PhysicalUnitStorage
    {
        private static readonly List<PhysicalUnit> _units = new List<PhysicalUnit>();
        private static readonly Dictionary<string, List<PhysicalUnit>> _unitsByFormula = new Dictionary<string, List<PhysicalUnit>>();
        private static bool _initialized = false;
            
        public static bool ShowMetrics { get; set; }
        public static bool ShowImperial { get; set; }
        public static bool ShowUS { get; set; }
        public static bool ShowAstronomic { get; set; }
        public static bool ShowOther { get; set; }

        /// <summary>
        /// Initialise le stockage avec toutes les unités
        /// </summary>
        public static void Initialize()
        {
            if (_initialized) return;

            // Charger toutes les unités depuis la bibliothèque
            PhysicalUnitLibrary.LoadAll(_units);
                
            ShowMetrics = true;
            ShowImperial = true;
            ShowAstronomic = true;
            ShowOther = true;
            ShowUS = true;

            // Indexer par formule dimensionnelle
            foreach (var unit in _units)
            {
                var formula = unit.DimensionalFormula;
                if (!_unitsByFormula.ContainsKey(formula))
                    _unitsByFormula[formula] = new List<PhysicalUnit>();
                _unitsByFormula[formula].Add(unit);
            }

            _initialized = true;
        }

        /// <summary>
        /// Méthode générique pour obtenir des unités avec filtres multiples
        /// </summary>
        /// <param name="domain">Domaine des unités (optionnel)</param>
        /// <param name="unitType">Type d'unité spécifique (optionnel)</param>
        /// <param name="unitSystem">Système d'unités (optionnel)</param>
        /// <param name="onlySI">Filtrer uniquement les unités SI (optionnel)</param>
        /// <param name="withPrefix">Filtrer par préfixe spécifique (optionnel)</param>
        /// <param name="searchText">Texte de recherche dans nom/symbole (optionnel)</param>
        /// <param name="dimensionalFormula">Formule dimensionnelle exacte (optionnel)</param>
        /// <returns>Liste des unités correspondant aux critères</returns>
        public static List<PhysicalUnit> GetUnits(
            bool UseStandardFilters = true,
            PhysicalUnitDomain? domain = null,
            UnitType? unitType = null
            )
        {
            Initialize();

            // Commencer avec toutes les unités
            var query = _units.AsQueryable();

            // Appliquer les filtres selon les paramètres fournis

            // Filtre par domaine
            if (domain.HasValue)
            {
                query = query.Where(u => u.UnitType.GetDomain() == domain.Value);
            }

            // Filtre par type d'unité
            if (unitType.HasValue)
            {
                query = query.Where(u => u.UnitType == unitType.Value);
            }

            // Filtre si standardFilters
            if (UseStandardFilters)
            {
                query = query.Where(u =>
                      ShowMetrics && u.UnitSystem == StandardUnitSystem.Metric ||
                      ShowImperial && u.UnitSystem == StandardUnitSystem.Imperial ||
                      ShowOther && u.UnitSystem == StandardUnitSystem.Other ||
                      ShowUS && u.UnitSystem == StandardUnitSystem.US ||
                      ShowAstronomic && u.UnitSystem == StandardUnitSystem.Astronomical ||
                      u.UnitSystem == StandardUnitSystem.SI || 
                      u.UnitSystem == StandardUnitSystem.Mixed
                  );
            }


            return query.ToList();
        }

        /// <summary>
        /// Obtient toutes les unités d'un type donné
        /// </summary>
        public static List<PhysicalUnit> GetUnitsOfType(UnitType type, bool WithFilter = true)
        {
            Initialize();
            return _units.Where(u => u.UnitType == type).ToList();
        }

        /// <summary>
        /// Obtient la donnée SI d'un type
        /// </summary>
        public static PhysicalUnit GetSIUnitsOfType(UnitType type)
        {
            Initialize();
            return PhysicalUnit.Clone(_units.Where(u => u.UnitType == type && u.IsSI == true && !u.HasPrefixes()).FirstOrDefault()) ;
        }


        /// <summary>
        /// Obtient toutes les unités
        /// </summary>
        public static IEnumerable<PhysicalUnit> GetAllUnits()
        {
            Initialize();
            return _units;
        }

        /// <summary>
        /// Obtient les domaines disponibles
        /// </summary>
        public static IEnumerable<PhysicalUnitDomain> GetDomains()
        {
            return Enum.GetValues<PhysicalUnitDomain>()
                .Where(d => d != PhysicalUnitDomain.Undefined);
        }

        /// <summary>
        /// Obtient les types d'unités pour un domaine
        /// </summary>
        public static List<UnitType> GetUnitTypesForDomain(PhysicalUnitDomain? domain, bool WithFilter = true)
        {
            Initialize();
            if(domain == null)
            {
                return _units
                .Select(u => u.UnitType)
                .Distinct()
                .OrderBy(t => t.ToString())
                .ToList();
            }
            else
            {
                return _units
                .Where(u => u.UnitType.GetDomain() == domain)
                .Select(u => u.UnitType)
                .Distinct()
                .OrderBy(t => t.ToString())
                .ToList();
            }
        }

        /// <summary>
        /// Obtient les unités pour un type donné
        /// </summary>
        public static List<PhysicalUnit> GetUnitsPerType(UnitType type, bool WithFilter = true)
        {
            Initialize();
            return _units
                .Where(u => u.UnitType == type)
                .ToList();
        }

        /// <summary>
        /// Trouve les unités correspondant à une formule dimensionnelle
        /// </summary>
        public static List<PhysicalUnit> GetUnitsFromDimensionalFormula(string formula)
        {
            Initialize();
            return _unitsByFormula.TryGetValue(formula, out var units)
                ? new List<PhysicalUnit>(units)
                : new List<PhysicalUnit>();
        }

        /// <summary>
        /// Recherche des unités par texte
        /// </summary>
        public static List<PhysicalUnit> SearchUnits(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return new List<PhysicalUnit>();

            Initialize();
            var searchLower = searchText.ToLowerInvariant();

            return _units
                .Where(u =>
                    u.Name.ToLowerInvariant().Contains(searchLower) ||
                    u.BaseUnits.Any(b =>
                        b.Name.ToLowerInvariant().Contains(searchLower) ||
                        b.Symbol.ToLowerInvariant().Contains(searchLower)) ||
                    u.UnitType.ToString().ToLowerInvariant().Contains(searchLower))
                .ToList();
        }
        /// <summary>
        /// Recherche des unités par texte
        /// </summary>
        public static List<PhysicalUnit> SearchUnits(string searchText, string formula)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return GetUnitsFromDimensionalFormula(formula);

            Initialize();
            var searchLower = searchText.ToLowerInvariant();
            var compatibleunits = GetUnitsFromDimensionalFormula(formula);
            return compatibleunits
                .Where(u =>
                    u.Name.ToLowerInvariant().Contains(searchLower) ||
                    u.BaseUnits.Any(b =>
                        b.Name.ToLowerInvariant().Contains(searchLower) ||
                        b.Symbol.ToLowerInvariant().Contains(searchLower)) ||
                    u.UnitType.ToString().ToLowerInvariant().Contains(searchLower))
                .ToList();
        }


    }
}

