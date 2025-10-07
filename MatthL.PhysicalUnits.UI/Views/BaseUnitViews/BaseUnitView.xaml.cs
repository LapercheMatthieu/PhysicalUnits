using MatthL.PhysicalUnits.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MatthL.PhysicalUnits.UI.Views.BaseUnitViews
{
    /// <summary>
    /// Logique d'interaction pour BaseUnitView.xaml
    /// </summary>
    public partial class BaseUnitView : UserControl
    {
        public BaseUnitView()
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
