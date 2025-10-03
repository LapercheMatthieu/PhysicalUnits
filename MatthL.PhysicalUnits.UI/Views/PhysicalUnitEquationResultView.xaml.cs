using MatthL.PhysicalUnits.Computation.Helpers;
using MatthL.PhysicalUnits.Computation.Models;
using MatthL.PhysicalUnits.Core.EquationModels;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.DimensionalFormulas;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace MatthL.PhysicalUnits.UI.Views
{
    /// <summary>
    /// Logique d'interaction pour PhysicalUnitEquationResultView.xaml
    /// </summary>
    public partial class PhysicalUnitEquationResultView : UserControl, INotifyPropertyChanged
    {
        #region Dependency Properties

        /// <summary>
        /// Les termes de l'équation physique
        /// </summary>
        public static readonly DependencyProperty EquationTermsProperty =
            DependencyProperty.Register(
                nameof(EquationTerms),
                typeof(EquationTerms),
                typeof(PhysicalUnitEquationResultView),
                new PropertyMetadata(null, OnEquationTermsChanged));

        /// <summary>
        /// L'unité sélectionnée parmi les suggestions
        /// </summary>
        public static readonly DependencyProperty SelectedUnitProperty =
            DependencyProperty.Register(
                nameof(SelectedUnit),
                typeof(PhysicalUnit),
                typeof(PhysicalUnitEquationResultView),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Nombre maximum de suggestions à afficher
        /// </summary>
        public static readonly DependencyProperty MaxSuggestionsProperty =
            DependencyProperty.Register(
                nameof(MaxSuggestions),
                typeof(int),
                typeof(PhysicalUnitEquationResultView),
                new PropertyMetadata(10));

        #endregion Dependency Properties

        #region Properties

        public EquationTerms EquationTerms
        {
            get => (EquationTerms)GetValue(EquationTermsProperty);
            set => SetValue(EquationTermsProperty, value);
        }

        public PhysicalUnit SelectedUnit
        {
            get => (PhysicalUnit)GetValue(SelectedUnitProperty);
            set => SetValue(SelectedUnitProperty, value);
        }

        public int MaxSuggestions
        {
            get => (int)GetValue(MaxSuggestionsProperty);
            set => SetValue(MaxSuggestionsProperty, value);
        }

        private ObservableCollection<UnitSuggestion> _suggestions = new ObservableCollection<UnitSuggestion>();

        public ObservableCollection<UnitSuggestion> Suggestions
        {
            get => _suggestions;
            set
            {
                _suggestions = value;
                OnPropertyChanged();
            }
        }

        private UnitSuggestion _selectedSuggestion;

        public UnitSuggestion SelectedSuggestion
        {
            get => _selectedSuggestion;
            set
            {
                _selectedSuggestion = value;
                OnPropertyChanged();

                // Mettre à jour l'unité sélectionnée
                if (value != null)
                {
                    SelectedUnit = value.Unit;
                }
            }
        }

        private string _equationFormula;

        public string EquationFormula
        {
            get => _equationFormula;
            set
            {
                _equationFormula = value;
                OnPropertyChanged();
            }
        }

        private bool _showExplanations = false;

        public bool ShowExplanations
        {
            get => _showExplanations;
            set
            {
                _showExplanations = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        #region Constructor

        public PhysicalUnitEquationResultView()
        {
            InitializeComponent();
            DataContext = this;
        }

        #endregion Constructor

        #region Methods

        private static void OnEquationTermsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (PhysicalUnitEquationResultView)d;
            control.UpdateSuggestions();
        }

        private void UpdateSuggestions()
        {
            Suggestions.Clear();

            if (EquationTerms?.Terms == null || !EquationTerms.Terms.Any())
            {
                EquationFormula = string.Empty;
                return;
            }

            // Calculer la formule dimensionnelle
            var terms = EquationTerms.Terms.ToArray();
            EquationFormula = FormulaBuilder.GetDimensionalFormula(terms);

            // Obtenir les suggestions
            var suggestions = UnitSuggestionHelper.GetUnitSuggestions(terms);

            // Limiter le nombre de suggestions et les ajouter
            foreach (var suggestion in suggestions.Take(MaxSuggestions))
            {
                Suggestions.Add(suggestion);
            }

            // Sélectionner automatiquement la meilleure suggestion
            if (Suggestions.Any())
            {
                SelectedSuggestion = Suggestions.First();
            }
        }

        #endregion Methods

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged
    }
}