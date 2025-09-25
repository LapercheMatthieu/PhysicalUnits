using MahApps.Metro.IconPacks;
using MatthL.PhysicalUnits.Models;
using MatthL.PhysicalUnits.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MatthL.PhysicalUnits.ViewsButtons
{
    public enum UnitCategory
    {
        Time,
        Electric
    }

    public partial class SpecificUnitSelector : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty SelectedUnitProperty =
            DependencyProperty.Register(
                nameof(SelectedUnit),
                typeof(PhysicalUnit),
                typeof(SpecificUnitSelector),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty CategoryProperty =
            DependencyProperty.Register(
                nameof(Category),
                typeof(UnitCategory),
                typeof(SpecificUnitSelector),
                new PropertyMetadata(UnitCategory.Time, OnCategoryChanged));

        public static readonly DependencyProperty AvailableUnitsProperty =
            DependencyProperty.Register(
                nameof(AvailableUnits),
                typeof(List<PhysicalUnit>),
                typeof(SpecificUnitSelector),
                new PropertyMetadata(new List<PhysicalUnit>()));

        #endregion

        #region Properties

        public PhysicalUnit SelectedUnit
        {
            get => (PhysicalUnit)GetValue(SelectedUnitProperty);
            set => SetValue(SelectedUnitProperty, value);
        }

        public UnitCategory Category
        {
            get => (UnitCategory)GetValue(CategoryProperty);
            set => SetValue(CategoryProperty, value);
        }

        public List<PhysicalUnit> AvailableUnits
        {
            get => (List<PhysicalUnit>)GetValue(AvailableUnitsProperty);
            set => SetValue(AvailableUnitsProperty, value);
        }

        #endregion

        public SpecificUnitSelector()
        {
            InitializeComponent();
            UpdateAvailableUnits();
        }

        private static void OnCategoryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SpecificUnitSelector)d).UpdateAvailableUnits();
        }

        private void UpdateAvailableUnits()
        {
            switch (Category)
            {
                case UnitCategory.Time:
                    AvailableUnits = GetTimeUnits();
                    break;
                case UnitCategory.Electric:
                    AvailableUnits = GetElectricUnits();
                    break;
            }
        }


        private List<PhysicalUnit> GetTimeUnits()
        {
            return new List<PhysicalUnit>
            {
                StandardUnits.Second(Enums.Prefix.nano),
                StandardUnits.Second(Enums.Prefix.micro),
                StandardUnits.Second(Enums.Prefix.milli),
                StandardUnits.Second(Enums.Prefix.SI),
                StandardUnits.Minute,
                StandardUnits.Hour,
                StandardUnits.Day,
                StandardUnits.Week,
                StandardUnits.Month,
                StandardUnits.Year,
            };
        }

        private List<PhysicalUnit> GetElectricUnits()
        {
            return new List<PhysicalUnit>
            {
                               StandardUnits.Ampere(Enums.Prefix.micro),
                StandardUnits.Ampere(Enums.Prefix.milli),
                StandardUnits.Ampere(),
                StandardUnits.Volt(Enums.Prefix.micro),
                StandardUnits.Volt(Enums.Prefix.milli),
                StandardUnits.Volt(),
            };
        }
    }
}
