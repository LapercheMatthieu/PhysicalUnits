using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.DimensionalFormulas.Extensions;
using MatthL.PhysicalUnits.UI.ViewModels;
using MatthL.PhysicalUnits.UI.Views.PhysicalUnitBuilderViews;
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

namespace MatthL.PhysicalUnits.UI.ViewsButtons.PhysicalUnitBuilderButtonViews
{
    /// <summary>
    /// Logique d'interaction pour PhysicalUnitBuilderButtonView.xaml
    /// </summary>
    public partial class PhysicalUnitBuilderButtonView : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty SelectedUnitProperty =
            DependencyProperty.Register(
                "SelectedUnit",
                typeof(PhysicalUnit),
                typeof(PhysicalUnitBuilderButtonView),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedUnitChanged));

        public static readonly DependencyProperty IsPopupOpenProperty =
            DependencyProperty.Register(
                "IsPopupOpen",
                typeof(bool),
                typeof(PhysicalUnitBuilderButtonView),
                new PropertyMetadata(false));

        public static readonly DependencyProperty UnitTooltipProperty =
            DependencyProperty.Register(
                "UnitTooltip",
                typeof(string),
                typeof(PhysicalUnitBuilderButtonView),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty UnitToConvertProperty =
            DependencyProperty.Register(
                nameof(UnitToConvert),
                typeof(PhysicalUnit),
                typeof(PhysicalUnitBuilderButtonView),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty IsOnlyConvertionProperty =
            DependencyProperty.Register(
                "IsOnlyConvertion",
                typeof(bool),
                typeof(PhysicalUnitBuilderButtonView),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public PhysicalUnit UnitToConvert
        {
            get => (PhysicalUnit)GetValue(UnitToConvertProperty);
            set => SetValue(UnitToConvertProperty, value);
        }

        public bool IsOnlyConvertion
        {
            get => (bool)GetValue(IsOnlyConvertionProperty);
            set => SetValue(IsOnlyConvertionProperty, value);
        }

        #endregion Dependency Properties

        #region Properties

        /// <summary>
        /// Unité sélectionnée
        /// </summary>
        public PhysicalUnit SelectedUnit
        {
            get { return (PhysicalUnit)GetValue(SelectedUnitProperty); }
            set { SetValue(SelectedUnitProperty, value); }
        }

        /// <summary>
        /// Indique si le popup est ouvert
        /// </summary>
        public bool IsPopupOpen
        {
            get { return (bool)GetValue(IsPopupOpenProperty); }
            set { SetValue(IsPopupOpenProperty, value); }
        }

        /// <summary>
        /// Texte d'info-bulle pour l'unité
        /// </summary>
        public string UnitTooltip
        {
            get { return (string)GetValue(UnitTooltipProperty); }
            set { SetValue(UnitTooltipProperty, value); }
        }

        #endregion Properties

        private PhysicalUnitBuilderViewModel _internalViewModel;

        private static void OnSelectedUnitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = (PhysicalUnitBuilderButtonView)d;
            button.UpdateUnitTooltip();

            // Synchroniser avec le ViewModel interne
            if (button._internalViewModel != null && button._internalViewModel.SelectedUnit != e.NewValue)
            {
                button._internalViewModel.SelectedUnit = e.NewValue as PhysicalUnit;
            }
        }

        public PhysicalUnitBuilderButtonView()
        {
            InitializeComponent();
            UpdateUnitTooltip();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // S'assurer que UnitSelector est chargé
            if (UnitSelector != null && UnitSelector.DataContext is PhysicalUnitBuilderViewModel vm)
            {
                _internalViewModel = vm;

                _internalViewModel.GotModified += VM_GotModified;
                _internalViewModel.SelectedUnitChanged += VM_SelectedUnitChanged;

                // Synchroniser l'état initial
                if (SelectedUnit != null)
                {
                    _internalViewModel.SelectedUnit = SelectedUnit;
                }
            }
        }

        private void VM_SelectedUnitChanged(object? sender, PhysicalUnit e)
        {
            // Mettre à jour la DependencyProperty sans déclencher une boucle infinie
            if (SelectedUnit != e)
            {
                SetCurrentValue(SelectedUnitProperty, e);
            }
        }

        private void VM_GotModified()
        {
            // Forcer WPF à détecter le changement
            SetCurrentValue(SelectedUnitProperty, new PhysicalUnit(_internalViewModel.SelectedUnit));
        }

        private void UpdateUnitTooltip()
        {
            if (SelectedUnit != null)
            {
                UnitTooltip = $"{SelectedUnit.Name} ({SelectedUnit.GetDimensionalFormula()})";
            }
            else
            {
                UnitTooltip = "Aucune unité sélectionnée";
            }
        }

        private void UnitButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsOnlyConvertion && UnitToConvert == null) return;
            IsPopupOpen = !IsPopupOpen;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            IsPopupOpen = false;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedUnit = null;
            // Optionnel: fermer le popup après effacement
            // IsPopupOpen = false;
        }
    }
}
