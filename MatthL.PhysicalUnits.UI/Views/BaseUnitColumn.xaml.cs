using MatthL.PhysicalUnits.UI.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace MatthL.PhysicalUnits.UI.Views
{
    /// <summary>
    /// Logique d'interaction pour BaseUnitColumn.xaml
    /// </summary>
    public partial class BaseUnitColumn : UserControl
    {
        public BaseUnitColumn()
        {
            InitializeComponent();
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = (BaseUnitViewModel)DataContext;
            vm.RaiseAskDeletion();
        }
    }
}