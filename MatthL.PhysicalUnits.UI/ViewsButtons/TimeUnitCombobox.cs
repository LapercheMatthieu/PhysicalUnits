using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.Core.Services;
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
                StandardUnits.Second(Prefix.nano),
                StandardUnits.Second(Prefix.micro),
                StandardUnits.Second(Prefix.milli),
                StandardUnits.Second(Prefix.SI),
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
