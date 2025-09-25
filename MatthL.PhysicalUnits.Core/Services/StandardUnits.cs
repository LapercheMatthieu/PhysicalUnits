using MatthL.PhysicalUnits.Enums;
using MatthL.PhysicalUnits.Models;
using System;

namespace MatthL.PhysicalUnits.Services
{
    public static class StandardUnits
    {
        private static bool _initialized = false;
        private static readonly object _lock = new object();

        private static void EnsureInitialized()
        {
            if (!_initialized)
            {
                lock (_lock)
                {
                    if (!_initialized)
                    {
                        PhysicalUnitStorage.Initialize();
                        _initialized = true;
                    }
                }
            }
        }

        private static PhysicalUnit GetUnit(string unitKey, Prefix prefix = Prefix.SI)
        {
            EnsureInitialized();

            var baseUnit = PhysicalUnitLibraryFactory.GetRegisteredUnit(unitKey);
            if (baseUnit == null)
                throw new InvalidOperationException($"Unit '{unitKey}' not found in registry");

            if (prefix == Prefix.SI || prefix == Prefix.SI)
                return PhysicalUnit.Clone(baseUnit);

            return PhysicalUnitLibraryFactory.WithPrefix(unitKey, prefix);
        }

        // Length
        public static PhysicalUnit Meter(Prefix prefix = Prefix.SI) => GetUnit("Meter", prefix);
        public static PhysicalUnit Foot(Prefix prefix = Prefix.SI) => GetUnit("Foot", prefix);
        public static PhysicalUnit Inch(Prefix prefix = Prefix.SI) => GetUnit("Inch", prefix);
        public static PhysicalUnit Mile(Prefix prefix = Prefix.SI) => GetUnit("Mile", prefix);
        public static PhysicalUnit Yard(Prefix prefix = Prefix.SI) => GetUnit("Yard", prefix);
        public static PhysicalUnit Chain(Prefix prefix = Prefix.SI) => GetUnit("Chain", prefix);
        public static PhysicalUnit Fathom(Prefix prefix = Prefix.SI) => GetUnit("Fathom", prefix);
        public static PhysicalUnit NauticalMile(Prefix prefix = Prefix.SI) => GetUnit("NauticalMile", prefix);
        public static PhysicalUnit LightYear(Prefix prefix = Prefix.SI) => GetUnit("LightYear", prefix);
        public static PhysicalUnit AstronomicalUnit(Prefix prefix = Prefix.SI) => GetUnit("AstronomicalUnit", prefix);
        public static PhysicalUnit Parsec(Prefix prefix = Prefix.SI) => GetUnit("Parsec", prefix);

        // Mass
        public static PhysicalUnit Kilogram(Prefix prefix = Prefix.SI) => GetUnit("Kilogram", prefix);
        public static PhysicalUnit Gram(Prefix prefix = Prefix.SI) => GetUnit("Gram", prefix);
        public static PhysicalUnit Pound(Prefix prefix = Prefix.SI) => GetUnit("Pound", prefix);
        public static PhysicalUnit Ounce(Prefix prefix = Prefix.SI) => GetUnit("Ounce", prefix);
        public static PhysicalUnit Tonne(Prefix prefix = Prefix.SI) => GetUnit("Tonne", prefix);
        public static PhysicalUnit Stone(Prefix prefix = Prefix.SI) => GetUnit("Stone", prefix);
        public static PhysicalUnit Grain(Prefix prefix = Prefix.SI) => GetUnit("Grain", prefix);
        public static PhysicalUnit ShortTon(Prefix prefix = Prefix.SI) => GetUnit("ShortTon", prefix);
        public static PhysicalUnit LongTon(Prefix prefix = Prefix.SI) => GetUnit("LongTon", prefix);
        public static PhysicalUnit Slug(Prefix prefix = Prefix.SI) => GetUnit("Slug", prefix);

        // Time
        public static PhysicalUnit Second(Prefix prefix = Prefix.SI) => GetUnit("Second", prefix);
        public static PhysicalUnit Minute => GetUnit("Minute");
        public static PhysicalUnit Hour => GetUnit("Hour");
        public static PhysicalUnit Day => GetUnit("Day");
        public static PhysicalUnit Week => GetUnit("Week");
        public static PhysicalUnit Month => GetUnit("Month30");
        public static PhysicalUnit Year => GetUnit("Year365");

        // Temperature
        public static PhysicalUnit Kelvin(Prefix prefix = Prefix.SI) => GetUnit("Kelvin", prefix);
        public static PhysicalUnit Celsius => GetUnit("Celsius");
        public static PhysicalUnit Fahrenheit => GetUnit("Fahrenheit");
        public static PhysicalUnit Rankine => GetUnit("Rankine");

        // Electric Current
        public static PhysicalUnit Ampere(Prefix prefix = Prefix.SI) => GetUnit("Ampere", prefix);

        // Amount of Substance
        public static PhysicalUnit Mole(Prefix prefix = Prefix.SI) => GetUnit("Mole", prefix);
        public static PhysicalUnit PoundMole => GetUnit("PoundMole");

        // Luminous Intensity
        public static PhysicalUnit Candela(Prefix prefix = Prefix.SI) => GetUnit("Candela", prefix);

        // Angle
        public static PhysicalUnit Radian(Prefix prefix = Prefix.SI) => GetUnit("Radian", prefix);
        public static PhysicalUnit Degree => GetUnit("Degree");
        public static PhysicalUnit Revolution => GetUnit("Revolution");
        public static PhysicalUnit Gradian => GetUnit("Gradian");
        public static PhysicalUnit Arcminute => GetUnit("Arcminute");
        public static PhysicalUnit Arcsecond => GetUnit("Arcsecond");

        // Ratio
        public static PhysicalUnit Ratio => GetUnit("Ratio");
        public static PhysicalUnit Percent => GetUnit("Percent");
        public static PhysicalUnit PartPerThousand => GetUnit("PartPerThousand");
        public static PhysicalUnit PPM => GetUnit("PartPerMillion");
        public static PhysicalUnit PPB => GetUnit("PartPerBillion");
        public static PhysicalUnit PPT => GetUnit("PartPerTrillion");

        // Currency
        public static PhysicalUnit USDollar => GetUnit("USDollar");
        public static PhysicalUnit Euro => GetUnit("Euro");
        public static PhysicalUnit BritishPound => GetUnit("BritishPound");

        // Information
        public static PhysicalUnit Bit(Prefix prefix = Prefix.SI) => GetUnit("Bit", prefix);
        public static PhysicalUnit Byte(Prefix prefix = Prefix.SI) => GetUnit("Byte", prefix);

        // Force
        public static PhysicalUnit Newton(Prefix prefix = Prefix.SI) => GetUnit("Newton", prefix);
        public static PhysicalUnit Dyn(Prefix prefix = Prefix.SI) => GetUnit("Dyn", prefix);
        public static PhysicalUnit PoundForce => GetUnit("PoundForce");
        public static PhysicalUnit KilogramForce => GetUnit("KilogramForce");
        public static PhysicalUnit TonneForce => GetUnit("TonneForce");
        public static PhysicalUnit Poundal => GetUnit("Poundal");
        public static PhysicalUnit OunceForce => GetUnit("OunceForce");
        public static PhysicalUnit ShortTonForce => GetUnit("ShortTonForce");

        // Pressure
        public static PhysicalUnit Pascal(Prefix prefix = Prefix.SI) => GetUnit("Pascal", prefix);
        public static PhysicalUnit Bar(Prefix prefix = Prefix.SI) => GetUnit("Bar", prefix);
        public static PhysicalUnit Atmosphere => GetUnit("Atmosphere");
        public static PhysicalUnit TechnicalAtmosphere => GetUnit("TechnicalAtmosphere");
        public static PhysicalUnit Torr => GetUnit("Torr");
        public static PhysicalUnit PSI => GetUnit("PoundPerSquareInch");
        public static PhysicalUnit PSF => GetUnit("PoundPerSquareFoot");
        public static PhysicalUnit InchOfMercury => GetUnit("InchOfMercury");
        public static PhysicalUnit InchOfWater => GetUnit("InchOfWaterColumn");
        public static PhysicalUnit FootOfWater => GetUnit("FootOfHead");
        public static PhysicalUnit MeterOfWater => GetUnit("MeterOfHead");
        public static PhysicalUnit MillimeterOfMercury => GetUnit("MillimeterOfMercury");

        // Energy
        public static PhysicalUnit Joule(Prefix prefix = Prefix.SI) => GetUnit("Joule", prefix);
        public static PhysicalUnit Calorie(Prefix prefix = Prefix.SI) => GetUnit("Calorie", prefix);
        public static PhysicalUnit BTU => GetUnit("BritishThermalUnit");
        public static PhysicalUnit ElectronVolt(Prefix prefix = Prefix.SI) => GetUnit("ElectronVolt", prefix);
        public static PhysicalUnit FootPound => GetUnit("FootPound");
        public static PhysicalUnit Erg => GetUnit("Erg");
        public static PhysicalUnit ThermEC => GetUnit("ThermEc");
        public static PhysicalUnit ThermUS => GetUnit("ThermUs");
        public static PhysicalUnit ThermImperial => GetUnit("ThermImperial");

        // Power
        public static PhysicalUnit Watt(Prefix prefix = Prefix.SI) => GetUnit("Watt", prefix);
        public static PhysicalUnit MechanicalHorsepower => GetUnit("MechanicalHorsepower");
        public static PhysicalUnit MetricHorsepower => GetUnit("MetricHorsepower");
        public static PhysicalUnit ElectricalHorsepower => GetUnit("ElectricalHorsepower");
        public static PhysicalUnit BoilerHorsepower => GetUnit("BoilerHorsepower");
        public static PhysicalUnit HydraulicHorsepower => GetUnit("HydraulicHorsepower");
        public static PhysicalUnit SolarLuminosity => GetUnit("SolarLuminosity");

        // Frequency
        public static PhysicalUnit Hertz(Prefix prefix = Prefix.SI) => GetUnit("Hertz", prefix);
        public static PhysicalUnit RPM => GetUnit("RevolutionsPerMinute");
        public static PhysicalUnit BPM => GetUnit("BeatPerMinute");

        // Volume
        public static PhysicalUnit Liter(Prefix prefix = Prefix.SI) => GetUnit("Liter", prefix);
        public static PhysicalUnit ImperialGallon => GetUnit("ImperialGallon");
        public static PhysicalUnit USGallon => GetUnit("UsGallon");
        public static PhysicalUnit ImperialOunce => GetUnit("ImperialOunce");
        public static PhysicalUnit USOunce => GetUnit("UsOunce");
        public static PhysicalUnit ImperialPint => GetUnit("ImperialPint");
        public static PhysicalUnit USPint => GetUnit("UsPint");
        public static PhysicalUnit USQuart => GetUnit("UsQuart");
        public static PhysicalUnit USTablespoon => GetUnit("UsTablespoon");
        public static PhysicalUnit USTeaspoon => GetUnit("UsTeaspoon");
        public static PhysicalUnit AUTablespoon => GetUnit("AuTablespoon");
        public static PhysicalUnit UKTablespoon => GetUnit("UkTablespoon");
        public static PhysicalUnit MetricCup => GetUnit("MetricCup");
        public static PhysicalUnit MetricTeaspoon => GetUnit("MetricTeaspoon");
        public static PhysicalUnit USCustomaryCup => GetUnit("UsCustomaryCup");
        public static PhysicalUnit USLegalCup => GetUnit("UsLegalCup");
        public static PhysicalUnit OilBarrel => GetUnit("OilBarrel");
        public static PhysicalUnit USBeerBarrel => GetUnit("UsBeerBarrel");
        public static PhysicalUnit ImperialBeerBarrel => GetUnit("ImperialBeerBarrel");
        public static PhysicalUnit BoardFoot => GetUnit("BoardFoot");
        public static PhysicalUnit AcreFoot => GetUnit("AcreFoot");

        // Area
        public static PhysicalUnit Acre => GetUnit("Acre");
        public static PhysicalUnit Hectare => GetUnit("Hectare");

        // Electrical
        public static PhysicalUnit Volt(Prefix prefix = Prefix.SI) => GetUnit("Volt", prefix);
        public static PhysicalUnit Ohm(Prefix prefix = Prefix.SI) => GetUnit("Ohm", prefix);
        public static PhysicalUnit Farad(Prefix prefix = Prefix.SI) => GetUnit("Farad", prefix);
        public static PhysicalUnit Henry(Prefix prefix = Prefix.SI) => GetUnit("Henry", prefix);
        public static PhysicalUnit Coulomb(Prefix prefix = Prefix.SI) => GetUnit("Coulomb", prefix);
        public static PhysicalUnit Tesla(Prefix prefix = Prefix.SI) => GetUnit("Tesla", prefix);
        public static PhysicalUnit Weber(Prefix prefix = Prefix.SI) => GetUnit("Weber", prefix);
        public static PhysicalUnit Gauss => GetUnit("Gauss");
        public static PhysicalUnit Siemens(Prefix prefix = Prefix.SI) => GetUnit("Siemens", prefix);

        // Viscosity
        public static PhysicalUnit Poise => GetUnit("Poise");
        public static PhysicalUnit Reyn => GetUnit("Reyn");
        public static PhysicalUnit Stokes => GetUnit("Stokes");

        // Acceleration
        public static PhysicalUnit StandardGravity => GetUnit("StandardGravity");

        // Speed
        public static PhysicalUnit Knot => GetUnit("Knot");

        // Optical
        public static PhysicalUnit Lux => GetUnit("Lux");
        public static PhysicalUnit Lumen => GetUnit("Lumen");

        // Special
        public static PhysicalUnit Decibel => GetUnit("Decibel");
        public static PhysicalUnit MillionUSGallonsPerDay => GetUnit("MillionUsGallonsPerDay");

        // Composite Units - Area
        public static PhysicalUnit SquareMeter => PhysicalUnitLibraryFactory.Composite(UnitType.Area_Mech, ("Meter", 2));
        public static PhysicalUnit SquareFoot => PhysicalUnitLibraryFactory.Composite(UnitType.Area_Mech, ("Foot", 2));
        public static PhysicalUnit SquareInch => PhysicalUnitLibraryFactory.Composite(UnitType.Area_Mech, ("Inch", 2));
        public static PhysicalUnit SquareYard => PhysicalUnitLibraryFactory.Composite(UnitType.Area_Mech, ("Yard", 2));
        public static PhysicalUnit SquareMile => PhysicalUnitLibraryFactory.Composite(UnitType.Area_Mech, ("Mile", 2));

        // Composite Units - Volume
        public static PhysicalUnit CubicMeter => PhysicalUnitLibraryFactory.Composite(UnitType.Volume_Mech, ("Meter", 3));
        public static PhysicalUnit CubicFoot => PhysicalUnitLibraryFactory.Composite(UnitType.Volume_Mech, ("Foot", 3));
        public static PhysicalUnit CubicInch => PhysicalUnitLibraryFactory.Composite(UnitType.Volume_Mech, ("Inch", 3));
        public static PhysicalUnit CubicYard => PhysicalUnitLibraryFactory.Composite(UnitType.Volume_Mech, ("Yard", 3));

        // Composite Units - Speed
        public static PhysicalUnit MeterPerSecond => PhysicalUnitLibraryFactory.Composite(UnitType.Speed_Mech, ("Meter", 1), ("Second", -1));
        public static PhysicalUnit MeterPerHour => PhysicalUnitLibraryFactory.Composite(UnitType.Speed_Mech, ("Meter", 1), ("Hour", -1));
        public static PhysicalUnit FootPerSecond => PhysicalUnitLibraryFactory.Composite(UnitType.Speed_Mech, ("Foot", 1), ("Second", -1));
        public static PhysicalUnit MilePerHour => PhysicalUnitLibraryFactory.Composite(UnitType.Speed_Mech, ("Mile", 1), ("Hour", -1));
        public static PhysicalUnit InchPerSecond => PhysicalUnitLibraryFactory.Composite(UnitType.Speed_Mech, ("Inch", 1), ("Second", -1));

        // Composite Units - Acceleration
        public static PhysicalUnit MeterPerSecondSquared => PhysicalUnitLibraryFactory.Composite(UnitType.Acceleration_Mech, ("Meter", 1), ("Second", -2));
        public static PhysicalUnit FootPerSecondSquared => PhysicalUnitLibraryFactory.Composite(UnitType.Acceleration_Mech, ("Foot", 1), ("Second", -2));

        // Composite Units - Torque
        public static PhysicalUnit NewtonMeter => PhysicalUnitLibraryFactory.Composite(UnitType.Torque_Mech, ("Newton", 1), ("Meter", 1));
        public static PhysicalUnit PoundFoot => PhysicalUnitLibraryFactory.Composite(UnitType.Torque_Mech, ("PoundForce", 1), ("Foot", 1));
        public static PhysicalUnit PoundInch => PhysicalUnitLibraryFactory.Composite(UnitType.Torque_Mech, ("PoundForce", 1), ("Inch", 1));

        // Composite Units - Energy
        public static PhysicalUnit WattHour => PhysicalUnitLibraryFactory.Composite(UnitType.Energy_Mech, ("Watt", 1), ("Hour", 1));
        public static PhysicalUnit WattDay => PhysicalUnitLibraryFactory.Composite(UnitType.Energy_Mech, ("Watt", 1), ("Day", 1));

        // Composite Units - Density
        public static PhysicalUnit KilogramPerCubicMeter => PhysicalUnitLibraryFactory.Composite(UnitType.Density_Mech, ("Kilogram", 1), ("Meter", -3));
        public static PhysicalUnit GramPerLiter => PhysicalUnitLibraryFactory.Composite(UnitType.Density_Mech, ("Gram", 1), ("Liter", -1));
        public static PhysicalUnit PoundPerCubicFoot => PhysicalUnitLibraryFactory.Composite(UnitType.Density_Mech, ("Pound", 1), ("Foot", -3));

        // Composite Units - Flow Rate
        public static PhysicalUnit CubicMeterPerSecond => PhysicalUnitLibraryFactory.Composite(UnitType.VolumeFlow_Fluid, ("Meter", 3), ("Second", -1));
        public static PhysicalUnit LiterPerMinute => PhysicalUnitLibraryFactory.Composite(UnitType.VolumeFlow_Fluid, ("Liter", 1), ("Minute", -1));
        public static PhysicalUnit LiterPerSecond => PhysicalUnitLibraryFactory.Composite(UnitType.VolumeFlow_Fluid, ("Liter", 1), ("Second", -1));
        public static PhysicalUnit GallonPerMinute => PhysicalUnitLibraryFactory.Composite(UnitType.VolumeFlow_Fluid, ("UsGallon", 1), ("Minute", -1));
        public static PhysicalUnit KilogramPerSecond => PhysicalUnitLibraryFactory.Composite(UnitType.MassFlow_Fluid, ("Kilogram", 1), ("Second", -1));

        // Composite Units - Thermal
        public static PhysicalUnit WattPerMeterKelvin => PhysicalUnitLibraryFactory.Composite(UnitType.ThermalConductivity_Thermo, ("Watt", 1), ("Meter", -1), ("Kelvin", -1));
        public static PhysicalUnit JoulePerKilogramKelvin => PhysicalUnitLibraryFactory.Composite(UnitType.SpecificHeatCapacity_Thermo, ("Joule", 1), ("Kilogram", -1), ("Kelvin", -1));
    }
}