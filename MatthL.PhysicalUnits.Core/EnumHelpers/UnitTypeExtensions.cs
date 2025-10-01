using MatthL.PhysicalUnits.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Core.EnumHelpers
{
    /// <summary>
    /// Extensions to facilitate the Use of UnitType
    /// </summary>
    public static class UnitTypeExtensions
    {
        /// <summary>
        /// Get the physical domain for a specified unit type
        /// </summary>
        public static PhysicalUnitDomain GetDomain(this UnitType unitType)
        {
            var parts = unitType.ToString().Split('_');
            if (parts.Length < 2) return PhysicalUnitDomain.Undefined;

            return parts[1] switch
            {
                "Base" => PhysicalUnitDomain.BaseUnits,
                "Mech" => PhysicalUnitDomain.Mechanics,
                "Fluid" => PhysicalUnitDomain.Fluidics,
                "Thermo" => PhysicalUnitDomain.Thermodynamics,
                "Elec" => PhysicalUnitDomain.Electricity,
                "Chem" => PhysicalUnitDomain.Chemistry,
                "Optic" => PhysicalUnitDomain.Optics,
                "Info" => PhysicalUnitDomain.Computing,
                "Transport" => PhysicalUnitDomain.Transport,
                "Econ" => PhysicalUnitDomain.Economics,
                "Special" => PhysicalUnitDomain.Special,
                _ => PhysicalUnitDomain.Undefined
            };
        }

        /// <summary>
        /// Get the short name of the unit type
        /// </summary>
        public static string GetShortName(this UnitType unitType)
        {
            var parts = unitType.ToString().Split('_');
            var result = parts.Length > 0 ? parts[0] : unitType.ToString();

            // Ajouter des espaces avant les majuscules (sauf la première)
            return System.Text.RegularExpressions.Regex.Replace(
                result,
                "(?<!^)(?=[A-Z])",
                " "
            );
        }

        /// <summary>
        /// Get all types of a specified domain
        /// </summary>
        public static IEnumerable<UnitType> GetTypesForDomain(PhysicalUnitDomain domain)
        {
            var domainSuffix = domain switch
            {
                PhysicalUnitDomain.BaseUnits => "_Base",
                PhysicalUnitDomain.Mechanics => "_Mech",
                PhysicalUnitDomain.Fluidics => "_Fluid",
                PhysicalUnitDomain.Thermodynamics => "_Thermo",
                PhysicalUnitDomain.Electricity => "_Elec",
                PhysicalUnitDomain.Chemistry => "_Chem",
                PhysicalUnitDomain.Optics => "_Optic",
                PhysicalUnitDomain.Computing => "_Info",
                PhysicalUnitDomain.Transport => "_Transport",
                PhysicalUnitDomain.Economics => "_Econ",
                PhysicalUnitDomain.Special => "_Special",
                _ => ""
            };

            if (string.IsNullOrEmpty(domainSuffix))
                return Enumerable.Empty<UnitType>();

            return Enum.GetValues<UnitType>()
                .Where(t => t.ToString().EndsWith(domainSuffix));
        }
    }
}
