namespace MatthL.PhysicalUnits.Core.Enums
{
    /// <summary>
    /// Physical base dimensions (7 SI + extensions)
    /// </summary>
    public enum BaseUnitType
    {
        // The 7 base ones
        Length,              // m

        Mass,                // kg
        Time,                // s
        ElectricCurrent,     // A
        Temperature,         // K
        AmountOfSubstance,   // mol
        LuminousIntensity,   // cd

        // Extensions for code
        Angle,               // rad

        Currency,            // $, €, etc.
        Information,         // bit, byte
        Ratio                // %,
    }
}