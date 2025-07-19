using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  PhysicalUnitManagement.Enums
{
    /// <summary>
    /// Types de dimensions de base (7 SI + extensions)
    /// </summary>
    public enum BaseUnitType
    {
        // Les 7 dimensions SI de base
        Length,              // m
        Mass,                // kg
        Time,                // s
        ElectricCurrent,     // A
        Temperature,         // K
        AmountOfSubstance,   // mol
        LuminousIntensity,   // cd

        // Extensions logiques
        Angle,               // rad (distinguer de sans dimension)
        Currency,            // $, €, etc.
        Information,         // bit, byte
        Ratio                // %, sans dimension mais typé
    }

   
}
