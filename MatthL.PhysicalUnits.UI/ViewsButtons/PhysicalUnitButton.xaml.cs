using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.UI.ViewModels;
using MatthL.PhysicalUnits.UI.Views;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MatthL.PhysicalUnits.UI.ViewsButtons
{
    /// <summary>
    /// Logique d'interaction pour PhysicalUnitButton.xaml
    /// </summary>
    public partial class PhysicalUnitButton : UserControl
    {
        #region Dependency Properties
        public static readonly DependencyProperty SelectedUnitProperty =
            DependencyProperty.Register(
                "SelectedUnit",
                typeof(PhysicalUnit),
                typeof(PhysicalUnitButton),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedUnitChanged));

        public static readonly DependencyProperty IsPopupOpenProperty =
            DependencyProperty.Register(
                "IsPopupOpen",
                typeof(bool),
                typeof(PhysicalUnitButton),
                new PropertyMetadata(false));

        public static readonly DependencyProperty UnitTooltipProperty =
            DependencyProperty.Register(
                "UnitTooltip",
                typeof(string),
                typeof(PhysicalUnitButton),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty UnitToConvertProperty =
            DependencyProperty.Register(
                nameof(UnitToConvert),
                typeof(PhysicalUnit),
                typeof(PhysicalUnitButton),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty IsOnlyConvertionProperty =
            DependencyProperty.Register(
                "IsOnlyConvertion",
                typeof(bool),
                typeof(PhysicalUnitButton),
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
        #endregion

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
        #endregion

        private PhysicalUnitSelectorViewModel _internalViewModel;

        private static void OnSelectedUnitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = (PhysicalUnitButton)d;
            button.UpdateUnitTooltip();

            // Synchroniser avec le ViewModel interne
            if (button._internalViewModel != null && button._internalViewModel.SelectedUnit != e.NewValue)
            {
                button._internalViewModel.SelectedUnit = e.NewValue as PhysicalUnit;
            }
        }

        public PhysicalUnitButton()
        {
            InitializeComponent();
            UpdateUnitTooltip();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // S'assurer que UnitSelector est chargé
            if (UnitSelector != null && UnitSelector.DataContext is PhysicalUnitSelectorViewModel vm)
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
                UnitTooltip = $"{SelectedUnit.Name} ({SelectedUnit.DimensionalFormula})";
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