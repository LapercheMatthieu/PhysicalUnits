using MahApps.Metro.IconPacks;
using System.Globalization;
using System.Windows.Data;
using static MatthL.PhysicalUnits.UI.ViewsButtons.SpecificUnitSelectorViews.SpecificUnitSelectorView;

namespace MatthL.PhysicalUnits.UI.Converters
{
    public class CategoryToVaadinIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is UnitCategory category)
            {
                switch (category)
                {
                    case UnitCategory.Time:
                        return PackIconVaadinIconsKind.Hourglass;

                    case UnitCategory.Electric:
                        return PackIconVaadinIconsKind.Bolt;

                    default:
                        return PackIconVaadinIconsKind.Bolt;
                }
            }
            return PackIconVaadinIconsKind.Bolt;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}