using MahApps.Metro.IconPacks;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.Core.Services;
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

namespace MatthL.PhysicalUnits.UI.ViewsButtons
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
                StandardUnits.Second(Prefix.nano),
                StandardUnits.Second(Prefix.micro),
                StandardUnits.Second(Prefix.milli),
                StandardUnits.Second(Prefix.SI),
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
                               StandardUnits.Ampere(Prefix.micro),
                StandardUnits.Ampere(Prefix.milli),
                StandardUnits.Ampere(),
                StandardUnits.Volt(Prefix.micro),
                StandardUnits.Volt(Prefix.milli),
                StandardUnits.Volt(),
            };
        }
    }
}
