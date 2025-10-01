using MatthL.PhysicalUnits.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Core.EnumHelpers
{
    public static  class BaseUnitTypeExtensions
    {
        /// <summary>
        /// Obtient le symbole SI pour une dimension de base
        /// </summary>
        public static string GetBaseSymbol(BaseUnitType type) => type switch
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
    }
}
