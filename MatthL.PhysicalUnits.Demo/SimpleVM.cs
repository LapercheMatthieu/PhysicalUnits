using CommunityToolkit.Mvvm.ComponentModel;
using  PhysicalUnitManagement.Models;
using  PhysicalUnitManagement.Services;
using SQLiteManager.Authorizations;
using SQLiteManager.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  PhysicalUnitManager.Demo
{
    public partial class SimpleVM : ObservableObject
    {
        [ObservableProperty]
        private PhysicalUnit _unit1;

        [ObservableProperty]
        private PhysicalUnit _unit2;

        [ObservableProperty]
        private PhysicalUnit _mySelectedUnit;

        [ObservableProperty]
        private EquationTerms _myEquationTerms;

        public SimpleVM()
        {
            CreateDB();
            // Initialiser avec une instance vide
            MyEquationTerms = new EquationTerms();

            var allPhysicalValues = PhysicalUnitStorage.GetAllUnits();
            var notWorking = new List<PhysicalUnit>();
            var notWorking2 = new List<PhysicalUnit>();
            foreach (var unit in allPhysicalValues)
            {
                if(unit.GetSIUnit().ToString() == "SI")
                {
                    notWorking.Add(unit);
                }
            }
            foreach (var unit in allPhysicalValues)
            {
                if (unit.GetSIUnit().DimensionalFormula != unit.DimensionalFormula)
                {
                    notWorking2.Add(unit);
                }
            }
            var test = "";
        }
        private async void CreateDB()
        {
            var sqlmanager = new SQLManager(new PhysicalUnitRootDBContext(), "C:\\Users\\Matthieu.Laperche\\Documents\\EssaiDB", "test", new AdminAuthorization());
            var result = await sqlmanager.Create();
        }

        partial void OnUnit1Changed(PhysicalUnit value)
        {
            UpdateEquationTerms();
        }

        partial void OnUnit2Changed(PhysicalUnit value)
        {
            UpdateEquationTerms();
        }

        private void UpdateEquationTerms()
        {
            if (Unit1 == null || Unit2 == null)
            {
                MyEquationTerms = new EquationTerms();
            }
            else
            {
                MyEquationTerms = new EquationTerms
                {
                    Terms = new List<PhysicalUnitTerm>
                    {
                        new PhysicalUnitTerm { Unit = Unit1, Exponent = 1 },
                        new PhysicalUnitTerm { Unit = Unit2, Exponent = 1 }
                    }
                };
            }
        }
    }
}
