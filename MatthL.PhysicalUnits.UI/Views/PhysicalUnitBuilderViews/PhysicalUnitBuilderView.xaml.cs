using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.UI.ViewModels;
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

namespace MatthL.PhysicalUnits.UI.Views.PhysicalUnitBuilderViews
{
    /// <summary>
    /// Logique d'interaction pour PhysicalUnitBuilderViews.xaml
    /// </summary>
    public partial class PhysicalUnitBuilderView : UserControl
    {
        public static readonly DependencyProperty SelectedUnitProperty =
             DependencyProperty.Register(
                 nameof(SelectedUnit),
                 typeof(PhysicalUnit),
                 typeof(PhysicalUnitBuilderView),
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
                typeof(PhysicalUnitBuilderView),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnUnitToConvertChanged));

        public PhysicalUnit UnitToConvert
        {
            get => (PhysicalUnit)GetValue(UnitToConvertProperty);
            set => SetValue(UnitToConvertProperty, value);
        }

        private PhysicalUnitBuilderViewModel ViewModel { get; set; }

        private static void OnSelectedUnitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (PhysicalUnitBuilderView)d;
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
            var control = (PhysicalUnitBuilderView)d;
            if (control.ViewModel != null)
            {
                // Active/désactive automatiquement le mode conversion
                control.ViewModel.IsConverting = e.NewValue != null;
                control.ViewModel.UnitToConvert = e.NewValue as PhysicalUnit;
            }
        }

        public PhysicalUnitBuilderView()
        {
            InitializeComponent();

            // Créer le ViewModel
            ViewModel = new PhysicalUnitBuilderViewModel();
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