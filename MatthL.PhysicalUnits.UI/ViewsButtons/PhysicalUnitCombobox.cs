using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.Infrastructure.Repositories;
using System.Windows.Controls;

namespace MatthL.PhysicalUnits.UI.ViewsButtons
{
    public class PhysicalUnitCombobox : ComboBox
    {
        public PhysicalUnitCombobox()
        {
            ItemsSource = RepositorySearchEngine.GetAllUnits();
            DisplayMemberPath = "ToString";
        }

        public PhysicalUnitCombobox(UnitType unitType)
        {
            ItemsSource = RepositorySearchEngine.GetUnitsOfType(unitType);
            DisplayMemberPath = "ToString";
        }

        public void LimitSource(UnitType unitType)
        {
            ItemsSource = RepositorySearchEngine.GetUnitsOfType(unitType);
        }

        public void DurationTypeCombobox()
        {
            var timeUnits = RepositorySearchEngine.GetUnitsOfType(UnitType.Time_Base).ToList();

            // Trouver l'unité seconde (SI)
            var second = timeUnits.FirstOrDefault(u => u.IsSI);
            if (second != null)
            {
                // Ajouter les versions avec préfixes
                var millisecond = new PhysicalUnit(second, Prefix.milli);
                var microsecond = new PhysicalUnit(second, Prefix.micro);
                var nanosecond = new PhysicalUnit(second, Prefix.nano);

                // Créer une nouvelle liste avec toutes les unités
                var extendedList = new List<PhysicalUnit>(timeUnits)
                {
                    millisecond,
                    microsecond,
                    nanosecond
                };

                ItemsSource = extendedList;
            }
            else
            {
                ItemsSource = timeUnits;
            }
        }
    }
}