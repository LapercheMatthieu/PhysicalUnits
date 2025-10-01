using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.UI.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MatthL.PhysicalUnits.UI.Views
{
    /// <summary>
    /// Logique d'interaction pour PhysicalUnitBuilder.xaml
    /// </summary>
    public partial class PhysicalUnitBuilder : UserControl
    {
        public static readonly DependencyProperty SelectedUnitProperty =
             DependencyProperty.Register(
                 nameof(SelectedUnit),
                 typeof(PhysicalUnit),
                 typeof(PhysicalUnitBuilder),
                 new FrameworkPropertyMetadata(
                     null,
                     FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                     OnSelectedUnitChanged));

        public PhysicalUnit SelectedUnit
        {
            get => (PhysicalUnit)GetValue(SelectedUnitProperty);
            set => SetValue(SelectedUnitProperty, value);
        }

        public static readonly DependencyProperty UnitToConvertProperty =
            DependencyProperty.Register(
                nameof(UnitToConvert),
                typeof(PhysicalUnit),
                typeof(PhysicalUnitBuilder),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnUnitToConvertChanged));

        public PhysicalUnit UnitToConvert
        {
            get => (PhysicalUnit)GetValue(UnitToConvertProperty);
            set => SetValue(UnitToConvertProperty, value);
        }

        private PhysicalUnitSelectorViewModel ViewModel { get; set; }

        private static void OnSelectedUnitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (PhysicalUnitBuilder)d;
            if (control.ViewModel != null)
            {
                var newUnit = e.NewValue as PhysicalUnit;
                if (control.ViewModel.SelectedUnit != newUnit)
                {
                    control.ViewModel.SelectedUnit = newUnit;
                }
            }
        }

        private static void OnUnitToConvertChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (PhysicalUnitBuilder)d;
            if (control.ViewModel != null)
            {
                // Active/désactive automatiquement le mode conversion
                control.ViewModel.IsConverting = e.NewValue != null;
                control.ViewModel.UnitToConvert = e.NewValue as PhysicalUnit;
            }
        }

        public PhysicalUnitBuilder()
        {
            InitializeComponent();

            // Créer le ViewModel
            ViewModel = new PhysicalUnitSelectorViewModel();
            DataContext = ViewModel;

            // Synchroniser l'état initial
            ViewModel.IsConverting = UnitToConvert != null;
            ViewModel.UnitToConvert = UnitToConvert;
            ViewModel.SelectedUnit = SelectedUnit;

            // IMPORTANT : Synchroniser les changements du ViewModel vers la DependencyProperty
            ViewModel.SelectedUnitChanged += OnViewModelSelectedUnitChanged;
            ViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelSelectedUnitChanged(object sender, PhysicalUnit unit)
        {
            // Mettre à jour la DependencyProperty quand le ViewModel change
            if (SelectedUnit != unit)
            {
                SetCurrentValue(SelectedUnitProperty, unit);
            }
        }

        private void OnViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.SelectedUnit))
            {
                // Synchroniser aussi via PropertyChanged au cas où
                if (SelectedUnit != ViewModel.SelectedUnit)
                {
                    SetCurrentValue(SelectedUnitProperty, ViewModel.SelectedUnit);
                }
            }
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel?.IsInBuilding == true && UnitListBox.SelectedItem is PhysicalUnit selectedUnit)
            {
                ViewModel.AddPhysicalUnitToBuild(selectedUnit);
            }
        }
    }
}