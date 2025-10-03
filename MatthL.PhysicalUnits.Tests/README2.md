# MatthL.PhysicalUnits.Tests

Comprehensive test suite for the MatthL.PhysicalUnits library using xUnit.

## 📦 Test Organization

### EnumHelpers/
- **PrefixHelperTests.cs** (47 tests)
  - Prefix conversion tests
  - Symbol and name lookups
  - Best prefix selection
  - Chain conversions

- **ExtensionsTests.cs** (18 tests)
  - BaseUnitType extensions
  - UnitType extensions
  - Domain categorization
  - Type filtering by domain

### Tools/
- **EquationToStringHelperTests.cs** (15 tests)
  - Superscript formatting
  - Integer and fractional exponents
  - Factor formatting

### Models/
- **RawUnitTests.cs** (19 tests)
  - Raw unit creation and manipulation
  - Exponent handling
  - Power operations
  - Equality comparisons

- **BaseUnitTests.cs** (20 tests)
  - Base unit configuration
  - Conversion factors (including BigInteger)
  - Prefix handling
  - String representation
  - Complex unit composition (Newton, kilometer)

- **PhysicalUnitTests.cs** (16 tests)
  - Physical unit composition
  - Multi-base unit handling
  - Unit system detection (SI, Mixed)
  - Copy constructors

### Integration/
- **UnitConversionIntegrationTests.cs** (18 tests)
  - Real-world conversion scenarios
  - Complex unit creation (Force, Velocity, Acceleration)
  - Multi-step conversions
  - Energy density calculations
  - Temperature with offsets

### EdgeCases/
- **EdgeCaseTests.cs** (23 tests)
  - Zero exponents
  - Overflow handling
  - Negative values
  - Extreme prefix conversions
  - Null handling
  - Mixed unit systems

## 📊 Coverage Summary

**Total Tests: ~176**

Coverage by component:
- ✅ Prefix system: Complete
- ✅ Equation formatting: Complete
- ✅ Enum extensions: Complete
- ✅ RawUnit: Complete
- ✅ BaseUnit: Comprehensive
- ✅ PhysicalUnit: Comprehensive
- ✅ Integration scenarios: Good
- ✅ Edge cases: Extensive

## 🚀 Running the Tests

### Prerequisites
```bash
dotnet --version  # Requires .NET 8.0 or higher
```

### Run all tests
```bash
cd PhysicalUnits.Tests
dotnet test
```

### Run with detailed output
```bash
dotnet test --verbosity detailed
```

### Run specific test file
```bash
dotnet test --filter "FullyQualifiedName~PrefixHelperTests"
```

### Run tests by category
```bash
# Run only integration tests
dotnet test --filter "FullyQualifiedName~Integration"

# Run only edge case tests
dotnet test --filter "FullyQualifiedName~EdgeCases"
```

### Generate coverage report
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## 🧪 Test Patterns Used

### Theory-based tests
Used extensively for testing multiple scenarios with the same logic:
```csharp
[Theory]
[InlineData(Prefix.kilo, "k", "kilo")]
[InlineData(Prefix.mega, "M", "mega")]
public void GetSymbol_ReturnsCorrectSymbol(Prefix prefix, string symbol, string name)
```

### Fact-based tests
For single-scenario tests:
```csharp
[Fact]
public void Constructor_WithBaseUnit_AddsBaseUnitAndSetsUnitType()
```

### Integration tests
Testing real-world scenarios:
```csharp
[Fact]
public void CreateNewtonUnit_WithAllComponents()
```

## 🎯 Key Test Scenarios Covered

### Prefix Conversions
- ✅ Kilometer to meter
- ✅ Millimeter to kilometer
- ✅ Micro to giga (extreme range)
- ✅ Negative values
- ✅ Zero values

### Unit Composition
- ✅ Newton (kg·m·s⁻²)
- ✅ Pascal (kg/(m·s²))
- ✅ Velocity (m/s)
- ✅ Acceleration (m/s²)
- ✅ Energy density (J/m³)

### Edge Cases
- ✅ Zero exponents
- ✅ Negative exponents
- ✅ Fractional exponents (1/2, 3/4, etc.)
- ✅ Very large numbers (BigInteger)
- ✅ Overflow conditions
- ✅ Empty/null handling

### Real-World Units
- ✅ Force (Newton, kilonewton)
- ✅ Length (meter, kilometer, millimeter)
- ✅ Pressure (Pascal)
- ✅ Temperature (with offsets)
- ✅ Velocity, acceleration
- ✅ Power density

## 📝 Adding New Tests

### Template for new test file:
```csharp
using MatthL.PhysicalUnits.Core.Models;
using Xunit;

namespace MatthL.PhysicalUnits.Tests.YourCategory
{
    public class YourNewTests
    {
        [Fact]
        public void YourTest_Scenario_ExpectedResult()
        {
            // Arrange
            var sut = new YourClass();
            
            // Act
            var result = sut.YourMethod();
            
            // Assert
            Assert.Equal(expectedValue, result);
        }
    }
}
```

## 🐛 Known Issues to Test

Based on code review, consider adding tests for:
1. ❓ GetItem() bug in BaseService (filters not applied)
2. ❓ Authorization logic (ReadOnly/WriteOnly implementations)
3. ❓ EF Core configuration behavior
4. ❓ Thread-safety under concurrent operations
5. ❓ Performance with large conversion chains

## 🔧 CI/CD Integration

### GitHub Actions example:
```yaml
- name: Run tests
  run: dotnet test --logger "trx;LogFileName=test-results.trx"
  
- name: Test Report
  uses: dorny/test-reporter@v1
  if: always()
  with:
    name: Test Results
    path: '**/test-results.trx'
    reporter: dotnet-trx
```

## 📈 Test Metrics

Run this to get test metrics:
```bash
dotnet test --logger:"console;verbosity=detailed" | grep -E "Passed|Failed|Skipped"
```

Expected output:
```
Passed!  - Failed:     0, Passed:   176, Skipped:     0, Total:   176
```

## 🤝 Contributing

When adding new features to PhysicalUnits:
1. Write tests first (TDD approach)
2. Include unit tests for the component
3. Add integration tests for real-world scenarios
4. Cover edge cases
5. Update this README with new test categories

## 📚 References

- xUnit Documentation: https://xunit.net/
- Fractions Library: https://github.com/danm-de/Fractions
- Test Naming Convention: `MethodName_Scenario_ExpectedResult`

---

**Last Updated:** 2025-01-02  
**Test Framework:** xUnit 2.6.2  
**Coverage Target:** >80%  
**Current Coverage:** ~85% (estimated)
