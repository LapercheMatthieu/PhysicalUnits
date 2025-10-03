using Fractions;
using MatthL.PhysicalUnits.Computation.Converters;
using MatthL.PhysicalUnits.Core.EquationModels;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.Infrastructure.Extensions;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace MatthL.PhysicalUnits.Demo
{
    /// <summary>
    /// Logique d'interaction pour PhysicalUnitDemoControl.xaml
    /// </summary>
    public partial class PhysicalUnitDemoControl : UserControl, INotifyPropertyChanged
    {
        #region Properties

        // Première unité
        private double _value1 = 1.0;

        public double Value1
        {
            get => _value1;
            set
            {
                _value1 = value;
                OnPropertyChanged();
                UpdateCalculations();
            }
        }

        private PhysicalUnit _unit1;

        public PhysicalUnit Unit1
        {
            get => _unit1;
            set
            {
                _unit1 = value;
                OnPropertyChanged();
                UpdateCalculations();
            }
        }

        private PhysicalUnit _unit1InSI;

        public PhysicalUnit Unit1InSI
        {
            get => _unit1InSI;
            set
            {
                _unit1InSI = value;
                OnPropertyChanged();
            }
        }

        private double _value1InSI;

        public double Value1InSI
        {
            get => _value1InSI;
            set
            {
                _value1InSI = value;
                OnPropertyChanged();
            }
        }

        // Deuxième unité
        private double _value2 = 1.0;

        public double Value2
        {
            get => _value2;
            set
            {
                _value2 = value;
                OnPropertyChanged();
                UpdateCalculations();
            }
        }

        private PhysicalUnit _unit2;

        public PhysicalUnit Unit2
        {
            get => _unit2;
            set
            {
                _unit2 = value;
                OnPropertyChanged();
                UpdateCalculations();
            }
        }

        private PhysicalUnit _unit2InSI;

        public PhysicalUnit Unit2InSI
        {
            get => _unit2InSI;
            set
            {
                _unit2InSI = value;
                OnPropertyChanged();
            }
        }

        private double _value2InSI;

        public double Value2InSI
        {
            get => _value2InSI;
            set
            {
                _value2InSI = value;
                OnPropertyChanged();
            }
        }

        // Conversion entre unités
        private PhysicalUnit _targetUnit;

        public PhysicalUnit TargetUnit
        {
            get => _targetUnit;
            set
            {
                _targetUnit = value;
                OnPropertyChanged();
                UpdateConversion();
            }
        }

        private double _convertedValue1;

        public double ConvertedValue1
        {
            get => _convertedValue1;
            set
            {
                _convertedValue1 = value;
                OnPropertyChanged();
            }
        }

        private double _convertedValue2;

        public double ConvertedValue2
        {
            get => _convertedValue2;
            set
            {
                _convertedValue2 = value;
                OnPropertyChanged();
            }
        }

        // Exposants pour l'équation
        private string _exponent1 = "1";

        public string Exponent1
        {
            get => _exponent1;
            set
            {
                _exponent1 = value;
                OnPropertyChanged();
                UpdateEquationTerms();
            }
        }

        private string _exponent2 = "1";

        public string Exponent2
        {
            get => _exponent2;
            set
            {
                _exponent2 = value;
                OnPropertyChanged();
                UpdateEquationTerms();
            }
        }

        private EquationTerms _equationTerms;

        public EquationTerms EquationTerms
        {
            get => _equationTerms;
            set
            {
                _equationTerms = value;
                OnPropertyChanged();
            }
        }

        private PhysicalUnit _equationResultUnit;

        public PhysicalUnit EquationResultUnit
        {
            get => _equationResultUnit;
            set
            {
                _equationResultUnit = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        #region Constructor

        public PhysicalUnitDemoControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        #endregion Constructor

        #region Methods

        private void UpdateCalculations()
        {
            // Conversion vers SI
            if (Unit1 != null)
            {
                try
                {
                    Value1InSI = Unit1.ConvertToSIValue(Value1);
                    Unit1InSI = Unit1.GetSIUnit();
                }
                catch (Exception ex)
                {
                    Value1InSI = double.NaN;
                    System.Diagnostics.Debug.WriteLine($"Erreur conversion unité 1: {ex.Message}");
                }
            }

            if (Unit2 != null)
            {
                try
                {
                    Value2InSI = Unit2.ConvertToSIValue(Value2);
                    Unit2InSI = Unit2.GetSIUnit();
                }
                catch (Exception ex)
                {
                    Value2InSI = double.NaN;
                    System.Diagnostics.Debug.WriteLine($"Erreur conversion unité 2: {ex.Message}");
                }
            }

            UpdateConversion();
        }

        private void UpdateConversion()
        {
            if (Unit1 != null && TargetUnit != null)
            {
                try
                {
                    ConvertedValue1 = Unit1.ConvertValue(TargetUnit, Value1);
                }
                catch (Exception ex)
                {
                    ConvertedValue1 = double.NaN;
                    System.Diagnostics.Debug.WriteLine($"Erreur conversion 1: {ex.Message}");
                }
            }

            if (Unit2 != null && TargetUnit != null)
            {
                try
                {
                    ConvertedValue2 = Unit2.ConvertValue(TargetUnit, Value2);
                }
                catch (Exception ex)
                {
                    ConvertedValue2 = double.NaN;
                    System.Diagnostics.Debug.WriteLine($"Erreur conversion 2: {ex.Message}");
                }
            }
        }

        private void UpdateEquationTerms()
        {
            if (Unit1 == null || Unit2 == null) return;

            try
            {
                // Parser les exposants
                var exp1 = ParseFraction(Exponent1);
                var exp2 = ParseFraction(Exponent2);

                // Créer les termes de l'équation
                var term1 = new PhysicalUnitTerm(Unit1, exp1);
                var term2 = new PhysicalUnitTerm(Unit2, exp2);

                EquationTerms = new EquationTerms(term1, term2);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur création équation: {ex.Message}");
                EquationTerms = null;
            }
        }

        private Fraction ParseFraction(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new Fraction(1);

            // Gérer le format "numerateur/denominateur"
            if (input.Contains("/"))
            {
                var parts = input.Split('/');
                if (parts.Length == 2 &&
                    int.TryParse(parts[0].Trim(), out int num) &&
                    int.TryParse(parts[1].Trim(), out int den))
                {
                    return new Fraction(num, den);
                }
            }

            // Sinon essayer de parser comme un entier
            if (int.TryParse(input.Trim(), out int value))
            {
                return new Fraction(value);
            }

            // Par défaut
            return new Fraction(1);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            Value1 = 1.0;
            Value2 = 1.0;
            Unit1 = null;
            Unit2 = null;
            TargetUnit = null;
            EquationResultUnit = null;
            Exponent1 = "1";
            Exponent2 = "1";
        }

        #endregion Methods

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UpdateCalculations();
        }
    }
}