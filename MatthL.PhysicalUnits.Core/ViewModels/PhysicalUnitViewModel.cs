using CommunityToolkit.Mvvm.ComponentModel;
using MatthL.PhysicalUnits.Enums;
using MatthL.PhysicalUnits.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MatthL.PhysicalUnits.ViewModels
{
    /// <summary>
    /// ViewModel qui reflète exactement un PhysicalUnit
    /// </summary>
    public partial class PhysicalUnitViewModel : ObservableObject
    {
        private readonly PhysicalUnit _model;
        public event Action GotModified;

        [ObservableProperty]
        private ObservableCollection<BaseUnitViewModel> _baseUnitViewModels;

        [ObservableProperty]
        private bool _canEdit = true;

        public PhysicalUnit Model => _model;

        // Propriétés directes du modèle en lecture
        public int Id => _model.Id;

        // Propriétés calculées du modèle
        public string Name => _model.Name;
        public string Symbol => _model.ToString();
        public bool IsSI => _model.IsSI;
        public PhysicalUnit SIUnit
        {
            get
            {
                return _model.GetSIUnit();
            }
        }
        public StandardUnitSystem UnitSystem => _model.UnitSystem;
        public string DimensionalFormula => _model.DimensionalFormula;

        // Propriété éditable
        public UnitType UnitType
        {
            get => _model.UnitType;
            set
            {
                if (_model.UnitType != value)
                {
                    _model.UnitType = value;
                    OnPropertyChanged();
                }
            }
        }
        public string UnitTypeName
        {
            get
            {
                return UnitType.GetShortName();
            }
        }

        public PhysicalUnitViewModel(PhysicalUnit model)
        {
            _model = PhysicalUnit.Clone(model) ?? throw new ArgumentNullException(nameof(model));
            _baseUnitViewModels = new ObservableCollection<BaseUnitViewModel>();
            SyncBaseUnits();
        }

        partial void OnCanEditChanged(bool value)
        {
            // Propager CanEdit à tous les BaseUnitViewModels
            foreach (var vm in BaseUnitViewModels)
            {
                vm.CanEdit = value;
            }
        }

        /// <summary>
        /// Synchronise les BaseUnitViewModels avec le modèle
        /// </summary>
        private void SyncBaseUnits()
        {
            // Nettoyer les anciens ViewModels
            foreach (var vm in _baseUnitViewModels)
            {
                vm.PropertyChanged -= OnBaseUnitViewModelChanged;
                vm.AskDeletion -= Vm_AskDeletion;
                vm.GotModified -= Vm_GotModified;
            }
            _baseUnitViewModels.Clear();

            // Créer les nouveaux ViewModels
            foreach (var baseUnit in _model.BaseUnits)
            {
                var vm = new BaseUnitViewModel(baseUnit);
                vm.PropertyChanged += OnBaseUnitViewModelChanged;
                vm.AskDeletion += Vm_AskDeletion;
                vm.GotModified += Vm_GotModified;
                _baseUnitViewModels.Add(vm);
            }
        }

        private void OnBaseUnitViewModelChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Propager les changements qui affectent l'unité globale
            if (e.PropertyName == nameof(BaseUnitViewModel.Prefix) ||
                e.PropertyName == nameof(BaseUnitViewModel.Exponent_Numerator) ||
                e.PropertyName == nameof(BaseUnitViewModel.Exponent_Denominator))
            {
                OnPropertyChanged(nameof(Symbol));
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(DimensionalFormula));
            }
        }

        /// <summary>
        /// Ajoute un BaseUnit au modèle
        /// </summary>
        public void AddBaseUnit(BaseUnit baseUnit)
        {
            var newUnit = BaseUnit.Clone(baseUnit);
            _model.BaseUnits.Add(newUnit);

            var vm = new BaseUnitViewModel(newUnit);
            vm.PropertyChanged += OnBaseUnitViewModelChanged;
            vm.AskDeletion += Vm_AskDeletion;
            vm.GotModified += Vm_GotModified;
            _baseUnitViewModels.Add(vm);


            RefreshCalculatedProperties();
        }

        private void Vm_GotModified(BaseUnitViewModel obj)
        {
            RefreshCalculatedProperties();
            GotModified?.Invoke();
        }

        private void Vm_AskDeletion(BaseUnitViewModel obj)
        {
            _model.BaseUnits.Remove(obj.Model);

            BaseUnitViewModels.Remove(obj);
            obj.PropertyChanged -= OnBaseUnitViewModelChanged;
            obj.AskDeletion -= Vm_AskDeletion;
            obj.GotModified -= Vm_GotModified;
            RefreshCalculatedProperties();
        }

        /// <summary>
        /// Ajoute tous les BaseUnits d'un autre PhysicalUnit
        /// </summary>
        public void AddPhysicalUnit(PhysicalUnit physicalUnit)
        {
            UnitType = UnitType.Unknown_Special;
            foreach (var baseUnit in physicalUnit.BaseUnits)
            {
                AddBaseUnit(baseUnit);
            }
            RefreshCalculatedProperties();
        }

        /// <summary>
        /// Supprime un BaseUnit
        /// </summary>
        public void RemoveBaseUnit(BaseUnitViewModel baseUnitViewModel)
        {
            if (baseUnitViewModel == null) return;

            _model.BaseUnits.Remove(baseUnitViewModel.Model);
            baseUnitViewModel.PropertyChanged -= OnBaseUnitViewModelChanged;
            _baseUnitViewModels.Remove(baseUnitViewModel);

            RefreshCalculatedProperties();
        }

        /// <summary>
        /// Vide tous les BaseUnits
        /// </summary>
        public void Clear()
        {
            _model.BaseUnits.Clear();

            foreach (var vm in _baseUnitViewModels)
            {
                vm.PropertyChanged -= OnBaseUnitViewModelChanged;
            }
            _baseUnitViewModels.Clear();

            RefreshCalculatedProperties();
        }

        /// <summary>
        /// Force le rafraîchissement des propriétés calculées
        /// </summary>
        public void RefreshCalculatedProperties()
        {
            OnPropertyChanged();
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(SIUnit));
            OnPropertyChanged(nameof(Symbol));
            OnPropertyChanged(nameof(IsSI));
            OnPropertyChanged(nameof(UnitSystem));
            OnPropertyChanged(nameof(DimensionalFormula));
        }

        /// <summary>
        /// Force le rafraîchissement complet
        /// </summary>
        public void Refresh()
        {
            SyncBaseUnits();
            OnPropertyChanged();
            OnPropertyChanged(string.Empty);
        }
    }
}