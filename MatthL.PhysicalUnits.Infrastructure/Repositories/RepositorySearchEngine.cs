using MatthL.PhysicalUnits.Core.EnumHelpers;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static MatthL.PhysicalUnits.Infrastructure.Repositories.PhysicalUnitRepository;

namespace MatthL.PhysicalUnits.Infrastructure.Repositories
{
    /// <summary>
    /// Search engine for physical units with filtering and search capabilities
    /// </summary>
    public static class RepositorySearchEngine
    {
        /// <summary>
        /// Generic method to get units with multiple filters
        /// </summary>
        /// <param name="UseStandardFilters">Apply standard system filters</param>
        /// <param name="domain">Unit domain (optional)</param>
        /// <param name="unitType">Specific unit type (optional)</param>
        /// <returns>List of units matching the criteria</returns>
        public static List<PhysicalUnit> GetUnits(
            bool UseStandardFilters = true,
            PhysicalUnitDomain? domain = null,
            UnitType? unitType = null)
        {
            Initialize();

            // Start with all units
            var query = AvailableUnits.AsQueryable();

            // Apply filters based on provided parameters

            // Filter by domain
            if (domain.HasValue)
            {
                query = query.Where(u => u.UnitType.GetDomain() == domain.Value);
            }

            // Filter by unit type
            if (unitType.HasValue)
            {
                query = query.Where(u => u.UnitType == unitType.Value);
            }

            // Apply standard filters
            if (UseStandardFilters)
            {
                query = query.Where(u =>
                      Settings.ShowMetrics && u.UnitSystem == StandardUnitSystem.Metric ||
                      Settings.ShowImperial && u.UnitSystem == StandardUnitSystem.Imperial ||
                      Settings.ShowOther && u.UnitSystem == StandardUnitSystem.Other ||
                      Settings.ShowUS && u.UnitSystem == StandardUnitSystem.US ||
                      Settings.ShowAstronomic && u.UnitSystem == StandardUnitSystem.Astronomical ||
                      u.UnitSystem == StandardUnitSystem.SI ||
                      u.UnitSystem == StandardUnitSystem.Mixed
                  );
            }

            return query.ToList();
        }

        /// <summary>
        /// Gets all units of a given type
        /// </summary>
        public static List<PhysicalUnit> GetUnitsOfType(UnitType type, bool WithFilter = true)
        {
            Initialize();
            return AvailableUnits.Where(u => u.UnitType == type).ToList();
        }

        /// <summary>
        /// Gets the SI unit of a specific type
        /// </summary>
        public static PhysicalUnit GetSIUnitsOfType(UnitType type)
        {
            Initialize();
            return AvailableUnits.Where(u => u.UnitType == type && u.IsSI == true && !u.HasPrefixes()).FirstOrDefault()?.Clone();
        }

        /// <summary>
        /// Gets all units
        /// </summary>
        public static IEnumerable<PhysicalUnit> GetAllUnits()
        {
            Initialize();
            return AvailableUnits;
        }

        /// <summary>
        /// Gets available domains
        /// </summary>
        public static IEnumerable<PhysicalUnitDomain> GetDomains()
        {
            return Enum.GetValues<PhysicalUnitDomain>()
                .Where(d => d != PhysicalUnitDomain.Undefined);
        }

        /// <summary>
        /// Gets unit types for a domain
        /// </summary>
        public static List<UnitType> GetUnitTypesForDomain(PhysicalUnitDomain? domain, bool WithFilter = true)
        {
            Initialize();
            if (domain == null)
            {
                return AvailableUnits
                .Select(u => u.UnitType)
                .Distinct()
                .OrderBy(t => t.ToString())
                .ToList();
            }
            else
            {
                return AvailableUnits
                .Where(u => u.UnitType.GetDomain() == domain)
                .Select(u => u.UnitType)
                .Distinct()
                .OrderBy(t => t.ToString())
                .ToList();
            }
        }

        /// <summary>
        /// Gets units for a given type
        /// </summary>
        public static List<PhysicalUnit> GetUnitsPerType(UnitType type, bool WithFilter = true)
        {
            Initialize();
            return AvailableUnits
                .Where(u => u.UnitType == type)
                .ToList();
        }

        /// <summary>
        /// Finds units matching a dimensional formula
        /// </summary>
        public static List<PhysicalUnit> GetUnitsFromDimensionalFormula(string formula)
        {
            Initialize();
            return AvailableUnitsByFormula.TryGetValue(formula, out var units)
                ? new List<PhysicalUnit>(units)
                : new List<PhysicalUnit>();
        }

        /// <summary>
        /// Search units by text in name, symbol, or unit type
        /// </summary>
        public static List<PhysicalUnit> SearchUnits(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return new List<PhysicalUnit>();

            Initialize();
            var searchLower = searchText.ToLowerInvariant();

            return AvailableUnits
                .Where(u =>
                    u.Name.ToLowerInvariant().Contains(searchLower) ||
                    u.BaseUnits.Any(b =>
                        b.Name.ToLowerInvariant().Contains(searchLower) ||
                        b.Symbol.ToLowerInvariant().Contains(searchLower)) ||
                    u.UnitType.ToString().ToLowerInvariant().Contains(searchLower))
                .ToList();
        }

        /// <summary>
        /// Search units by text within units matching a dimensional formula
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