using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.DimensionalFormulas.Extensions;
using MatthL.PhysicalUnits.Infrastructure.Library;

namespace MatthL.PhysicalUnits.Infrastructure.Repositories
{
    /// <summary>
    /// Storage and repository for physical units
    /// </summary>
    public static class PhysicalUnitRepository
    {
        public static readonly List<PhysicalUnit> AvailableUnits = new List<PhysicalUnit>();

        public static readonly Dictionary<string, List<PhysicalUnit>> AvailableUnitsByFormula = new Dictionary<string, List<PhysicalUnit>>();

        private static bool _initialized = false;

        public static RepositorySettings Settings { get; set; }

        /// <summary>
        /// Initialize the storage with all units
        /// </summary>
        public static void Initialize()
        {
            if (_initialized) return;
            Settings = new RepositorySettings();
            Settings.Initialize();
            // Load all units from the library
            PhysicalUnitLibrary.LoadAll(AvailableUnits);

            // Index by dimensional formula
            foreach (var unit in AvailableUnits)
            {
                var formula = unit.GetDimensionalFormula();
                if (!AvailableUnitsByFormula.ContainsKey(formula))
                    AvailableUnitsByFormula[formula] = new List<PhysicalUnit>();
                AvailableUnitsByFormula[formula].Add(unit);
            }

            _initialized = true;
        }

    }
}