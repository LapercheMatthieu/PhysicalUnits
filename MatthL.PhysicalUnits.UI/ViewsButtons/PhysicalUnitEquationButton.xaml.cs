using MatthL.PhysicalUnits.Core.EquationModels;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.DimensionalFormulas;
using MatthL.PhysicalUnits.DimensionalFormulas.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace MatthL.PhysicalUnits.UI.ViewsButtons
{
    /// <summary>
    /// Logique d'interaction pour PhysicalUnitEquationButton.xaml
    /// </summary>
    public partial class PhysicalUnitEquationButton : UserControl
    {
        #region Dependency Properties

        /// <summary>
        /// Les termes de l'équation à utiliser pour les suggestions
        /// </summary>
        public static readonly DependencyProperty EquationTermsProperty =
            DependencyProperty.Register(
                "EquationTerms",
                typeof(EquationTerms),
                typeof(PhysicalUnitEquationButton),
                new PropertyMetadata(null, OnEquationTermsChanged));

        /// <summary>
        /// L'unité sélectionnée parmi les suggestions
        /// </summary>
        public static readonly DependencyProperty SelectedUnitProperty =
            DependencyProperty.Register(
                "SelectedUnit",
                typeof(PhysicalUnit),
                typeof(PhysicalUnitEquationButton),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedUnitChanged));

        /// <summary>
        /// Indique si le popup est ouvert
        /// </summary>
        public static readonly DependencyProperty IsPopupOpenProperty =
            DependencyProperty.Register(
                "IsPopupOpen",
                typeof(bool),
                typeof(PhysicalUnitEquationButton),
                new PropertyMetadata(false));

        /// <summary>
        /// Texte d'info-bulle pour l'unité
        /// </summary>
        public static readonly DependencyProperty UnitTooltipProperty =
            DependencyProperty.Register(
                "UnitTooltip",
                typeof(string),
                typeof(PhysicalUnitEquationButton),
                new PropertyMetadata(string.Empty));

        /// <summary>
        /// Texte affiché sur le bouton
        /// </summary>
        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register(
                "ButtonText",
                typeof(string),
                typeof(PhysicalUnitEquationButton),
                new PropertyMetadata("Sélectionner..."));

        /// <summary>
        /// Nombre maximum de suggestions à afficher
        /// </summary>
        public static readonly DependencyProperty MaxSuggestionsProperty =
            DependencyProperty.Register(
                "MaxSuggestions",
                typeof(int),
                typeof(PhysicalUnitEquationButton),
                new PropertyMetadata(10));

        #endregion Dependency Properties

        #region Properties

        /// <summary>
        /// Les termes de l'équation
        /// </summary>
        public EquationTerms EquationTerms
        {
            get { return (EquationTerms)GetValue(EquationTermsProperty); }
            set { SetValue(EquationTermsProperty, value); }
        }

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

        /// <summary>
        /// Texte du bouton
        /// </summary>
        public string ButtonText
        {
            get { return (string)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }

        /// <summary>
        /// Nombre maximum de suggestions
        /// </summary>
        public int MaxSuggestions
        {
            get { return (int)GetValue(MaxSuggestionsProperty); }
            set { SetValue(MaxSuggestionsProperty, value); }
        }

        #endregion Properties

        #region Constructor

        public PhysicalUnitEquationButton()
        {
            InitializeComponent();
            DataContext = this;
            UpdateButtonText();
            UpdateUnitTooltip();
        }

        #endregion Constructor

        #region Methods

        private static void OnEquationTermsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = (PhysicalUnitEquationButton)d;
            button.UpdateButtonText();
            button.UpdateUnitTooltip();
        }

        private static void OnSelectedUnitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = (PhysicalUnitEquationButton)d;
            button.UpdateButtonText();
            button.UpdateUnitTooltip();
        }

        private void UpdateButtonText()
        {
            if (SelectedUnit != null)
            {
                ButtonText = SelectedUnit.ToString();
            }
            else if (EquationTerms?.Terms != null && EquationTerms.Terms.Any())
            {
                ButtonText = "Choisir parmi les suggestions...";
            }
            else
            {
                ButtonText = "Aucune équation définie";
            }
        }

        private void UpdateUnitTooltip()
        {
            if (SelectedUnit != null)
            {
                UnitTooltip = $"{SelectedUnit.Name} ({SelectedUnit.GetDimensionalFormula()})";
            }
            else if (EquationTerms?.Terms != null && EquationTerms.Terms.Any())
            {
                var formula = FormulaBuilder.GetDimensionalFormula(EquationTerms.Terms.ToArray());
                UnitTooltip = $"Formule: {formula}";
            }
            else
            {
                UnitTooltip = "Aucune équation définie";
            }
        }

        private void UnitButton_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir le popup seulement si on a des termes d'équation
            if (EquationTerms?.Terms != null && EquationTerms.Terms.Any())
            {
                IsPopupOpen = !IsPopupOpen;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            IsPopupOpen = false;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedUnit = null;
            UpdateButtonText();
            UpdateUnitTooltip();
        }

        #endregion Methods
    }
}