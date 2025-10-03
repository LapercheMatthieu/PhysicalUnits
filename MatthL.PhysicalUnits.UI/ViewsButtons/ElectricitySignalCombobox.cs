using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.Infrastructure.Library;
using System.Windows.Controls;

namespace MatthL.PhysicalUnits.ViewsButtons
{
    public class ElectricitySignalCombobox : ComboBox
    {
        public ElectricitySignalCombobox()
        {
            var UnitList = new List<PhysicalUnit>()
            {
                StandardUnits.Ampere(Prefix.micro),
                StandardUnits.Ampere(Prefix.milli),
                StandardUnits.Ampere(),
                StandardUnits.Volt(Prefix.micro),
                StandardUnits.Volt(Prefix.milli),
                StandardUnits.Volt(),
            };
            ItemsSource = UnitList;
            DisplayMemberPath = "ToString";
        }
    }
}