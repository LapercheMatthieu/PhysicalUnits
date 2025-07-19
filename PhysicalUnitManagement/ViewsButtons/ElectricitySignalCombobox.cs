using  PhysicalUnitManagement.Models;
using  PhysicalUnitManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace  PhysicalUnitManagement.ViewsButtons
{
    public class ElectricitySignalCombobox : ComboBox
    {
        public ElectricitySignalCombobox()
        {
            var UnitList = new List<PhysicalUnit>()
            {
                StandardUnits.Ampere(Enums.Prefix.micro),
                StandardUnits.Ampere(Enums.Prefix.milli),
                StandardUnits.Ampere(),
                StandardUnits.Volt(Enums.Prefix.micro),
                StandardUnits.Volt(Enums.Prefix.milli),
                StandardUnits.Volt(),
            };
            ItemsSource = UnitList;
            DisplayMemberPath = "ToString";

        }
    }
}
