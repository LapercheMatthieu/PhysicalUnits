# MatthL.PhysicalUnits.WPF

A comprehensive WPF controls library for working with physical units in .NET applications. This package provides ready-to-use UI components that leverage the power of [MatthL.PhysicalUnits](https://www.nuget.org/packages/MatthL.PhysicalUnits/) to create intuitive, user-friendly interfaces for unit selection, conversion, and manipulation.

[![NuGet](https://img.shields.io/nuget/v/MatthL.PhysicalUnits.WPF.svg)](https://www.nuget.org/packages/MatthL.PhysicalUnits.WPF/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

## 🎯 Key Features

- ✅ **Ready-to-Use WPF Controls** - Drop-in UI components for unit management
- ✅ **Unit Selection ComboBoxes** - Context-aware unit pickers with intelligent filtering
- ✅ **Unit Builders** - Visual tools for creating composite units (m/s, N·m, etc.)
- ✅ **Conversion Displays** - Real-time conversion visualization
- ✅ **Equation Result Displays** - Show results of unit operations with suggestions
- ✅ **Prefix Selectors** - Easy selection of SI prefixes (kilo, milli, etc.)
- ✅ **MVVM-Friendly** - Full data binding support

## 📦 Installation

```bash
dotnet add package MatthL.PhysicalUnits.WPF
```

**Prerequisites:**
- .NET 6.0 or higher
- WPF application
- MatthL.PhysicalUnits (installed automatically as dependency)

## 🚀 Quick Start

### 1. Initialize the Repository

Before using any controls, initialize the physical unit repository in your application startup:

```csharp
using MatthL.PhysicalUnits.Infrastructure.Repositories;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
        // Initialize the repository with 400+ units
        PhysicalUnitRepository.Initialize();
    }
}
```

### 2. Add Namespace References

In your XAML files, add the namespace:

```xaml
xmlns:wpf="clr-namespace:MatthL.PhysicalUnits.WPF;assembly=MatthL.PhysicalUnits.WPF"
```

### 3. Use the Controls

```xaml
<Window x:Class="YourApp.MainWindow"
        xmlns:wpf="clr-namespace:MatthL.PhysicalUnits.WPF;assembly=MatthL.PhysicalUnits.WPF">
    
    <StackPanel>
        <!-- Unit selector -->
        <wpf:PhysicalUnitView SelectedUnit="{Binding MyUnit, Mode=TwoWay}" />
        
        <!-- Unit builder for composite units -->
        <wpf:PhysicalUnitBuilderView ResultUnit="{Binding CompositeUnit, Mode=TwoWay}" />
    </StackPanel>
</Window>
```

## 🎨 Available Controls

### PhysicalUnitView
![PhysicalUnitView](https://raw.githubusercontent.com/LapercheMatthieu/MatthL.PhysicalUnits/main/Resources/PhysicalUnitView.png)

A comprehensive control for selecting and displaying physical units with filtering by domain and type.

```xaml
<wpf:PhysicalUnitView 
    SelectedUnit="{Binding MyUnit, Mode=TwoWay}"
    ShowDomainFilter="True"
    ShowTypeFilter="True" />
```

**Properties:**
- `SelectedUnit` (PhysicalUnit) - The currently selected unit
- `ShowDomainFilter` (bool) - Show domain filtering dropdown
- `ShowTypeFilter` (bool) - Show unit type filtering dropdown

### PhysicalUnitBuilderView
![PhysicalUnitBuilderView](https://raw.githubusercontent.com/LapercheMatthieu/MatthL.PhysicalUnits/main/Resources/PhysicalUnitBuilder.png)

Build composite units by multiplying, dividing, and raising units to powers.

```xaml
<wpf:PhysicalUnitBuilderView 
    ResultUnit="{Binding BuilderResult, Mode=TwoWay}"
    ShowPreview="True" />
```

**Properties:**
- `ResultUnit` (PhysicalUnit) - The resulting composite unit
- `ShowPreview` (bool) - Display real-time preview of the unit

**Features:**
- Add multiple unit terms
- Set exponents (including fractions like 1/2)
- Real-time dimensional formula preview
- Smart unit suggestions based on the composition

### BaseUnitView
![BaseUnitView](https://raw.githubusercontent.com/LapercheMatthieu/MatthL.PhysicalUnits/main/Resources/BaseUnitView.png)

Select base units (length, mass, time, etc.) with optional prefix selection.

```xaml
<wpf:BaseUnitView 
    SelectedBaseUnit="{Binding MyBaseUnit, Mode=TwoWay}"
    ShowPrefix="True" />
```

### PrefixSelectorView
![PrefixSelector](https://raw.githubusercontent.com/LapercheMatthieu/MatthL.PhysicalUnits/main/Resources/PrefixSelector.png)

Standalone prefix selector for SI prefixes (nano, micro, milli, kilo, mega, etc.).

```xaml
<wpf:PrefixSelectorView 
    SelectedPrefix="{Binding MyPrefix, Mode=TwoWay}" />
```

### PhysicalUnitEquationResultView
![EquationResult](https://raw.githubusercontent.com/LapercheMatthieu/MatthL.PhysicalUnits/main/Resources/FormulaResultnmperus.png)

Display equation results with intelligent unit suggestions.

```xaml
<wpf:PhysicalUnitEquationResultView 
    EquationTerms="{Binding MyEquation}"
    SelectedResultUnit="{Binding ResultUnit, Mode=TwoWay}"
    ShowSuggestions="True" />
```

**Features:**
- Displays the dimensional formula of the result
- Shows top unit suggestions with relevance scores
- Includes explanations for each suggested unit

### ConvertForKgView
![ConvertForKg](https://raw.githubusercontent.com/LapercheMatthieu/MatthL.PhysicalUnits/main/Resources/ConvertForKg.png)

Specialized conversion control optimized for specific use cases.

## 📝 Complete Example

Here's a complete example demonstrating unit selection, conversion, and equation building:

```xaml
<Window x:Class="PhysicalUnitsDemo.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:wpf="clr-namespace:MatthL.PhysicalUnits.WPF;assembly=MatthL.PhysicalUnits.WPF"
        Title="Physical Units Demo" Height="600" Width="800">
    
    <Grid Margin="20">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>
        
        <!-- First Unit Input -->
        <GroupBox Header="Unit 1" Grid.Row="0" Margin="0,0,0,10">
            <StackPanel>
                <StackPanel Orientation="Horizontal" Margin="0,5">
                    <TextBlock Text="Value:" Width="80" VerticalAlignment="Center"/>
                    <TextBox Text="{Binding Value1, UpdateSourceTrigger=PropertyChanged}" 
                             Width="100" Margin="5,0"/>
                    <wpf:PhysicalUnitView SelectedUnit="{Binding Unit1, Mode=TwoWay}" 
                                          Margin="10,0,0,0"/>
                </StackPanel>
                
                <StackPanel Orientation="Horizontal" Margin="0,5">
                    <TextBlock Text="In SI:" Width="80" VerticalAlignment="Center"/>
                    <TextBlock Text="{Binding Value1InSI, StringFormat=N4}" 
                               Width="100" VerticalAlignment="Center"/>
                    <TextBlock Text="{Binding Unit1InSI.Symbol}" 
                               Margin="5,0" VerticalAlignment="Center"/>
                </StackPanel>
            </StackPanel>
        </GroupBox>
        
        <!-- Second Unit Input -->
        <GroupBox Header="Unit 2" Grid.Row="1" Margin="0,0,0,10">
            <StackPanel>
                <StackPanel Orientation="Horizontal" Margin="0,5">
                    <TextBlock Text="Value:" Width="80" VerticalAlignment="Center"/>
                    <TextBox Text="{Binding Value2, UpdateSourceTrigger=PropertyChanged}" 
                             Width="100" Margin="5,0"/>
                    <wpf:PhysicalUnitView SelectedUnit="{Binding Unit2, Mode=TwoWay}" 
                                          Margin="10,0,0,0"/>
                </StackPanel>
                
                <StackPanel Orientation="Horizontal" Margin="0,5">
                    <TextBlock Text="In SI:" Width="80" VerticalAlignment="Center"/>
                    <TextBlock Text="{Binding Value2InSI, StringFormat=N4}" 
                               Width="100" VerticalAlignment="Center"/>
                    <TextBlock Text="{Binding Unit2InSI.Symbol}" 
                               Margin="5,0" VerticalAlignment="Center"/>
                </StackPanel>
            </StackPanel>
        </GroupBox>
        
        <!-- Unit Conversion -->
        <GroupBox Header="Convert To" Grid.Row="2" Margin="0,0,0,10">
            <StackPanel>
                <wpf:PhysicalUnitView SelectedUnit="{Binding TargetUnit, Mode=TwoWay}" 
                                      Margin="0,5"/>
                
                <Grid Margin="0,5">
                    <Grid.ColumnDefinitions>
                        <ColumnDefinition Width="*"/>
                        <ColumnDefinition Width="*"/>
                    </Grid.ColumnDefinitions>
                    
                    <StackPanel Grid.Column="0" Orientation="Horizontal">
                        <TextBlock Text="Value1 → " VerticalAlignment="Center"/>
                        <TextBlock Text="{Binding ConvertedValue1, StringFormat=N4}" 
                                   FontWeight="Bold" VerticalAlignment="Center"/>
                    </StackPanel>
                    
                    <StackPanel Grid.Column="1" Orientation="Horizontal">
                        <TextBlock Text="Value2 → " VerticalAlignment="Center"/>
                        <TextBlock Text="{Binding ConvertedValue2, StringFormat=N4}" 
                                   FontWeight="Bold" VerticalAlignment="Center"/>
                    </StackPanel>
                </Grid>
            </StackPanel>
        </GroupBox>
        
        <!-- Equation Builder -->
        <GroupBox Header="Unit Equation" Grid.Row="3" Margin="0,0,0,10">
            <StackPanel>
                <Grid Margin="0,5">
                    <Grid.ColumnDefinitions>
                        <ColumnDefinition Width="Auto"/>
                        <ColumnDefinition Width="100"/>
                        <ColumnDefinition Width="Auto"/>
                        <ColumnDefinition Width="100"/>
                    </Grid.ColumnDefinitions>
                    
                    <TextBlock Grid.Column="0" Text="Exponent 1:" 
                               VerticalAlignment="Center" Margin="0,0,10,0"/>
                    <TextBox Grid.Column="1" Text="{Binding Exponent1, UpdateSourceTrigger=PropertyChanged}"/>
                    
                    <TextBlock Grid.Column="2" Text="Exponent 2:" 
                               VerticalAlignment="Center" Margin="20,0,10,0"/>
                    <TextBox Grid.Column="3" Text="{Binding Exponent2, UpdateSourceTrigger=PropertyChanged}"/>
                </Grid>
                
                <wpf:PhysicalUnitEquationResultView 
                    EquationTerms="{Binding EquationTerms}"
                    SelectedResultUnit="{Binding EquationResultUnit, Mode=TwoWay}"
                    Margin="0,10,0,0"/>
            </StackPanel>
        </GroupBox>
        
        <!-- Reset Button -->
        <Button Grid.Row="4" Content="Reset All" 
                Command="{Binding ResetCommand}"
                HorizontalAlignment="Right" VerticalAlignment="Bottom"
                Width="100" Height="30" Margin="0,10,0,0"/>
    </Grid>
</Window>
```

**ViewModel:**

```csharp
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.Core.EquationModels;
using MatthL.PhysicalUnits.Infrastructure.Extensions;
using Fractions;

public class MainViewModel : INotifyPropertyChanged
{
    private double _value1 = 1.0;
    private double _value2 = 1.0;
    private PhysicalUnit _unit1;
    private PhysicalUnit _unit2;
    private PhysicalUnit _targetUnit;
    private EquationTerms _equationTerms;
    
    public double Value1
    {
        get => _value1;
        set { _value1 = value; OnPropertyChanged(); UpdateCalculations(); }
    }
    
    public PhysicalUnit Unit1
    {
        get => _unit1;
        set { _unit1 = value; OnPropertyChanged(); UpdateCalculations(); }
    }
    
    public double Value1InSI => Unit1?.ConvertToSIValue(Value1) ?? 0;
    public PhysicalUnit Unit1InSI => Unit1?.GetSIUnit();
    
    // Similar properties for Unit2, TargetUnit, etc.
    
    private void UpdateCalculations()
    {
        OnPropertyChanged(nameof(Value1InSI));
        OnPropertyChanged(nameof(Unit1InSI));
        OnPropertyChanged(nameof(ConvertedValue1));
        
        // Update equation terms
        if (Unit1 != null && Unit2 != null)
        {
            var exp1 = ParseFraction(Exponent1);
            var exp2 = ParseFraction(Exponent2);
            
            EquationTerms = new EquationTerms(
                new PhysicalUnitTerm(Unit1, exp1),
                new PhysicalUnitTerm(Unit2, exp2)
            );
        }
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
```

## 🎨 Customization

### Custom Button Styles

All controls support custom button styling through resources:

```xaml
<wpf:PhysicalUnitBuilderView>
    <wpf:PhysicalUnitBuilderView.Resources>
        <Style TargetType="Button" x:Key="CustomButtonStyle">
            <Setter Property="Background" Value="#2196F3"/>
            <Setter Property="Foreground" Value="White"/>
            <Setter Property="Padding" Value="10,5"/>
        </Style>
    </wpf:PhysicalUnitBuilderView.Resources>
</wpf:PhysicalUnitBuilderView>
```

### Icons and Images

The package includes built-in icons for various operations:

- ![Composed Unit](https://raw.githubusercontent.com/LapercheMatthieu/MatthL.PhysicalUnits/main/Resources/ComposedUnit.png) Composite units
- ![Custom Unit](https://raw.githubusercontent.com/LapercheMatthieu/MatthL.PhysicalUnits/main/Resources/CustomUnit.png) Custom units
- ![Empty Builder](https://raw.githubusercontent.com/LapercheMatthieu/MatthL.PhysicalUnits/main/Resources/EmptyPhysicalUnitBuilder.png) Empty state
- ![Newton](https://raw.githubusercontent.com/LapercheMatthieu/MatthL.PhysicalUnits/main/Resources/NewtonRepresentation.png) Physical quantities
- ![Quick Time](https://raw.githubusercontent.com/LapercheMatthieu/MatthL.PhysicalUnits/main/Resources/QuickTime.png) Time units
- ![Search](https://raw.githubusercontent.com/LapercheMatthieu/MatthL.PhysicalUnits/main/Resources/SearchForce.png) Search functionality

## 🔧 Advanced Features

### Repository Settings View

Configure which units are available in your application:

```xaml
<wpf:RepositorySettingView 
    AllowCustomUnits="True"
    EnabledDomains="{Binding EnabledDomains}"
    EnabledTypes="{Binding EnabledTypes}" />
```

### Specific Unit Selector Views

For domain-specific applications, use specialized selectors:

```xaml
<!-- Only show time-related units -->
<wpf:SpecificUnitSelectorView 
    AllowedDomains="Time"
    SelectedUnit="{Binding Duration, Mode=TwoWay}" />
```

### Physical Unit Builder Button

Add quick-access builder buttons to your interface:

```xaml
<wpf:PhysicalUnitBuilderButton 
    ResultUnit="{Binding MyCompositeUnit, Mode=TwoWay}"
    ButtonText="Build Unit" />
```

## 📊 Unit Type Coverage

The controls support all unit types from the core library:

- **Base Units**: Length, Mass, Time, Temperature, Current, Amount, Luminosity
- **Mechanics**: Force, Pressure, Energy, Power, Torque, Momentum
- **Electricity**: Voltage, Resistance, Capacitance, Inductance, Charge
- **Thermodynamics**: Heat Flux, Thermal Conductivity, Entropy
- **Fluidics**: Flow Rate, Viscosity, Density
- **Chemistry**: Molarity, Molar Mass, Concentration
- **Optics**: Illuminance, Luminous Intensity
- **Economics**: Cost per Unit, Energy Cost
- **Computing**: Bit Rate, Data Size
- **Transport**: Fuel Efficiency

## 🧪 Testing Your UI

When testing your WPF application:

```csharp
[TestInitialize]
public void Setup()
{
    // Initialize repository before tests
    PhysicalUnitRepository.Initialize();
}

[TestMethod]
public void TestUnitSelection()
{
    var viewModel = new MainViewModel();
    var meter = StandardUnits.Meter();
    
    viewModel.Unit1 = meter;
    viewModel.Value1 = 100;
    
    Assert.AreEqual(100, viewModel.Value1InSI);
}
```

## 🔗 Related Packages

- [MatthL.PhysicalUnits](https://www.nuget.org/packages/MatthL.PhysicalUnits/) - Core library (auto-installed)
- [MatthL.PhysicalUnits.Core](https://www.nuget.org/packages/MatthL.PhysicalUnits.Core/) - Base models
- [MatthL.PhysicalUnits.Infrastructure](https://www.nuget.org/packages/MatthL.PhysicalUnits.Infrastructure/) - Unit library

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 👤 Author

**MatthL**

## 🙏 Acknowledgments

- Built on top of MatthL.PhysicalUnits core library
- Designed for real-world WPF applications
- Community-driven development

---

<div align="center">

**Need help?** Check out the [Wiki](https://github.com/LapercheMatthieu/MatthL.PhysicalUnits/wiki) or open an [Issue](https://github.com/LapercheMatthieu/MatthL.PhysicalUnits/issues)

Made with ❤️ by [Matthieu L](https://github.com/LapercheMatthieu)

</div>
