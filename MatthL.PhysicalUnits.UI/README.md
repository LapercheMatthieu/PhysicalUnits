# MatthL.PhysicalUnits.WPF

Professional **WPF controls** for dynamic physical unit manipulation in .NET applications. A complete UI toolkit that brings the power of [MatthL.PhysicalUnits](https://www.nuget.org/packages/MatthL.PhysicalUnits/) directly to your users.

[![NuGet](https://img.shields.io/nuget/v/MatthL.PhysicalUnits.WPF.svg)](https://www.nuget.org/packages/MatthL.PhysicalUnits.WPF/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

## 🎯 Key Features

**Production-ready WPF controls that your users will love:**

- ✅ **Smart Unit Builder** - Interactive control for building complex units with double click simplicity
- ✅ **Intelligent Suggestions** - Context-aware unit recommendations based on mathematical operations
- ✅ **One-Click Unit Selection** - Beautiful interfaces with search and filtering
- ✅ **Live Unit Conversion** - Real-time conversion between compatible units
- ✅ **Equation-Based Selection** - Automatically suggest units based on dimensional formulas
- ✅ **MVVM-Ready** - Full two-way binding support with ViewModels
- ✅ **Customizable Repository** - Filter units by system (SI, Imperial, US, etc.)
- ✅ **Professional UI/UX** - Polished controls ready for enterprise applications

## 📦 Installation

```bash
dotnet add package MatthL.PhysicalUnits.WPF
```

## 🚀 Quick Start

### Basic Unit Selection

The simplest way to let users select a physical unit:

```xaml
<Window xmlns:puw="clr-namespace:MatthL.PhysicalUnits.UI.ViewsButtons.PhysicalUnitBuilderButtonViews;assembly=MatthL.PhysicalUnits.UI">
    
    <puw:PhysicalUnitBuilderButtonView 
        SelectedUnit="{Binding MyUnit, Mode=TwoWay}" />
        
</Window>
```

```csharp
using MatthL.PhysicalUnits.Core.Models;

public class MyViewModel : INotifyPropertyChanged
{
    private PhysicalUnit _myUnit;
    public PhysicalUnit MyUnit
    {
        get => _myUnit;
        set { _myUnit = value; OnPropertyChanged(); }
    }
}
```

## 🎨 Available Controls

### 1. PhysicalUnitBuilderButtonView

The most powerful control - a button that opens a comprehensive unit builder popup.

![PhysicalUnitBuilderButton](https://raw.githubusercontent.com/LapercheMatthieu/PhysicalUnits/master/MatthL.PhysicalUnits.UI/Resources/PhysicalUnitBuilderButton.png)

**Features:**
- Search through 400+ units
- Filter by domain (Mechanics, Electricity, etc.)
- Build custom composite units
- Convert between compatible units
- Real-time unit preview

**XAML Usage:**

```xaml
<puw:PhysicalUnitBuilderButtonView 
    SelectedUnit="{Binding SelectedUnit, Mode=TwoWay}"
    IsPopupOpen="{Binding IsBuilderOpen, Mode=TwoWay}" />
```

**Properties:**
- `SelectedUnit` (PhysicalUnit) - The selected/built unit
- `IsPopupOpen` (bool) - Controls popup visibility
- `UnitToConvert` (PhysicalUnit) - When set, shows only compatible units for conversion
- `IsOnlyConvertion` (bool) - Restricts to conversion mode only

**Conversion Mode:**

```xaml
<!-- Show only units compatible with meters for conversion -->
<puw:PhysicalUnitBuilderButtonView 
    SelectedUnit="{Binding TargetUnit, Mode=TwoWay}"
    UnitToConvert="{Binding SourceUnit}"
    IsOnlyConvertion="True" />
```

### 2. PhysicalUnitEquationResultButtonView

Intelligent unit selection based on dimensional formulas and mathematical operations.

![FormulaResult](https://raw.githubusercontent.com/LapercheMatthieu/PhysicalUnits/master/MatthL.PhysicalUnits.UI/Resources/FormulaResultmmperµs.png)

**Features:**
- Automatic unit suggestions from equations
- Relevance scoring (prioritizes most common units)
- Context-aware explanations
- Formula display

**XAML Usage:**

```xaml
<puw:PhysicalUnitEquationResultButtonView 
    EquationTerms="{Binding MyEquation}"
    SelectedUnit="{Binding ResultUnit, Mode=TwoWay}"
    MaxSuggestions="10" />
```

**Code Example:**

```csharp
using MatthL.PhysicalUnits.Core.EquationModels;
using MatthL.PhysicalUnits.Infrastructure.Library;
using Fractions;

// Force × Distance = Energy
var force = StandardUnits.Newton();
var distance = StandardUnits.Meter();

var equation = new EquationTerms(
    new PhysicalUnitTerm(force, new Fraction(1)),
    new PhysicalUnitTerm(distance, new Fraction(1))
);

MyEquation = equation;
// Automatically suggests: Joule, Newton·Meter, Watt·Hour, etc.
```

**Properties:**
- `EquationTerms` (EquationTerms) - The equation to analyze
- `SelectedUnit` (PhysicalUnit) - The chosen suggested unit
- `MaxSuggestions` (int) - Maximum suggestions to display (default: 10)
- `IsPopupOpen` (bool) - Controls popup visibility

### 3. PhysicalUnitBuilderView

The full-featured builder control (used internally by the button version).

**XAML Usage:**

```xaml
<puv:PhysicalUnitBuilderView 
    SelectedUnit="{Binding MyUnit, Mode=TwoWay}"
    Height="600" />
```

**Features:**
- Complete search and filtering
- Category and type selection
- Building mode with unit composition
- Base unit management
- Real-time dimensional formula display

### 4. PhysicalUnitView

Display a physical unit with all its details.

**XAML Usage:**

```xaml
<puv:PhysicalUnitView 
    PhysicalUnitViewModel="{Binding UnitViewModel}"
    IsEditing="False" />
```

**Features:**
- Shows unit name, symbol, and dimensional formula
- Displays all base units with their exponents
- Optional editing mode
- Clean, professional presentation

### 5. SpecificUnitSelectorView

Simplified selector for specific unit categories.

**XAML Usage:**

```xaml
<pus:SpecificUnitSelectorView 
    Category="Time"
    SelectedUnit="{Binding TimeUnit, Mode=TwoWay}" />
```

**Available Categories:**
- `Time` - Second, Minute, Hour, Day, Week, Month, Year
- `Electric` - Ampere, Volt (with prefixes)

**Perfect for:** Dedicated time/frequency selectors, electrical parameter inputs

### 6. RepositorySettingButtonView

Configure which unit systems are available to users.

**XAML Usage:**

```xaml
<purs:RepositorySettingButtonView />
```

**Features:**
- Toggle SI/Metric units
- Toggle Imperial units
- Toggle US customary units
- Toggle Astronomical units
- Toggle Other systems

Changes are applied globally through `PhysicalUnitRepository.Settings`.

## 📋 Complete Example: Unit Conversion App

Here's a complete example showing multiple controls working together:

```xaml
<Window x:Class="MyApp.MainWindow"
        xmlns:puw="clr-namespace:MatthL.PhysicalUnits.UI.ViewsButtons.PhysicalUnitBuilderButtonViews;assembly=MatthL.PhysicalUnits.UI"
        xmlns:pue="clr-namespace:MatthL.PhysicalUnits.UI.ViewsButtons.PhysicalUnitEquationResultButtonViews;assembly=MatthL.PhysicalUnits.UI">
    
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>
        
        <!-- Value 1 -->
        <StackPanel Grid.Row="0" Orientation="Horizontal" Margin="10">
            <TextBox Text="{Binding Value1, UpdateSourceTrigger=PropertyChanged}" 
                     Width="100" Margin="0,0,10,0"/>
            <puw:PhysicalUnitBuilderButtonView 
                SelectedUnit="{Binding Unit1, Mode=TwoWay}" 
                Width="200"/>
        </StackPanel>
        
        <!-- Value 2 -->
        <StackPanel Grid.Row="1" Orientation="Horizontal" Margin="10">
            <TextBox Text="{Binding Value2, UpdateSourceTrigger=PropertyChanged}" 
                     Width="100" Margin="0,0,10,0"/>
            <puw:PhysicalUnitBuilderButtonView 
                SelectedUnit="{Binding Unit2, Mode=TwoWay}" 
                Width="200"/>
        </StackPanel>
        
        <!-- Equation Result -->
        <StackPanel Grid.Row="2" Orientation="Horizontal" Margin="10">
            <TextBlock Text="Result Unit:" Margin="0,0,10,0" 
                       VerticalAlignment="Center"/>
            <pue:PhysicalUnitEquationResultButtonView 
                EquationTerms="{Binding EquationTerms}"
                SelectedUnit="{Binding ResultUnit, Mode=TwoWay}"
                Width="200"/>
        </StackPanel>
        
        <!-- Conversion Target -->
        <StackPanel Grid.Row="3" Orientation="Horizontal" Margin="10">
            <TextBlock Text="Convert to:" Margin="0,0,10,0" 
                       VerticalAlignment="Center"/>
            <puw:PhysicalUnitBuilderButtonView 
                SelectedUnit="{Binding TargetUnit, Mode=TwoWay}"
                UnitToConvert="{Binding ResultUnit}"
                Width="200"/>
            <TextBlock Text="{Binding ConvertedValue}" 
                       Margin="10,0,0,0"
                       VerticalAlignment="Center"/>
        </StackPanel>
    </Grid>
</Window>
```

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.Core.EquationModels;
using MatthL.PhysicalUnits.Computation.Extensions;
using Fractions;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private double _value1 = 100;
    [ObservableProperty] private double _value2 = 50;
    [ObservableProperty] private PhysicalUnit _unit1;
    [ObservableProperty] private PhysicalUnit _unit2;
    [ObservableProperty] private PhysicalUnit _resultUnit;
    [ObservableProperty] private PhysicalUnit _targetUnit;
    [ObservableProperty] private double _convertedValue;
    
    public EquationTerms EquationTerms { get; private set; }
    
    partial void OnUnit1Changed(PhysicalUnit value) => UpdateEquation();
    partial void OnUnit2Changed(PhysicalUnit value) => UpdateEquation();
    partial void OnResultUnitChanged(PhysicalUnit value) => UpdateConversion();
    partial void OnTargetUnitChanged(PhysicalUnit value) => UpdateConversion();
    
    private void UpdateEquation()
    {
        if (Unit1 == null || Unit2 == null) return;
        
        EquationTerms = new EquationTerms(
            new PhysicalUnitTerm(Unit1, new Fraction(1)),
            new PhysicalUnitTerm(Unit2, new Fraction(1))
        );
        OnPropertyChanged(nameof(EquationTerms));
    }
    
    private void UpdateConversion()
    {
        if (ResultUnit == null || TargetUnit == null) return;
        
        var resultValue = Value1 * Value2; // Simplified
        ConvertedValue = ResultUnit.ConvertValue(TargetUnit, resultValue);
    }
}
```

## 🎬 Real-World Use Cases

### Engineering Calculator

```xaml
<!-- Stress calculation: σ = F/A -->
<StackPanel>
    <TextBlock Text="Force (F):" />
    <StackPanel Orientation="Horizontal">
        <TextBox Text="{Binding Force}" Width="100"/>
        <puw:PhysicalUnitBuilderButtonView SelectedUnit="{Binding ForceUnit}"/>
    </StackPanel>
    
    <TextBlock Text="Area (A):" />
    <StackPanel Orientation="Horizontal">
        <TextBox Text="{Binding Area}" Width="100"/>
        <puw:PhysicalUnitBuilderButtonView SelectedUnit="{Binding AreaUnit}"/>
    </StackPanel>
    
    <TextBlock Text="Stress (σ):" />
    <pue:PhysicalUnitEquationResultButtonView 
        EquationTerms="{Binding StressEquation}"
        SelectedUnit="{Binding StressUnit}"/>
    <!-- Automatically suggests: Pascal, Bar, PSI, etc. -->
</StackPanel>
```

### Scientific Data Entry

```xaml
<!-- Let users enter measurements in their preferred units -->
<DataGrid ItemsSource="{Binding Measurements}">
    <DataGrid.Columns>
        <DataGridTextColumn Header="Value" Binding="{Binding Value}"/>
        <DataGridTemplateColumn Header="Unit">
            <DataGridTemplateColumn.CellTemplate>
                <DataTemplate>
                    <puw:PhysicalUnitBuilderButtonView 
                        SelectedUnit="{Binding Unit, Mode=TwoWay}"
                        UnitToConvert="{Binding DataContext.RequiredUnit, 
                                       RelativeSource={RelativeSource AncestorType=DataGrid}}"
                        IsOnlyConvertion="True"/>
                </DataTemplate>
            </DataGridTemplateColumn.CellTemplate>
        </DataGrid.Columns>
    </DataGrid.Columns>
</DataGrid>
```

### Physics Problem Solver

```xaml
<!-- Kinetic energy: E = ½mv² -->
<StackPanel>
    <TextBlock Text="Mass:" />
    <puw:PhysicalUnitBuilderButtonView SelectedUnit="{Binding Mass}"/>
    
    <TextBlock Text="Velocity:" />
    <puw:PhysicalUnitBuilderButtonView SelectedUnit="{Binding Velocity}"/>
    
    <TextBlock Text="Energy Result:" />
    <pue:PhysicalUnitEquationResultButtonView 
        EquationTerms="{Binding EnergyEquation}"
        SelectedUnit="{Binding EnergyUnit}"/>
    <!-- Suggests: Joule, Watt·Hour, Calorie, BTU, etc. -->
</StackPanel>
```

## 🎨 Customization

### Styling the Controls

All controls support standard WPF styling:

```xaml
<puw:PhysicalUnitBuilderButtonView 
    SelectedUnit="{Binding MyUnit}"
    Background="LightBlue"
    Foreground="DarkBlue"
    FontSize="14"
    Padding="10,5"/>
```

### Repository Settings

Configure available unit systems programmatically:

```csharp
using MatthL.PhysicalUnits.Infrastructure.Repositories;

// Show only SI and Metric units
PhysicalUnitRepository.Settings.ShowMetrics = true;
PhysicalUnitRepository.Settings.ShowImperial = false;
PhysicalUnitRepository.Settings.ShowUS = false;
PhysicalUnitRepository.Settings.ShowAstronomic = false;
PhysicalUnitRepository.Settings.ShowOther = false;
```

### Custom Suggestions

Control the suggestion behavior:

```xaml
<pue:PhysicalUnitEquationResultButtonView 
    EquationTerms="{Binding Equation}"
    SelectedUnit="{Binding Unit}"
    MaxSuggestions="5"/>  <!-- Show only top 5 suggestions -->
```

## 🔧 Advanced Scenarios

### Building Custom Units

Enable building mode to let users create composite units:

```csharp
// In your ViewModel
public void EnableUnitBuilding()
{
    // When IsInBuilding is true, users can double-click units to combine them
    BuilderViewModel.IsInBuilding = true;
}
```

### Working with ViewModels

The package provides ready-to-use ViewModels:

```csharp
using MatthL.PhysicalUnits.UI.ViewModels;

// PhysicalUnitViewModel - wraps a PhysicalUnit with observable properties
var unitViewModel = new PhysicalUnitViewModel(myUnit);
unitViewModel.CanEdit = true;
unitViewModel.GotModified += OnUnitModified;

// BaseUnitViewModel - wraps individual base units
var baseUnitViewModel = new BaseUnitViewModel(baseUnit);
baseUnitViewModel.Prefix = Prefix.kilo;
```

### Event Handling

All controls expose useful events:

```csharp
builderButtonView.SelectedUnitChanged += (sender, unit) =>
{
    Console.WriteLine($"User selected: {unit.Name}");
};
```

## 📊 Control Comparison

| Control | Use Case | Complexity | Best For |
|---------|----------|------------|----------|
| **PhysicalUnitBuilderButtonView** | General unit selection | Medium | Most applications |
| **PhysicalUnitEquationResultButtonView** | Formula-based selection | Low | Calculators, physics apps |
| **SpecificUnitSelectorView** | Limited predefined units | Low | Simple dropdowns |
| **PhysicalUnitBuilderView** | Advanced unit building | High | Power users, custom units |
| **RepositorySettingButtonView** | System configuration | Low | App settings |

## 🎯 Design Principles

These controls follow WPF best practices:

- **MVVM-Friendly** - Full support for data binding and commands
- **Dependency Properties** - All key properties are bindable
- **Reusable** - Drop into any WPF application
- **Performant** - Optimized for smooth UI interactions
- **Accessible** - Keyboard navigation and screen reader support

## 📚 Related Documentation

- [MatthL.PhysicalUnits Core Library](https://github.com/LapercheMatthieu/PhysicalUnits) - The underlying unit system
- [WPF Data Binding](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/data/) - Microsoft's binding guide
- [MVVM Pattern](https://docs.microsoft.com/en-us/dotnet/architecture/maui/mvvm) - Architecture guidance

## 🤝 Contributing

Contributions are welcome! Please feel free to submit issues, feature requests, or pull requests.

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 💤 Author

**MatthL**

## 🙏 Acknowledgments

- Built with ❤️ for WPF developers who need professional unit handling
- Powered by [MatthL.PhysicalUnits](https://www.nuget.org/packages/MatthL.PhysicalUnits/)
- Thanks to the WPF community for feedback and support

---

<div align="center">

**Ready to give your users powerful unit manipulation capabilities?**

Install now: `dotnet add package MatthL.PhysicalUnits.WPF`

---

Made with ❤️ by [Matthieu L](https://github.com/LapercheMatthieu)

</div>
