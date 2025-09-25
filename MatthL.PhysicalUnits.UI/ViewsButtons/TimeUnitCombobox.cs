using MatthL.PhysicalUnits.Enums;
using MatthL.PhysicalUnits.Models;
using MatthL.PhysicalUnits.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MatthL.PhysicalUnits.ViewsButtons
{
    public class TimeUnitCombobox : ComboBox
    {
        public TimeUnitCombobox()
        {
            var UnitList = new List<PhysicalUnit>()
            {
                StandardUnits.Second(Enums.Prefix.nano),
                StandardUnits.Second(Enums.Prefix.micro),
                StandardUnits.Second(Enums.Prefix.milli),
                StandardUnits.Second(Enums.Prefix.SI),
                StandardUnits.Minute,
                StandardUnits.Hour,
                StandardUnits.Day,
                StandardUnits.Week,
                StandardUnits.Month,
                StandardUnits.Year,
            };
            ItemsSource = UnitList;
            DisplayMemberPath = "ToString";
        }
    }
}
