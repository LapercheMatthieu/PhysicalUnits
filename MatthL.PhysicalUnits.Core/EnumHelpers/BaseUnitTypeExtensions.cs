using MatthL.PhysicalUnits.Core.Enums;

namespace MatthL.PhysicalUnits.Core.EnumHelpers
{
    public static class BaseUnitTypeExtensions
    {
        /// <summary>
        /// Obtient le symbole SI pour une dimension de base
        /// </summary>
        public static string GetBaseSymbol(this BaseUnitType type) => type switch
        {
            BaseUnitType.Length => "m",
            BaseUnitType.Mass => "kg",
            BaseUnitType.Time => "s",
            BaseUnitType.ElectricCurrent => "A",
            BaseUnitType.Temperature => "K",
            BaseUnitType.AmountOfSubstance => "mol",
            BaseUnitType.LuminousIntensity => "cd",
            BaseUnitType.Angle => "rad",
            BaseUnitType.Currency => "$",
            BaseUnitType.Information => "bit",
            BaseUnitType.Ratio => "1",
            _ => "?"
        };

        public static bool IsPhysicalBase(this BaseUnitType type) => type switch
        {
            BaseUnitType.Length => true,
            BaseUnitType.Mass => true,
            BaseUnitType.Time => true,
            BaseUnitType.ElectricCurrent => true,
            BaseUnitType.Temperature => true,
            BaseUnitType.AmountOfSubstance => true,
            BaseUnitType.LuminousIntensity => true,
            BaseUnitType.Angle => false,
            BaseUnitType.Currency =>false,
            BaseUnitType.Information => false,
            BaseUnitType.Ratio => false,
            _ => false
        };

    }
}