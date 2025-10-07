using CommunityToolkit.Mvvm.ComponentModel;
using Fractions;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.DimensionalFormulas.Extensions;

namespace MatthL.PhysicalUnits.UI.ViewModels
{
    /// <summary>
    /// ViewModel pour représenter un BaseUnit avec toutes ses propriétés observables
    /// </summary>
    public partial class BaseUnitViewModel : ObservableObject
    {
        public event Action<BaseUnitViewModel> AskDeletion;

        public event Action<BaseUnitViewModel> GotModified;

        private readonly BaseUnit _model;

        [ObservableProperty]
        private bool _canEdit = true;

        public BaseUnit Model => _model;

        // Propriétés directes du modèle en lecture
        public int Exponent_Numerator
        {
            get => _model.Exponent_Numerator;
            set
            {
                if (_model.Exponent_Numerator != value && CanEdit)
                {
                    _model.Exponent_Numerator = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Exponent));
                    GotModified?.Invoke(this);
                }
            }
        }

        public int Exponent_Denominator
        {
            get => _model.Exponent_Denominator;
            set
            {
                if (_model.Exponent_Denominator != value && CanEdit)
                {
                    _model.Exponent_Denominator = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Exponent));
                    GotModified?.Invoke(this);
                }
            }
        }

        public BaseUnitViewModel(BaseUnit model, bool canEdit = true)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _canEdit = canEdit;
        }

        public int Id => _model.Id;
        public UnitType UnitType => _model.UnitType;
        public StandardUnitSystem UnitSystem => _model.UnitSystem;
        public string Name => _model.Name;
        public string Symbol => _model.Symbol;
        public bool IsSI => _model.IsSI;
        public double Offset => _model.Offset;

        // Propriétés calculées du modèle
        public string DimensionalFormula => _model.GetDimensionalFormula();

        public string PrefixedSymbol => _model.PrefixedSymbol;
        public string PrefixedName => _model.PrefixedName;
        public Fraction ConversionFactor => _model.ConversionFactor;
        public string Exponent => _model.Exponent.ToString();

        // Propriétés éditables avec synchronisation bidirectionnelle
        public Prefix Prefix
        {
            get => _model.Prefix;
            set
            {
                if (_model.Prefix != value)
                {
                    _model.Prefix = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(PrefixedSymbol));
                    OnPropertyChanged(nameof(PrefixedName));
                    GotModified?.Invoke(this);
                }
            }
        }

        /// <summary>
        /// Met à jour toutes les propriétés d'affichage
        /// </summary>
        public void Refresh()
        {
            OnPropertyChanged(string.Empty); // Notifie toutes les propriétés
        }

        public void RaiseAskDeletion()
        {
            AskDeletion?.Invoke(this);
        }
    }
}