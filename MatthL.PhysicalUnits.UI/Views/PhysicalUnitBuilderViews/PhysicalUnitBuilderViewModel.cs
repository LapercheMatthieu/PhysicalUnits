using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.DimensionalFormulas.Extensions;
using MatthL.PhysicalUnits.Infrastructure.Repositories;
using MatthL.PhysicalUnits.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.UI.Views.PhysicalUnitBuilderViews
{
    public partial class PhysicalUnitBuilderViewModel : ObservableObject
    {
        #region CONVERTING REGION

        [ObservableProperty] private bool _IsConverting;
        [ObservableProperty] private PhysicalUnit _UnitToConvert;

        partial void OnUnitToConvertChanged(PhysicalUnit value)
        {
            if (value != null)
            {
                GetCompatibleUnits();
            }
        }

        private void GetCompatibleUnits()
        {
            IsSearching = true;
            SearchResults.Clear();
            var results = RepositorySearchEngine.GetUnitsFromDimensionalFormula(UnitToConvert.GetDimensionalFormula());
            foreach (var unit in results)
            {
                SearchResults.Add(unit);
            }
            IsInBuilding = false;
            OnPropertyChanged(nameof(AvailableUnits));
        }

        #endregion CONVERTING REGION

        #region Search Region

        [ObservableProperty]
        private string _searchText;

        [ObservableProperty]
        private bool _isSearching;

        [ObservableProperty]
        private ObservableCollection<PhysicalUnit> _searchResults = new ObservableCollection<PhysicalUnit>();

        partial void OnSearchTextChanged(string value)
        {
            if (IsConverting)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    SearchResults.Clear();
                    GetCompatibleUnits();
                }
                else
                {
                    PerformSearch(value, UnitToConvert.GetDimensionalFormula());
                }
                return;
            }
            IsSearching = !string.IsNullOrWhiteSpace(value);

            if (IsSearching)
            {
                PerformSearch(value);
            }
            else
            {
                SearchResults.Clear();
            }

            OnPropertyChanged(nameof(AvailableUnits));
        }

        private void PerformSearch(string searchText)
        {
            SearchResults.Clear();

            if (string.IsNullOrWhiteSpace(searchText))
                return;

            var results = RepositorySearchEngine.SearchUnits(searchText);
            foreach (var unit in results)
            {
                SearchResults.Add(unit);
            }
        }

        private void PerformSearch(string searchText, string formula)
        {
            SearchResults.Clear();

            if (string.IsNullOrWhiteSpace(searchText))
                return;

            var results = RepositorySearchEngine.SearchUnits(searchText, formula);
            foreach (var unit in results)
            {
                SearchResults.Add(unit);
            }
        }

        #endregion Search Region

        #region Categories

        public IEnumerable<PhysicalUnitDomain> AvailableCategories => RepositorySearchEngine.GetDomains();

        [ObservableProperty]
        private PhysicalUnitDomain? _selectedCategory;

        partial void OnSelectedCategoryChanged(PhysicalUnitDomain? value)
        {
            OnPropertyChanged(nameof(AvailableUnitTypes));
            OnPropertyChanged(nameof(AvailableUnits));
            SelectedUnitType = null;
        }

        #endregion Categories

        #region Unit Types

        public List<UnitType> AvailableUnitTypes => RepositorySearchEngine.GetUnitTypesForDomain(SelectedCategory);

        [ObservableProperty]
        private UnitType? _selectedUnitType;

        partial void OnSelectedUnitTypeChanged(UnitType? value)
        {
            OnPropertyChanged(nameof(AvailableUnits));
            SelectedUnitName = null;
        }

        #endregion Unit Types

        #region Available Units

        public ObservableCollection<PhysicalUnit> AvailableUnits
        {
            get
            {
                if (IsSearching)
                {
                    return SearchResults;
                }
                return new ObservableCollection<PhysicalUnit>(
                    RepositorySearchEngine.GetUnits(domain: SelectedCategory, unitType: SelectedUnitType));
            }
        }

        [ObservableProperty]
        private PhysicalUnit _selectedUnitName;

        partial void OnSelectedUnitNameChanged(PhysicalUnit value)
        {
            if (value == null) return;

            if (!IsInBuilding)
            {
                // Mode normal : créer un nouveau ViewModel pour l'unité sélectionnée
                SelectedUnitViewModel = new PhysicalUnitViewModel(value);
            }
        }

        #endregion Available Units

        #region Building Mode

        [ObservableProperty]
        private bool _isInBuilding;

        partial void OnIsInBuildingChanged(bool value)
        {
            if (value && IsConverting)
            {
                IsInBuilding = false;
                return;
            }
            if (value && SelectedUnitViewModel == null)
            {
                // Entrer en mode building : créer une nouvelle unité vide
                var newUnit = new PhysicalUnit();
                SelectedUnitViewModel = new PhysicalUnitViewModel(newUnit);
            }

            // Mettre à jour CanEdit sur le ViewModel sélectionné
            if (SelectedUnitViewModel != null)
            {
                SelectedUnitViewModel.CanEdit = value;
            }
        }

        #endregion Building Mode

        #region Selected Unit Management

        [ObservableProperty]
        private PhysicalUnit _selectedUnit;

        [ObservableProperty]
        private PhysicalUnitViewModel _selectedUnitViewModel;

        partial void OnSelectedUnitViewModelChanged(PhysicalUnitViewModel value)
        {
            SelectedUnit = value?.Model;

            // S'assurer que CanEdit est correctement défini
            if (value != null)
            {
                value.CanEdit = IsInBuilding;
                value.GotModified += Value_GotModified;
            }

            SelectedUnitChanged?.Invoke(this, SelectedUnit);
            GotModified?.Invoke();
        }

        public event Action GotModified;

        private void Value_GotModified()
        {
            OnPropertyChanged(nameof(SelectedUnit));
            GotModified?.Invoke();
        }

        #endregion Selected Unit Management

        #region Events

        public event EventHandler<PhysicalUnit> SelectedUnitChanged;

        #endregion Events

        #region Constructor

        public PhysicalUnitBuilderViewModel()
        {
            PhysicalUnitRepository.Initialize();
        }

        public PhysicalUnitBuilderViewModel(PhysicalUnit UnitToConvert)
        {
            PhysicalUnitRepository.Initialize();
        }

        #endregion Constructor

        #region Commands

        [RelayCommand]
        private void ClearSelection()
        {
            SelectedUnit = null;
            SelectedUnitName = null;
            SelectedUnitType = null;
            SelectedUnitViewModel = null;
            SearchText = string.Empty;
        }

        /// <summary>
        /// Ajoute une unité physique au mode building
        /// </summary>
        public void AddPhysicalUnitToBuild(PhysicalUnit physicalUnit)
        {
            if (!IsInBuilding || physicalUnit == null) return;

            if (SelectedUnitViewModel == null)
            {
                // Première unité : créer le ViewModel
                SelectedUnitViewModel = new PhysicalUnitViewModel(physicalUnit);
            }
            else
            {
                // Ajouter à l'unité existante
                SelectedUnitViewModel.AddPhysicalUnit(physicalUnit);
            }
        }

        [RelayCommand]
        private void RemoveBaseUnit(BaseUnitViewModel baseUnit)
        {
            if (SelectedUnitViewModel != null && IsInBuilding)
            {
                SelectedUnitViewModel.RemoveBaseUnit(baseUnit);
            }
        }

        #endregion Commands
    }
}