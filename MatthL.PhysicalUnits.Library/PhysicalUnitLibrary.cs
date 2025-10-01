using Fractions;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MatthL.PhysicalUnits.Core.Services.PhysicalUnitLibraryFactory;

namespace MatthL.PhysicalUnits.Core.Services
{
    /// <summary>
    /// Bibliothèque contenant toutes les définitions d'unités de base
    /// Note: Les préfixes (kilo, milli, etc.) seront gérés dynamiquement
    /// </summary>
    public static class PhysicalUnitLibrary
    {
        /// <summary>
        /// Charge toutes les unités dans le dictionnaire fourni
        /// </summary>
        public static void LoadAll(List<PhysicalUnit> units)
        {
            // D'abord enregistrer TOUTES les unités de base pour les composites
            RegisterAllBaseUnits();

            // Puis charger toutes les unités dans le storage
            LoadBaseUnits(units);
            LoadMechanicalUnits(units);
            LoadElectricalUnits(units);
            LoadThermodynamicUnits(units);
            LoadFluidUnits(units);
            LoadChemicalUnits(units);
            LoadOpticalUnits(units);
            LoadEconomicUnits(units);
            LoadInformationUnits(units);
            LoadTransportUnits(units);
            LoadSpecialUnits(units);
        }

        private static void RegisterAllBaseUnits()
        {
            // Helper local functions pour réduire la verbosité
            void Reg(string key, UnitType type, string name, string symbol, StandardUnitSystem sys, bool isSI, decimal factor, double offset, params (BaseUnitType, Fraction)[] raw)
                => RegisterUnit(key, CreateSimpleUnit(type, name, symbol, sys, isSI, factor, offset, Prefix.SI, raw));

            void SI(string key, UnitType type, string name, string symbol, params (BaseUnitType, Fraction)[] raw)
                => Reg(key, type, name, symbol, StandardUnitSystem.SI, true, 1m, 0.0, raw);

            void Metric(string key, UnitType type, string name, string symbol, decimal factor, params (BaseUnitType, Fraction)[] raw)
                => Reg(key, type, name, symbol, StandardUnitSystem.Metric, false, factor, 0.0, raw);

            void Imperial(string key, UnitType type, string name, string symbol, decimal factor, params (BaseUnitType, Fraction)[] raw)
                => Reg(key, type, name, symbol, StandardUnitSystem.Imperial, false, factor, 0.0, raw);

            void US(string key, UnitType type, string name, string symbol, decimal factor, params (BaseUnitType, Fraction)[] raw)
                => Reg(key, type, name, symbol, StandardUnitSystem.US, false, factor, 0.0, raw);

            void Other(string key, UnitType type, string name, string symbol, decimal factor, params (BaseUnitType, Fraction)[] raw)
                => Reg(key, type, name, symbol, StandardUnitSystem.Other, false, factor, 0.0, raw);

            void Astro(string key, UnitType type, string name, string symbol, decimal factor, params (BaseUnitType, Fraction)[] raw)
                => Reg(key, type, name, symbol, StandardUnitSystem.Astronomical, false, factor, 0.0, raw);

            void WithOffset(string key, UnitType type, string name, string symbol, StandardUnitSystem sys, decimal factor, double offset, params (BaseUnitType, Fraction)[] raw)
                => Reg(key, type, name, symbol, sys, false, factor, offset, raw);

            // ===== LONGUEUR =====
            SI("Meter", UnitType.Length_Base, "Meter", "m", (BaseUnitType.Length, 1));
            Imperial("Foot", UnitType.Length_Base, "Foot", "ft", 0.3048m, (BaseUnitType.Length, 1));
            Imperial("Inch", UnitType.Length_Base, "Inch", "in", 0.0254m, (BaseUnitType.Length, 1));
            Imperial("Mile", UnitType.Length_Base, "Mile", "mi", 1609.344m, (BaseUnitType.Length, 1));
            Imperial("Yard", UnitType.Length_Base, "Yard", "yd", 0.9144m, (BaseUnitType.Length, 1));
            Imperial("Chain", UnitType.Length_Base, "Chain", "ch", 20.1168m, (BaseUnitType.Length, 1));
            Imperial("Fathom", UnitType.Length_Base, "Fathom", "ftm", 1.8288m, (BaseUnitType.Length, 1));
            Imperial("Mil", UnitType.Length_Base, "Mil", "mil", 0.0000254m, (BaseUnitType.Length, 1));
            Imperial("Hand", UnitType.Length_Base, "Hand", "hand", 0.1016m, (BaseUnitType.Length, 1));
            US("UsSurveyFoot", UnitType.Length_Base, "UsSurveyFoot", "ft(US)", 0.304800609601219m, (BaseUnitType.Length, 1));
            Other("NauticalMile", UnitType.Length_Base, "NauticalMile", "nmi", 1852m, (BaseUnitType.Length, 1));
            Astro("LightYear", UnitType.Length_Base, "LightYear", "ly", 9.4607304725808e15m, (BaseUnitType.Length, 1));
            Astro("AstronomicalUnit", UnitType.Length_Base, "AstronomicalUnit", "AU", 149597870700m, (BaseUnitType.Length, 1));
            Astro("Parsec", UnitType.Length_Base, "Parsec", "pc", 3.0857e16m, (BaseUnitType.Length, 1));
            Astro("SolarRadius", UnitType.Length_Base, "SolarRadius", "R☉", 6.96e8m, (BaseUnitType.Length, 1));
            Other("Twip", UnitType.Length_Base, "Twip", "twip", 1.7638888889e-5m, (BaseUnitType.Length, 1));
            Other("DtpPoint", UnitType.Length_Base, "DtpPoint", "pt", 0.0003527777778m, (BaseUnitType.Length, 1));
            Other("DtpPica", UnitType.Length_Base, "DtpPica", "pica", 0.0042333333m, (BaseUnitType.Length, 1));
            Imperial("Shackle", UnitType.Length_Base, "Shackle", "shackle", 27.432m, (BaseUnitType.Length, 1));
            Metric("Centimeter", UnitType.Length_Base, "Centimeter", "cm", 0.01m, (BaseUnitType.Length, 1));
            Metric("Decimeter", UnitType.Length_Base, "Decimeter", "dm", 0.1m, (BaseUnitType.Length, 1));
            Metric("Kilometer", UnitType.Length_Base, "Kilometer", "km", 1000m, (BaseUnitType.Length, 1));
            Metric("Hectometer", UnitType.Length_Base, "Hectometer", "hm", 100m, (BaseUnitType.Length, 1));

            // ===== MASSE =====
            SI("Kilogram", UnitType.Mass_Base, "Kilogram", "kg", (BaseUnitType.Mass, 1));
            Metric("Gram", UnitType.Mass_Base, "Gram", "g", 0.001m, (BaseUnitType.Mass, 1));
            Metric("Tonne", UnitType.Mass_Base, "Tonne", "t", 1000m, (BaseUnitType.Mass, 1));
            Imperial("Pound", UnitType.Mass_Base, "Pound", "lb", 0.45359237m, (BaseUnitType.Mass, 1));
            Imperial("Ounce", UnitType.Mass_Base, "Ounce", "oz", 0.028349523125m, (BaseUnitType.Mass, 1));
            Imperial("Stone", UnitType.Mass_Base, "Stone", "st", 6.35029318m, (BaseUnitType.Mass, 1));
            Imperial("Grain", UnitType.Mass_Base, "Grain", "gr", 0.00006479891m, (BaseUnitType.Mass, 1));
            US("ShortTon", UnitType.Mass_Base, "ShortTon", "ton(US)", 907.18474m, (BaseUnitType.Mass, 1));
            Imperial("LongTon", UnitType.Mass_Base, "LongTon", "ton(UK)", 1016.0469088m, (BaseUnitType.Mass, 1));
            Imperial("Slug", UnitType.Mass_Base, "Slug", "slug", 14.593903m, (BaseUnitType.Mass, 1));
            US("ShortHundredweight", UnitType.Mass_Base, "ShortHundredweight", "cwt(US)", 45.359237m, (BaseUnitType.Mass, 1));
            Imperial("LongHundredweight", UnitType.Mass_Base, "LongHundredweight", "cwt(UK)", 50.80234544m, (BaseUnitType.Mass, 1));
            Astro("EarthMass", UnitType.Mass_Base, "EarthMass", "M⊕", 5.972e24m, (BaseUnitType.Mass, 1));

            // ===== TEMPS =====
            SI("Second", UnitType.Time_Base, "Second", "s", (BaseUnitType.Time, 1));
            Other("Minute", UnitType.Time_Base, "Minute", "min", 60m, (BaseUnitType.Time, 1));
            Other("Hour", UnitType.Time_Base, "Hour", "h", 3600m, (BaseUnitType.Time, 1));
            Other("Day", UnitType.Time_Base, "Day", "d", 86400m, (BaseUnitType.Time, 1));
            Other("Week", UnitType.Time_Base, "Week", "week", 604800m, (BaseUnitType.Time, 1));
            Other("Month30", UnitType.Time_Base, "Month30", "month", 2592000m, (BaseUnitType.Time, 1));
            Other("Year365", UnitType.Time_Base, "Year365", "year", 31536000m, (BaseUnitType.Time, 1));

            // ===== COURANT ÉLECTRIQUE =====
            SI("Ampere", UnitType.ElectricCurrent_Base, "Ampere", "A", (BaseUnitType.ElectricCurrent, 1));

            // ===== TEMPÉRATURE =====
            SI("Kelvin", UnitType.Temperature_Base, "Kelvin", "K", (BaseUnitType.Temperature, 1));
            WithOffset("Celsius", UnitType.Temperature_Base, "Celsius", "°C", StandardUnitSystem.Metric, 1m, 273.15, (BaseUnitType.Temperature, Fraction.One));
            WithOffset("Fahrenheit", UnitType.Temperature_Base, "Fahrenheit", "°F", StandardUnitSystem.Imperial, 5m / 9m, 459.67 * 5.0 / 9.0, (BaseUnitType.Temperature, Fraction.One));
            Imperial("Rankine", UnitType.Temperature_Base, "Rankine", "°R", 5m / 9m, (BaseUnitType.Temperature, 1));

            // ===== QUANTITÉ DE MATIÈRE =====
            SI("Mole", UnitType.AmountOfSubstance_Base, "Mole", "mol", (BaseUnitType.AmountOfSubstance, 1));
            Imperial("PoundMole", UnitType.AmountOfSubstance_Base, "PoundMole", "lbmol", 453.59237m, (BaseUnitType.AmountOfSubstance, 1));

            // ===== INTENSITÉ LUMINEUSE =====
            SI("Candela", UnitType.LuminousIntensity_Base, "Candela", "cd", (BaseUnitType.LuminousIntensity, 1));

            // ===== ANGLE =====
            SI("Radian", UnitType.Angle_Base, "Radian", "rad", (BaseUnitType.Angle, 1));
            Other("Degree", UnitType.Angle_Base, "Degree", "°", 0.017453292519943295m, (BaseUnitType.Angle, 1));
            Other("Revolution", UnitType.Angle_Base, "Revolution", "rev", 6.283185307179586m, (BaseUnitType.Angle, 1));
            Other("Gradian", UnitType.Angle_Base, "Gradian", "gon", 0.015707963267949m, (BaseUnitType.Angle, 1));
            Other("Arcminute", UnitType.Angle_Base, "Arcminute", "'", 0.0002908882086657m, (BaseUnitType.Angle, 1));
            Other("Arcsecond", UnitType.Angle_Base, "Arcsecond", "\"", 4.84813681109536e-6m, (BaseUnitType.Angle, 1));

            // ===== RATIO =====
            SI("Ratio", UnitType.Ratio_Base, "Ratio", "pu", (BaseUnitType.Ratio, 1));
            Other("Percent", UnitType.Ratio_Base, "Percent", "%", 0.01m, (BaseUnitType.Ratio, 1));
            Other("PartPerThousand", UnitType.Ratio_Base, "PartPerThousand", "‰", 0.001m, (BaseUnitType.Ratio, 1));
            Other("PartPerMillion", UnitType.Ratio_Base, "PartPerMillion", "ppm", 1e-6m, (BaseUnitType.Ratio, 1));
            Other("PartPerBillion", UnitType.Ratio_Base, "PartPerBillion", "ppb", 1e-9m, (BaseUnitType.Ratio, 1));
            Other("PartPerTrillion", UnitType.Ratio_Base, "PartPerTrillion", "ppt", 1e-12m, (BaseUnitType.Ratio, 1));

            // ===== MONNAIE =====
            SI("USDollar", UnitType.Currency_Base, "US Dollar", "$", (BaseUnitType.Currency, 1));
            Other("Euro", UnitType.Currency_Base, "Euro", "€", 0.95m, (BaseUnitType.Currency, 1));
            Other("BritishPound", UnitType.Currency_Base, "British Pound", "£", 0.79m, (BaseUnitType.Currency, 1));
            Other("JapaneseYen", UnitType.Currency_Base, "Japanese Yen", "JPY", 157m, (BaseUnitType.Currency, 1));
            Other("SwissFranc", UnitType.Currency_Base, "Swiss Franc", "CHF", 0.91m, (BaseUnitType.Currency, 1));
            Other("CanadianDollar", UnitType.Currency_Base, "Canadian Dollar", "C$", 1.44m, (BaseUnitType.Currency, 1));
            Other("AustralianDollar", UnitType.Currency_Base, "Australian Dollar", "A$", 1.61m, (BaseUnitType.Currency, 1));
            Other("ChineseYuan", UnitType.Currency_Base, "Chinese Yuan", "CNY", 7.3m, (BaseUnitType.Currency, 1));
            Other("IndianRupee", UnitType.Currency_Base, "Indian Rupee", "₹", 86m, (BaseUnitType.Currency, 1));
            Other("MexicanPeso", UnitType.Currency_Base, "Mexican Peso", "MX$", 20.5m, (BaseUnitType.Currency, 1));
            Other("BrazilianReal", UnitType.Currency_Base, "Brazilian Real", "R$", 6.1m, (BaseUnitType.Currency, 1));
            Other("SouthKoreanWon", UnitType.Currency_Base, "South Korean Won", "₩", 1460m, (BaseUnitType.Currency, 1));

            // ===== INFORMATION =====
            SI("Bit", UnitType.Information_Base, "Bit", "bit", (BaseUnitType.Information, 1));
            Other("Byte", UnitType.Information_Base, "Byte", "B", 8m, (BaseUnitType.Information, 1));

            // ===== FORCE =====
            SI("Newton", UnitType.Force_Mech, "Newton", "N", (BaseUnitType.Mass, 1), (BaseUnitType.Length, 1), (BaseUnitType.Time, -2));
            Other("Dyn", UnitType.Force_Mech, "Dyn", "dyn", 1e-5m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, 1), (BaseUnitType.Time, -2));
            Imperial("PoundForce", UnitType.Force_Mech, "PoundForce", "lbf", 4.4482216152605m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, 1), (BaseUnitType.Time, -2));
            Metric("KilogramForce", UnitType.Force_Mech, "KilogramForce", "kgf", 9.80665m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, 1), (BaseUnitType.Time, -2));
            Metric("TonneForce", UnitType.Force_Mech, "TonneForce", "tf", 9806.65m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, 1), (BaseUnitType.Time, -2));
            Imperial("Poundal", UnitType.Force_Mech, "Poundal", "pdl", 0.138254954376m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, 1), (BaseUnitType.Time, -2));
            Imperial("OunceForce", UnitType.Force_Mech, "OunceForce", "ozf", 0.2780138509537812m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, 1), (BaseUnitType.Time, -2));
            US("ShortTonForce", UnitType.Force_Mech, "ShortTonForce", "tonf(US)", 8896.443230521m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, 1), (BaseUnitType.Time, -2));

            // ===== PRESSION =====
            SI("Pascal", UnitType.Pressure_Mech, "Pascal", "Pa", (BaseUnitType.Mass, 1), (BaseUnitType.Length, -1), (BaseUnitType.Time, -2));
            Other("Bar", UnitType.Pressure_Mech, "Bar", "bar", 100000m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, -1), (BaseUnitType.Time, -2));
            Other("Atmosphere", UnitType.Pressure_Mech, "Atmosphere", "atm", 101325m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, -1), (BaseUnitType.Time, -2));
            Other("TechnicalAtmosphere", UnitType.Pressure_Mech, "TechnicalAtmosphere", "at", 98066.5m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, -1), (BaseUnitType.Time, -2));
            Other("Torr", UnitType.Pressure_Mech, "Torr", "Torr", 133.322368421m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, -1), (BaseUnitType.Time, -2));
            Imperial("PoundPerSquareInch", UnitType.Pressure_Mech, "PoundPerSquareInch", "psi", 6894.757293168m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, -1), (BaseUnitType.Time, -2));
            Imperial("PoundPerSquareFoot", UnitType.Pressure_Mech, "PoundPerSquareFoot", "psf", 47.88025898m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, -1), (BaseUnitType.Time, -2));
            Imperial("InchOfMercury", UnitType.Pressure_Mech, "InchOfMercury", "inHg", 3386.389m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, -1), (BaseUnitType.Time, -2));
            Imperial("InchOfWaterColumn", UnitType.Pressure_Mech, "InchOfWaterColumn", "inH2O", 249.08891m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, -1), (BaseUnitType.Time, -2));
            Imperial("FootOfHead", UnitType.Pressure_Mech, "FootOfHead", "ftH2O", 2989.06692m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, -1), (BaseUnitType.Time, -2));
            Metric("MeterOfHead", UnitType.Pressure_Mech, "MeterOfHead", "mH2O", 9806.65m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, -1), (BaseUnitType.Time, -2));
            Metric("MillimeterOfMercury", UnitType.Pressure_Mech, "MillimeterOfMercury", "mmHg", 133.322387415m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, -1), (BaseUnitType.Time, -2));

            // ===== ÉNERGIE =====
            SI("Joule", UnitType.Energy_Mech, "Joule", "J", (BaseUnitType.Mass, 1), (BaseUnitType.Length, 2), (BaseUnitType.Time, -2));
            Metric("Calorie", UnitType.Energy_Mech, "Calorie", "cal", 4.184m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, 2), (BaseUnitType.Time, -2));
            Imperial("BritishThermalUnit", UnitType.Energy_Mech, "BritishThermalUnit", "BTU", 1055.05585262m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, 2), (BaseUnitType.Time, -2));
            Other("ElectronVolt", UnitType.Energy_Mech, "ElectronVolt", "eV", 1.602176634e-19m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, 2), (BaseUnitType.Time, -2));
            Imperial("FootPound", UnitType.Energy_Mech, "FootPound", "ft·lb", 1.3558179483314m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, 2), (BaseUnitType.Time, -2));
            Other("Erg", UnitType.Energy_Mech, "Erg", "erg", 1e-7m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, 2), (BaseUnitType.Time, -2));
            Other("ThermEc", UnitType.Energy_Mech, "ThermEc", "thm(EC)", 105505600m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, 2), (BaseUnitType.Time, -2));
            US("ThermUs", UnitType.Energy_Mech, "ThermUs", "thm(US)", 105480400m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, 2), (BaseUnitType.Time, -2));
            Imperial("ThermImperial", UnitType.Energy_Mech, "ThermImperial", "thm(UK)", 105505585.257348m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, 2), (BaseUnitType.Time, -2));

            // ===== PUISSANCE =====
            SI("Watt", UnitType.Power_Mech, "Watt", "W", (BaseUnitType.Mass, 1), (BaseUnitType.Length, 2), (BaseUnitType.Time, -3));
            Imperial("MechanicalHorsepower", UnitType.Power_Mech, "MechanicalHorsepower", "hp(I)", 745.69987158227m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, 2), (BaseUnitType.Time, -3));
            Metric("MetricHorsepower", UnitType.Power_Mech, "MetricHorsepower", "hp(M)", 735.49875m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, 2), (BaseUnitType.Time, -3));
            Other("ElectricalHorsepower", UnitType.Power_Mech, "ElectricalHorsepower", "hp(E)", 746m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, 2), (BaseUnitType.Time, -3));
            Other("BoilerHorsepower", UnitType.Power_Mech, "BoilerHorsepower", "hp(S)", 9812.5m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, 2), (BaseUnitType.Time, -3));
            Other("HydraulicHorsepower", UnitType.Power_Mech, "HydraulicHorsepower", "hp(H)", 745.7m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, 2), (BaseUnitType.Time, -3));
            Astro("SolarLuminosity", UnitType.Power_Mech, "SolarLuminosity", "L☉", 3.828e26m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, 2), (BaseUnitType.Time, -3));

            // ===== FRÉQUENCE =====
            SI("Hertz", UnitType.Frequency_Mech, "Hertz", "Hz", (BaseUnitType.Time, -1));
            Other("RevolutionsPerMinute", UnitType.Frequency_Mech, "RevolutionsPerMinute", "rpm", 0.016666666666667m, (BaseUnitType.Time, -1));
            Other("BeatPerMinute", UnitType.Frequency_Mech, "BeatPerMinute", "bpm", 0.016666666666667m, (BaseUnitType.Time, -1));

            // ===== VOLUME =====
            // À AJOUTER :
            SI("CubicMeter", UnitType.Volume_Mech, "Cubic Meter", "m³", (BaseUnitType.Length, 3));
            Imperial("CubicFoot", UnitType.Volume_Mech, "Cubic Foot", "ft³", 0.028316846592m, (BaseUnitType.Length, 3));
            Imperial("CubicInch", UnitType.Volume_Mech, "Cubic Inch", "in³", 0.000016387064m, (BaseUnitType.Length, 3));
            Imperial("CubicYard", UnitType.Volume_Mech, "Cubic Yard", "yd³", 0.764554857984m, (BaseUnitType.Length, 3));
            Imperial("CubicMile", UnitType.Volume_Mech, "Cubic Mile", "mi³", 4168181825.440579584m, (BaseUnitType.Length, 3));

            Metric("Liter", UnitType.Volume_Mech, "Liter", "L", 0.001m, (BaseUnitType.Length, 3));
            Imperial("ImperialGallon", UnitType.Volume_Mech, "ImperialGallon", "gal(UK)", 0.00454609m, (BaseUnitType.Length, 3));
            US("UsGallon", UnitType.Volume_Mech, "UsGallon", "gal(US)", 0.003785411784m, (BaseUnitType.Length, 3));
            Imperial("ImperialOunce", UnitType.Volume_Mech, "ImperialOunce", "fl oz(UK)", 2.84130625e-5m, (BaseUnitType.Length, 3));
            US("UsOunce", UnitType.Volume_Mech, "UsOunce", "fl oz(US)", 2.95735295625e-5m, (BaseUnitType.Length, 3));
            Imperial("ImperialPint", UnitType.Volume_Mech, "ImperialPint", "pt(UK)", 0.00056826125m, (BaseUnitType.Length, 3));
            US("UsPint", UnitType.Volume_Mech, "UsPint", "pt(US)", 0.000473176473m, (BaseUnitType.Length, 3));
            US("UsQuart", UnitType.Volume_Mech, "UsQuart", "qt(US)", 0.000946352946m, (BaseUnitType.Length, 3));
            US("UsTablespoon", UnitType.Volume_Mech, "UsTablespoon", "tbsp", 1.47867648e-5m, (BaseUnitType.Length, 3));
            US("UsTeaspoon", UnitType.Volume_Mech, "UsTeaspoon", "tsp", 4.92892159e-6m, (BaseUnitType.Length, 3));
            Other("AuTablespoon", UnitType.Volume_Mech, "AuTablespoon", "tbsp(AU)", 2e-5m, (BaseUnitType.Length, 3));
            Imperial("UkTablespoon", UnitType.Volume_Mech, "UkTablespoon", "tbsp(UK)", 1.77581714e-5m, (BaseUnitType.Length, 3));
            Metric("MetricCup", UnitType.Volume_Mech, "MetricCup", "cup(metric)", 0.00025m, (BaseUnitType.Length, 3));
            Metric("MetricTeaspoon", UnitType.Volume_Mech, "MetricTeaspoon", "tsp(metric)", 5e-6m, (BaseUnitType.Length, 3));
            US("UsCustomaryCup", UnitType.Volume_Mech, "UsCustomaryCup", "cup(US)", 0.0002365882365m, (BaseUnitType.Length, 3));
            US("UsLegalCup", UnitType.Volume_Mech, "UsLegalCup", "cup(legal)", 0.00024m, (BaseUnitType.Length, 3));
            Other("OilBarrel", UnitType.Volume_Mech, "OilBarrel", "bbl", 0.158987294928m, (BaseUnitType.Length, 3));
            US("UsBeerBarrel", UnitType.Volume_Mech, "UsBeerBarrel", "bbl(US)", 0.117347765304m, (BaseUnitType.Length, 3));
            Imperial("ImperialBeerBarrel", UnitType.Volume_Mech, "ImperialBeerBarrel", "bbl(UK)", 0.16365924m, (BaseUnitType.Length, 3));
            Imperial("BoardFoot", UnitType.Volume_Mech, "BoardFoot", "bf", 0.00236127216m, (BaseUnitType.Length, 3));
            Imperial("AcreFoot", UnitType.Volume_Mech, "AcreFoot", "ac·ft", 1233.48183754752m, (BaseUnitType.Length, 3));

            // ===== SURFACE =====
            // À AJOUTER :
            SI("SquareMeter", UnitType.Area_Mech, "Square Meter", "m²", (BaseUnitType.Length, 2));
            Imperial("SquareFoot", UnitType.Area_Mech, "Square Foot", "ft²", 0.09290304m, (BaseUnitType.Length, 2));
            Imperial("SquareInch", UnitType.Area_Mech, "Square Inch", "in²", 0.00064516m, (BaseUnitType.Length, 2));
            Imperial("SquareYard", UnitType.Area_Mech, "Square Yard", "yd²", 0.83612736m, (BaseUnitType.Length, 2));
            Imperial("SquareMile", UnitType.Area_Mech, "Square Mile", "mi²", 2589988.110336m, (BaseUnitType.Length, 2));
            Other("SquareNauticalMile", UnitType.Area_Mech, "Square Nautical Mile", "nmi²", 3429904m, (BaseUnitType.Length, 2));
            Imperial("SquareChain", UnitType.Area_Mech, "Square Chain", "ch²", 404.68564224m, (BaseUnitType.Length, 2));

            Imperial("Acre", UnitType.Area_Mech, "Acre", "ac", 4046.8564224m, (BaseUnitType.Length, 2));
            Metric("Hectare", UnitType.Area_Mech, "Hectare", "ha", 10000m, (BaseUnitType.Length, 2));

            // ===== ÉLECTRICITÉ =====
            SI("Volt", UnitType.ElectricPotential_Elec, "Volt", "V", (BaseUnitType.Mass, 1), (BaseUnitType.Length, 2), (BaseUnitType.Time, -3), (BaseUnitType.ElectricCurrent, -1));
            SI("Ohm", UnitType.ElectricResistance_Elec, "Ohm", "Ω", (BaseUnitType.Mass, 1), (BaseUnitType.Length, 2), (BaseUnitType.Time, -3), (BaseUnitType.ElectricCurrent, -2));
            SI("Farad", UnitType.Capacitance_Elec, "Farad", "F", (BaseUnitType.Mass, -1), (BaseUnitType.Length, -2), (BaseUnitType.Time, 4), (BaseUnitType.ElectricCurrent, 2));
            SI("Henry", UnitType.ElectricInductance_Elec, "Henry", "H", (BaseUnitType.Mass, 1), (BaseUnitType.Length, 2), (BaseUnitType.Time, -2), (BaseUnitType.ElectricCurrent, -2));
            SI("Coulomb", UnitType.ElectricCharge_Elec, "Coulomb", "C", (BaseUnitType.Time, 1), (BaseUnitType.ElectricCurrent, 1));
            SI("Tesla", UnitType.MagneticField_Elec, "Tesla", "T", (BaseUnitType.Mass, 1), (BaseUnitType.Time, -2), (BaseUnitType.ElectricCurrent, -1));
            SI("Weber", UnitType.MagneticFlux_Elec, "Weber", "Wb", (BaseUnitType.Mass, 1), (BaseUnitType.Length, 2), (BaseUnitType.Time, -2), (BaseUnitType.ElectricCurrent, -1));
            Other("Gauss", UnitType.MagneticField_Elec, "Gauss", "G", 1e-4m, (BaseUnitType.Mass, 1), (BaseUnitType.Time, -2), (BaseUnitType.ElectricCurrent, -1));
            SI("Siemens", UnitType.ElectricConductance_Elec, "Siemens", "S", (BaseUnitType.Mass, -1), (BaseUnitType.Length, -2), (BaseUnitType.Time, 3), (BaseUnitType.ElectricCurrent, 2));

            // ===== VISCOSITÉ =====
            SI("PascalSecond", UnitType.DynamicViscosity_Fluid, "Pascal Second", "Pa·s", (BaseUnitType.Mass, 1), (BaseUnitType.Length, -1), (BaseUnitType.Time, -1));
            Other("Poise", UnitType.DynamicViscosity_Fluid, "Poise", "P", 0.1m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, -1), (BaseUnitType.Time, -1));
            Imperial("Reyn", UnitType.DynamicViscosity_Fluid, "Reyn", "reyn", 6894.757293168m, (BaseUnitType.Mass, 1), (BaseUnitType.Length, -1), (BaseUnitType.Time, -1));

            SI("SquareMeterPerSecond", UnitType.KinematicViscosity_Fluid, "Square Meter Per Second", "m²/s", (BaseUnitType.Length, 2), (BaseUnitType.Time, -1));
            Other("Stokes", UnitType.KinematicViscosity_Fluid, "Stokes", "St", 1e-4m, (BaseUnitType.Length, 2), (BaseUnitType.Time, -1));

            // ===== ACCÉLÉRATION =====
            SI("MeterPerSecondSquared", UnitType.Acceleration_Mech, "Meter Per Second Squared", "m/s²", (BaseUnitType.Length, 1), (BaseUnitType.Time, -2));
            Other("StandardGravity", UnitType.Acceleration_Mech, "Standard Gravity", "g", 9.80665m, (BaseUnitType.Length, 1), (BaseUnitType.Time, -2));

            // ===== VITESSE =====
            // À AJOUTER :
            SI("MeterPerSecond", UnitType.Speed_Mech, "Meter Per Second", "m/s", (BaseUnitType.Length, 1), (BaseUnitType.Time, -1));
            Metric("MeterPerHour", UnitType.Speed_Mech, "Meter Per Hour", "m/h", 1m / 3600m, (BaseUnitType.Length, 1), (BaseUnitType.Time, -1));
            Metric("MeterPerMinute", UnitType.Speed_Mech, "Meter Per Minute", "m/min", 1m / 60m, (BaseUnitType.Length, 1), (BaseUnitType.Time, -1));
            Imperial("FootPerSecond", UnitType.Speed_Mech, "Foot Per Second", "ft/s", 0.3048m, (BaseUnitType.Length, 1), (BaseUnitType.Time, -1));
            Imperial("FootPerHour", UnitType.Speed_Mech, "Foot Per Hour", "ft/h", 0.3048m / 3600m, (BaseUnitType.Length, 1), (BaseUnitType.Time, -1));
            Imperial("FootPerMinute", UnitType.Speed_Mech, "Foot Per Minute", "ft/min", 0.3048m / 60m, (BaseUnitType.Length, 1), (BaseUnitType.Time, -1));
            Imperial("MilePerHour", UnitType.Speed_Mech, "Mile Per Hour", "mph", 1609.344m / 3600m, (BaseUnitType.Length, 1), (BaseUnitType.Time, -1));
            Imperial("MilePerSecond", UnitType.Speed_Mech, "Mile Per Second", "mi/s", 1609.344m, (BaseUnitType.Length, 1), (BaseUnitType.Time, -1));
            Imperial("InchPerSecond", UnitType.Speed_Mech, "Inch Per Second", "in/s", 0.0254m, (BaseUnitType.Length, 1), (BaseUnitType.Time, -1));
            Imperial("InchPerMinute", UnitType.Speed_Mech, "Inch Per Minute", "in/min", 0.0254m / 60m, (BaseUnitType.Length, 1), (BaseUnitType.Time, -1));
            Imperial("InchPerHour", UnitType.Speed_Mech, "Inch Per Hour", "in/h", 0.0254m / 3600m, (BaseUnitType.Length, 1), (BaseUnitType.Time, -1));
            Imperial("YardPerSecond", UnitType.Speed_Mech, "Yard Per Second", "yd/s", 0.9144m, (BaseUnitType.Length, 1), (BaseUnitType.Time, -1));
            Imperial("YardPerMinute", UnitType.Speed_Mech, "Yard Per Minute", "yd/min", 0.9144m / 60m, (BaseUnitType.Length, 1), (BaseUnitType.Time, -1));
            Imperial("YardPerHour", UnitType.Speed_Mech, "Yard Per Hour", "yd/h", 0.9144m / 3600m, (BaseUnitType.Length, 1), (BaseUnitType.Time, -1));
            US("UsSurveyFootPerSecond", UnitType.Speed_Mech, "US Survey Foot Per Second", "ft(US)/s", 0.304800609601219m, (BaseUnitType.Length, 1), (BaseUnitType.Time, -1));
            US("UsSurveyFootPerMinute", UnitType.Speed_Mech, "US Survey Foot Per Minute", "ft(US)/min", 0.304800609601219m / 60m, (BaseUnitType.Length, 1), (BaseUnitType.Time, -1));
            US("UsSurveyFootPerHour", UnitType.Speed_Mech, "US Survey Foot Per Hour", "ft(US)/h", 0.304800609601219m / 3600m, (BaseUnitType.Length, 1), (BaseUnitType.Time, -1));

            Other("Knot", UnitType.Speed_Mech, "Knot", "kn", 0.514444444444m, (BaseUnitType.Length, 1), (BaseUnitType.Time, -1));

            // ===== OPTIQUE =====
            SI("Lux", UnitType.Illuminance_Optic, "Lux", "lx", (BaseUnitType.LuminousIntensity, 1), (BaseUnitType.Length, -2));
            SI("Lumen", UnitType.LuminousFlux_Optic, "Lumen", "lm", (BaseUnitType.LuminousIntensity, 1));

            // ===== AUTRES =====
            SI("Decibel", UnitType.Level_Special, "Decibel", "dB", (BaseUnitType.Ratio, 1));
            SI("CubicMeterPerSecond", UnitType.VolumeFlow_Fluid, "Cubic Meter Per Second", "m³/s", (BaseUnitType.Length, 3), (BaseUnitType.Time, -1));
            Other("MillionUsGallonsPerDay", UnitType.VolumeFlow_Fluid, "MillionUsGallonsPerDay", "MGD", 0.0437719298m, (BaseUnitType.Length, 3), (BaseUnitType.Time, -1));
        }

        private static void LoadBaseUnits(List<PhysicalUnit> units)
        {
            // Helper pour ajouter une unité
            void Add(string key)
            {
                var unit = GetRegisteredUnit(key);
                if (unit != null)
                {
                    units.Add(unit);
                }
            }

            // ===== LONGUEUR =====
            Add("Meter");        // SI
            Add("Foot");
            Add("Inch");
            Add("Mile");
            Add("Yard");
            Add("Chain");
            Add("Fathom");
            Add("Mil");
            Add("Hand");
            Add("UsSurveyFoot");
            Add("NauticalMile");
            Add("LightYear");
            Add("AstronomicalUnit");
            Add("Parsec");
            Add("SolarRadius");
            Add("Twip");
            Add("DtpPoint");
            Add("DtpPica");
            Add("Shackle");

            // ===== MASSE =====
            Add("Kilogram");     // SI
            Add("Gram");
            Add("Tonne");
            Add("Pound");
            Add("Ounce");
            Add("Stone");
            Add("Grain");
            Add("ShortTon");
            Add("LongTon");
            Add("Slug");
            Add("ShortHundredweight");
            Add("LongHundredweight");
            Add("EarthMass");

            // ===== TEMPS =====
            Add("Second");       // SI
            Add("Minute");
            Add("Hour");
            Add("Day");
            Add("Week");
            Add("Month30");
            Add("Year365");

            // ===== COURANT ÉLECTRIQUE =====
            Add("Ampere");       // SI

            // ===== TEMPÉRATURE =====
            Add("Kelvin");       // SI
            Add("Celsius");
            Add("Fahrenheit");
            Add("Rankine");

            // ===== QUANTITÉ DE MATIÈRE =====
            Add("Mole");         // SI
            Add("PoundMole");

            // ===== INTENSITÉ LUMINEUSE =====
            Add("Candela");      // SI

            // ===== ANGLE =====
            Add("Radian");       // SI
            Add("Degree");
            Add("Revolution");
            Add("Gradian");
            Add("Arcminute");
            Add("Arcsecond");

            // ===== RATIO =====
            Add("Ratio");
            Add("Percent");
            Add("PartPerThousand");
            Add("PartPerMillion");
            Add("PartPerBillion");
            Add("PartPerTrillion");

            // ===== MONNAIE =====
            Add("Currency");
            Add("USDollar");
            Add("Euro");
            Add("BritishPound");

            // ===== INFORMATION =====
            Add("Bit");
            Add("Byte");
        }

        private static void LoadMechanicalUnits(List<PhysicalUnit> units)
        {
            // Helpers
            void Add(PhysicalUnit unit) => units.Add(unit);
            void AddComposite(UnitType type, params (string unitKey, int exponent)[] components)
                => Add(Composite(type, components));

            // ===== SURFACE =====
            Add(GetRegisteredUnit("SquareMeter"));
            Add(GetRegisteredUnit("SquareFoot"));
            Add(GetRegisteredUnit("SquareInch"));
            Add(GetRegisteredUnit("SquareYard"));
            Add(GetRegisteredUnit("SquareMile"));
            Add(GetRegisteredUnit("SquareNauticalMile"));
            Add(GetRegisteredUnit("SquareChain"));
            Add(GetRegisteredUnit("Acre"));
            Add(GetRegisteredUnit("Hectare"));

            // ===== VOLUME =====
            Add(GetRegisteredUnit("CubicMeter"));
            Add(GetRegisteredUnit("CubicFoot"));
            Add(GetRegisteredUnit("CubicInch"));
            Add(GetRegisteredUnit("CubicYard"));
            Add(GetRegisteredUnit("CubicMile"));
            Add(GetRegisteredUnit("Liter"));
            Add(GetRegisteredUnit("ImperialGallon"));
            Add(GetRegisteredUnit("UsGallon"));
            Add(GetRegisteredUnit("ImperialOunce"));
            Add(GetRegisteredUnit("UsOunce"));
            Add(GetRegisteredUnit("ImperialPint"));
            Add(GetRegisteredUnit("UsPint"));
            Add(GetRegisteredUnit("UsQuart"));
            Add(GetRegisteredUnit("UsTablespoon"));
            Add(GetRegisteredUnit("UsTeaspoon"));
            Add(GetRegisteredUnit("AuTablespoon"));
            Add(GetRegisteredUnit("UkTablespoon"));
            Add(GetRegisteredUnit("MetricCup"));
            Add(GetRegisteredUnit("MetricTeaspoon"));
            Add(GetRegisteredUnit("UsCustomaryCup"));
            Add(GetRegisteredUnit("UsLegalCup"));
            Add(GetRegisteredUnit("OilBarrel"));
            Add(GetRegisteredUnit("UsBeerBarrel"));
            Add(GetRegisteredUnit("ImperialBeerBarrel"));
            Add(GetRegisteredUnit("BoardFoot"));
            Add(GetRegisteredUnit("AcreFoot"));

            // ===== VITESSE =====
            Add(GetRegisteredUnit("MeterPerSecond"));
            Add(GetRegisteredUnit("MeterPerHour"));
            Add(GetRegisteredUnit("MeterPerMinute"));
            Add(GetRegisteredUnit("FootPerSecond"));
            Add(GetRegisteredUnit("FootPerHour"));
            Add(GetRegisteredUnit("FootPerMinute"));
            Add(GetRegisteredUnit("MilePerHour"));
            Add(GetRegisteredUnit("MilePerSecond"));
            Add(GetRegisteredUnit("InchPerSecond"));
            Add(GetRegisteredUnit("InchPerMinute"));
            Add(GetRegisteredUnit("InchPerHour"));
            Add(GetRegisteredUnit("YardPerSecond"));
            Add(GetRegisteredUnit("YardPerMinute"));
            Add(GetRegisteredUnit("YardPerHour"));
            Add(GetRegisteredUnit("UsSurveyFootPerSecond"));
            Add(GetRegisteredUnit("UsSurveyFootPerMinute"));
            Add(GetRegisteredUnit("UsSurveyFootPerHour"));
            Add(GetRegisteredUnit("Knot"));

            // ===== ACCÉLÉRATION =====
            Add(GetRegisteredUnit("MeterPerSecondSquared"));
            AddComposite(UnitType.Acceleration_Mech, ("Foot", 1), ("Second", -2));
            AddComposite(UnitType.Acceleration_Mech, ("Inch", 1), ("Second", -2));
            AddComposite(UnitType.Acceleration_Mech, ("Knot", 1), ("Second", -1));
            AddComposite(UnitType.Acceleration_Mech, ("Knot", 1), ("Minute", -1));
            AddComposite(UnitType.Acceleration_Mech, ("Knot", 1), ("Hour", -1));
            Add(GetRegisteredUnit("StandardGravity"));

            // ===== JERK =====
            AddComposite(UnitType.Jerk_Mech, ("Meter", 1), ("Second", -3));

            // ===== FORCE =====
            Add(GetRegisteredUnit("Newton"));
            Add(GetRegisteredUnit("Dyn"));
            Add(GetRegisteredUnit("PoundForce"));
            Add(GetRegisteredUnit("KilogramForce"));
            Add(GetRegisteredUnit("TonneForce"));
            Add(GetRegisteredUnit("Poundal"));
            Add(GetRegisteredUnit("OunceForce"));
            Add(GetRegisteredUnit("ShortTonForce"));

            // ===== COUPLE =====
            AddComposite(UnitType.Torque_Mech, ("Newton", 1), ("Meter", 1));
            AddComposite(UnitType.Torque_Mech, ("PoundForce", 1), ("Foot", 1));
            AddComposite(UnitType.Torque_Mech, ("PoundForce", 1), ("Inch", 1));
            AddComposite(UnitType.Torque_Mech, ("Poundal", 1), ("Foot", 1));
            AddComposite(UnitType.Torque_Mech, ("KilogramForce", 1), ("Meter", 1));
            AddComposite(UnitType.Torque_Mech, ("TonneForce", 1), ("Meter", 1));

            // ===== PRESSION =====
            Add(GetRegisteredUnit("Pascal"));
            Add(GetRegisteredUnit("Bar"));
            Add(GetRegisteredUnit("Atmosphere"));
            Add(GetRegisteredUnit("TechnicalAtmosphere"));
            Add(GetRegisteredUnit("Torr"));
            Add(GetRegisteredUnit("PoundPerSquareInch"));
            Add(GetRegisteredUnit("PoundPerSquareFoot"));
            Add(GetRegisteredUnit("InchOfMercury"));
            Add(GetRegisteredUnit("InchOfWaterColumn"));
            Add(GetRegisteredUnit("FootOfHead"));
            Add(GetRegisteredUnit("MeterOfHead"));
            Add(GetRegisteredUnit("MillimeterOfMercury"));
            AddComposite(UnitType.Pressure_Mech, ("KilogramForce", 1), ("Meter", -2));
            AddComposite(UnitType.Pressure_Mech, ("TonneForce", 1), ("Meter", -2));
            AddComposite(UnitType.Pressure_Mech, ("Pound", 1), ("Inch", -1), ("Second", -2)); // PoundPerInchSecondSquared

            // ===== ÉNERGIE =====
            Add(GetRegisteredUnit("Joule"));
            Add(GetRegisteredUnit("Calorie"));
            Add(GetRegisteredUnit("BritishThermalUnit"));
            Add(GetRegisteredUnit("ElectronVolt"));
            Add(GetRegisteredUnit("FootPound"));
            Add(GetRegisteredUnit("Erg"));
            Add(GetRegisteredUnit("ThermEc"));
            Add(GetRegisteredUnit("ThermUs"));
            Add(GetRegisteredUnit("ThermImperial"));
            AddComposite(UnitType.Energy_Mech, ("Watt", 1), ("Hour", 1));
            AddComposite(UnitType.Energy_Mech, ("Watt", 1), ("Day", 1));
            AddComposite(UnitType.Energy_Mech, ("MechanicalHorsepower", 1), ("Hour", 1));

            // ===== PUISSANCE =====
            Add(GetRegisteredUnit("Watt"));
            Add(GetRegisteredUnit("MechanicalHorsepower"));
            Add(GetRegisteredUnit("MetricHorsepower"));
            Add(GetRegisteredUnit("ElectricalHorsepower"));
            Add(GetRegisteredUnit("BoilerHorsepower"));
            Add(GetRegisteredUnit("HydraulicHorsepower"));
            Add(GetRegisteredUnit("SolarLuminosity"));
            AddComposite(UnitType.Power_Mech, ("BritishThermalUnit", 1), ("Hour", -1));
            AddComposite(UnitType.Power_Mech, ("BritishThermalUnit", 1), ("Minute", -1));
            AddComposite(UnitType.Power_Mech, ("BritishThermalUnit", 1), ("Second", -1));
            AddComposite(UnitType.Power_Mech, ("Calorie", 1), ("Second", -1));
            AddComposite(UnitType.Power_Mech, ("Joule", 1), ("Hour", -1));

            // ===== DENSITÉ =====
            AddComposite(UnitType.Density_Mech, ("Kilogram", 1), ("Meter", -3));
            AddComposite(UnitType.Density_Mech, ("Gram", 1), ("Liter", -1));
            AddComposite(UnitType.Density_Mech, ("Kilogram", 1), ("Liter", -1));
            AddComposite(UnitType.Density_Mech, ("Pound", 1), ("Foot", -3));
            AddComposite(UnitType.Density_Mech, ("Pound", 1), ("Inch", -3));
            AddComposite(UnitType.Density_Mech, ("Pound", 1), ("ImperialGallon", -1));
            AddComposite(UnitType.Density_Mech, ("Pound", 1), ("UsGallon", -1));
            AddComposite(UnitType.Density_Mech, ("Slug", 1), ("Foot", -3));
            AddComposite(UnitType.Density_Mech, ("Tonne", 1), ("Meter", -3));

            // ===== FRÉQUENCE =====
            Add(GetRegisteredUnit("Hertz"));
            Add(GetRegisteredUnit("RevolutionsPerMinute"));
            Add(GetRegisteredUnit("BeatPerMinute"));
            AddComposite(UnitType.Frequency_Mech, ("Radian", 1), ("Second", -1));

            // ===== VITESSE ANGULAIRE =====
            AddComposite(UnitType.RotationalSpeed_Mech, ("Radian", 1), ("Second", -1));
            AddComposite(UnitType.RotationalSpeed_Mech, ("Degree", 1), ("Second", -1));
            AddComposite(UnitType.RotationalSpeed_Mech, ("Revolution", 1), ("Minute", -1));
            Add(GetRegisteredUnit("Hertz"));
            Add(GetRegisteredUnit("BeatPerMinute"));

            // ===== MOMENT D'INERTIE MASSIQUE =====
            AddComposite(UnitType.MassMomentOfInertia_Mech, ("Kilogram", 1), ("Meter", 2));
            AddComposite(UnitType.MassMomentOfInertia_Mech, ("Gram", 1), ("Meter", 2));
            AddComposite(UnitType.MassMomentOfInertia_Mech, ("Pound", 1), ("Foot", 2));
            AddComposite(UnitType.MassMomentOfInertia_Mech, ("Pound", 1), ("Inch", 2));
            AddComposite(UnitType.MassMomentOfInertia_Mech, ("Slug", 1), ("Foot", 2));
            AddComposite(UnitType.MassMomentOfInertia_Mech, ("Slug", 1), ("Inch", 2));
            AddComposite(UnitType.MassMomentOfInertia_Mech, ("Tonne", 1), ("Meter", 2));

            // ===== MOMENT D'INERTIE DE SURFACE =====
            AddComposite(UnitType.AreaMomentOfInertia_Mech, ("Meter", 4));
            AddComposite(UnitType.AreaMomentOfInertia_Mech, ("Foot", 4));
            AddComposite(UnitType.AreaMomentOfInertia_Mech, ("Inch", 4));

            // ===== MOMENT D'INERTIE DE GAUCHISSEMENT =====
            AddComposite(UnitType.WarpingMomentOfInertia_Mech, ("Meter", 6));
            AddComposite(UnitType.WarpingMomentOfInertia_Mech, ("Foot", 6));
            AddComposite(UnitType.WarpingMomentOfInertia_Mech, ("Inch", 6));

            // ===== FORCE PAR LONGUEUR =====
            AddComposite(UnitType.ForcePerLength_Mech, ("Newton", 1), ("Meter", -1));
            AddComposite(UnitType.ForcePerLength_Mech, ("PoundForce", 1), ("Foot", -1));
            AddComposite(UnitType.ForcePerLength_Mech, ("PoundForce", 1), ("Inch", -1));
            AddComposite(UnitType.ForcePerLength_Mech, ("PoundForce", 1), ("Yard", -1));
            AddComposite(UnitType.ForcePerLength_Mech, ("KilogramForce", 1), ("Meter", -1));
            AddComposite(UnitType.ForcePerLength_Mech, ("TonneForce", 1), ("Meter", -1));

            // ===== COUPLE PAR LONGUEUR =====
            AddComposite(UnitType.TorquePerLength_Mech, ("Newton", 1), ("Meter", 1), ("Meter", -1));
            AddComposite(UnitType.TorquePerLength_Mech, ("PoundForce", 1), ("Foot", 1), ("Foot", -1));
            AddComposite(UnitType.TorquePerLength_Mech, ("PoundForce", 1), ("Inch", 1), ("Foot", -1));
            AddComposite(UnitType.TorquePerLength_Mech, ("KilogramForce", 1), ("Meter", 1), ("Meter", -1));
            AddComposite(UnitType.TorquePerLength_Mech, ("TonneForce", 1), ("Meter", 1), ("Meter", -1));

            // ===== DENSITÉ LINÉAIRE =====
            AddComposite(UnitType.LinearDensity_Mech, ("Kilogram", 1), ("Meter", -1));
            AddComposite(UnitType.LinearDensity_Mech, ("Gram", 1), ("Meter", -1));
            AddComposite(UnitType.LinearDensity_Mech, ("Pound", 1), ("Foot", -1));
            AddComposite(UnitType.LinearDensity_Mech, ("Pound", 1), ("Inch", -1));

            // ===== DENSITÉ SURFACIQUE =====
            AddComposite(UnitType.AreaDensity_Mech, ("Kilogram", 1), ("Meter", -2));

            // ===== VOLUME SPÉCIFIQUE =====
            AddComposite(UnitType.SpecificVolume_Mech, ("Meter", 3), ("Kilogram", -1));

            // ===== POIDS SPÉCIFIQUE =====
            AddComposite(UnitType.SpecificWeight_Mech, ("Newton", 1), ("Meter", -3));
            AddComposite(UnitType.SpecificWeight_Mech, ("PoundForce", 1), ("Foot", -3));
            AddComposite(UnitType.SpecificWeight_Mech, ("PoundForce", 1), ("Inch", -3));
            AddComposite(UnitType.SpecificWeight_Mech, ("KilogramForce", 1), ("Meter", -3));
            AddComposite(UnitType.SpecificWeight_Mech, ("TonneForce", 1), ("Meter", -3));

            // ===== ÉNERGIE SPÉCIFIQUE =====
            AddComposite(UnitType.SpecificEnergy_Mech, ("Joule", 1), ("Kilogram", -1));
            AddComposite(UnitType.SpecificEnergy_Mech, ("BritishThermalUnit", 1), ("Pound", -1));
            AddComposite(UnitType.SpecificEnergy_Mech, ("Calorie", 1), ("Gram", -1));
            AddComposite(UnitType.SpecificEnergy_Mech, ("Watt", 1), ("Hour", 1), ("Kilogram", -1));
            AddComposite(UnitType.SpecificEnergy_Mech, ("Watt", 1), ("Day", 1), ("Kilogram", -1));
            AddComposite(UnitType.SpecificEnergy_Mech, ("Watt", 1), ("Day", 1), ("Tonne", -1));
            AddComposite(UnitType.SpecificEnergy_Mech, ("Watt", 1), ("Day", 1), ("ShortTon", -1));

            // ===== DENSITÉ DE PUISSANCE =====
            AddComposite(UnitType.PowerDensity_Mech, ("Watt", 1), ("Meter", -3));
            AddComposite(UnitType.PowerDensity_Mech, ("Watt", 1), ("Liter", -1));
            AddComposite(UnitType.PowerDensity_Mech, ("Watt", 1), ("Inch", -3));
            AddComposite(UnitType.PowerDensity_Mech, ("Watt", 1), ("Foot", -3));

            // ===== DENSITÉ LINÉAIRE DE PUISSANCE =====
            AddComposite(UnitType.LinearPowerDensity_Mech, ("Watt", 1), ("Meter", -1));
            AddComposite(UnitType.LinearPowerDensity_Mech, ("Watt", 1), ("Foot", -1));
            AddComposite(UnitType.LinearPowerDensity_Mech, ("Watt", 1), ("Inch", -1));

            // ===== TAUX DE CHANGEMENT DE FORCE =====
            AddComposite(UnitType.ForceChangeRate_Mech, ("Newton", 1), ("Second", -1));
            AddComposite(UnitType.ForceChangeRate_Mech, ("Newton", 1), ("Minute", -1));
            AddComposite(UnitType.ForceChangeRate_Mech, ("PoundForce", 1), ("Second", -1));
            AddComposite(UnitType.ForceChangeRate_Mech, ("PoundForce", 1), ("Minute", -1));

            // ===== TAUX DE CHANGEMENT DE PRESSION =====
            AddComposite(UnitType.PressureChangeRate_Mech, ("Pascal", 1), ("Second", -1));
            AddComposite(UnitType.PressureChangeRate_Mech, ("Pascal", 1), ("Minute", -1));
            AddComposite(UnitType.PressureChangeRate_Mech, ("Atmosphere", 1), ("Second", -1));
            AddComposite(UnitType.PressureChangeRate_Mech, ("PoundPerSquareInch", 1), ("Second", -1));
            AddComposite(UnitType.PressureChangeRate_Mech, ("PoundPerSquareInch", 1), ("Minute", -1));
        }

        private static void LoadElectricalUnits(List<PhysicalUnit> units)
        {
            // Helpers
            void Add(PhysicalUnit unit) => units.Add(unit);
            void AddComposite(UnitType type, params (string unitKey, int exponent)[] components)
                => Add(Composite(type, components));

            // ===== CHARGE ÉLECTRIQUE =====
            Add(GetRegisteredUnit("Coulomb"));
            AddComposite(UnitType.ElectricCharge_Elec, ("Ampere", 1), ("Hour", 1));

            // ===== POTENTIEL ÉLECTRIQUE =====
            Add(GetRegisteredUnit("Volt"));

            // ===== RÉSISTANCE ÉLECTRIQUE =====
            Add(GetRegisteredUnit("Ohm"));

            // ===== CONDUCTANCE ÉLECTRIQUE =====
            Add(GetRegisteredUnit("Siemens"));

            // ===== CAPACITÉ =====
            Add(GetRegisteredUnit("Farad"));

            // ===== INDUCTANCE =====
            Add(GetRegisteredUnit("Henry"));

            // ===== CHAMP MAGNÉTIQUE =====
            Add(GetRegisteredUnit("Tesla"));
            Add(GetRegisteredUnit("Gauss"));

            // ===== FLUX MAGNÉTIQUE =====
            Add(GetRegisteredUnit("Weber"));

            // ===== CONDUCTIVITÉ ÉLECTRIQUE =====
            AddComposite(UnitType.ElectricConductivity_Elec, ("Siemens", 1), ("Meter", -1));
            AddComposite(UnitType.ElectricConductivity_Elec, ("Siemens", 1), ("Inch", -1));
            AddComposite(UnitType.ElectricConductivity_Elec, ("Siemens", 1), ("Foot", -1));

            // ===== RÉSISTIVITÉ ÉLECTRIQUE =====
            AddComposite(UnitType.ElectricResistivity_Elec, ("Ohm", 1), ("Meter", 1));

            // ===== PUISSANCE APPARENTE =====
            AddComposite(UnitType.ApparentPower_Elec, ("Volt", 1), ("Ampere", 1));

            // ===== PUISSANCE RÉACTIVE =====
            AddComposite(UnitType.ReactivePower_Elec, ("Volt", 1), ("Ampere", 1)); // var (voltampere reactive)

            // ===== ÉNERGIE APPARENTE =====
            AddComposite(UnitType.ApparentEnergy_Elec, ("Volt", 1), ("Ampere", 1), ("Hour", 1));

            // ===== ÉNERGIE RÉACTIVE =====
            AddComposite(UnitType.ReactiveEnergy_Elec, ("Volt", 1), ("Ampere", 1), ("Hour", 1)); // varh

            // ===== DENSITÉ DE COURANT =====
            AddComposite(UnitType.ElectricCurrentDensity_Elec, ("Ampere", 1), ("Meter", -2));
            AddComposite(UnitType.ElectricCurrentDensity_Elec, ("Ampere", 1), ("Inch", -2));
            AddComposite(UnitType.ElectricCurrentDensity_Elec, ("Ampere", 1), ("Foot", -2));

            // ===== CHAMP ÉLECTRIQUE =====
            AddComposite(UnitType.ElectricField_Elec, ("Volt", 1), ("Meter", -1));

            // ===== DENSITÉ DE CHARGE =====
            AddComposite(UnitType.ElectricChargeDensity_Elec, ("Coulomb", 1), ("Meter", -3));

            // ===== DENSITÉ SURFACIQUE DE CHARGE =====
            AddComposite(UnitType.ElectricSurfaceChargeDensity_Elec, ("Coulomb", 1), ("Meter", -2));
            AddComposite(UnitType.ElectricSurfaceChargeDensity_Elec, ("Coulomb", 1), ("Inch", -2));

            // ===== GRADIENT DE COURANT =====
            AddComposite(UnitType.ElectricCurrentGradient_Elec, ("Ampere", 1), ("Second", -1));

            // ===== TAUX DE CHANGEMENT DE POTENTIEL =====
            AddComposite(UnitType.ElectricPotentialChangeRate_Elec, ("Volt", 1), ("Second", -1));
            AddComposite(UnitType.ElectricPotentialChangeRate_Elec, ("Volt", 1), ("Minute", -1));
            AddComposite(UnitType.ElectricPotentialChangeRate_Elec, ("Volt", 1), ("Hour", -1));

            // ===== PERMÉABILITÉ =====
            AddComposite(UnitType.Permeability_Elec, ("Henry", 1), ("Meter", -1));

            // ===== PERMITTIVITÉ =====
            AddComposite(UnitType.Permittivity_Elec, ("Farad", 1), ("Meter", -1));

            // ===== MAGNÉTISATION =====
            AddComposite(UnitType.Magnetization_Elec, ("Ampere", 1), ("Meter", -1));
        }

        private static void LoadThermodynamicUnits(List<PhysicalUnit> units)
        {
            // Helpers
            void Add(PhysicalUnit unit) => units.Add(unit);
            void AddComposite(UnitType type, params (string unitKey, int exponent)[] components)
                => Add(Composite(type, components));

            // ===== FLUX THERMIQUE =====
            AddComposite(UnitType.HeatFlux_Thermo, ("Watt", 1), ("Meter", -2));
            AddComposite(UnitType.HeatFlux_Thermo, ("Watt", 1), ("Inch", -2));
            AddComposite(UnitType.HeatFlux_Thermo, ("Watt", 1), ("Foot", -2));
            AddComposite(UnitType.HeatFlux_Thermo, ("BritishThermalUnit", 1), ("Hour", -1), ("Foot", -2));
            AddComposite(UnitType.HeatFlux_Thermo, ("BritishThermalUnit", 1), ("Minute", -1), ("Foot", -2));
            AddComposite(UnitType.HeatFlux_Thermo, ("BritishThermalUnit", 1), ("Second", -1), ("Foot", -2));
            AddComposite(UnitType.HeatFlux_Thermo, ("BritishThermalUnit", 1), ("Second", -1), ("Inch", -2));
            AddComposite(UnitType.HeatFlux_Thermo, ("PoundForce", 1), ("Foot", 1), ("Second", -1));
            AddComposite(UnitType.HeatFlux_Thermo, ("Pound", 1), ("Second", -3)); // PoundPerSecondCubed

            // ===== COEFFICIENT DE TRANSFERT THERMIQUE =====
            AddComposite(UnitType.HeatTransferCoefficient_Thermo, ("Watt", 1), ("Meter", -2), ("Kelvin", -1));
            AddComposite(UnitType.HeatTransferCoefficient_Thermo, ("Watt", 1), ("Meter", -2), ("Celsius", -1));
            AddComposite(UnitType.HeatTransferCoefficient_Thermo, ("BritishThermalUnit", 1), ("Foot", -2), ("Fahrenheit", -1));

            // ===== CONDUCTIVITÉ THERMIQUE =====
            AddComposite(UnitType.ThermalConductivity_Thermo, ("Watt", 1), ("Meter", -1), ("Kelvin", -1));
            AddComposite(UnitType.ThermalConductivity_Thermo, ("BritishThermalUnit", 1), ("Hour", -1), ("Foot", -1), ("Fahrenheit", -1));

            // ===== RÉSISTANCE THERMIQUE =====
            AddComposite(UnitType.ThermalResistance_Thermo, ("Meter", 2), ("Celsius", 1), ("Watt", -1));
            AddComposite(UnitType.ThermalResistance_Thermo, ("Meter", 2), ("Kelvin", 1), ("Watt", -1));
            AddComposite(UnitType.ThermalResistance_Thermo, ("Hour", 1), ("Foot", 2), ("Fahrenheit", 1), ("BritishThermalUnit", -1));

            // ===== RÉSISTANCE THERMIQUE SPÉCIFIQUE =====
            AddComposite(UnitType.SpecificThermalResistance_Thermo, ("Meter", 1), ("Celsius", 1), ("Watt", -1));
            AddComposite(UnitType.SpecificThermalResistance_Thermo, ("Meter", 1), ("Kelvin", 1), ("Watt", -1));

            // ===== COEFFICIENT DE TRANSFERT VOLUMÉTRIQUE =====
            AddComposite(UnitType.VolumetricHeatTransferCoefficient_Thermo, ("Watt", 1), ("Meter", -3), ("Kelvin", -1));

            // ===== CAPACITÉ THERMIQUE SPÉCIFIQUE =====
            AddComposite(UnitType.SpecificHeatCapacity_Thermo, ("Joule", 1), ("Kilogram", -1), ("Kelvin", -1));
            AddComposite(UnitType.SpecificHeatCapacity_Thermo, ("Joule", 1), ("Kilogram", -1), ("Celsius", -1));
            AddComposite(UnitType.SpecificHeatCapacity_Thermo, ("Joule", 1), ("Kilogram", -1), ("Rankine", -1));
            AddComposite(UnitType.SpecificHeatCapacity_Thermo, ("Calorie", 1), ("Gram", -1), ("Kelvin", -1));
            AddComposite(UnitType.SpecificHeatCapacity_Thermo, ("BritishThermalUnit", 1), ("Pound", -1), ("Kelvin", -1));
            AddComposite(UnitType.SpecificHeatCapacity_Thermo, ("BritishThermalUnit", 1), ("Pound", -1), ("Rankine", -1));
            AddComposite(UnitType.SpecificHeatCapacity_Thermo, ("BritishThermalUnit", 1), ("Pound", -1), ("Fahrenheit", -1));

            // ===== ENTHALPIE SPÉCIFIQUE =====
            AddComposite(UnitType.Enthalpy_Thermo, ("Joule", 1), ("Kilogram", -1));
            AddComposite(UnitType.Enthalpy_Thermo, ("BritishThermalUnit", 1), ("Pound", -1));
            AddComposite(UnitType.Enthalpy_Thermo, ("Calorie", 1), ("Gram", -1));
            AddComposite(UnitType.Enthalpy_Thermo, ("Watt", 1), ("Day", 1), ("Kilogram", -1));
            AddComposite(UnitType.Enthalpy_Thermo, ("Watt", 1), ("Hour", 1), ("Kilogram", -1));
            AddComposite(UnitType.Enthalpy_Thermo, ("Watt", 1), ("Day", 1), ("Tonne", -1));
            AddComposite(UnitType.Enthalpy_Thermo, ("Watt", 1), ("Day", 1), ("ShortTon", -1));

            // ===== ENTROPIE =====
            AddComposite(UnitType.Entropy_Thermo, ("Joule", 1), ("Kelvin", -1));
            AddComposite(UnitType.Entropy_Thermo, ("Calorie", 1), ("Kelvin", -1));
            AddComposite(UnitType.Entropy_Thermo, ("Joule", 1), ("Celsius", -1));

            // ===== ENTROPIE SPÉCIFIQUE =====
            AddComposite(UnitType.SpecificEntropy_Thermo, ("Joule", 1), ("Kilogram", -1), ("Kelvin", -1));
            AddComposite(UnitType.SpecificEntropy_Thermo, ("Joule", 1), ("Kilogram", -1), ("Celsius", -1));
            AddComposite(UnitType.SpecificEntropy_Thermo, ("Joule", 1), ("Kilogram", -1), ("Rankine", -1));
            AddComposite(UnitType.SpecificEntropy_Thermo, ("Calorie", 1), ("Gram", -1), ("Kelvin", -1));
            AddComposite(UnitType.SpecificEntropy_Thermo, ("BritishThermalUnit", 1), ("Pound", -1), ("Kelvin", -1));
            AddComposite(UnitType.SpecificEntropy_Thermo, ("BritishThermalUnit", 1), ("Pound", -1), ("Rankine", -1));
            AddComposite(UnitType.SpecificEntropy_Thermo, ("BritishThermalUnit", 1), ("Pound", -1), ("Fahrenheit", -1));

            // ===== COEFFICIENT DE DILATATION THERMIQUE =====
            AddComposite(UnitType.CoefficientOfThermalExpansion_Thermo, ("Kelvin", -1));
            AddComposite(UnitType.CoefficientOfThermalExpansion_Thermo, ("Celsius", -1));
            AddComposite(UnitType.CoefficientOfThermalExpansion_Thermo, ("Fahrenheit", -1));

            // ===== TAUX DE CHANGEMENT DE TEMPÉRATURE =====
            AddComposite(UnitType.TemperatureChangeRate_Thermo, ("Celsius", 1), ("Second", -1));
            AddComposite(UnitType.TemperatureChangeRate_Thermo, ("Celsius", 1), ("Minute", -1));

            // ===== GRADIENT THERMIQUE (LAPSE RATE) =====
            AddComposite(UnitType.LapseRate_Thermo, ("Celsius", 1), ("Meter", -1));
        }

        private static void LoadFluidUnits(List<PhysicalUnit> units)
        {
            // Helpers
            void Add(PhysicalUnit unit) => units.Add(unit);
            void AddComposite(UnitType type, params (string unitKey, int exponent)[] components)
                => Add(Composite(type, components));

            // ===== DÉBIT VOLUMIQUE =====
            Add(GetRegisteredUnit("CubicMeterPerSecond"));
            
            Add(GetRegisteredUnit("SquareMeterPerSecond"));
            AddComposite(UnitType.VolumeFlow_Fluid, ("Meter", 3), ("Hour", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("Meter", 3), ("Day", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("Meter", 3), ("Minute", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("Foot", 3), ("Second", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("Foot", 3), ("Hour", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("Foot", 3), ("Minute", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("Yard", 3), ("Second", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("Yard", 3), ("Hour", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("Yard", 3), ("Day", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("Yard", 3), ("Minute", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("Liter", 1), ("Second", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("Liter", 1), ("Minute", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("Liter", 1), ("Hour", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("Liter", 1), ("Day", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("UsGallon", 1), ("Minute", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("UsGallon", 1), ("Hour", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("UsGallon", 1), ("Day", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("UsGallon", 1), ("Second", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("ImperialGallon", 1), ("Minute", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("ImperialGallon", 1), ("Hour", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("ImperialGallon", 1), ("Day", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("ImperialGallon", 1), ("Second", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("OilBarrel", 1), ("Day", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("OilBarrel", 1), ("Hour", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("OilBarrel", 1), ("Minute", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("OilBarrel", 1), ("Second", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("AcreFoot", 1), ("Day", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("AcreFoot", 1), ("Hour", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("AcreFoot", 1), ("Minute", -1));
            AddComposite(UnitType.VolumeFlow_Fluid, ("AcreFoot", 1), ("Second", -1));
            Add(GetRegisteredUnit("MillionUsGallonsPerDay"));

            // ===== DÉBIT MASSIQUE =====

            AddComposite(UnitType.MassFlow_Fluid, ("Kilogram", 1), ("Second", -1));
            AddComposite(UnitType.MassFlow_Fluid, ("Kilogram", 1), ("Minute", -1));
            AddComposite(UnitType.MassFlow_Fluid, ("Kilogram", 1), ("Hour", -1));
            AddComposite(UnitType.MassFlow_Fluid, ("Kilogram", 1), ("Day", -1));
            AddComposite(UnitType.MassFlow_Fluid, ("Gram", 1), ("Second", -1));
            AddComposite(UnitType.MassFlow_Fluid, ("Gram", 1), ("Hour", -1));
            AddComposite(UnitType.MassFlow_Fluid, ("Gram", 1), ("Day", -1));
            AddComposite(UnitType.MassFlow_Fluid, ("Tonne", 1), ("Day", -1));
            AddComposite(UnitType.MassFlow_Fluid, ("Tonne", 1), ("Hour", -1));
            AddComposite(UnitType.MassFlow_Fluid, ("Pound", 1), ("Second", -1));
            AddComposite(UnitType.MassFlow_Fluid, ("Pound", 1), ("Minute", -1));
            AddComposite(UnitType.MassFlow_Fluid, ("Pound", 1), ("Hour", -1));
            AddComposite(UnitType.MassFlow_Fluid, ("Pound", 1), ("Day", -1));
            AddComposite(UnitType.MassFlow_Fluid, ("ShortTon", 1), ("Hour", -1));

            // ===== FLUX MASSIQUE =====
            AddComposite(UnitType.MassFlux_Fluid, ("Kilogram", 1), ("Second", -1), ("Meter", -2));
            AddComposite(UnitType.MassFlux_Fluid, ("Gram", 1), ("Hour", -1), ("Meter", -2));
            AddComposite(UnitType.MassFlux_Fluid, ("Gram", 1), ("Second", -1), ("Meter", -2));
            AddComposite(UnitType.MassFlux_Fluid, ("Kilogram", 1), ("Hour", -1), ("Meter", -2));

            // ===== VISCOSITÉ DYNAMIQUE =====
            Add(GetRegisteredUnit("Poise"));
            Add(GetRegisteredUnit("Reyn"));
            Add(GetRegisteredUnit("PascalSecond"));
            AddComposite(UnitType.DynamicViscosity_Fluid, ("Newton", 1), ("Second", 1), ("Meter", -2));
            AddComposite(UnitType.DynamicViscosity_Fluid, ("Pound", 1), ("Foot", -1), ("Second", -1));
            AddComposite(UnitType.DynamicViscosity_Fluid, ("PoundForce", 1), ("Second", 1), ("Foot", -2));
            AddComposite(UnitType.DynamicViscosity_Fluid, ("PoundForce", 1), ("Second", 1), ("Inch", -2));

            // ===== VISCOSITÉ CINÉMATIQUE =====
            Add(GetRegisteredUnit("SquareMeterPerSecond"));
            Add(GetRegisteredUnit("Stokes"));

            // ===== VOLUME PAR LONGUEUR =====
            AddComposite(UnitType.VolumePerLength_Fluid, ("Meter", 3), ("Meter", -1));
            AddComposite(UnitType.VolumePerLength_Fluid, ("Yard", 3), ("Foot", -1));
            AddComposite(UnitType.VolumePerLength_Fluid, ("Yard", 3), ("UsSurveyFoot", -1));
            AddComposite(UnitType.VolumePerLength_Fluid, ("Liter", 1), ("Meter", -1));
            AddComposite(UnitType.VolumePerLength_Fluid, ("OilBarrel", 1), ("Foot", -1));
        }

        private static void LoadChemicalUnits(List<PhysicalUnit> units)
        {
            // Helpers
            void Add(PhysicalUnit unit) => units.Add(unit);
            void AddComposite(UnitType type, params (string unitKey, int exponent)[] components)
                => Add(Composite(type, components));

            // ===== MOLARITÉ =====
            AddComposite(UnitType.Molarity_Chem, ("Mole", 1), ("Meter", -3));
            AddComposite(UnitType.Molarity_Chem, ("Mole", 1), ("Liter", -1));

            // ===== MASSE MOLAIRE =====
            AddComposite(UnitType.MolarMass_Chem, ("Kilogram", 1), ("Mole", -1));
            AddComposite(UnitType.MolarMass_Chem, ("Gram", 1), ("Mole", -1));
            AddComposite(UnitType.MolarMass_Chem, ("Pound", 1), ("Mole", -1));
            AddComposite(UnitType.MolarMass_Chem, ("Pound", 1), ("PoundMole", -1));

            // ===== DÉBIT MOLAIRE =====
            AddComposite(UnitType.MolarFlow_Chem, ("Mole", 1), ("Second", -1));

            // ===== ÉNERGIE MOLAIRE =====
            AddComposite(UnitType.MolarEnergy_Chem, ("Joule", 1), ("Mole", -1));

            // ===== ENTROPIE MOLAIRE =====
            AddComposite(UnitType.MolarEntropy_Chem, ("Joule", 1), ("Mole", -1), ("Kelvin", -1));
        }

        private static void LoadOpticalUnits(List<PhysicalUnit> units)
        {
            // Helpers
            void Add(PhysicalUnit unit) => units.Add(unit);
            void AddComposite(UnitType type, params (string unitKey, int exponent)[] components)
                => Add(Composite(type, components));

            // ===== ÉCLAIREMENT =====
            Add(GetRegisteredUnit("Lux")); // lx = cd/m²

            // ===== FLUX LUMINEUX =====
            Add(GetRegisteredUnit("Lumen")); // lm = cd

            // ===== IRRADIANCE =====
            AddComposite(UnitType.Irradiance_Optic, ("Watt", 1), ("Meter", -2));

            // ===== IRRADIATION =====
            AddComposite(UnitType.Irradiation_Optic, ("Joule", 1), ("Meter", -2));
            AddComposite(UnitType.Irradiation_Optic, ("Watt", 1), ("Hour", 1), ("Meter", -2));
        }

        private static void LoadEconomicUnits(List<PhysicalUnit> units)
        {
            // Helpers
            void Add(PhysicalUnit unit) => units.Add(unit);
            void AddComposite(UnitType type, params (string unitKey, int exponent)[] components)
                => Add(Composite(type, components));

            // ===== COÛT =====
            Add(GetRegisteredUnit("USDollar"));
            Add(GetRegisteredUnit("Euro"));
            Add(GetRegisteredUnit("BritishPound"));

            // ===== COÛT PAR ÉNERGIE =====
            AddComposite(UnitType.EnergyCost_Econ, ("USDollar", 1), ("Joule", -1));
            AddComposite(UnitType.EnergyCost_Econ, ("USDollar", 1), ("Watt", -1), ("Hour", -1));
            AddComposite(UnitType.EnergyCost_Econ, ("Euro", 1), ("Watt", -1), ("Hour", -1));

            // ===== COÛT PAR PUISSANCE =====
            AddComposite(UnitType.PowerCost_Econ, ("USDollar", 1), ("Watt", -1));

            // ===== COÛT PAR MASSE =====
            AddComposite(UnitType.MassCost_Econ, ("USDollar", 1), ("Kilogram", -1));
            AddComposite(UnitType.MassCost_Econ, ("USDollar", 1), ("Tonne", -1));
            AddComposite(UnitType.MassCost_Econ, ("Euro", 1), ("Kilogram", -1));

            // ===== COÛT PAR LONGUEUR =====
            AddComposite(UnitType.LengthCost_Econ, ("USDollar", 1), ("Meter", -1));
            AddComposite(UnitType.LengthCost_Econ, ("Euro", 1), ("Meter", -1));

            // ===== COÛT PAR SURFACE =====
            AddComposite(UnitType.AreaCost_Econ, ("USDollar", 1), ("Meter", -2));
            AddComposite(UnitType.AreaCost_Econ, ("Euro", 1), ("Meter", -2));

            // ===== COÛT PAR VOLUME =====
            AddComposite(UnitType.VolumeCost_Econ, ("USDollar", 1), ("Meter", -3));
            AddComposite(UnitType.VolumeCost_Econ, ("Euro", 1), ("Meter", -3));
        }

        private static void LoadInformationUnits(List<PhysicalUnit> units)
        {
            // Helpers
            void Add(PhysicalUnit unit) => units.Add(unit);
            void AddComposite(UnitType type, params (string unitKey, int exponent)[] components)
                => Add(Composite(type, components));

            // ===== QUANTITÉ D'INFORMATION =====
          //  Add(GetRegisteredUnit("Bit"));
          //  Add(GetRegisteredUnit("Byte"));

            // ===== DÉBIT D'INFORMATION =====
            AddComposite(UnitType.BitRate_Info, ("Bit", 1), ("Second", -1));
            AddComposite(UnitType.BitRate_Info, ("Byte", 1), ("Second", -1));
        }

        private static void LoadTransportUnits(List<PhysicalUnit> units)
        {
            // Helpers
            void Add(PhysicalUnit unit) => units.Add(unit);
            void AddComposite(UnitType type, params (string unitKey, int exponent)[] components)
                => Add(Composite(type, components));

            // ===== EFFICACITÉ DE CARBURANT =====
            AddComposite(UnitType.FuelEfficiency_Transport, ("Meter", 1), ("Liter", -1));
            AddComposite(UnitType.FuelEfficiency_Transport, ("Mile", 1), ("ImperialGallon", -1));
            AddComposite(UnitType.FuelEfficiency_Transport, ("Mile", 1), ("UsGallon", -1));

            // ===== CONSOMMATION SPÉCIFIQUE =====
            AddComposite(UnitType.BrakeSpecificFuelConsumption_Transport, ("Kilogram", 1), ("Joule", -1));
            AddComposite(UnitType.BrakeSpecificFuelConsumption_Transport, ("Gram", 1), ("Watt", -1), ("Hour", -1));
            AddComposite(UnitType.BrakeSpecificFuelConsumption_Transport, ("Pound", 1), ("MechanicalHorsepower", -1), ("Hour", -1));
        }

        private static void LoadSpecialUnits(List<PhysicalUnit> units)
        {
            // Helpers
            void Add(PhysicalUnit unit) => units.Add(unit);
            void AddComposite(UnitType type, params (string unitKey, int exponent)[] components)
                => Add(Composite(type, components));

            // ===== NIVEAU (dB, etc.) =====
            Add(GetRegisteredUnit("Decibel"));

            // ===== HUMIDITÉ RELATIVE =====
            Add(GetRegisteredUnit("Percent"));

            // ===== CONCENTRATION VOLUMIQUE =====
            Add(GetRegisteredUnit("PartPerMillion"));
            Add(GetRegisteredUnit("PartPerBillion"));
            Add(GetRegisteredUnit("PartPerTrillion"));
        }
    }
}
