# MatthL.PhysicalUnits

A comprehensive, **100% dynamic** physical units library for .NET applications that enables
runtime unit manipulation, conversion, and validation. 
Unlike traditional static unit libraries, 
this system is designed for **end-users** to work with any unit combination dynamically.
Package with already built WPF Controls available as MatthL.PhysicalUnits.WPF

[![NuGet](https://img.shields.io/nuget/v/MatthL.PhysicalUnits.svg)](https://www.nuget.org/packages/MatthL.PhysicalUnits/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

## 🎯 Key Differentiators

**This is NOT another UnitsNet clone.** While UnitsNet provides static, compile-time units for developers, MatthL.PhysicalUnits offers:

- ✅ **100% Dynamic Runtime System** - Create, combine, and manipulate units at runtime
- ✅ **User-Facing Design** - Built for end-users in applications, not just developers in code
- ✅ **Intelligent Unit Suggestions** - Context-aware recommendations after operations
- ✅ **Dimensional Formula Validation** - Automatic homogeneity checking
- ✅ **Database Integration** - Store and persist units with Entity Framework
- ✅ **Fraction-Based Precision** - Exact calculations without floating-point errors
- ✅ **Automatic Unit Recognition** - Smart matching of operation results to known units

## 📦 Installation

```bash
dotnet add package MatthL.PhysicalUnits
```

## 🚀 Quick Start

The main class is the PhysicalUnit. 
The PhysicalUnitRepository is a static repository that build all needed physical units for the user to pick, modify, assemble
The StandardUnits is a static helper class to create easily already tuned physical units for the programmer (like UnitNet). 

```csharp
using MatthL.PhysicalUnits.Infrastructure.Library;
using MatthL.PhysicalUnits.Computation.Extensions;

// Get standard units
var meter = StandardUnits.Meter();
var second = StandardUnits.Second();

// Perform operations
var speed = meter.Divide(second);
Console.WriteLine(speed); // Output: m/s

// Convert values
double speedInSI = 100; // 100 m/s
var speedInKmh = meter.ConvertValue(StandardUnits.Kilometer().Divide(StandardUnits.Hour()), speedInSI);
Console.WriteLine($"{speedInKmh} km/h"); // Output: 360 km/h
```

## ✨ Core Features

### 1. **400+ Dynamic Units**

Access over 400 pre-defined units across all physical domains:

```csharp
// Length units
var meter = StandardUnits.Meter();
var foot = StandardUnits.Foot();
var lightyear = StandardUnits.LightYear();

// With SI prefixes
var kilometer = StandardUnits.Meter(Prefix.kilo);
var millimeter = StandardUnits.Meter(Prefix.milli);

// Specialized units
var psi = StandardUnits.PSI();
var horsepower = StandardUnits.MechanicalHorsepower();
var btу = StandardUnits.BTU();
```

### 2. **Dynamic Unit Operations**

Multiply, divide, and raise units to powers:

```csharp
var force = StandardUnits.Newton();
var distance = StandardUnits.Meter();

// Multiply: Force × Distance = Energy
var energy = force.Multiply(distance);
Console.WriteLine(energy.GetDimensionalFormula()); // Output: kg·m²/s²

// Divide: Distance / Time = Speed
var time = StandardUnits.Second();
var speed = distance.Divide(time);

// Power: Area = Length²
var area = distance.Pow(new Fraction(2));
```

### 3. **Intelligent Unit Suggestions**

Get context-aware unit recommendations after operations:

```csharp
using MatthL.PhysicalUnits.Computation.Helpers;

var force = StandardUnits.Newton();
var distance = StandardUnits.Meter();

var suggestions = UnitSuggestionHelper.MultiplyUnits(force, distance);

foreach (var suggestion in suggestions.Take(3))
{
    Console.WriteLine($"Unit: {suggestion.Unit.Name}");
    Console.WriteLine($"Score: {suggestion.RelevanceScore}");
    Console.WriteLine($"Explanation: {suggestion.Explanation}");
    Console.WriteLine();
}
```

**Output:**
```
Unit: Joule
Score: 5.0
Explanation: Travail effectué par une force sur une distance (W = F × d)

Unit: Newton Meter
Score: 4.5
Explanation: Moment de force (couple) (τ = F × r)
```

### 4. **Automatic Homogeneity Verification**

Ensure dimensional consistency in your calculations:

```csharp
using MatthL.PhysicalUnits.Computation.Helpers;

var meter = StandardUnits.Meter();
var second = StandardUnits.Second();
var newton = StandardUnits.Newton();

// Check if units are homogeneous (can be added/subtracted)
bool compatible = HomogeneityHelper.VerifyHomogeneity(meter, meter); // true
bool incompatible = HomogeneityHelper.VerifyHomogeneity(meter, second); // false

// Verify equation homogeneity
var force1 = newton;
var force2 = StandardUnits.Kilogram().Multiply(
    StandardUnits.Meter(),
    StandardUnits.Second().Pow(-2)
);
bool same = HomogeneityHelper.VerifyHomogeneity(force1, force2); // true
```

### 5. **Precise Unit Conversions**

Convert between any compatible units:

```csharp
var meter = StandardUnits.Meter();
var foot = StandardUnits.Foot();

// Single value conversion
double meters = 10.0;
double feet = meter.ConvertValue(foot, meters);
Console.WriteLine($"{meters} m = {feet} ft"); // 10 m = 32.808399 ft

// Array conversion (optimized)
double[] meterValues = { 1.0, 2.0, 3.0 };
double[] footValues = meter.ConvertValues(foot, meterValues);

// Temperature conversion (handles offsets)
var celsius = StandardUnits.Celsius();
var fahrenheit = StandardUnits.Fahrenheit();
double tempC = 25.0;
double tempF = celsius.ConvertValue(fahrenheit, tempC);
Console.WriteLine($"{tempC}°C = {tempF}°F"); // 25°C = 77°F
```

### 6. **Dimensional Formula Analysis**

Get and compare dimensional formulas:

```csharp
using MatthL.PhysicalUnits.DimensionalFormulas.Extensions;

var newton = StandardUnits.Newton();
var formula = newton.GetDimensionalFormula();
Console.WriteLine(formula); // Output: kg·m/s²

// Find all units with the same dimensional formula
var compatibleUnits = RepositorySearchEngine.GetUnitsFromDimensionalFormula(formula);
// Returns: Newton, Dyne, PoundForce, etc.
```

### 7. **Advanced Search & Filtering**

Search and filter units by domain, type, or system:

```csharp
using MatthL.PhysicalUnits.Infrastructure.Repositories;

// Get all units of a specific type
var pressureUnits = RepositorySearchEngine.GetUnitsOfType(UnitType.Pressure_Mech);

// Filter by domain
var mechanicalUnits = RepositorySearchEngine.GetUnits(
    domain: PhysicalUnitDomain.Mechanics
);

// Search by text
var results = RepositorySearchEngine.SearchUnits("pascal");

// Get unit types for a domain
var types = RepositorySearchEngine.GetUnitTypesForDomain(PhysicalUnitDomain.Electricity);
```

### 8. **Database Persistence**

Store and retrieve units from a database:

```csharp
using MatthL.PhysicalUnits.Infrastructure.Repositories;

var context = new PhysicalUnitRootDBContext();

// Save a custom unit
var customUnit = /* your PhysicalUnit */;
context.PhysicalUnits.Add(customUnit);
await context.SaveChangesAsync();

// Query units
var savedUnits = await context.PhysicalUnits
    .Include(u => u.BaseUnits)
    .ThenInclude(b => b.RawUnits)
    .ToListAsync();
```

### 9. **Fraction-Based Precision**

Exact calculations without floating-point errors:

```csharp
using Fractions;

var unit = StandardUnits.Meter();
var sqrtUnit = unit.Pow(new Fraction(1, 2)); // Square root
var cubicUnit = unit.Pow(new Fraction(3)); // Cubic

// Conversion factors stored as fractions
var foot = StandardUnits.Foot();
var conversionFactor = foot.BaseUnits.First().ConversionFactor;
Console.WriteLine($"1 ft = {conversionFactor} m"); // Exact: 381/1250 m
```

## 🏗️ Architecture

The library is organized into four main packages:

### **Core** (`MatthL.PhysicalUnits.Core`)
- Base models: `PhysicalUnit`, `BaseUnit`, `RawUnit`
- Enums: `UnitType`, `BaseUnitType`, `Prefix`, etc.
- Core abstractions and helpers

### **DimensionalFormulas** (`MatthL.PhysicalUnits.DimensionalFormulas`)
- Dimensional formula calculation and simplification
- Formula ordering and string formatting
- Homogeneity verification

### **Infrastructure** (`MatthL.PhysicalUnits.Infrastructure`)
- **400+ unit definitions** across all domains
- Unit library and factory
- Repository and search engine
- Database context (Entity Framework)
- Unit cloning and manipulation extensions

### **Computation** (`MatthL.PhysicalUnits.Computation`)
- Unit conversions (to/from SI)
- Mathematical operations (multiply, divide, power)
- Intelligent unit suggestions with scoring
- Context-aware recommendations
- Unit builders

## 📋 Supported Domains

- **Base Units**: Length, Mass, Time, Temperature, etc.
- **Mechanics**: Force, Pressure, Energy, Power, Torque, etc.
- **Electricity**: Voltage, Current, Resistance, Capacitance, etc.
- **Thermodynamics**: Heat flux, Thermal conductivity, Entropy, etc.
- **Fluidics**: Flow rates, Viscosity, etc.
- **Chemistry**: Molarity, Molar mass, etc.
- **Optics**: Illuminance, Luminous flux, etc.
- **Economics**: Cost per unit, Energy cost, etc.
- **Computing**: Bit rate, Data storage, etc.
- **Transport**: Fuel efficiency, etc.

## 🔬 Unit Systems

- **SI** (International System)
- **Metric** (Non-SI metric units)
- **Imperial** (British system)
- **US** (US customary)
- **Astronomical** (Light years, parsecs, etc.)
- **Other** (Mixed and specialized systems)

## 🧪 Example Use Cases

### Engineering Calculations

```csharp
// Calculate stress: σ = F/A
var force = StandardUnits.Newton(Prefix.kilo); // kN
var area = StandardUnits.Meter().Pow(2); // m²

var stress = force.Divide(area);
var suggestions = UnitSuggestionHelper.DivideUnits(force, area);
// Suggests: Pascal, Bar, PSI, etc.
```

### Physics Problems

```csharp
// Kinetic energy: E = ½mv²
var mass = StandardUnits.Kilogram();
var velocity = StandardUnits.Meter().Divide(StandardUnits.Second());

var energy = mass.Multiply(velocity.Pow(2));
var suggestions = UnitSuggestionHelper.GetUnitSuggestions(
    mass.ToTerm(),
    velocity.ToTerm(2)
);
// Suggests: Joule (score: 5.0), Watt·Hour, etc.
```

### User-Defined Conversions

```csharp
// Let users select their preferred units
var availableLengthUnits = RepositorySearchEngine.GetUnitsOfType(UnitType.Length_Base);

// User selects "Mile" from dropdown
var selectedUnit = availableLengthUnits.First(u => u.Name == "Mile");

// Convert measurement
double kilometers = 100;
var miles = StandardUnits.Kilometer().ConvertValue(selectedUnit, kilometers);
```

## 📊 Testing

The library includes **380+ unit tests** covering:

- ✅ Unit conversions (all combinations)
- ✅ Mathematical operations
- ✅ Dimensional formula validation
- ✅ Homogeneity checking
- ✅ Suggestion scoring
- ✅ Prefix handling
- ✅ Temperature offset calculations
- ✅ Edge cases and error handling

## 🤝 Contributing

Contributions are welcome! Please feel free to submit issues, feature requests, or pull requests.

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 👤 Author

**MatthL**

## 🙏 Acknowledgments

- Built with passion for precise scientific computing
- Inspired by the need for dynamic, user-facing unit systems
- Thanks to the .NET community for feedback and support

---

**Note**: This library is designed for applications where users need to work with physical units dynamically at runtime. If you need a static, compile-time unit system for developer use, consider [UnitsNet](https://github.com/angularsen/UnitsNet). Both approaches serve different needs!

<div align="center">

---

Made with ❤️ by [Matthieu L](https://github.com/LapercheMatthieu)

</div>